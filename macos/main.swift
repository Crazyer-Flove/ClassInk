import AppKit
import CoreGraphics

final class Canvas: NSView {
    var lines: [[CGPoint]] = []
    var background = NSColor.clear
    var snapshot: NSImage?
    var zoom: CGFloat = 1
    var origin = CGPoint.zero
    override var isFlipped: Bool { true }
    override var acceptsFirstResponder: Bool { true }
    override func draw(_ dirtyRect: NSRect) {
        background.setFill(); NSBezierPath(rect: bounds).fill()
        if let snapshot {
            snapshot.draw(in: CGRect(x: origin.x, y: origin.y, width: bounds.width * zoom, height: bounds.height * zoom))
        }
        NSColor.systemRed.setStroke()
        for line in lines where !line.isEmpty {
            let path = NSBezierPath(); path.lineWidth = 4; path.lineCapStyle = .round; path.lineJoinStyle = .round
            path.move(to: line[0]); for point in line.dropFirst() { path.line(to: point) }
            if line.count == 1 { path.line(to: CGPoint(x: line[0].x + 0.1, y: line[0].y)) }
            path.stroke()
        }
    }
    override func mouseDown(with event: NSEvent) { lines.append([convert(event.locationInWindow, from: nil)]); needsDisplay = true }
    override func mouseDragged(with event: NSEvent) {
        guard !lines.isEmpty else { return }
        lines[lines.count - 1].append(convert(event.locationInWindow, from: nil)); needsDisplay = true
    }
    override func scrollWheel(with event: NSEvent) {
        guard snapshot != nil else { return }
        let anchor = convert(event.locationInWindow, from: nil)
        let next = min(8, max(1, zoom * pow(1.015, event.scrollingDeltaY)))
        origin = CGPoint(x: anchor.x + (origin.x - anchor.x) * next / zoom, y: anchor.y + (origin.y - anchor.y) * next / zoom)
        zoom = next; clampOrigin()
    }
    override func rightMouseDragged(with event: NSEvent) {
        guard snapshot != nil else { return }
        origin.x += event.deltaX; origin.y += event.deltaY; clampOrigin()
    }
    private func clampOrigin() {
        origin.x = min(0, max(bounds.width * (1 - zoom), origin.x))
        origin.y = min(0, max(bounds.height * (1 - zoom), origin.y)); needsDisplay = true
    }
    override func keyDown(with event: NSEvent) {
        if event.keyCode == 53 { (NSApp.delegate as? App)?.mouseMode() }
        else if event.modifierFlags.contains(.command), event.charactersIgnoringModifiers == "z" { undoLine() }
        else { super.keyDown(with: event) }
    }
    func undoLine() { if !lines.isEmpty { lines.removeLast(); needsDisplay = true } }
}

final class App: NSObject, NSApplicationDelegate {
    let canvas = Canvas()
    var overlay: NSWindow!
    var toolbar: NSPanel!
    var previousLines: [[CGPoint]]?
    var shortcut: Any?
    func applicationDidFinishLaunching(_ notification: Notification) {
        guard let screen = NSScreen.screens.first else { NSApp.terminate(nil); return }
        overlay = OverlayWindow(contentRect: screen.frame, styleMask: [.borderless], backing: .buffered, defer: false)
        overlay.isReleasedWhenClosed = false; overlay.level = .floating
        overlay.isOpaque = false; overlay.backgroundColor = .clear; overlay.hasShadow = false
        overlay.contentView = canvas
        toolbar = NSPanel(contentRect: NSRect(x: 0, y: 0, width: 510, height: 48), styleMask: [.titled, .closable, .utilityWindow], backing: .buffered, defer: false)
        toolbar.title = "课堂笔 ClassInk"; toolbar.isFloatingPanel = true; toolbar.level = .statusBar
        let tools = NSStackView(); tools.spacing = 5; tools.edgeInsets = NSEdgeInsets(top: 8, left: 8, bottom: 8, right: 8)
        for (label, action) in [("画笔", #selector(pen)), ("鼠标", #selector(mouseMode)), ("板面", #selector(board)), ("缩放", #selector(zoomScreen)), ("撤销", #selector(undo)), ("清空", #selector(clear)), ("导出", #selector(exportPNG)), ("打赏", #selector(support))] {
            let button = NSButton(title: label, target: self, action: action); tools.addArrangedSubview(button)
        }
        toolbar.contentView = tools; toolbar.center(); toolbar.makeKeyAndOrderFront(nil)
        let menu = NSMenu(); let item = NSMenuItem(); menu.addItem(item)
        item.submenu = NSMenu(title: "课堂笔"); item.submenu?.addItem(withTitle: "退出课堂笔", action: #selector(NSApplication.terminate(_:)), keyEquivalent: "q"); NSApp.mainMenu = menu
        shortcut = NSEvent.addLocalMonitorForEvents(matching: .keyDown) { [weak self] event in
            if event.modifierFlags.contains(.control), event.charactersIgnoringModifiers == "1" { self?.zoomScreen(); return nil }
            if event.keyCode == 53 { self?.mouseMode(); return nil }
            return event
        }
        mouseMode(); NSApp.activate(ignoringOtherApps: true)
    }
    func applicationShouldTerminateAfterLastWindowClosed(_ sender: NSApplication) -> Bool { true }
    @objc func pen() { overlay.ignoresMouseEvents = false; overlay.makeKeyAndOrderFront(nil); overlay.makeFirstResponder(canvas) }
    @objc func mouseMode() {
        if let previousLines { canvas.lines = previousLines; self.previousLines = nil; canvas.snapshot = nil; canvas.needsDisplay = true }
        overlay.ignoresMouseEvents = true; overlay.orderFrontRegardless()
    }
    @objc func board() {
        mouseMode()
        canvas.background = canvas.background == .clear ? .white : canvas.background == .white ? .black : .clear
        canvas.needsDisplay = true; pen()
    }
    @objc func zoomScreen() {
        if previousLines != nil { mouseMode(); return }
        guard CGPreflightScreenCaptureAccess() || CGRequestScreenCaptureAccess() else {
            let alert = NSAlert(); alert.messageText = "请在系统设置中允许课堂笔录制屏幕"; alert.informativeText = "授权后重新打开课堂笔，再使用缩放工具。"; alert.runModal(); return
        }
        overlay.orderOut(nil); toolbar.orderOut(nil)
        DispatchQueue.main.asyncAfter(deadline: .now() + 0.2) { [weak self] in
            guard let self else { return }
            let image = CGDisplayCreateImage(CGMainDisplayID())
            self.toolbar.orderFrontRegardless()
            guard let image else { self.mouseMode(); return }
            self.previousLines = self.canvas.lines; self.canvas.lines = []
            self.canvas.snapshot = NSImage(cgImage: image, size: self.canvas.bounds.size)
            self.canvas.zoom = 2; self.canvas.origin = CGPoint(x: -self.canvas.bounds.width / 2, y: -self.canvas.bounds.height / 2); self.canvas.needsDisplay = true; self.pen()
        }
    }
    @objc func undo() { canvas.undoLine() }
    @objc func clear() { canvas.lines.removeAll(); canvas.needsDisplay = true }
    @objc func exportPNG() {
        guard let rep = canvas.bitmapImageRepForCachingDisplay(in: canvas.bounds) else { return }
        canvas.cacheDisplay(in: canvas.bounds, to: rep)
        guard let data = rep.representation(using: .png, properties: [:]) else { return }
        let save = NSSavePanel(); save.nameFieldStringValue = "ClassInk.png"
        guard save.runModal() == .OK, let url = save.url else { return }
        do { try data.write(to: url, options: .atomic) } catch { NSAlert(error: error).runModal() }
    }
    @objc func support() {
        let row = NSStackView(); row.frame = CGRect(x: 0, y: 0, width: 410, height: 224)
        for index in 1...2 {
            let column = NSStackView(); column.orientation = .vertical; column.alignment = .centerX
            let label = NSTextField(labelWithString: index == 1 ? "支付宝" : "微信")
            label.font = .systemFont(ofSize: 15, weight: .semibold)
            column.addArrangedSubview(label)
            let view = NSImageView(); view.imageScaling = .scaleProportionallyUpOrDown
            if let url = Bundle.main.url(forResource: "support-\(index)", withExtension: "jpg") { view.image = NSImage(contentsOf: url) }
            view.widthAnchor.constraint(equalToConstant: 196).isActive = true
            view.heightAnchor.constraint(equalToConstant: 196).isActive = true
            column.addArrangedSubview(view); row.addArrangedSubview(column)
        }
        let alert = NSAlert(); alert.messageText = "打赏支持课堂笔"; alert.informativeText = "请用支付宝或微信扫描对应二维码，自愿支持。"; alert.accessoryView = row; alert.runModal()
    }
}

final class OverlayWindow: NSWindow { override var canBecomeKey: Bool { true } }
let app = NSApplication.shared
let delegate = App()
app.delegate = delegate
app.setActivationPolicy(.regular)
app.run()

