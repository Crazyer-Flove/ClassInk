using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using Microsoft.Win32;

[assembly: System.Reflection.AssemblyTitle("课堂笔 ClassInk")]
[assembly: System.Reflection.AssemblyProduct("ClassInk")]
[assembly: System.Reflection.AssemblyVersion("1.4.1.0")]
[assembly: System.Reflection.AssemblyFileVersion("1.4.1.0")]

namespace ClassInk {
 public sealed class ToolIcon : FrameworkElement {
  readonly string name;
  public ToolIcon(string value){name=value;Width=20;Height=20;IsHitTestVisible=false;}
  protected override void OnRender(DrawingContext dc){
   bool smallArrow=name=="prev"||name=="next";if(smallArrow)dc.PushTransform(new ScaleTransform(0.5,0.5));
   var owner=System.Windows.Media.VisualTreeHelper.GetParent(this);Brush brush=Brushes.SlateGray;
   while(owner!=null){var c=owner as Control;if(c!=null){brush=c.Foreground;break;}owner=System.Windows.Media.VisualTreeHelper.GetParent(owner);}
   string path="";
   switch(name){
    case "mouse":path="M4,2 L16,11 10,12 7,18 Z";break;
    case "pen":path="M4,13 L13,4 Q14,3 15,4 L17,6 Q18,7 17,8 L8,17 3,18 Z M12,5 L16,9 M4,13 L8,17";break;
    case "laser":path="M13,10 A3,3 0 1 1 7,10 A3,3 0 1 1 13,10 M10,2 A8,8 0 0 1 18,10 M10,18 A8,8 0 0 1 2,10";break;
    case "highlight":path="M5,11 L12,4 17,9 10,16 Z M5,11 L4,16 7,18 10,16 M3,19 L11,19";break;
    case "erase":path="M3,12 L11,4 Q12,3 13,4 L18,9 9,18 7,18 Z M7,8 L14,15 M9,18 L18,18";break;
    case "text":path="M4,5 L16,5 M10,5 L10,17 M7,17 L13,17 M4,5 L4,8 M16,5 L16,8";break;
    case "select":path="M4,7 L4,4 7,4 M13,4 L16,4 16,7 M16,13 L16,16 13,16 M7,16 L4,16 4,13";break;
    case "line":path="M4,16 L16,4";break;
    case "arrow":path="M3,10 L17,10 M12,5 L17,10 12,15";break;
    case "rect":path="M5,4 L15,4 Q17,4 17,6 L17,14 Q17,16 15,16 L5,16 Q3,16 3,14 L3,6 Q3,4 5,4";break;
    case "ellipse":path="M17,10 A7,6 0 1 1 3,10 A7,6 0 1 1 17,10";break;
    case "undo":path="M7,5 L3,9 7,13 M3,9 L12,9 Q17,9 17,14 L17,16";break;
    case "redo":path="M13,5 L17,9 13,13 M17,9 L8,9 Q3,9 3,14 L3,16";break;
    case "trash":path="M3,5 L17,5 M7,5 L7,3 13,3 13,5 M5,5 L6,18 14,18 15,5 M8,8 L8,14 M12,8 L12,14";break;
    case "clock":path="M18,10 A8,8 0 1 1 2,10 A8,8 0 1 1 18,10 M10,5 L10,10 13,12";break;
    case "reset":path="M4,7 A7,7 0 1 1 3,13 M4,3 L4,7 8,7";break;
    case "custom":path="M10,3 L10,17 M3,10 L17,10";break;
    case "prev":path="M12,6 L8,10 12,14";break;
    case "next":path="M8,6 L12,10 8,14";break;
    case "thin":case "medium":case "bold":case "thick":case "heavy":
     double size=name=="thin"?2:name=="medium"?3.5:name=="bold"?5:name=="thick"?6.5:8;
     dc.DrawEllipse(brush,null,new Point(10,10),size,size);return;
    case "board":case "blackboard":case "greenboard":
     if(name!="board")dc.DrawRectangle(new SolidColorBrush(name=="greenboard"?Colors.SeaGreen:Colors.Black),null,new Rect(3,4,14,11));
     path="M3,4 L17,4 17,15 3,15 Z M10,15 L10,18 M6,18 L14,18";break;
    case "screen":path="M3,8 L3,4 7,4 M13,4 L17,4 17,8 M17,12 L17,16 13,16 M7,16 L3,16 3,12 M8,8 L13,11 10,12 9,14 Z";break;
    case "pages":path="M6,3 L16,3 16,15 6,15 Z M4,6 L4,17 13,17";break;
    case "eye":case "eyeoff":
     path="M2,10 Q10,1 18,10 Q10,19 2,10 Z M12,10 A2,2 0 1 1 8,10 A2,2 0 1 1 12,10"+(name=="eyeoff"?" M3,3 L17,17":"");break;
    case "save":path="M4,3 L14,3 17,6 17,17 3,17 3,3 Z M7,3 L7,8 13,8 13,3 M6,17 L6,12 14,12 14,17";break;
    case "open":path="M3,7 L3,4 8,4 10,7 17,7 M3,7 L17,7 15,17 3,17 Z";break;
    case "export":path="M4,12 L4,17 16,17 16,12 M10,3 L10,13 M6,7 L10,3 14,7";break;
    case "image":path="M3,4 L17,4 17,16 3,16 Z M3,14 L8,9 12,13 14,11 17,14 M14,7 L14.1,7";break;
    case "zoom":path="M14,8 A6,6 0 1 1 2,8 A6,6 0 1 1 14,8 M12,12 L18,18 M5,8 L11,8 M8,5 L8,11";break;
    case "pdf":path="M5,2 L12,2 16,6 16,18 5,18 Z M12,2 L12,6 16,6 M8,10 L13,10 M8,14 L13,14";break;
    case "exit":path="M8,4 L3,4 3,17 8,17 M8,10 L18,10 M14,6 L18,10 14,14";break;
    case "settings":path="M8,3 L12,3 13,5 16,5 18,8 17,10 18,12 16,15 13,15 12,17 8,17 7,15 4,15 2,12 3,10 2,8 4,5 7,5 Z M13,10 A3,3 0 1 1 7,10 A3,3 0 1 1 13,10";break;
   }
   if(path.Length>0)dc.DrawGeometry(null,new Pen(brush,1.5){StartLineCap=PenLineCap.Round,EndLineCap=PenLineCap.Round,LineJoin=PenLineJoin.Round},Geometry.Parse(path));
   if(smallArrow)dc.Pop();
  }
 }
 public static class Program {
  [STAThread] public static void Main(string[] args) {
   if(args.Contains("--self-test")) {
    try{SelfTest.Run();}
    catch(Exception error){File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"self-test.txt"),Environment.NewLine+error);Environment.ExitCode=1;}
    return;
   }
   bool first;using(var instance=new System.Threading.Mutex(true,"Local\\ClassInk.Desktop.Singleton",out first)){
   if(!first)return;
   // Older releases predate the mutex. Do not create another orb alongside them.
   int current=System.Diagnostics.Process.GetCurrentProcess().Id;
   if(System.Diagnostics.Process.GetProcesses().Any(p=>p.Id!=current&&p.ProcessName.StartsWith("ClassInk",StringComparison.OrdinalIgnoreCase))){MessageBox.Show("请先从旧版工具栏点击退出，再启动本版本。","课堂笔 · 旧版正在运行");return;}
   var app = new Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
   app.DispatcherUnhandledException += (s,e)=>{ MessageBox.Show(e.Exception.Message,"课堂笔 · 操作未完成"); e.Handled=true; };
   app.Run(new Desk());
   }
  }
 }
 public static class Native {
  [DllImport("user32.dll")] public static extern int GetWindowLong(IntPtr h,int n);
  [DllImport("user32.dll")] public static extern int SetWindowLong(IntPtr h,int n,int v);
  [DllImport("user32.dll")] public static extern bool RegisterHotKey(IntPtr h,int id,uint mod,uint key);
  [DllImport("user32.dll")] public static extern bool UnregisterHotKey(IntPtr h,int id);
 }
 sealed class LaserRipples : FrameworkElement {
  readonly System.Diagnostics.Stopwatch pulseClock=new System.Diagnostics.Stopwatch();
  readonly System.Windows.Threading.DispatcherTimer animation=new System.Windows.Threading.DispatcherTimer();
  static readonly Brush ambient=Glow(0,45,68,32);
  static readonly Brush halo=Glow(0,36,152,24);
  static readonly Brush inner=Glow(120,220,248,180);
  Point point;
  bool visible;
  const double CycleSeconds=1.05;

  public LaserRipples(){
   IsHitTestVisible=false;
   animation.Interval=TimeSpan.FromMilliseconds(16);
   animation.Tick+=(s,e)=>InvalidateVisual();
  }
  static Brush Glow(byte center,byte middle,byte peak,byte edge){
   var brush=new RadialGradientBrush();
   brush.GradientStops.Add(new GradientStop(Color.FromArgb(center,255,42,57),0));
   brush.GradientStops.Add(new GradientStop(Color.FromArgb(middle,255,42,57),0.32));
   brush.GradientStops.Add(new GradientStop(Color.FromArgb(peak,255,42,57),0.52));
   brush.GradientStops.Add(new GradientStop(Color.FromArgb(edge,255,42,57),0.72));
   brush.GradientStops.Add(new GradientStop(Colors.Transparent,1));
   brush.Freeze();
   return brush;
  }
  public void Click(Point location){
   Move(location);
   pulseClock.Restart();
  }
  public void Move(Point location){
   point=location;
   if(!visible){visible=true;pulseClock.Restart();animation.Start();}
   InvalidateVisual();
  }
  public void End(){
   visible=false;animation.Stop();pulseClock.Stop();InvalidateVisual();
  }
  public void Clear(){End();}
  protected override void OnRender(DrawingContext dc){
   if(!visible)return;
   double phase=pulseClock.Elapsed.TotalSeconds%CycleSeconds/CycleSeconds;
   double radius=26+37*(1-Math.Pow(1-phase,1.65));
   dc.DrawEllipse(ambient,null,point,34,34);
   dc.PushOpacity(1-phase*0.87);
   dc.DrawEllipse(halo,null,point,radius,radius);
   dc.Pop();
   dc.DrawEllipse(inner,null,point,13,13);
   dc.DrawEllipse(Brushes.Red,null,point,9.5,9.5);
   dc.DrawEllipse(Brushes.MistyRose,null,point,3.6,3.6);
  }
 }
 sealed class MouseInkStroke {
  Point lastInput, lastOutput;
  public readonly Stroke InkStroke;
  public MouseInkStroke(Point start,DrawingAttributes attributes){
   lastInput=lastOutput=start;
   var points=new StylusPointCollection{new StylusPoint(start.X,start.Y)};
   var drawing=attributes.Clone();drawing.FitToCurve=false;
   InkStroke=new Stroke(points,drawing);
  }
  public void Append(Point point){
   if((point-lastInput).LengthSquared<0.64)return;
   var movement=point-lastInput;
   var smoothedEnd=point-movement*Math.Min(0.5,3/movement.Length);
   int segments=Math.Min(20,Math.Max(1,(int)Math.Ceiling(((lastInput-lastOutput).Length+(smoothedEnd-lastInput).Length)/4)));
   var batch=new StylusPointCollection(segments);
   for(int step=1;step<=segments;step++){
    double t=(double)step/segments,u=1-t;
    batch.Add(new StylusPoint(u*u*lastOutput.X+2*u*t*lastInput.X+t*t*smoothedEnd.X,
     u*u*lastOutput.Y+2*u*t*lastInput.Y+t*t*smoothedEnd.Y));
   }
   InkStroke.StylusPoints.Add(batch);
   lastInput=point;lastOutput=smoothedEnd;
  }
  public void Complete(Point? endpoint){
   if(endpoint.HasValue)Append(endpoint.Value);
   var end=endpoint??lastInput;
   if((end-lastOutput).LengthSquared>0.0001)InkStroke.StylusPoints.Add(new StylusPoint(end.X,end.Y));
  }
 }
 public sealed class Board : Window {
  public InkCanvas Ink = new InkCanvas();
  readonly LaserRipples laser=new LaserRipples();
  public string Tool="pen";
  public Color Color=Colors.Coral;
  public double Size=4;
  public bool White;
  public Action Changed = delegate {};
  Point start; Stroke preview; bool drawing;
  MouseInkStroke mouseInk;
  TextBox editor;
  Point textPosition;
  double textResizeScale=1;
  public string Surface {get;private set;}
  public bool AnnotationsHidden {get{return Ink.Opacity==0;}}
  public void SetAnnotationsHidden(bool hidden){Ink.Opacity=hidden?0:1;}
  public void FinishActiveInput(){
   if(mouseInk!=null)FinishMouseInk();
   if(drawing)Finish();
   FinishText();
  }
  static readonly Brush chalkboard=CreateChalkboard();
  public Board() {
   Surface="screen";
   Title="课堂笔 · 标注层";
   WindowStyle=WindowStyle.None; AllowsTransparency=true; Background=Brushes.Transparent;
   Topmost=true; ShowInTaskbar=false; ResizeMode=ResizeMode.NoResize;
   Left=SystemParameters.VirtualScreenLeft; Top=SystemParameters.VirtualScreenTop;
   Width=SystemParameters.VirtualScreenWidth; Height=SystemParameters.VirtualScreenHeight;
   Ink.Background=new SolidColorBrush(System.Windows.Media.Color.FromArgb(1,0,0,0));
   var layers=new Grid();layers.Children.Add(Ink);layers.Children.Add(laser);Content=layers;
   Ink.PreviewMouseLeftButtonDown+=Down; Ink.PreviewMouseMove+=Move; Ink.PreviewMouseLeftButtonUp+=Up;
   Ink.LostMouseCapture+=(s,e)=>{if(mouseInk!=null)FinishMouseInk();if(drawing)Finish();};
   Ink.MouseEnter+=(s,e)=>{if(Tool=="laser")laser.Move(e.GetPosition(Ink));};
   Ink.MouseLeave+=(s,e)=>laser.End();
   Ink.SelectionMoved+=(s,e)=>Changed();
   Ink.SelectionResizing+=(s,e)=>{
    textResizeScale=e.OldRectangle.Width>0&&e.OldRectangle.Height>0
     ?Math.Min(e.NewRectangle.Width/e.OldRectangle.Width,e.NewRectangle.Height/e.OldRectangle.Height):1;
   };
   Ink.SelectionResized+=(s,e)=>{
    var elements=Ink.GetSelectedElements();
    if(elements.Count==1&&Ink.GetSelectedStrokes().Count==0&&elements[0] is TextBlock){
     var text=(TextBlock)elements[0];
     text.FontSize=Math.Max(1,Math.Min(500,text.FontSize*textResizeScale));
     text.Width=double.NaN;text.Height=double.NaN;
    }
    Changed();
   };
   Ink.StrokeCollected+=(s,e)=>Dispatcher.BeginInvoke(new Action(()=>Changed()));
   Ink.StrokeErased+=(s,e)=>Dispatcher.BeginInvoke(new Action(()=>Changed()));
   PreviewKeyDown+=(s,e)=>{if(editor!=null){if(e.Key==Key.Escape){FinishText(false);e.Handled=true;}else if(e.Key==Key.Enter&&(Keyboard.Modifiers&ModifierKeys.Control)!=0){FinishText(true,true);e.Handled=true;}return;} if(e.Key==Key.Escape) { SetTool("mouse"); Changed(); } };
   ApplyPen();
  }
  public void ApplyPen() { Ink.DefaultDrawingAttributes=new DrawingAttributes { Color=Color, Width=Size, Height=Size, FitToCurve=true, IsHighlighter=Tool=="highlight" }; }
  public void SetTool(string tool) {
   if(mouseInk!=null)FinishMouseInk();
   if(Tool=="laser"&&tool!="laser")laser.Clear();
   FinishText();
   Tool=tool; ApplyPen();
   if(tool!="mouse")SetAnnotationsHidden(false);
   Ink.EditingMode=tool=="pen"||tool=="highlight"?InkCanvasEditingMode.Ink:tool=="erase"?InkCanvasEditingMode.EraseByStroke:tool=="select"?InkCanvasEditingMode.Select:InkCanvasEditingMode.None;
   var h=new WindowInteropHelper(this).Handle;
   int style=Native.GetWindowLong(h,-20); Native.SetWindowLong(h,-20,tool=="mouse"?style|0x20|0x8000000:style&~0x20&~0x8000000);
   Cursor=tool=="text"?Cursors.IBeam:tool=="mouse"?Cursors.Arrow:Cursors.Pen;
   if(tool!="mouse") Activate();
   if(tool=="laser"&&Ink.IsMouseOver)laser.Move(Mouse.GetPosition(Ink));
  }
  void Down(object s,MouseButtonEventArgs e) {
   if(Tool=="laser"){laser.Click(e.GetPosition(Ink));e.Handled=true;return;}
   if(editor!=null){if(editor.IsMouseOver)return;FinishText(true,true);e.Handled=true;return;}
   FinishText();
   if(e.StylusDevice==null&&(Tool=="pen"||Tool=="highlight")&&Ink.CaptureMouse()){
    mouseInk=new MouseInkStroke(e.GetPosition(Ink),Ink.DefaultDrawingAttributes);
    Ink.Strokes.Add(mouseInk.InkStroke);e.Handled=true;return;
   }
   if(Tool=="text") { e.Handled=true;BeginText(e.GetPosition(Ink));return; }
   if(Tool=="line"||Tool=="arrow"||Tool=="rect"||Tool=="ellipse") { start=e.GetPosition(Ink); drawing=true; Ink.CaptureMouse(); e.Handled=true; }
  }
  void Move(object s,MouseEventArgs e) {
   if(mouseInk!=null){
    var position=e.GetPosition(Ink);
    if(e.LeftButton==MouseButtonState.Pressed)mouseInk.Append(position);else FinishMouseInk(position);
    e.Handled=true;return;
   }
   if(Tool=="laser"){laser.Move(e.GetPosition(Ink));return;}
   if(!drawing) return; if(preview!=null) Ink.Strokes.Remove(preview);
   preview=Shape(Tool,start,e.GetPosition(Ink),Color,Size); Ink.Strokes.Add(preview);
  }
  void Up(object s,MouseButtonEventArgs e) {
   if(mouseInk!=null){FinishMouseInk(e.GetPosition(Ink));e.Handled=true;return;}
   if(Tool=="laser"){e.Handled=true;return;}
   if(drawing){Move(s,e);Finish();e.Handled=true;}
  }
  void FinishMouseInk(Point? endpoint=null){
   var stroke=mouseInk;mouseInk=null;
   stroke.Complete(endpoint);
   if(Ink.IsMouseCaptured)Ink.ReleaseMouseCapture();
   Changed();
  }
  void Finish() { drawing=false; preview=null; Ink.ReleaseMouseCapture(); Changed(); }
  public static Stroke Shape(string kind,Point a,Point b,Color color,double size) {
   var pts=new StylusPointCollection(); Action<double,double> add=(x,y)=>pts.Add(new StylusPoint(x,y));
   if(kind=="rect") { add(a.X,a.Y);add(b.X,a.Y);add(b.X,b.Y);add(a.X,b.Y);add(a.X,a.Y); }
   else if(kind=="ellipse") { for(int i=0;i<=96;i++) { double t=i*Math.PI/48; add((a.X+b.X)/2+Math.Abs(b.X-a.X)/2*Math.Cos(t),(a.Y+b.Y)/2+Math.Abs(b.Y-a.Y)/2*Math.Sin(t)); } }
   else { add(a.X,a.Y);add(b.X,b.Y); if(kind=="arrow") { double t=Math.Atan2(b.Y-a.Y,b.X-a.X),r=Math.Max(14,size*4); add(b.X-r*Math.Cos(t-.5),b.Y-r*Math.Sin(t-.5)); add(b.X,b.Y); add(b.X-r*Math.Cos(t+.5),b.Y-r*Math.Sin(t+.5)); } }
   return new Stroke(pts,new DrawingAttributes { Color=color, Width=size, Height=size });
  }
  public TextBlock AddText(string text,double x,double y,string color,double size) {
   var t=new TextBlock { Text=text, Foreground=(Brush)new BrushConverter().ConvertFromString(color), FontSize=size, FontFamily=new FontFamily("Microsoft YaHei UI"), Padding=new Thickness(3), Background=Brushes.Transparent };
   InkCanvas.SetLeft(t,x); InkCanvas.SetTop(t,y); Ink.Children.Add(t);
   t.MouseRightButtonDown+=(s,e)=>{ Ink.Children.Remove(t); Changed(); e.Handled=true; };
   return t;
  }
  public void BeginText(Point location){
   FinishText();textPosition=new Point(Math.Max(0,Math.Min(Width-80,location.X)),Math.Max(0,Math.Min(Height-48,location.Y)));
   editor=new TextBox {AcceptsReturn=true,FontFamily=new FontFamily("Microsoft YaHei UI"),FontSize=Math.Max(22,Size*4),Foreground=new SolidColorBrush(Color),CaretBrush=Brushes.DodgerBlue,Background=Brushes.Transparent,BorderBrush=Brushes.CornflowerBlue,BorderThickness=new Thickness(1),Padding=new Thickness(2),MinWidth=64,MaxWidth=Math.Max(64,Width-textPosition.X-4),HorizontalScrollBarVisibility=ScrollBarVisibility.Auto};
   InkCanvas.SetLeft(editor,textPosition.X);InkCanvas.SetTop(editor,textPosition.Y);Ink.Children.Add(editor);
   var active=editor;active.LostKeyboardFocus+=(s,e)=>{if(editor==active)FinishText();};
   active.Focus();Keyboard.Focus(active);
  }
  public void FinishText(bool commit=true,bool select=false){
   if(editor==null)return;var active=editor;editor=null;
   string text=active.Text;Ink.Children.Remove(active);
   if(commit&&!String.IsNullOrWhiteSpace(text)){
    var label=AddText(text,textPosition.X,textPosition.Y,active.Foreground.ToString(),active.FontSize);
    if(select){SetTool("select");Ink.Select(new UIElement[]{label});}
    Changed();
   }
  }
  public void SetWhite(bool value){SetSurface(value?"white":"screen");}
  public void SetSurface(string surface){
   Surface=surface;White=surface=="white";
   switch(surface){
    case "white":Ink.Background=Brushes.White;break;
    case "black":Ink.Background=new SolidColorBrush(System.Windows.Media.Color.FromRgb(20,22,24));break;
    case "green":Ink.Background=chalkboard;break;
    default:Surface="screen";Ink.Background=new SolidColorBrush(System.Windows.Media.Color.FromArgb(1,0,0,0));break;
   }
  }
  static Brush CreateChalkboard(){
   var drawing=new DrawingGroup();
   using(var dc=drawing.Open()){
    dc.DrawRectangle(new SolidColorBrush(System.Windows.Media.Color.FromRgb(27,66,53)),null,new Rect(0,0,192,192));
    var grain=new Random(31);
    for(int i=0;i<650;i++){
     var brush=new SolidColorBrush(System.Windows.Media.Color.FromArgb((byte)grain.Next(4,21),210,235,217));
     dc.DrawEllipse(brush,null,new Point(grain.NextDouble()*192,grain.NextDouble()*192),grain.NextDouble()*1.8+0.3,0.55);
    }
   }
   var tile=new DrawingBrush(drawing){TileMode=TileMode.Tile,ViewportUnits=BrushMappingMode.Absolute,Viewport=new Rect(0,0,192,192),ViewboxUnits=BrushMappingMode.Absolute,Viewbox=new Rect(0,0,192,192)};
   tile.Freeze();return tile;
  }
 }
 sealed class ScreenView : Window {
  readonly BitmapSource screenshot;
  readonly InkCanvas canvas=new InkCanvas();
  readonly Canvas overlay=new Canvas{IsHitTestVisible=false};
  readonly System.Windows.Shapes.Rectangle selection=new System.Windows.Shapes.Rectangle{Stroke=Brushes.DeepSkyBlue,StrokeThickness=2,Fill=new SolidColorBrush(Color.FromArgb(30,30,160,255))};
  readonly MatrixTransform transform=new MatrixTransform();
  readonly bool crop;
  Point start,offset,panOrigin;
  bool dragging;
  double scale=1;
  double targetScale=1, fromScale=1;
  Point targetOffset, fromOffset;
  readonly System.Diagnostics.Stopwatch zoomClock=new System.Diagnostics.Stopwatch();
  readonly System.Windows.Threading.DispatcherTimer zoomTimer=new System.Windows.Threading.DispatcherTimer{Interval=TimeSpan.FromMilliseconds(16)};
  public BitmapSource Result {get;private set;}
  public ScreenView(BitmapSource screenshot,Board board,bool crop){
   this.screenshot=screenshot;this.crop=crop;
   Left=board.Left;Top=board.Top;Width=board.Width;Height=board.Height;
   WindowStyle=WindowStyle.None;ResizeMode=ResizeMode.NoResize;Topmost=true;ShowInTaskbar=false;Background=Brushes.Black;
   var viewport=new Grid{ClipToBounds=true};Content=viewport;
   canvas.Background=new ImageBrush(screenshot){Stretch=Stretch.Fill};canvas.RenderTransform=transform;
   canvas.DefaultDrawingAttributes=board.Ink.DefaultDrawingAttributes.Clone();canvas.DefaultDrawingAttributes.IsHighlighter=false;
   canvas.EditingMode=crop?InkCanvasEditingMode.None:InkCanvasEditingMode.Ink;
   viewport.Children.Add(canvas);viewport.Children.Add(overlay);overlay.Children.Add(selection);selection.Visibility=Visibility.Collapsed;
   var tip=new Border{Background=new SolidColorBrush(Color.FromArgb(229,24,33,49)),CornerRadius=new CornerRadius(8),Padding=new Thickness(12,8,12,8),Margin=new Thickness(16),HorizontalAlignment=HorizontalAlignment.Center,VerticalAlignment=VerticalAlignment.Top,IsHitTestVisible=false};
   tip.Child=new TextBlock{Foreground=Brushes.White,Text=crop?"拖动框选截图并贴到板书 · Esc 取消":"滚轮缩放 · 右键平移 · 左键写画 · B 蓝 / R 红 / G 绿 · E 清除 · Esc 退出"};viewport.Children.Add(tip);
   zoomTimer.Tick+=(s,e)=>{
    double progress=Math.Min(1,zoomClock.Elapsed.TotalMilliseconds/155);
    double eased=1-Math.Pow(1-progress,3);
    scale=fromScale+(targetScale-fromScale)*eased;
    offset=fromOffset+(targetOffset-fromOffset)*eased;
    ApplyTransform();
    if(progress>=1)zoomTimer.Stop();
   };
   Closed+=(s,e)=>zoomTimer.Stop();
   Loaded+=(s,e)=>{Activate();if(!crop)ZoomAt(Mouse.GetPosition(viewport),2);};
   PreviewKeyDown+=(s,e)=>{
    if(e.Key==Key.Escape){Close();e.Handled=true;}
    else if(!crop&&e.Key==Key.Z&&(Keyboard.Modifiers&ModifierKeys.Control)!=0&&canvas.Strokes.Count>0){canvas.Strokes.RemoveAt(canvas.Strokes.Count-1);e.Handled=true;}
    else if(!crop&&Keyboard.Modifiers==ModifierKeys.None){
     switch(e.Key){
      case Key.B:canvas.DefaultDrawingAttributes.Color=Colors.DodgerBlue;break;
      case Key.R:canvas.DefaultDrawingAttributes.Color=Colors.Red;break;
      case Key.G:canvas.DefaultDrawingAttributes.Color=Colors.LimeGreen;break;
      case Key.E:canvas.Strokes.Clear();break;
      default:return;
     }
     e.Handled=true;
    }
   };
   viewport.PreviewMouseWheel+=(s,e)=>{if(!crop&&!dragging&&Mouse.LeftButton!=MouseButtonState.Pressed){ZoomAt(e.GetPosition(viewport),targetScale*Math.Pow(1.25,e.Delta/120.0));e.Handled=true;}};
   viewport.PreviewMouseDown+=(s,e)=>{
    zoomTimer.Stop();targetScale=scale;
    if(e.ChangedButton!=(crop?MouseButton.Left:MouseButton.Right))return;
    start=e.GetPosition(viewport);panOrigin=offset;dragging=viewport.CaptureMouse();e.Handled=true;
   };
   viewport.LostMouseCapture+=(s,e)=>{dragging=false;};
   viewport.PreviewMouseMove+=(s,e)=>{
    if(!dragging)return;
    Point point=e.GetPosition(viewport);
    if(crop){var rect=Region(point);Canvas.SetLeft(selection,rect.X);Canvas.SetTop(selection,rect.Y);selection.Width=rect.Width;selection.Height=rect.Height;selection.Visibility=Visibility.Visible;}
    else{offset=panOrigin+(point-start);ApplyTransform();}
    e.Handled=true;
   };
   viewport.PreviewMouseUp+=(s,e)=>{
    if(!dragging||e.ChangedButton!=(crop?MouseButton.Left:MouseButton.Right))return;
    dragging=false;viewport.ReleaseMouseCapture();e.Handled=true;
    if(!crop)return;
    Rect region=Region(e.GetPosition(viewport));
    if(region.Width<4||region.Height<4){selection.Visibility=Visibility.Collapsed;return;}
    int x=(int)(region.Left*screenshot.PixelWidth/ActualWidth),y=(int)(region.Top*screenshot.PixelHeight/ActualHeight);
    int right=Math.Min(screenshot.PixelWidth,(int)Math.Ceiling(region.Right*screenshot.PixelWidth/ActualWidth));
    int bottom=Math.Min(screenshot.PixelHeight,(int)Math.Ceiling(region.Bottom*screenshot.PixelHeight/ActualHeight));
    var bitmap=new CroppedBitmap(screenshot,new Int32Rect(x,y,right-x,bottom-y));bitmap.Freeze();Result=bitmap;Close();
   };
  }
  Rect Region(Point end){return new Rect(start,new Point(Math.Max(0,Math.Min(ActualWidth,end.X)),Math.Max(0,Math.Min(ActualHeight,end.Y))));}
  void ZoomAt(Point anchor,double requested){
   double next=Math.Max(1,Math.Min(8,requested)),ratio=next/scale;
   fromScale=scale;fromOffset=offset;targetScale=next;
   var requestedOffset=anchor+(offset-anchor)*ratio;
   targetOffset=new Point(Math.Min(0,Math.Max(ActualWidth*(1-next),requestedOffset.X)),Math.Min(0,Math.Max(ActualHeight*(1-next),requestedOffset.Y)));
   zoomClock.Restart();zoomTimer.Start();
  }
  void ApplyTransform(){
   offset=new Point(Math.Min(0,Math.Max(ActualWidth*(1-scale),offset.X)),Math.Min(0,Math.Max(ActualHeight*(1-scale),offset.Y)));
   transform.Matrix=new Matrix(scale,0,0,scale,offset.X,offset.Y);
   canvas.DefaultDrawingAttributes.Width=Math.Max(0.1,3/scale);canvas.DefaultDrawingAttributes.Height=canvas.DefaultDrawingAttributes.Width;
  }
 }
 public static class Document {
  public static string Save(Board b,bool blank=false) {
   var doc=new XmlDocument(); var root=doc.CreateElement("ClassInk"); root.SetAttribute("version","1");root.SetAttribute("white",b.White.ToString());root.SetAttribute("surface",b.Surface);doc.AppendChild(root);
   var ink=doc.CreateElement("Strokes"); using(var ms=new MemoryStream()) { (blank?new StrokeCollection():b.Ink.Strokes).Save(ms);ink.InnerText=Convert.ToBase64String(ms.ToArray()); }root.AppendChild(ink);
   if(!blank)foreach(TextBlock t in b.Ink.Children.OfType<TextBlock>()) { var n=doc.CreateElement("Text");n.SetAttribute("x",InkCanvas.GetLeft(t).ToString(CultureInfo.InvariantCulture));n.SetAttribute("y",InkCanvas.GetTop(t).ToString(CultureInfo.InvariantCulture));n.SetAttribute("size",t.FontSize.ToString(CultureInfo.InvariantCulture));n.SetAttribute("color",t.Foreground.ToString());n.InnerText=t.Text;root.AppendChild(n); }
   if(!blank)foreach(var image in b.Ink.Children.OfType<Image>()){
    var node=doc.CreateElement("Image");
    foreach(var property in new[]{"x","y","width","height"}){
     double value=property=="x"?InkCanvas.GetLeft(image):property=="y"?InkCanvas.GetTop(image):property=="width"?image.Width:image.Height;
     node.SetAttribute(property,value.ToString(CultureInfo.InvariantCulture));
    }
    node.InnerText=(string)image.Tag;root.AppendChild(node);
   }
   return doc.OuterXml;
  }
  internal static Image Picture(BitmapSource bitmap,string png,double x,double y,double width,double height){
   var image=new Image{Source=bitmap,Tag=png,Width=width,Height=height,Stretch=Stretch.Fill};
   InkCanvas.SetLeft(image,x);InkCanvas.SetTop(image,y);return image;
  }
  internal static XmlElement ReadRoot(string xml){
   var doc=new XmlDocument { XmlResolver=null }; using(var reader=XmlReader.Create(new StringReader(xml),new XmlReaderSettings { DtdProcessing=DtdProcessing.Prohibit, MaxCharactersInDocument=80000000, XmlResolver=null })) doc.Load(reader);
   return doc.DocumentElement;
  }
  public static void Load(Board b,string xml){LoadPage(b,ReadRoot(xml));}
  internal static void LoadPage(Board b,XmlElement root){
   if(root.Name!="ClassInk"||root.GetAttribute("version")!="1"||root["Strokes"]==null)throw new InvalidDataException("不是支持的课堂笔文件。");
   StrokeCollection strokes; using(var ms=new MemoryStream(Convert.FromBase64String(root["Strokes"].InnerText))) strokes=new StrokeCollection(ms);
   var texts=new List<Tuple<string,double,double,string,double>>();
   foreach(XmlElement n in root.SelectNodes("Text")) { double x=Double.Parse(n.GetAttribute("x"),CultureInfo.InvariantCulture),y=Double.Parse(n.GetAttribute("y"),CultureInfo.InvariantCulture),size=Double.Parse(n.GetAttribute("size"),CultureInfo.InvariantCulture); if(Double.IsNaN(x)||Double.IsInfinity(x)||Double.IsNaN(y)||Double.IsInfinity(y)||size<1||size>500||Double.IsNaN(size)) throw new InvalidDataException("文字数据无效。"); new BrushConverter().ConvertFromString(n.GetAttribute("color")); texts.Add(Tuple.Create(n.InnerText,x,y,n.GetAttribute("color"),size)); }
   var pictures=new List<Image>();
   foreach(XmlElement node in root.SelectNodes("Image")){
    var bounds=new[]{"x","y","width","height"}.Select(key=>Double.Parse(node.GetAttribute(key),CultureInfo.InvariantCulture)).ToArray();
    if(bounds.Any(n=>Double.IsInfinity(n)||Double.IsNaN(n))||bounds[2]<=0||bounds[3]<=0)throw new InvalidDataException("截图尺寸无效。");
    string png=node.InnerText;
    using(var stream=new MemoryStream(Convert.FromBase64String(png))){
     var bitmap=BitmapFrame.Create(stream,BitmapCreateOptions.None,BitmapCacheOption.OnLoad);bitmap.Freeze();
     pictures.Add(Picture(bitmap,png,bounds[0],bounds[1],bounds[2],bounds[3]));
    }
   }
   if(b==null)return;
   b.FinishText(false);b.Ink.Select(new UIElement[0]);b.Ink.Strokes=strokes; b.Ink.Children.Clear();
   foreach(var picture in pictures)b.Ink.Children.Add(picture);
   foreach(var t in texts)b.AddText(t.Item1,t.Item2,t.Item3,t.Item4,t.Item5);
   if(root.HasAttribute("surface"))b.SetSurface(root.GetAttribute("surface"));else b.SetWhite(root.GetAttribute("white")=="True");
  }
 }
 sealed class LessonDocument {
  public readonly string[] Pages;
  public readonly int ActivePage;
  public readonly string[] Titles;
  public LessonDocument(string[] pages,int activePage,string[] titles=null){Pages=pages;ActivePage=activePage;Titles=titles??pages.Select(page=>"").ToArray();}
  public string Save(){
   var xml=new System.Text.StringBuilder();
   xml.Append("<ClassInkLesson version=\"1\" active=\"").Append(ActivePage.ToString(CultureInfo.InvariantCulture)).Append("\">");
   for(int i=0;i<Pages.Length;i++){
    if(String.IsNullOrEmpty(Titles[i])){xml.Append(Pages[i]);continue;}
    var page=Document.ReadRoot(Pages[i]);page.SetAttribute("title",Titles[i]);xml.Append(page.OuterXml);
   }
   return xml.Append("</ClassInkLesson>").ToString();
  }
  public static LessonDocument Load(string xml){
   var root=Document.ReadRoot(xml);
   if(root.Name=="ClassInk"){
    Document.LoadPage(null,root);
    return new LessonDocument(new[]{root.OuterXml},0);
   }
   int active;
   var pages=root.ChildNodes.OfType<XmlElement>().ToArray();
   if(root.Name!="ClassInkLesson"||root.GetAttribute("version")!="1"||!Int32.TryParse(root.GetAttribute("active"),out active)||active<0||active>=pages.Length)
    throw new InvalidDataException("不是支持的课堂笔课程文件。");
   var titles=new string[pages.Length];
   for(int i=0;i<pages.Length;i++){
    Document.LoadPage(null,pages[i]);titles[i]=pages[i].GetAttribute("title");pages[i].RemoveAttribute("title");
   }
   return new LessonDocument(pages.Select(page=>page.OuterXml).ToArray(),active,titles);
  }
 }
 sealed class BoardExporter : IDisposable {
  readonly Board board=new Board();
  public BitmapSource Render(string snapshot,int maxWidth,int maxHeight,bool whiteBackground){
   Document.Load(board,snapshot);
   double width=board.Width,height=board.Height;
   double scale=Math.Min(1,Math.Min(maxWidth/width,maxHeight/height));
   int pixelWidth=Math.Max(1,(int)Math.Round(width*scale));
   int pixelHeight=Math.Max(1,(int)Math.Round(height*scale));
   if(board.Surface=="screen")board.Ink.Background=whiteBackground?Brushes.White:Brushes.Transparent;
   board.Ink.Measure(new Size(width,height));board.Ink.Arrange(new Rect(0,0,width,height));board.Ink.UpdateLayout();
   var scene=new DrawingVisual();
   using(var dc=scene.RenderOpen())dc.DrawRectangle(new VisualBrush(board.Ink){Stretch=Stretch.Fill},null,new Rect(0,0,pixelWidth,pixelHeight));
   var bitmap=new RenderTargetBitmap(pixelWidth,pixelHeight,96,96,PixelFormats.Pbgra32);
   bitmap.Render(scene);bitmap.Freeze();return bitmap;
  }
  public void Dispose(){board.Close();}
  public void WritePdf(Stream output,LessonDocument lesson){
   var offsets=new List<long>{0};
   Action<string> write=value=>{var bytes=System.Text.Encoding.ASCII.GetBytes(value);output.Write(bytes,0,bytes.Length);};
   Action<int,string> obj=(id,body)=>{offsets.Add(output.Position);write(id+" 0 obj\n"+body+"\nendobj\n");};
   write("%PDF-1.4\n");
   obj(1,"<< /Type /Catalog /Pages 2 0 R >>");
   string children=String.Join(" ",Enumerable.Range(0,lesson.Pages.Length).Select(i=>(4+i*3)+" 0 R"));
   obj(2,"<< /Type /Pages /Count "+lesson.Pages.Length+" /Kids ["+children+"] >>");
   obj(3,"<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>");
   for(int i=0;i<lesson.Pages.Length;i++){
    var bitmap=Render(lesson.Pages[i],1600,1100,true);
    var encoder=new JpegBitmapEncoder{QualityLevel=92};
    encoder.Frames.Add(BitmapFrame.Create(new FormatConvertedBitmap(bitmap,PixelFormats.Bgr24,null,0)));
    byte[] jpeg;using(var buffer=new MemoryStream()){encoder.Save(buffer);jpeg=buffer.ToArray();}
    int page=4+i*3,image=page+1,contents=page+2;
    obj(page,"<< /Type /Page /Parent 2 0 R /MediaBox [0 0 842 595] /Resources << /XObject << /Board "+image+" 0 R >> /Font << /F1 3 0 R >> >> /Contents "+contents+" 0 R >>");
    offsets.Add(output.Position);
    write(image+" 0 obj\n<< /Type /XObject /Subtype /Image /Width "+bitmap.PixelWidth+" /Height "+bitmap.PixelHeight+" /ColorSpace /DeviceRGB /BitsPerComponent 8 /Filter /DCTDecode /Length "+jpeg.Length+" >>\nstream\n");
    output.Write(jpeg,0,jpeg.Length);write("\nendstream\nendobj\n");
    double scale=Math.Min(794.0/bitmap.PixelWidth,535.0/bitmap.PixelHeight);
    double w=bitmap.PixelWidth*scale,h=bitmap.PixelHeight*scale;
    string paint=String.Format(CultureInfo.InvariantCulture,"q {0:F3} 0 0 {1:F3} {2:F3} {3:F3} cm /Board Do Q\nBT /F1 10 Tf 24 17 Td (ClassInk  |  {4} / {5}) Tj ET\n",w,h,(842-w)/2,32+(535-h)/2,i+1,lesson.Pages.Length);
    obj(contents,"<< /Length "+paint.Length+" >>\nstream\n"+paint+"endstream");
   }
   long xref=output.Position;
   write("xref\n0 "+offsets.Count+"\n0000000000 65535 f \n");
   foreach(long offset in offsets.Skip(1))write(offset.ToString("0000000000",CultureInfo.InvariantCulture)+" 00000 n \n");
   write("trailer\n<< /Size "+offsets.Count+" /Root 1 0 R >>\nstartxref\n"+xref+"\n%%EOF\n");
  }
 }
 sealed class CountdownAudio : IDisposable {
  public static readonly string[] Names={"清脆铃声","柔和双音","电子提示"};
  static readonly int[][] WarningNotes={new[]{880},new[]{523,659},new[]{1047,1047}};
  static readonly int[][] EndNotes={new[]{659,880,1175},new[]{392,523,659},new[]{784,1047,1568}};
  System.Media.SoundPlayer player;
  MemoryStream sound;
  public void Play(int profile,bool finished){
   Dispose();
   sound=new MemoryStream();
   var notes=(finished?EndNotes:WarningNotes)[profile];
   const int sampleRate=16000;
   int samplesPerNote=finished?7200:profile==1?2720:profile==2?1520:2240;
   int gap=finished?640:profile==2?720:240;
   int sampleCount=notes.Length*(samplesPerNote+gap);
   var writer=new BinaryWriter(sound);
   writer.Write(new[]{(byte)'R',(byte)'I',(byte)'F',(byte)'F'});writer.Write(36+sampleCount*2);
   writer.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt "));
   writer.Write(16);writer.Write((short)1);writer.Write((short)1);writer.Write(sampleRate);
   writer.Write(sampleRate*2);writer.Write((short)2);writer.Write((short)16);
   writer.Write(System.Text.Encoding.ASCII.GetBytes("data"));writer.Write(sampleCount*2);
   foreach(int note in notes){
    for(int i=0;i<samplesPerNote;i++){
     double phase=2*Math.PI*note*i/sampleRate;
     double progress=(double)i/samplesPerNote;
     double envelope=Math.Min(1,i/80.0)*Math.Pow(1-progress,1.5);
     double tone=Math.Sin(phase)+(profile==0?0.2*Math.Sin(phase*2):0);
     writer.Write((short)(8500*envelope*tone));
    }
    for(int i=0;i<gap;i++)writer.Write((short)0);
   }
   writer.Flush();sound.Position=0;
   player=new System.Media.SoundPlayer(sound);
   player.Load();player.Play();
  }
  public void Dispose(){
   if(player!=null){player.Stop();player.Dispose();player=null;}
   if(sound!=null){sound.Dispose();sound=null;}
  }
 }
 public sealed class Desk : Window {
  Board board=new Board(); StackPanel panel; Border card; Button orb; TextBlock status; bool collapsed; bool exiting; bool saved=true;
  sealed class PageState {
   public readonly List<string> History=new List<string>();
   public int Index;
   public string Title="";
   string previewSnapshot;
   BitmapSource preview;
   public PageState(string initial=null){if(initial!=null)History.Add(initial);}
   public string Snapshot {get{return History[Index];}}
   public BitmapSource Thumbnail(){
    if(previewSnapshot!=Snapshot){using(var exporter=new BoardExporter())preview=exporter.Render(Snapshot,112,64,true);previewSnapshot=Snapshot;}
    return preview;
   }
  }
  sealed class PagePreview {
   readonly PageState page;
   public string Name {get;private set;}
   public BitmapSource Image {get{return page.Thumbnail();}}
   public PagePreview(PageState page,string name){this.page=page;Name=name;}
  }
  readonly List<PageState> pages=new List<PageState>{new PageState()};
  int pageIndex;
  PageState deletedPage;
  int deletedPageIndex;
  string[] savedPages=new string[0];
  string[] savedTitles=new string[0];
  List<string> history {get{return pages[pageIndex].History;}}
  int index {get{return pages[pageIndex].Index;}set{pages[pageIndex].Index=value;}}
  TextBlock pageNumber;
  Button pageButton, visibilityButton;
  readonly Dictionary<string,Button> tools=new Dictionary<string,Button>();
  static readonly string[] boardSurfaces={"screen","white","black","green"};
  int themeIndex=ReadTheme();
  string saveDirectory=ReadSaveDirectory();
  static string SaveDirectoryPreference(){return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"ClassInk","save-directory.txt");}
  static string ReadSaveDirectory(){
   try{string path=File.ReadAllText(SaveDirectoryPreference()).Trim();if(Directory.Exists(path))return path;}
   catch(IOException){}catch(UnauthorizedAccessException){}
   return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
  }
  string DefaultFolder(){return Directory.Exists(saveDirectory)?saveDirectory:Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);}
  void ShowFileSettings(Button anchor){
   var box=new StackPanel{Width=300,Margin=new Thickness(5)};
   box.Children.Add(new TextBlock{Text="板书默认保存目录",Foreground=UiInk,Margin=new Thickness(3,4,3,5)});
   var location=new TextBlock{Text=DefaultFolder(),Foreground=UiInk,TextWrapping=TextWrapping.Wrap,ToolTip=DefaultFolder(),Margin=new Thickness(3,0,3,7)};
   box.Children.Add(location);
   var actions=new StackPanel{Orientation=Orientation.Horizontal};box.Children.Add(actions);
   actions.Children.Add(Button("选择文件夹",()=>{
    flyout.IsOpen=false;
    using(var picker=new System.Windows.Forms.FolderBrowserDialog{Description="选择默认板书保存目录",SelectedPath=DefaultFolder(),ShowNewFolderButton=true}){
     if(picker.ShowDialog(new DialogOwner(new WindowInteropHelper(this).Handle))!=System.Windows.Forms.DialogResult.OK)return;
     var preference=SaveDirectoryPreference();Directory.CreateDirectory(Path.GetDirectoryName(preference));
     AtomicWrite(preference,picker.SelectedPath);saveDirectory=picker.SelectedPath;
    }
    ShowFileSettings(anchor);
   }));
   actions.Children.Add(Button("打开保存目录",()=>{
    var path=DefaultFolder();if(!Directory.Exists(path))throw new DirectoryNotFoundException("默认保存目录不可用，请重新选择。");
    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(path){UseShellExecute=true});
   }));
   OpenFlyout(anchor,box);
  }
  static string ThemePreferencePath(){return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"ClassInk","theme.txt");}
  static int ReadTheme(){
   try{int value;if(Int32.TryParse(File.ReadAllText(ThemePreferencePath()),out value)&&value>=0&&value<themeNames.Length)return value;}
   catch(IOException){}catch(UnauthorizedAccessException){}
   return 3;
  }
  readonly CountdownAudio countdownAudio=new CountdownAudio();
  int soundProfile=ReadSoundProfile();
  static string SoundPreferencePath(){return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"ClassInk","countdown-sound.txt");}
  static int ReadSoundProfile(){
   try{int value;if(Int32.TryParse(File.ReadAllText(SoundPreferencePath()),out value)&&value>=0&&value<CountdownAudio.Names.Length)return value;}
   catch(IOException){}catch(UnauthorizedAccessException){}
   return 0;
  }
  void SelectSoundProfile(int profile){
   string path=SoundPreferencePath();Directory.CreateDirectory(Path.GetDirectoryName(path));
   AtomicWrite(path,profile.ToString(CultureInfo.InvariantCulture));
   soundProfile=profile;
  }
  static readonly string[] themeNames={"白色","黑色","深海青","演示栏"};
  static readonly string[] themeSurfaces={"#FAFFFFFF","#FF171C25","#FF13263B","#FF303038"};
  Brush UiInk {get{return B(themeIndex==0?"#526077":"#F0F4FA");}}
  Brush ActiveInk {get{return B(themeIndex==0?"#356DE6":themeIndex==1?"#BDD5FF":themeIndex==2?"#70EDCF":"#FFE080");}}
  Brush ActiveFill {get{return B(themeIndex==0?"#EAF1FF":themeIndex==1?"#634E80D0":themeIndex==2?"#234C55":"#635131");}}
  Brush UiBorder {get{return B(themeIndex==0?"#DEE4EC":themeIndex==1?"#6679879C":themeIndex==2?"#416579":"#5B5C69");}}
  void ShowSettings(Button anchor){
   var settings=new StackPanel();
   var row=new StackPanel {Orientation=Orientation.Horizontal};settings.Children.Add(row);
   for(int i=0;i<themeNames.Length;i++){
    int choice=i;
    var button=Button(themeNames[i],()=>{
     string path=ThemePreferencePath();Directory.CreateDirectory(Path.GetDirectoryName(path));
     AtomicWrite(path,choice.ToString(CultureInfo.InvariantCulture));
     flyout.IsOpen=false;ApplyTheme(choice);
    });
    button.ToolTip="应用并设为启动默认皮肤";
    button.Width=50;
    if(i==themeIndex){button.Background=ActiveFill;button.Foreground=ActiveInk;}
    row.Children.Add(button);
   }
   var actions=new StackPanel{Orientation=Orientation.Horizontal};settings.Children.Add(actions);
   actions.Children.Add(Button("计时音效",()=>ShowSoundSettings(anchor)));
   actions.Children.Add(Button("文件管理",()=>ShowFileSettings(anchor)));
   actions.Children.Add(Button("打赏支持",ShowSupport));
   actions.Children.Add(Button("快捷键",ShowShortcuts));
   OpenFlyout(anchor,settings);
  }
  sealed class Shortcut {
   public uint Key;public string Label,Description;public Action Run;
   public Shortcut(uint key,string label,string description,Action run){Key=key;Label=label;Description=description;Run=run;}
  }
  Shortcut[] shortcuts;
  readonly HashSet<int> registeredShortcuts=new HashSet<int>();
  Shortcut[] CreateShortcuts(){return new[]{
   new Shortcut(0x44,"D","鼠标 / 画笔",()=>ActivateTool(board.Tool=="mouse"?"pen":"mouse")),
   new Shortcut(0x48,"H","隐藏 / 恢复板书",ToggleAnnotations),
   new Shortcut(0x21,"PageUp","上一页",()=>SwitchPage(pageIndex-1)),
   new Shortcut(0x22,"PageDown","下一页",()=>SwitchPage(pageIndex+1)),
   new Shortcut(0x31,"1","屏幕缩放 / 退出缩放",()=>OpenScreenView(false)),
   new Shortcut(0x50,"P","画笔",()=>ActivateTool("pen")),
   new Shortcut(0x4C,"L","激光笔",()=>ActivateTool("laser")),
   new Shortcut(0x4D,"M","荧光笔",()=>ActivateTool("highlight")),
   new Shortcut(0x41,"A","箭头",()=>ActivateTool("arrow")),
   new Shortcut(0x52,"R","矩形",()=>ActivateTool("rect")),
   new Shortcut(0x54,"T","文字",()=>ActivateTool("text")),
   new Shortcut(0x56,"V","选择 / 移动",()=>ActivateTool("select")),
   new Shortcut(0x45,"E","橡皮擦",()=>ActivateTool("erase")),
   new Shortcut(0x57,"W","切换板面",()=>{board.SetSurface(boardSurfaces[(Array.IndexOf(boardSurfaces,board.Surface)+1)%boardSurfaces.Length]);Commit();}),
   new Shortcut(0x43,"C","框选截图贴到板书",()=>OpenScreenView(true)),
   new Shortcut(0x53,"S","保存课程",Save),
   new Shortcut(0x4F,"O","打开课程",Open),
   new Shortcut(0x4E,"N","新建板书页",NewPage),
   new Shortcut(0x5A,"Z","撤销",()=>Travel(-1)),
   new Shortcut(0x59,"Y","重做",()=>Travel(1)),
   new Shortcut(0x4B,"K","打开计时器",ShowClock)
  };}
  void ShowShortcuts(){
   if(flyout!=null)flyout.IsOpen=false;
   var rows=new StackPanel{Margin=new Thickness(18)};
   rows.Children.Add(new TextBlock{Text="课堂笔快捷键",FontSize=19,FontWeight=FontWeights.SemiBold,Foreground=UiInk,Margin=new Thickness(0,0,0,10)});
   for(int i=0;i<shortcuts.Length;i++){
    var item=shortcuts[i];
    string keys=(i==4?"Ctrl + ":"Ctrl + Alt + ")+item.Label;
    rows.Children.Add(new TextBlock{Text=keys+"    "+item.Description+(registeredShortcuts.Contains(i+1)?"":"  （未注册）"),Foreground=UiInk,Margin=new Thickness(0,3,0,3)});
   }
   rows.Children.Add(new TextBlock{Text="缩放中：B 蓝笔 · R 红笔 · G 绿笔 · E 清空临时笔迹\nCtrl+Z 撤销 · Esc 退出缩放",Foreground=ActiveInk,Margin=new Thickness(0,12,0,0)});
   var dialog=new Window{Title="快捷键说明",Owner=this,ShowInTaskbar=false,Topmost=true,Width=470,Height=Math.Min(640,SystemParameters.WorkArea.Height-50),WindowStartupLocation=WindowStartupLocation.CenterScreen,Background=B(themeSurfaces[themeIndex]),Content=new ScrollViewer{Content=rows,VerticalScrollBarVisibility=ScrollBarVisibility.Auto}};
   dialog.PreviewKeyDown+=(s,e)=>{if(e.Key==Key.Escape)dialog.Close();};dialog.ShowDialog();
  }
  void ShowSupport(){
   if(flyout!=null)flyout.IsOpen=false;
   var row=new StackPanel{Orientation=Orientation.Horizontal,HorizontalAlignment=HorizontalAlignment.Center};
   var panel=new StackPanel{Margin=new Thickness(18)};
   panel.Children.Add(new TextBlock{Text="请用支付宝或微信扫描对应二维码，自愿支持课堂笔",FontSize=14,Foreground=UiInk,Margin=new Thickness(4,0,4,12)});
   panel.Children.Add(row);
   for(int i=1;i<=2;i++){
    using(var stream=typeof(Desk).Assembly.GetManifestResourceStream("ClassInk.Support"+i+".jpg")){
     if(stream==null)throw new InvalidOperationException("打赏图片资源缺失。");
     var card=new Border{Background=Brushes.White,Padding=new Thickness(8),Margin=new Thickness(4)};
     var contents=new StackPanel();
     contents.Children.Add(new TextBlock{Text=i==1?"支付宝":"微信",Foreground=Brushes.Black,FontSize=15,FontWeight=FontWeights.SemiBold,HorizontalAlignment=HorizontalAlignment.Center,Margin=new Thickness(0,2,0,8)});
     contents.Children.Add(new Image{Source=BitmapFrame.Create(stream,BitmapCreateOptions.None,BitmapCacheOption.OnLoad),Width=205,Height=205,Stretch=Stretch.Uniform});
     card.Child=contents;row.Children.Add(card);
    }
   }
   var window=new Window{Title="打赏支持 · 课堂笔",Owner=this,ShowInTaskbar=false,Topmost=true,SizeToContent=SizeToContent.WidthAndHeight,ResizeMode=ResizeMode.NoResize,WindowStartupLocation=WindowStartupLocation.CenterScreen,Background=B(themeSurfaces[themeIndex]),Content=panel};
   window.PreviewKeyDown+=(s,e)=>{if(e.Key==Key.Escape)window.Close();};window.ShowDialog();
  }
  void ShowSoundSettings(Button anchor){
   var panel=new StackPanel();
   var choicesRow=new StackPanel{Orientation=Orientation.Horizontal};panel.Children.Add(choicesRow);
   var buttons=new List<Button>();
   for(int i=0;i<CountdownAudio.Names.Length;i++){
    int profile=i;
    var button=Button(CountdownAudio.Names[i],()=>{
     SelectSoundProfile(profile);
     for(int n=0;n<buttons.Count;n++)buttons[n].Background=n==profile?ActiveFill:Brushes.Transparent;
    });
    button.Width=76;button.Background=profile==soundProfile?ActiveFill:Brushes.Transparent;
    buttons.Add(button);choicesRow.Children.Add(button);
   }
   var previews=new StackPanel{Orientation=Orientation.Horizontal,HorizontalAlignment=HorizontalAlignment.Center};panel.Children.Add(previews);
   previews.Children.Add(Button("试听十秒提示",()=>countdownAudio.Play(soundProfile,false)));
   previews.Children.Add(Button("试听结束音",()=>countdownAudio.Play(soundProfile,true)));
   OpenFlyout(anchor,panel);
  }
  void ApplyTheme(int index){
   themeIndex=index;
   card.Background=B(themeSurfaces[index]);card.BorderBrush=UiBorder;
   card.CornerRadius=new CornerRadius(index==3?7:15);
   UpdateOrbTheme();
   foreach(var button in DescendantButtons(panel)){
    if(button==customColor)continue;
    button.Foreground=UiInk;
    foreach(var icon in LogicalTreeHelper.GetChildren(button).OfType<ToolIcon>())icon.InvalidateVisual();
   }
   Refresh();UpdateClock();
  }
  void UpdateOrbTheme(){orb.Background=B(themeIndex==0?(orb.IsMouseOver?"#F8FFFFFF":"#BBFFFFFF"):themeSurfaces[themeIndex]);orb.Foreground=themeIndex>=2?ActiveInk:UiInk;((ToolIcon)orb.Content).InvalidateVisual();}
  static IEnumerable<Button> DescendantButtons(Panel root){
   foreach(UIElement child in root.Children){
    var button=child as Button;if(button!=null)yield return button;
    var nested=child as Panel;if(nested!=null)foreach(var item in DescendantButtons(nested))yield return item;
   }
  }
  static Brush B(string c) { return (Brush)new BrushConverter().ConvertFromString(c); }
  Viewbox strip;
  Border reveal;
  readonly TranslateTransform stripMotion=new TranslateTransform();
  int motionVersion;
  readonly System.Diagnostics.Stopwatch lessonWatch=new System.Diagnostics.Stopwatch();
  bool countdownMode=true;
  TimeSpan countdownDuration=ReadCountdownDuration();
  TimeSpan customPreset=ReadTimePreference("countdown-preset.txt",TimeSpan.Zero);
  static string CountdownPreferencePath(){return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"ClassInk","countdown-duration.txt");}
  static TimeSpan ReadCountdownDuration(){return ReadTimePreference("countdown-duration.txt",TimeSpan.FromMinutes(5));}
  static TimeSpan ReadTimePreference(string file,TimeSpan fallback){
   try{
    int seconds;
    var path=Path.Combine(Path.GetDirectoryName(CountdownPreferencePath()),file);
    if(Int32.TryParse(File.ReadAllText(path),out seconds)&&seconds>0&&seconds<360000)return TimeSpan.FromSeconds(seconds);
   }catch(IOException){}catch(UnauthorizedAccessException){}
   return fallback;
  }
  int lastWarningSecond;
  bool completionPlayed;
  LessonClockWindow timerWindow;
  readonly System.Windows.Threading.DispatcherTimer clockTick=new System.Windows.Threading.DispatcherTimer();
  TextBlock clockText;
  Button clockButton;
  Point floatPosition=new Point(SystemParameters.WorkArea.Left,SystemParameters.WorkArea.Top);
  Point dragStart,dragOrigin;
  bool orbPressed,orbDragged;
  Button customColor;
  int[] customPalette=new int[16];
  readonly List<ChoiceGroup> choices=new List<ChoiceGroup>();
  readonly string[] palette={"#EF3333","#F58220","#F5C518","#28A745","#12BDD0","#3176EE","#924BDD","#111111","#FFFFFF"};
  readonly string[] paletteNames={"赤","橙","黄","绿","青","蓝","紫","黑","白"};
  int paletteIndex;
  sealed class ChoiceGroup {
   public string[] Icons,Names;public int Index;public Button Main;public Action<int> Apply;public bool Immediate;public Desk Desk;
   public void Step(int delta){Index=(Index+delta+Icons.Length)%Icons.Length;if(Immediate)Apply(Index);Refresh();}
   public void Refresh(){Main.Content=new ToolIcon(Icons[Index]);Main.ToolTip=Names[Index]+" · 滚轮切换 · 右键展开";bool active=Immediate&&Desk.board.Tool==Icons[Index];Main.Background=active?Desk.ActiveFill:Brushes.Transparent;Main.Foreground=active?Desk.ActiveInk:Desk.UiInk;}
  }
  void ActivateTool(string tool){board.SetTool(tool);Refresh();}
  Button ArrowButton(bool next,Action action){var b=Button("",action);b.Content=new ToolIcon(next?"next":"prev"){Width=10,Height=10};b.MinWidth=0;b.Width=10;b.Margin=new Thickness(0);b.Padding=new Thickness(0);b.ToolTip=next?"下一项":"上一项";return b;}
  Popup flyout;
  Popup notice;
  bool noticeOffersUndo;
  string lastReportedTool="mouse";
  readonly System.Windows.Threading.DispatcherTimer noticeTimer=new System.Windows.Threading.DispatcherTimer();
  void CloseNotice(){
   noticeTimer.Stop();noticeOffersUndo=false;
   if(notice!=null){notice.IsOpen=false;notice=null;}
  }
  void ShowNotice(string message,Action undo=null){
   if(!IsLoaded||!IsVisible)return;
   CloseNotice();
   var row=new StackPanel{Orientation=Orientation.Horizontal};
   row.Children.Add(new TextBlock{Text=message,Foreground=UiInk,FontSize=12,VerticalAlignment=VerticalAlignment.Center,Margin=new Thickness(6,0,6,0)});
   if(undo!=null){var button=Button("撤销",undo);button.Height=28;button.Foreground=ActiveInk;button.Background=ActiveFill;row.Children.Add(button);}
   var frame=FlyoutFrame(row);frame.IsHitTestVisible=undo!=null;
   notice=new Popup{PlacementTarget=orb,Placement=PlacementMode.Right,HorizontalOffset=8,AllowsTransparency=true,StaysOpen=true,IsHitTestVisible=undo!=null,Child=frame};
   noticeOffersUndo=undo!=null;notice.IsOpen=true;
   noticeTimer.Interval=TimeSpan.FromSeconds(undo==null?1.35:5);noticeTimer.Start();
  }
  void ReportToolChange(){
   if(!IsLoaded)return;
   if(lastReportedTool==board.Tool)return;
   lastReportedTool=board.Tool;
   if(noticeOffersUndo)return;
   if(board.Tool=="mouse"){ShowNotice("鼠标 · 可操作课件");return;}
   var group=choices.FirstOrDefault(item=>item.Icons.Contains(board.Tool));
   string name=group==null?"橡皮擦":group.Names[Array.IndexOf(group.Icons,board.Tool)];
   ShowNotice(name+" · Esc 返回鼠标");
  }
  void OpenFlyout(UIElement anchor,FrameworkElement content){
   if(flyout!=null)flyout.IsOpen=false;
   var frame=FlyoutFrame(content);
   frame.PreviewMouseWheel+=(s,e)=>{
    var element=anchor as FrameworkElement;
    if(element==null)return;
    var point=e.GetPosition(element);
    if(!new Rect(element.RenderSize).Contains(point))return;
    var forwarded=new MouseWheelEventArgs(e.MouseDevice,e.Timestamp,e.Delta){RoutedEvent=Mouse.MouseWheelEvent};
    element.RaiseEvent(forwarded);e.Handled=forwarded.Handled;
   };
   var slide=new TranslateTransform();frame.RenderTransform=slide;
   flyout=new Popup {PlacementTarget=anchor,Placement=PlacementMode.Right,HorizontalOffset=8,AllowsTransparency=true,StaysOpen=false,Child=frame};
   flyout.Opened+=(s,e)=>{
    bool opensLeft=frame.PointToScreen(new Point(0,0)).X<anchor.PointToScreen(new Point(0,0)).X;
    var duration=TimeSpan.FromMilliseconds(180);
    slide.BeginAnimation(TranslateTransform.XProperty,new DoubleAnimation(opensLeft?14:-14,0,duration){EasingFunction=new CubicEase{EasingMode=EasingMode.EaseOut}});
    frame.BeginAnimation(OpacityProperty,new DoubleAnimation(0,1,duration));
   };
   flyout.IsOpen=true;
  }
  Border FlyoutFrame(FrameworkElement content){return new Border {Background=B(themeSurfaces[themeIndex]),BorderBrush=UiBorder,BorderThickness=new Thickness(1),CornerRadius=new CornerRadius(12),Padding=new Thickness(6),Child=content};}
  FrameworkElement ChoicePanel(ChoiceGroup group){
   var row=new StackPanel {Orientation=Orientation.Horizontal};
   for(int n=0;n<group.Icons.Length;n++){int choice=n;var b=Button("",()=>{group.Index=choice;flyout.IsOpen=false;group.Apply(choice);group.Refresh();});
    b.Content=new ToolIcon(group.Icons[n]);b.Height=34;b.MinWidth=34;b.ToolTip=group.Names[n];b.Background=n==group.Index?ActiveFill:Brushes.Transparent;row.Children.Add(b);
   }return row;
  }
  void ShowChoice(ChoiceGroup group){if(flyout!=null&&flyout.IsOpen&&flyout.PlacementTarget==group.Main)return;OpenFlyout(group.Main,ChoicePanel(group));}
  void AddChoice(string[] icons,string[] names,Action<int> apply,bool immediate,int initial=0){
   var group=new ChoiceGroup{Icons=icons,Names=names,Apply=apply,Immediate=immediate,Index=initial,Desk=this};choices.Add(group);
   bool actionGroup=icons[0]=="zoom"||icons[0]=="save";
   bool wheelSelected=false;
   var row=new Grid();group.Main=Button("",()=>{if(actionGroup&&wheelSelected){wheelSelected=false;group.Apply(group.Index);}else ShowChoice(group);});group.Main.MinWidth=30;row.Children.Add(group.Main);
   row.Children.Add(new TextBlock{Text="›",FontSize=10,Foreground=B("#94A3B8"),HorizontalAlignment=HorizontalAlignment.Right,VerticalAlignment=VerticalAlignment.Bottom,Margin=new Thickness(0,0,3,4),IsHitTestVisible=false});
   group.Main.MouseRightButtonUp+=(s,e)=>{ShowChoice(group);e.Handled=true;};
   if(icons[0]!="thin"){
    var hover=new System.Windows.Threading.DispatcherTimer {Interval=TimeSpan.FromMilliseconds(180)};
    hover.Tick+=(s,e)=>{hover.Stop();if(IsLoaded&&!collapsed&&group.Main.IsMouseOver)ShowChoice(group);};
    group.Main.MouseEnter+=(s,e)=>hover.Start();group.Main.MouseLeave+=(s,e)=>hover.Stop();
   }
   group.Main.MouseWheel+=(s,e)=>{
    if(icons[0]=="thin"&&board.Tool!="pen"&&board.Tool!="highlight")return;
    if(flyout!=null&&flyout.PlacementTarget==group.Main)flyout.IsOpen=false;
    if(icons[0]=="screen"){
     group.Index=(group.Index+Math.Sign(e.Delta)+icons.Length)%icons.Length;
     group.Apply(group.Index);group.Refresh();
    }else group.Step(Math.Sign(e.Delta));
    wheelSelected=actionGroup;
    if(actionGroup)ShowNotice(group.Names[group.Index]+" · 点击执行");
    e.Handled=true;
   };
   group.Main.PreviewKeyDown+=(s,e)=>{if(e.Key==Key.Left||e.Key==Key.Right){group.Step(e.Key==Key.Left?-1:1);ShowChoice(group);e.Handled=true;}};
   panel.Children.Add(row);group.Refresh();
  }
  void AddIcon(string icon,string tip,Action action){var b=Button("",action);b.Content=new ToolIcon(icon);b.ToolTip=tip;panel.Children.Add(b);}
  void CycleColor(int delta){paletteIndex=(paletteIndex+delta+palette.Length)%palette.Length;SelectColor((Color)ColorConverter.ConvertFromString(palette[paletteIndex]),false);}
  sealed class DialogOwner : System.Windows.Forms.IWin32Window { public IntPtr Handle {get;private set;} public DialogOwner(IntPtr handle){Handle=handle;} }
  public Desk() {
   Title="课堂笔 ClassInk"; ShowInTaskbar=false;SizeToContent=SizeToContent.WidthAndHeight; WindowStyle=WindowStyle.None; AllowsTransparency=true;Background=Brushes.Transparent;Topmost=true;ResizeMode=ResizeMode.NoResize;
   var root=new StackPanel { Orientation=Orientation.Vertical, Margin=new Thickness(8), HorizontalAlignment=HorizontalAlignment.Left }; Content=root;
   orb=new Button { Content=new ToolIcon("pen"), Width=44, Height=44, HorizontalAlignment=HorizontalAlignment.Left, FontSize=25, Foreground=B("#334155"), Background=B("#BBFFFFFF"), BorderThickness=new Thickness(0), Cursor=Cursors.Hand, ToolTip="课堂笔 · 点击展开竖向工具栏" };
   orb.Template=(ControlTemplate)XamlReader.Parse("<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' TargetType='Button'><Border Background='{TemplateBinding Background}' BorderBrush='#CBE0E5EC' BorderThickness='1' CornerRadius='22'><ContentPresenter HorizontalAlignment='Center' VerticalAlignment='Center'/></Border></ControlTemplate>");
   orb.ToolTip="点击展开 / 收起；拖动移动悬浮球";
   orb.PreviewMouseLeftButtonDown+=(s,e)=>{BeginOrbDrag(orb.PointToScreen(e.GetPosition(orb)));orb.CaptureMouse();e.Handled=true;};
   orb.PreviewMouseMove+=(s,e)=>{if(orbPressed&&e.LeftButton==MouseButtonState.Pressed)MoveOrb(orb.PointToScreen(e.GetPosition(orb)));};
   orb.PreviewMouseLeftButtonUp+=(s,e)=>{if(!orbPressed)return;EndOrbDrag();orb.ReleaseMouseCapture();e.Handled=true;};
   orb.LostMouseCapture+=(s,e)=>{orbPressed=false;};
   orb.TouchDown+=(s,e)=>{BeginOrbDrag(orb.PointToScreen(e.GetTouchPoint(orb).Position));orb.CaptureTouch(e.TouchDevice);e.Handled=true;};
   orb.TouchMove+=(s,e)=>{if(orbPressed)MoveOrb(orb.PointToScreen(e.GetTouchPoint(orb).Position));e.Handled=true;};
   orb.TouchUp+=(s,e)=>{if(orbPressed)EndOrbDrag();orb.ReleaseTouchCapture(e.TouchDevice);e.Handled=true;};
   orb.MouseEnter+=(s,e)=>UpdateOrbTheme();
   orb.MouseLeave+=(s,e)=>UpdateOrbTheme();
   root.Children.Add(orb);
   card=new Border { Width=44, Background=B("#FAFFFFFF"), CornerRadius=new CornerRadius(15), Padding=new Thickness(2), Margin=new Thickness(0,8,0,0), BorderBrush=B("#DEE4EC"), BorderThickness=new Thickness(1) };
   panel=new StackPanel { Orientation=Orientation.Vertical };card.Child=panel;
   strip=new Viewbox { Stretch=Stretch.Uniform, StretchDirection=StretchDirection.DownOnly, VerticalAlignment=VerticalAlignment.Top, HorizontalAlignment=HorizontalAlignment.Left, Child=card, RenderTransform=stripMotion };
   reveal=new Border {Child=strip,ClipToBounds=true,Height=0,Visibility=Visibility.Collapsed,HorizontalAlignment=HorizontalAlignment.Left};root.Children.Add(reveal);
   status=new TextBlock();
   AddTool(panel,"mouse","鼠标穿透");
   var pens=new[]{"pen","highlight","laser"};
   AddChoice(pens,new[]{"画笔","荧光笔","激光笔"},i=>ActivateTool(pens[i]),true);
   AddTool(panel,"erase","橡皮擦");
   AddChoice(new[]{"arrow","line","rect","ellipse"},new[]{"箭头","直线","矩形","椭圆"},i=>ActivateTool(new[]{"arrow","line","rect","ellipse"}[i]),true);
   AddChoice(new[]{"text","select"},new[]{"文字","选择 / 移动"},i=>ActivateTool(i==0?"text":"select"),true);
   Separator();
   customColor=Button("",ShowPalette);customColor.Content=new ToolIcon("custom");customColor.Background=new SolidColorBrush(board.Color);customColor.ToolTip="展开颜色面板";panel.Children.Add(customColor);
   customColor.MouseWheel+=(s,e)=>{CycleColor(Math.Sign(e.Delta));e.Handled=true;};
   var widths=new[]{2.0,4.0,6.0,10.0,16.0};
   AddChoice(new[]{"thin","medium","bold","thick","heavy"},widths.Select(width=>width+" px · 滚轮调节").ToArray(),i=>{board.Size=widths[i];board.ApplyPen();},true,1);
   Separator();
   AddChoice(new[]{"undo","redo"},new[]{"撤销","重做"},i=>Travel(i==0?-1:1),false);
   AddChoice(new[]{"screen","board","blackboard","greenboard"},new[]{"屏幕标注","白板","黑板","仿真绿板"},i=>{board.SetSurface(boardSurfaces[i]);Commit();},false);
   pageNumber=new TextBlock{FontSize=9,HorizontalAlignment=HorizontalAlignment.Center};
   var pageFace=new StackPanel();pageFace.Children.Add(new ToolIcon("pages"){HorizontalAlignment=HorizontalAlignment.Center});pageFace.Children.Add(pageNumber);
   pageButton=Button("",ShowPages);pageButton.Content=pageFace;pageButton.Height=42;panel.Children.Add(pageButton);
   visibilityButton=Button("",ToggleAnnotations);panel.Children.Add(visibilityButton);
   AddIcon("trash","一键清空 · 可撤销",ClearBoard);
   AddChoice(new[]{"zoom","image"},new[]{"屏幕缩放 · Ctrl+1","框选截图并贴到板书"},i=>OpenScreenView(i==1),false);
   Action[] fileActions={Save,Open,Export,ExportBoard,ExportLessonPdf};
   AddChoice(new[]{"save","open","export","image","pdf"},new[]{"保存课程","打开课程","导出屏幕 PNG","导出当前板书 PNG","导出整节课 PDF"},i=>fileActions[i](),false);
   Separator();
   clockText=new TextBlock {Text="00:00",FontFamily=new FontFamily("Consolas"),FontSize=10,HorizontalAlignment=HorizontalAlignment.Center};
   var clockFace=new StackPanel {Orientation=Orientation.Vertical};clockFace.Children.Add(new ToolIcon("clock"){HorizontalAlignment=HorizontalAlignment.Center});clockFace.Children.Add(clockText);
   clockButton=Button("",ShowClock);clockButton.Content=clockFace;clockButton.ToolTip="左键打开计时器 · 右键选择预设";clockButton.Height=44;panel.Children.Add(clockButton);
   clockButton.MouseRightButtonUp+=(s,e)=>{ShowPresets();e.Handled=true;};
   AddIcon("reset","重置计时",ResetClock);
   Button settings=null;
   settings=Button("",()=>ShowSettings(settings));settings.Content=new ToolIcon("settings");settings.ToolTip="设置外观和计时音效";panel.Children.Add(settings);
   AddIcon("exit","直接退出",Exit);
   clockTick.Interval=TimeSpan.FromMilliseconds(200);clockTick.Tick+=(s,e)=>UpdateClock();clockTick.Start();
   SetCollapsed(true);
   ApplyTheme(themeIndex);
   noticeTimer.Tick+=(s,e)=>CloseNotice();
   PreviewKeyDown+=(s,e)=>{if(e.Key==Key.Escape&&board.Tool!="mouse"){if(flyout!=null)flyout.IsOpen=false;ActivateTool("mouse");e.Handled=true;}};
   Loaded+=(s,e)=>{
    board.Show();Owner=board;board.SetTool("mouse");
    string initial=Document.Save(board);history.Add(initial);RememberSavedLesson();
    board.Changed=Commit;Refresh(initial);
    var h=new WindowInteropHelper(this).Handle;HwndSource.FromHwnd(h).AddHook(Hook);
    shortcuts=CreateShortcuts();
    for(int i=0;i<shortcuts.Length;i++)if(Native.RegisterHotKey(h,i+1,i==4?0x4002u:0x4003u,shortcuts[i].Key))registeredShortcuts.Add(i+1);
    if(!registeredShortcuts.Contains(5))ShowNotice("Ctrl+1 已被占用，可从工具栏启动缩放");
    Activate();
   };
   Closing+=(s,e)=>{if(exiting)return;e.Cancel=true;Exit();};
  }
  static ControlTemplate SoftButtonTemplate(){return (ControlTemplate)XamlReader.Parse("<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' TargetType='Button'><Grid><Border Background='{TemplateBinding Background}' BorderBrush='{TemplateBinding BorderBrush}' BorderThickness='{TemplateBinding BorderThickness}' CornerRadius='8'/><Border x:Name='hover' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' Background='#0C356DE6' CornerRadius='8' Opacity='0'/><ContentPresenter HorizontalAlignment='Center' VerticalAlignment='Center' Margin='{TemplateBinding Padding}'/></Grid><ControlTemplate.Triggers><Trigger Property='IsMouseOver' Value='True'><Setter TargetName='hover' Property='Opacity' Value='1'/></Trigger><Trigger Property='IsPressed' Value='True'><Setter TargetName='hover' Property='Background' Value='#20356DE6'/></Trigger><Trigger Property='IsEnabled' Value='False'><Setter Property='Opacity' Value='0.35'/></Trigger></ControlTemplate.Triggers></ControlTemplate>");}
  static ControlTemplate SliderTemplate(){return (ControlTemplate)XamlReader.Parse("<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' TargetType='Slider'><Grid Height='24'><Border Height='3' Background='#E3E9F1' CornerRadius='2' Margin='5,0'/><Track Name='PART_Track' Minimum='{TemplateBinding Minimum}' Maximum='{TemplateBinding Maximum}' Value='{Binding Value, RelativeSource={RelativeSource TemplatedParent}, Mode=TwoWay}'><Track.DecreaseRepeatButton><RepeatButton Command='Slider.DecreaseLarge' Opacity='0'/></Track.DecreaseRepeatButton><Track.Thumb><Thumb Width='12' Height='12'><Thumb.Template><ControlTemplate TargetType='Thumb'><Ellipse Fill='#356DE6' Stroke='White' StrokeThickness='2'/></ControlTemplate></Thumb.Template></Thumb></Track.Thumb><Track.IncreaseRepeatButton><RepeatButton Command='Slider.IncreaseLarge' Opacity='0'/></Track.IncreaseRepeatButton></Track></Grid></ControlTemplate>");}
  void Separator(){panel.Children.Add(new Border {Height=1,Background=B("#E4E9F0"),Margin=new Thickness(4,4,4,4),VerticalAlignment=VerticalAlignment.Center});}
  void SetCollapsed(bool value,bool animate=true){
   if(collapsed==value&&IsLoaded&&animate)return;
   collapsed=value;if(value&&flyout!=null)flyout.IsOpen=false;double railHeight=MeasureRail();
   if(!value){var area=SystemParameters.WorkArea;Left=Math.Max(area.Left,Math.Min(area.Right-60,floatPosition.X));Top=Math.Max(area.Top,Math.Min(area.Bottom-railHeight-60,floatPosition.Y));floatPosition=new Point(Left,Top);}
   if(!IsLoaded||!animate){
    motionVersion++;reveal.BeginAnimation(HeightProperty,null);stripMotion.BeginAnimation(TranslateTransform.YProperty,null);strip.BeginAnimation(OpacityProperty,null);stripMotion.Y=0;strip.Opacity=1;
    reveal.Visibility=value?Visibility.Collapsed:Visibility.Visible;reveal.Height=value?0:railHeight;strip.IsHitTestVisible=!value;
    if(value){Left=floatPosition.X;Top=floatPosition.Y;}
   }else{
    StartSlide(value,()=>{if(value){reveal.Visibility=Visibility.Collapsed;Left=floatPosition.X;Top=floatPosition.Y;}});
   }
  }
  double MeasureRail(){strip.MaxHeight=Math.Max(120,SystemParameters.WorkArea.Height-60);strip.Measure(new Size(Double.PositiveInfinity,Double.PositiveInfinity));return strip.DesiredSize.Height;}
  void StartSlide(bool hide,Action finished){
   int version=++motionVersion;
   double distance=MeasureRail();
   bool hidden=reveal.Visibility!=Visibility.Visible;
   double fromHeight=hidden?0:reveal.Height,fromY=hidden?-12:stripMotion.Y,fromOpacity=hidden?0:strip.Opacity;
   reveal.BeginAnimation(HeightProperty,null);stripMotion.BeginAnimation(TranslateTransform.YProperty,null);strip.BeginAnimation(OpacityProperty,null);
   reveal.Height=fromHeight;stripMotion.Y=fromY;strip.Opacity=fromOpacity;reveal.Visibility=Visibility.Visible;strip.IsHitTestVisible=!hide;
   double targetHeight=hide?0:distance,targetY=hide?-12:0,targetOpacity=hide?0:1;
   // Resume from the displayed position when the user reverses an unfinished transition.
   double fraction=Math.Min(1,Math.Abs(targetHeight-fromHeight)/distance);
   var duration=TimeSpan.FromMilliseconds(Math.Max(100,(hide?240:320)*fraction));
   var ease=new CubicEase {EasingMode=hide?EasingMode.EaseInOut:EasingMode.EaseOut};
   var slide=new DoubleAnimation(fromY,targetY,duration){EasingFunction=ease};
   var growth=new DoubleAnimation(fromHeight,targetHeight,duration){EasingFunction=ease};
   var fade=new DoubleAnimation(fromOpacity,targetOpacity,duration){EasingFunction=ease};
   growth.Completed+=(s,e)=>{if(version!=motionVersion)return;reveal.BeginAnimation(HeightProperty,null);stripMotion.BeginAnimation(TranslateTransform.YProperty,null);strip.BeginAnimation(OpacityProperty,null);reveal.Height=targetHeight;stripMotion.Y=targetY;strip.Opacity=targetOpacity;finished();};
   reveal.BeginAnimation(HeightProperty,growth,HandoffBehavior.SnapshotAndReplace);
   stripMotion.BeginAnimation(TranslateTransform.YProperty,slide,HandoffBehavior.SnapshotAndReplace);
   strip.BeginAnimation(OpacityProperty,fade,HandoffBehavior.SnapshotAndReplace);
  }
  internal void VerifyMotion(){
   SetCollapsed(true,false);int completed=0;
   StartSlide(false,()=>completed++);PumpAnimation(90);
   if(stripMotion.Y>0||stripMotion.Y< -65||strip.Opacity<0||strip.Opacity>1)throw new Exception("Slide values outside bounds");
   int baseline=completed;StartSlide(true,()=>completed+=100);PumpAnimation(40);
   double interruptedY=stripMotion.Y;StartSlide(false,()=>completed+=10000);
   if(Math.Abs(stripMotion.Y-interruptedY)>0.01)throw new Exception("Slide reversal jumped");
   PumpAnimation(450);
   if(completed!=baseline+10000||Math.Abs(stripMotion.Y)>0.01||Math.Abs(strip.Opacity-1)>0.01||!strip.IsHitTestVisible)throw new Exception("Slide reversal did not complete correctly");
   StartSlide(true,()=>{reveal.Visibility=Visibility.Collapsed;});PumpAnimation(400);
   if(reveal.Visibility!=Visibility.Collapsed||reveal.Height!=0||strip.IsHitTestVisible)throw new Exception("Collapsed toolbar still intercepts input");
   SetCollapsed(true,false);
  }
  internal static void PumpAnimation(int milliseconds){
   var frame=new System.Windows.Threading.DispatcherFrame();
   var timeout=new System.Windows.Threading.DispatcherTimer {Interval=TimeSpan.FromMilliseconds(milliseconds)};
   timeout.Tick+=(s,e)=>{timeout.Stop();frame.Continue=false;};timeout.Start();System.Windows.Threading.Dispatcher.PushFrame(frame);
  }
  void BeginOrbDrag(Point screen){orbPressed=true;orbDragged=false;dragStart=screen;dragOrigin=new Point(Left,Top);}
  void MoveOrb(Point screen){
   Vector delta=screen-dragStart;var source=PresentationSource.FromVisual(this);if(source!=null)delta=source.CompositionTarget.TransformFromDevice.Transform(delta);
   if(!orbDragged&&delta.Length<5)return;
   if(!orbDragged){orbDragged=true;if(flyout!=null)flyout.IsOpen=false;}
   var area=SystemParameters.WorkArea;Left=Math.Max(area.Left,Math.Min(area.Right-60,dragOrigin.X+delta.X));Top=Math.Max(area.Top,Math.Min(area.Bottom-(collapsed?60:MeasureRail()+60),dragOrigin.Y+delta.Y));
   floatPosition=new Point(Left,Top);
  }
  void EndOrbDrag(){orbPressed=false;if(!orbDragged)SetCollapsed(!collapsed);else{floatPosition=new Point(Left,Top);}}
  void ToggleClock(){if(lessonWatch.IsRunning)lessonWatch.Stop();else lessonWatch.Start();UpdateClock();}
  void ResetClock(){ResetClockState();UpdateClock();}
  void ResetClockState(){lessonWatch.Reset();lastWarningSecond=0;completionPlayed=false;}
  void ShowPages(){
   board.FinishActiveInput();Commit();
   var box=new StackPanel{Width=338,Margin=new Thickness(4)};
   var row=new StackPanel{Orientation=Orientation.Horizontal};box.Children.Add(row);
   var previous=Button("上一页",()=>SwitchPage(pageIndex-1));previous.IsEnabled=pageIndex>0;row.Children.Add(previous);
   var next=Button("下一页",()=>SwitchPage(pageIndex+1));next.IsEnabled=pageIndex+1<pages.Count;row.Children.Add(next);
   row.Children.Add(Button("新建页",NewPage));
   row.Children.Add(Button("复制页",DuplicateCurrentPage));
   var delete=Button("删除当前页",DeleteCurrentPage);delete.IsEnabled=pages.Count>1;row.Children.Add(delete);
   if(deletedPage!=null)row.Children.Add(Button("恢复最近删除",RestoreDeletedPage));
   var rename=new DockPanel{Margin=new Thickness(4,5,4,6)};box.Children.Add(rename);
   var title=new TextBox{Text=pages[pageIndex].Title,MaxLength=40,MinHeight=28,VerticalContentAlignment=VerticalAlignment.Center,ToolTip="当前页标题，留空时显示页码"};
   Action apply=()=>{RenamePage(title.Text);ShowPages();};
   var renameButton=Button("设标题",apply);DockPanel.SetDock(renameButton,Dock.Right);rename.Children.Add(renameButton);rename.Children.Add(title);
   title.KeyDown+=(s,e)=>{if(e.Key==Key.Enter){apply();e.Handled=true;}};
   var list=new ListBox{Height=Math.Min(330,pages.Count*72+8),Background=Brushes.Transparent,BorderThickness=new Thickness(0),Foreground=UiInk};
   var item=new FrameworkElementFactory(typeof(StackPanel));item.SetValue(StackPanel.OrientationProperty,Orientation.Horizontal);item.SetValue(FrameworkElement.MarginProperty,new Thickness(4));
   var thumb=new FrameworkElementFactory(typeof(Image));thumb.SetBinding(Image.SourceProperty,new System.Windows.Data.Binding("Image"));thumb.SetValue(FrameworkElement.WidthProperty,112.0);thumb.SetValue(FrameworkElement.HeightProperty,60.0);item.AppendChild(thumb);
   var name=new FrameworkElementFactory(typeof(TextBlock));name.SetBinding(TextBlock.TextProperty,new System.Windows.Data.Binding("Name"));name.SetValue(FrameworkElement.MarginProperty,new Thickness(10,0,0,0));name.SetValue(FrameworkElement.MaxWidthProperty,172.0);name.SetValue(FrameworkElement.VerticalAlignmentProperty,VerticalAlignment.Center);name.SetValue(TextBlock.TextTrimmingProperty,TextTrimming.CharacterEllipsis);item.AppendChild(name);
   list.ItemTemplate=new DataTemplate{VisualTree=item};
   list.ItemsSource=pages.Select((page,i)=>new PagePreview(page,PageTitle(i))).ToArray();list.SelectedIndex=pageIndex;
   list.SelectionChanged+=(s,e)=>{if(list.SelectedIndex>=0)SwitchPage(list.SelectedIndex);};
   box.Children.Add(list);OpenFlyout(pageButton,box);list.ScrollIntoView(list.SelectedItem);
  }
  string PageTitle(int index){return "第 "+(index+1)+" 页"+(String.IsNullOrWhiteSpace(pages[index].Title)?"":" · "+pages[index].Title);}
  void RenamePage(string title){pages[pageIndex].Title=title.Trim();Refresh(history[index]);}
  void DuplicateCurrentPage(){
   board.FinishActiveInput();Commit();
   string title=pages[pageIndex].Title;
   string copyTitle=title.Length==0?"副本":title.Substring(0,Math.Min(title.Length,36))+"（副本）";
   pages.Insert(pageIndex+1,new PageState(history[index]){Title=copyTitle});
   SwitchPage(pageIndex+1);
  }
  void SwitchPage(int target){
   if(target<0||target>=pages.Count||target==pageIndex)return;
   board.FinishActiveInput();Commit();
   if(flyout!=null)flyout.IsOpen=false;CloseNotice();
   Document.Load(board,pages[target].Snapshot);pageIndex=target;Refresh();
  }
  void NewPage(){
   board.FinishActiveInput();Commit();
   string blank=Document.Save(board,true);
   pages.Insert(pageIndex+1,new PageState(blank));
   SwitchPage(pageIndex+1);
  }
  void DeleteCurrentPage(){
   if(pages.Count<=1)return;
   board.FinishActiveInput();Commit();
   int removed=pageIndex;
   int adjacent=removed+1<pages.Count?removed+1:removed-1;
   Document.Load(board,pages[adjacent].Snapshot);
   deletedPage=pages[removed];deletedPageIndex=removed;
   pages.RemoveAt(removed);pageIndex=Math.Min(removed,pages.Count-1);
   if(flyout!=null)flyout.IsOpen=false;
   Refresh(history[index]);
   ShowNotice("已删除第 "+(removed+1)+" 页",RestoreDeletedPage);
  }
  void RestoreDeletedPage(){
   if(deletedPage==null)return;
   board.FinishActiveInput();Commit();
   Document.Load(board,deletedPage.Snapshot);
   int target=Math.Min(deletedPageIndex,pages.Count);
   pages.Insert(target,deletedPage);pageIndex=target;deletedPage=null;
   if(flyout!=null)flyout.IsOpen=false;
   CloseNotice();Refresh(history[index]);
   ShowNotice("已恢复第 "+(pageIndex+1)+" 页");
  }
  void ToggleAnnotations(){
   bool hide=!board.AnnotationsHidden;
   if(hide){board.FinishActiveInput();board.SetTool("mouse");}
   board.SetAnnotationsHidden(hide);Refresh();
   ShowNotice(hide?"已隐藏板书 · 可操作课件":"板书已恢复");
  }
  void ShowPresets(){
   var list=new StackPanel();
   list.Children.Add(Button("思考 · 30 秒",()=>StartPreset(TimeSpan.FromSeconds(30))));
   list.Children.Add(Button("随堂练习 · 3 分钟",()=>StartPreset(TimeSpan.FromMinutes(3))));
   list.Children.Add(Button("独立编程 · 10 分钟",()=>StartPreset(TimeSpan.FromMinutes(10))));
   if(customPreset>TimeSpan.Zero)list.Children.Add(Button("自定义 · "+ClockLabel(customPreset),()=>StartPreset(customPreset)));
   list.Children.Add(Button("将当前时长存为自定义",()=>{
    var path=Path.Combine(Path.GetDirectoryName(CountdownPreferencePath()),"countdown-preset.txt");
    Directory.CreateDirectory(Path.GetDirectoryName(path));
    AtomicWrite(path,((int)countdownDuration.TotalSeconds).ToString(CultureInfo.InvariantCulture));
    customPreset=countdownDuration;flyout.IsOpen=false;ShowNotice("已保存自定义计时预设");
   }));
   OpenFlyout(clockButton,list);
  }
  void StartPreset(TimeSpan duration){
   ResetClockState();
   if(timerWindow!=null)timerWindow.SetDuration(duration);
   ConfigureClock(true,duration,false);
   if(IsLoaded)ShowClock();
  }
  void ShowClock(){
   board.FinishText();if(flyout!=null)flyout.IsOpen=false;
   if(timerWindow==null){
    if(countdownMode&&lessonWatch.Elapsed>=countdownDuration)ResetClockState();
    timerWindow=new LessonClockWindow(countdownMode,countdownDuration,ConfigureClock);timerWindow.Owner=this;timerWindow.Closed+=(s,e)=>timerWindow=null;
   }
   timerWindow.Show();UpdateClock();timerWindow.Activate();
  }
  void ConfigureClock(bool countdown,TimeSpan duration,bool reset){
   bool changed=countdownMode!=countdown||countdownDuration!=duration;
   countdownMode=countdown;countdownDuration=duration;
   if(changed&&countdown&&IsLoaded){
    try{
     string path=CountdownPreferencePath();Directory.CreateDirectory(Path.GetDirectoryName(path));
     AtomicWrite(path,((int)duration.TotalSeconds).ToString(CultureInfo.InvariantCulture));
    }catch(IOException){ShowNotice("计时时长未保存到本机");}catch(UnauthorizedAccessException){ShowNotice("计时时长未保存到本机");}
   }
   if(changed||reset||(countdown&&lessonWatch.Elapsed>=duration))ResetClockState();
   if(!reset)ToggleClock();else UpdateClock();
  }
  TimeSpan ClockValue(){return countdownMode?TimeSpan.FromSeconds(Math.Max(0,Math.Ceiling((countdownDuration-lessonWatch.Elapsed).TotalSeconds))):lessonWatch.Elapsed;}
  static string ClockLabel(TimeSpan t){return t.TotalHours>=1?((int)t.TotalHours).ToString("00")+":"+t.Minutes.ToString("00")+":"+t.Seconds.ToString("00"):((int)t.TotalMinutes).ToString("00")+":"+t.Seconds.ToString("00");}
  void SelectColor(Color value,bool custom){
   board.Color=value;board.ApplyPen();string hex=String.Format("#{0:X2}{1:X2}{2:X2}",value.R,value.G,value.B);
   
   customColor.Opacity=1;
   {customColor.Background=new SolidColorBrush(value);customColor.Foreground=(value.R*299+value.G*587+value.B*114)>150000?B("#334155"):Brushes.White;customColor.ToolTip="自定义颜色 "+hex+" · 点击重新选择";((ToolIcon)customColor.Content).InvalidateVisual();}
  }
  void ChooseCustomColor(){
   using(var picker=new System.Windows.Forms.ColorDialog {FullOpen=true,AnyColor=true,Color=System.Drawing.Color.FromArgb(board.Color.R,board.Color.G,board.Color.B),CustomColors=customPalette}){
    if(picker.ShowDialog(new DialogOwner(new WindowInteropHelper(this).Handle))!=System.Windows.Forms.DialogResult.OK)return;
    customPalette=picker.CustomColors;SelectColor(Color.FromRgb(picker.Color.R,picker.Color.G,picker.Color.B),true);
   }
  }
  void ShowPalette(){
   var row=new StackPanel{Orientation=Orientation.Horizontal};
   for(int i=0;i<palette.Length;i++){string chosen=palette[i];var swatch=Button("",()=>{SelectColor((Color)ColorConverter.ConvertFromString(chosen),false);flyout.IsOpen=false;});swatch.Background=B(chosen);swatch.Width=28;swatch.MinWidth=28;swatch.Height=28;swatch.BorderThickness=new Thickness(1);swatch.BorderBrush=B("#D5DCE6");swatch.ToolTip=paletteNames[i]+" "+chosen;row.Children.Add(swatch);}
   var custom=Button("",()=>{flyout.IsOpen=false;ChooseCustomColor();});custom.Content=new ToolIcon("custom");custom.ToolTip="自定义颜色 / RGB";row.Children.Add(custom);OpenFlyout(customColor,row);
  }
  void ClearBoard(){
   board.FinishActiveInput();
   board.FinishText(false);
   if(board.Ink.Strokes.Count==0&&board.Ink.Children.Count==0)return;
   board.Ink.Strokes.Clear();board.Ink.Children.Clear();Commit();
   ShowNotice("板书已清空",()=>Travel(-1));
  }
  internal void VerifyInteractions(){
   countdownMode=false;
   var files=choices.First(g=>g.Icons.Contains("save"));files.Step(1);if(files.Index!=1)throw new Exception("File choice did not advance");files.Step(-1);if(files.Index!=0)throw new Exception("File choice did not reverse");
   var thickness=choices.First(g=>g.Icons.Contains("thin"));thickness.Step(1);if(board.Size!=6||board.Ink.DefaultDrawingAttributes.Width!=6)throw new Exception("Grouped thickness did not apply");thickness.Step(-1);
   CycleColor(1);if(board.Color!=(Color)ColorConverter.ConvertFromString(palette[1]))throw new Exception("Color choice did not apply");CycleColor(-1);
   history.Clear();index=0;string initial=Document.Save(board);history.Add(initial);RememberSavedLesson();
   board.Ink.Strokes.Add(Board.Shape("arrow",new Point(10,20),new Point(100,100),Colors.Coral,4));board.AddText("课堂测试",20,30,"#FF123456",24);Commit();ClearBoard();
   if(board.Ink.Strokes.Count!=0||board.Ink.Children.Count!=0)throw new Exception("One-click clear failed");Travel(-1);
   if(board.Ink.Strokes.Count!=1||board.Ink.Children.Count!=1)throw new Exception("Undo clear failed");Travel(1);
   if(board.Ink.Strokes.Count!=0||board.Ink.Children.Count!=0)throw new Exception("Redo clear failed");
   BeginOrbDrag(new Point(0,0));MoveOrb(new Point(160,0));EndOrbDrag();var position=floatPosition;
   if(!collapsed||position.X<=SystemParameters.WorkArea.Left)throw new Exception("Orb drag failed");SetCollapsed(false);
   if(Top!=position.Y||Left!=position.X)throw new Exception("Vertical toolbar detached from orb");
   BeginOrbDrag(new Point(0,0));MoveOrb(new Point(12,0));EndOrbDrag();
   if(collapsed||reveal.Visibility!=Visibility.Visible)throw new Exception("Dragging collapsed an open toolbar");
   position=floatPosition;SetCollapsed(true);
   if(Left!=position.X||Top!=position.Y)throw new Exception("Orb position was lost");
   ToggleClock();SetCollapsed(true);System.Threading.Thread.Sleep(40);ToggleClock();
   var elapsed=lessonWatch.Elapsed;if(elapsed.TotalMilliseconds<30||lessonWatch.IsRunning)throw new Exception("Clock pause failed");System.Threading.Thread.Sleep(20);
   if(lessonWatch.Elapsed!=elapsed)throw new Exception("Paused clock advanced");ToggleClock();if(!lessonWatch.IsRunning||lessonWatch.Elapsed<elapsed)throw new Exception("Clock resume failed");ResetClock();
   if(lessonWatch.Elapsed!=TimeSpan.Zero||lessonWatch.IsRunning||clockText.Text!="00:00")throw new Exception("Clock reset failed");
   floatPosition=new Point(SystemParameters.WorkArea.Left,SystemParameters.WorkArea.Top);SetCollapsed(true);clockTick.Stop();
  }
  internal void VerifyLessonPages(){
   board.SetSurface("green");board.AddText("第一页",24,40,"#FFFFFFFF",24);Commit();
   NewPage();
   if(pageIndex!=1||pages.Count!=2||board.Surface!="green"||board.Ink.Children.Count!=0)throw new Exception("New lesson page did not start empty");
   board.SetSurface("white");board.AddText("第二页",34,50,"#FF000000",24);Commit();
   string second=Document.Save(board);
   ToggleAnnotations();
   if(!board.AnnotationsHidden||board.Tool!="mouse"||Document.Save(board)!=second)throw new Exception("Hiding annotations changed lesson content");
   ToggleAnnotations();
   if(board.AnnotationsHidden||Document.Save(board)!=second)throw new Exception("Restoring annotations changed lesson content");
   SwitchPage(0);
   if(board.Surface!="green"||board.Ink.Children.OfType<TextBlock>().Single().Text!="第一页")throw new Exception("First page content was lost");
   SwitchPage(1);Travel(-1);
   if(board.Ink.Children.Count!=0||board.Surface!="green")throw new Exception("Page-local undo was not preserved");
   Travel(1);
   var lesson=new LessonDocument(pages.Select(page=>page.Snapshot).ToArray(),pageIndex);
   LoadLesson(LessonDocument.Load(lesson.Save()));
   if(pageIndex!=1||pages.Count!=2||Document.Save(board)!=second||!MatchesSavedLesson())throw new Exception("Lesson did not round-trip");
   SwitchPage(0);
   if(!MatchesSavedLesson())throw new Exception("Navigation marked a saved lesson as changed");
   DeleteCurrentPage();
   if(pages.Count!=1||pageIndex!=0||Document.Save(board)!=second||MatchesSavedLesson())throw new Exception("Deleting the first page did not select its successor");
   DeleteCurrentPage();
   if(pages.Count!=1)throw new Exception("The last page could be deleted");
   RestoreDeletedPage();
   if(pages.Count!=2||pageIndex!=0||board.Surface!="green"||!MatchesSavedLesson())throw new Exception("Deleted page was not restored at its original position");
   SwitchPage(1);DeleteCurrentPage();
   if(pages.Count!=1||pageIndex!=0||board.Surface!="green")throw new Exception("Deleting the last indexed page did not select its predecessor");
   RestoreDeletedPage();
   if(pages.Count!=2||pageIndex!=1||Document.Save(board)!=second)throw new Exception("Restoring the last indexed page lost its content");
   RenamePage("答案 <C++>");
   if(MatchesSavedLesson())throw new Exception("Renaming a page did not mark the lesson changed");
   DuplicateCurrentPage();
   if(pages.Count!=3||pageIndex!=2||pages[2].Title!="答案 <C++>（副本）"||Document.Save(board)!=second)throw new Exception("Page duplicate lost its title or ink");
   ClearBoard();SwitchPage(1);
   if(Document.Save(board)!=second)throw new Exception("Editing a duplicate changed its original");
   SwitchPage(2);DeleteCurrentPage();
   var titled=LessonDocument.Load(CurrentLesson().Save());LoadLesson(titled);
   if(pages[1].Title!="答案 <C++>"||!MatchesSavedLesson())throw new Exception("Page title did not survive a round trip");
   using(var exporter=new BoardExporter()){
    string directory=AppDomain.CurrentDomain.BaseDirectory;
    var encoder=new PngBitmapEncoder();encoder.Frames.Add(BitmapFrame.Create(exporter.Render(titled.Pages[0],1600,1100,false)));
    WriteFile(Path.Combine(directory,"self-test-board.png"),encoder.Save);
    WriteFile(Path.Combine(directory,"self-test-lesson.pdf"),stream=>exporter.WritePdf(stream,titled));
   }
   LoadLesson(LessonDocument.Load(second));
   if(pages.Count!=1||pageIndex!=0||Document.Save(board)!=second)throw new Exception("Legacy page did not open as a lesson");
   board.SetSurface("screen");LoadLesson(new LessonDocument(new[]{Document.Save(board,true)},0));
  }
  void UpdateClock(){
   bool finished=countdownMode&&lessonWatch.Elapsed>=countdownDuration;
   var time=ClockValue();
   if(countdownMode&&lessonWatch.IsRunning){
    if(finished&&!completionPlayed){completionPlayed=true;if(IsLoaded)countdownAudio.Play(soundProfile,true);}
    else if(time.TotalSeconds>=1&&time.TotalSeconds<=10&&lastWarningSecond!=(int)time.TotalSeconds){
     lastWarningSecond=(int)time.TotalSeconds;if(IsLoaded)countdownAudio.Play(soundProfile,false);
    }
   }
   if(finished)lessonWatch.Stop();
   clockText.Text=ClockLabel(time);
   clockButton.Foreground=lessonWatch.IsRunning?ActiveInk:UiInk;
   clockButton.Background=lessonWatch.IsRunning?ActiveFill:Brushes.Transparent;
   foreach(var icon in ((StackPanel)clockButton.Content).Children.OfType<ToolIcon>())icon.InvalidateVisual();
   if(timerWindow!=null)timerWindow.Update(time,lessonWatch.IsRunning,finished);
  }
  internal void RenderPreview(bool expanded,string path){
   SetCollapsed(!expanded,false);Refresh();var root=(FrameworkElement)Content;root.Measure(new Size(Double.PositiveInfinity,Double.PositiveInfinity));root.Arrange(new Rect(root.DesiredSize));root.UpdateLayout();
   if(root.ActualWidth>60||root.ActualHeight>SystemParameters.WorkArea.Height)throw new Exception("Vertical toolbar exceeds screen bounds");
   if(expanded&&root.ActualHeight<=root.ActualWidth)throw new Exception("Toolbar is still horizontal");
   if(card.Width!=orb.Width)throw new Exception("Rail and orb widths differ");
   var target=new RenderTargetBitmap((int)Math.Ceiling(root.ActualWidth),(int)Math.Ceiling(root.ActualHeight),96,96,PixelFormats.Pbgra32);target.Render(root);var encoder=new PngBitmapEncoder();encoder.Frames.Add(BitmapFrame.Create(target));using(var f=File.Create(path))encoder.Save(f);
  }
  internal void RenderChoicePreview(string path){var frame=FlyoutFrame(ChoicePanel(choices.First(g=>g.Icons.Contains("arrow"))));frame.Measure(new Size(Double.PositiveInfinity,Double.PositiveInfinity));frame.Arrange(new Rect(frame.DesiredSize));var image=new RenderTargetBitmap((int)Math.Ceiling(frame.ActualWidth),(int)Math.Ceiling(frame.ActualHeight),96,96,PixelFormats.Pbgra32);image.Render(frame);var encoder=new PngBitmapEncoder();encoder.Frames.Add(BitmapFrame.Create(image));using(var f=File.Create(path))encoder.Save(f);}
  internal void VerifyCountdownAndRender(string path){
   StartPreset(TimeSpan.FromSeconds(30));
   if(countdownDuration!=TimeSpan.FromSeconds(30)||!lessonWatch.IsRunning||ClockValue().TotalSeconds!=30)throw new Exception("Countdown preset did not start");
   ConfigureClock(true,TimeSpan.FromSeconds(10),false);
   if(lastWarningSecond!=10)throw new Exception("Final ten seconds did not start ticking");
   PumpAnimation(1100);UpdateClock();
   if(lastWarningSecond!=9)throw new Exception("Final seconds did not continue ticking");
   ConfigureClock(true,TimeSpan.FromMilliseconds(60),false);if(!lessonWatch.IsRunning||ClockValue().TotalSeconds!=1)throw new Exception("Countdown start failed");
   if(lastWarningSecond!=1)throw new Exception("Countdown warning was not triggered");
   PumpAnimation(110);UpdateClock();if(lessonWatch.IsRunning||ClockValue()!=TimeSpan.Zero||!completionPlayed)throw new Exception("Countdown completion failed");
   ResetClock();if(ClockValue().TotalSeconds!=1)throw new Exception("Countdown reset failed");ConfigureClock(true,TimeSpan.FromMinutes(5),true);
   var clock=new LessonClockWindow(true,countdownDuration,ConfigureClock);var contents=(FrameworkElement)clock.Content;clock.Content=null;
   var surface=new Border {Width=840,Height=480,Background=clock.Background,Child=contents};surface.Measure(new Size(840,480));surface.Arrange(new Rect(0,0,840,480));surface.UpdateLayout();
   var image=new RenderTargetBitmap(840,480,96,96,PixelFormats.Pbgra32);image.Render(surface);var encoder=new PngBitmapEncoder();encoder.Frames.Add(BitmapFrame.Create(image));using(var f=File.Create(path))encoder.Save(f);
  }
  void Label(string text){panel.Children.Add(new TextBlock{Text=text,Foreground=B("#8896B3"),FontSize=11,Margin=new Thickness(3,12,0,6)});}
  Button Button(string text,Action action){var b=new Button {Content=text,MinWidth=30,Height=30,Margin=new Thickness(2,1,2,1),Foreground=UiInk,Background=Brushes.Transparent,BorderThickness=new Thickness(0),FontSize=11,FontFamily=new FontFamily("Microsoft YaHei UI"),Cursor=Cursors.Hand,Padding=new Thickness(2)};if(text=="undo"||text=="redo")b.Content=new ToolIcon(text);b.Template=SoftButtonTemplate();b.Click+=(s,e)=>{try{action();}catch(Exception ex){MessageBox.Show(ex.Message,"课堂笔 · 操作未完成");}};return b;}
  void AddTool(Panel grid,string key,string text){var b=Button(text,()=>{board.SetTool(key);Refresh();});b.Content=new ToolIcon(key);b.ToolTip=text;tools[key]=b;grid.Children.Add(b);}
  void Refresh(string serialized=null){
   foreach(var group in choices){
    int selected=group.Icons[0]=="screen"?Array.IndexOf(boardSurfaces,board.Surface):Array.IndexOf(group.Icons,board.Tool);
    if(selected>=0)group.Index=selected;
    group.Refresh();
   }
   foreach(var tool in tools){
    bool active=tool.Key==board.Tool;
    tool.Value.Background=active?ActiveFill:Brushes.Transparent;
    tool.Value.Foreground=active?ActiveInk:UiInk;
    ((ToolIcon)tool.Value.Content).InvalidateVisual();
   }
   pageNumber.Text=(pageIndex+1)+"/"+pages.Count;
   pageButton.ToolTip="第 "+(pageIndex+1)+" / "+pages.Count+" 页 · Ctrl+Alt+PageUp / PageDown 翻页";
   bool hidden=board.AnnotationsHidden;
   visibilityButton.Content=new ToolIcon(hidden?"eyeoff":"eye");
   visibilityButton.Foreground=hidden?ActiveInk:UiInk;
   visibilityButton.Background=hidden?ActiveFill:Brushes.Transparent;
   visibilityButton.ToolTip=(hidden?"恢复板书":"隐藏板书")+" · Ctrl+Alt+H";
   saved=MatchesSavedLesson(serialized);
   orb.ToolTip=(hidden?"● 板书已隐藏":board.Tool=="mouse"?"● 鼠标穿透 · 操作下层软件":"● 标注中 · Esc 返回鼠标")+(saved?"":"  · 未保存");
   ReportToolChange();
  }
  bool MatchesSavedLesson(string active=null){
   if(savedPages.Length!=pages.Count)return false;
   if(active==null)active=Document.Save(board);
   for(int i=0;i<pages.Count;i++)if(savedTitles[i]!=pages[i].Title||savedPages[i]!=(i==pageIndex?active:pages[i].Snapshot))return false;
   return true;
  }
  void Commit(){
   string state=Document.Save(board);
   if(history.Count==0||state!=history[index]){
    if(noticeOffersUndo)CloseNotice();
    if(index+1<history.Count)history.RemoveRange(index+1,history.Count-index-1);
    history.Add(state);
    if(history.Count>60)history.RemoveAt(0);
    index=history.Count-1;
   }
   Refresh(state);
  }
  void Travel(int step){
   board.FinishActiveInput();
   int target=index+step;if(target<0||target>=history.Count)return;
   CloseNotice();Document.Load(board,history[target]);index=target;Refresh(history[target]);
  }
  IntPtr Hook(IntPtr h,int msg,IntPtr w,IntPtr l,ref bool handled){
   if(msg!=0x312)return IntPtr.Zero;
   int id=w.ToInt32();
   if(!registeredShortcuts.Contains(id))return IntPtr.Zero;
   handled=true;
   if(!IsEnabled||capturingScreen&&(screenView==null||id!=5))return IntPtr.Zero;
   try{shortcuts[id-1].Run();}catch(Exception error){MessageBox.Show(error.Message,"课堂笔 · 快捷键");}
   return IntPtr.Zero;
  }
  bool Confirm(){
   board.FinishActiveInput();
   if(MatchesSavedLesson())return true;
   var result=MessageBox.Show("是否保存当前整节课的板书？","课堂笔",MessageBoxButton.YesNoCancel);
   if(result==MessageBoxResult.Cancel)return false;
   if(result==MessageBoxResult.Yes){Save();return MatchesSavedLesson();}
   return true;
  }
  void Save(){
   board.FinishActiveInput();Commit();
   var dialog=new SaveFileDialog {Filter="课堂笔课程文件|*.classink",InitialDirectory=DefaultFolder(),FileName="课堂板书-"+DateTime.Now.ToString("yyyyMMdd-HHmmss")};
   if(dialog.ShowDialog(this)!=true)return;
   AtomicWrite(dialog.FileName,CurrentLesson().Save());
   RememberSavedLesson();Refresh(history[index]);
  }
  LessonDocument CurrentLesson(){return new LessonDocument(pages.Select(page=>page.Snapshot).ToArray(),pageIndex,pages.Select(page=>page.Title).ToArray());}
  void RememberSavedLesson(){savedPages=pages.Select(page=>page.Snapshot).ToArray();savedTitles=pages.Select(page=>page.Title).ToArray();}
  internal static void AtomicWrite(string path,string content){
   WriteFile(path,stream=>{using(var writer=new StreamWriter(stream,System.Text.Encoding.UTF8))writer.Write(content);});
  }
  static void WriteFile(string path,Action<Stream> write){
   string temp=path+"."+Guid.NewGuid().ToString("N")+".tmp";
   try{
    using(var stream=new FileStream(temp,FileMode.CreateNew,FileAccess.Write))write(stream);
    if(File.Exists(path))File.Replace(temp,path,null);else File.Move(temp,path);
   }finally{if(File.Exists(temp))File.Delete(temp);}
  }
  void Open(){
   if(!Confirm())return;
   var dialog=new OpenFileDialog{Filter="课堂笔课程或板书|*.classink",InitialDirectory=DefaultFolder()};
   if(dialog.ShowDialog(this)!=true)return;
   var lesson=LessonDocument.Load(File.ReadAllText(dialog.FileName));
   LoadLesson(lesson);
  }
  void LoadLesson(LessonDocument lesson){
   Document.Load(board,lesson.Pages[lesson.ActivePage]);
   pages.Clear();
   for(int i=0;i<lesson.Pages.Length;i++)pages.Add(new PageState(lesson.Pages[i]){Title=lesson.Titles[i]});
   deletedPage=null;
   pageIndex=lesson.ActivePage;pages[pageIndex]=new PageState(Document.Save(board)){Title=lesson.Titles[pageIndex]};
   RememberSavedLesson();
   CloseNotice();Refresh(history[index]);
  }
  ScreenView screenView;
  bool capturingScreen;
  async void OpenScreenView(bool crop){
   if(screenView!=null){if(!crop)screenView.Close();return;}
   if(capturingScreen)return;
   capturingScreen=true;
   bool timerVisible=timerWindow!=null&&timerWindow.IsVisible;
   BitmapSource pasted=null;
   try{
    board.FinishActiveInput();CloseNotice();if(flyout!=null)flyout.IsOpen=false;
    Hide();board.Hide();if(timerVisible)timerWindow.Hide();
    await System.Threading.Tasks.Task.Delay(180);
    var bounds=System.Windows.Forms.SystemInformation.VirtualScreen;
    BitmapSource screenshot;
    using(var bitmap=new System.Drawing.Bitmap(bounds.Width,bounds.Height,System.Drawing.Imaging.PixelFormat.Format24bppRgb)){
     using(var graphics=System.Drawing.Graphics.FromImage(bitmap))graphics.CopyFromScreen(bounds.Left,bounds.Top,0,0,bounds.Size);
     using(var stream=new MemoryStream()){
      bitmap.Save(stream,System.Drawing.Imaging.ImageFormat.Png);stream.Position=0;
      screenshot=BitmapFrame.Create(stream,BitmapCreateOptions.None,BitmapCacheOption.OnLoad);screenshot.Freeze();
     }
    }
    var view=new ScreenView(screenshot,board,crop);screenView=view;
    view.ShowDialog();pasted=view.Result;
   }catch(Exception error){MessageBox.Show(error.Message,"屏幕工具");}
   finally{screenView=null;board.Show();Show();if(timerVisible&&timerWindow!=null)timerWindow.Show();capturingScreen=false;}
   if(pasted!=null){
    var encoder=new PngBitmapEncoder();encoder.Frames.Add(BitmapFrame.Create(pasted));
    string data;using(var stream=new MemoryStream()){encoder.Save(stream);data=Convert.ToBase64String(stream.ToArray());}
    double scale=Math.Min(1,Math.Min(board.Width*0.8/pasted.PixelWidth,board.Height*0.8/pasted.PixelHeight));
    double width=pasted.PixelWidth*scale,height=pasted.PixelHeight*scale;
    var image=Document.Picture(pasted,data,(board.Width-width)/2,(board.Height-height)/2,width,height);
    board.Ink.Children.Insert(0,image);ActivateTool("select");board.Ink.Select(new UIElement[]{image});Commit();
    ShowNotice("截图已贴到当前页，可移动或缩放");
   }
  }
  async void Export(){
   var d=new SaveFileDialog {Filter="PNG 图片|*.png",InitialDirectory=DefaultFolder(),FileName="课堂屏幕-"+DateTime.Now.ToString("yyyyMMdd-HHmmss")};if(d.ShowDialog(this)!=true)return;
   string tool=board.Tool;try { CloseNotice();Hide();board.SetTool("mouse");await System.Threading.Tasks.Task.Delay(240);
    var bounds=System.Windows.Forms.SystemInformation.VirtualScreen;
    using(var bmp=new System.Drawing.Bitmap(bounds.Width,bounds.Height)) {using(var g=System.Drawing.Graphics.FromImage(bmp))g.CopyFromScreen(bounds.Left,bounds.Top,0,0,bmp.Size);bmp.Save(d.FileName,System.Drawing.Imaging.ImageFormat.Png);}
   }catch(Exception ex){MessageBox.Show(ex.Message,"导出失败");}finally{Show();board.SetTool(tool);Refresh();}
  }
  void ExportBoard(){
   board.FinishActiveInput();Commit();
   var dialog=new SaveFileDialog{Filter="PNG 图片|*.png",InitialDirectory=DefaultFolder(),FileName="板书-第"+(pageIndex+1)+"页"};
   if(dialog.ShowDialog(this)!=true)return;
   using(var exporter=new BoardExporter()){
    var encoder=new PngBitmapEncoder();encoder.Frames.Add(BitmapFrame.Create(exporter.Render(history[index],4096,4096,false)));
    WriteFile(dialog.FileName,encoder.Save);
   }
   ShowNotice("已导出当前板书 PNG");
  }
  void ExportLessonPdf(){
   board.FinishActiveInput();Commit();
   var dialog=new SaveFileDialog{Filter="PDF 文档|*.pdf",InitialDirectory=DefaultFolder(),FileName="课堂板书-"+DateTime.Now.ToString("yyyyMMdd-HHmmss")};
   if(dialog.ShowDialog(this)!=true)return;
   using(var exporter=new BoardExporter())WriteFile(dialog.FileName,stream=>exporter.WritePdf(stream,CurrentLesson()));
   ShowNotice("已导出整节课 PDF");
  }
  void Exit(){exiting=true;CloseNotice();clockTick.Stop();countdownAudio.Dispose();var handle=new WindowInteropHelper(this).Handle;foreach(int id in registeredShortcuts)Native.UnregisterHotKey(handle,id);Application.Current.Shutdown();}
 }
 public sealed class LessonClockWindow : Window {
  readonly int[] durationParts=new int[3];
  readonly TextBlock[] digits=new TextBlock[3];
  readonly StackPanel digitRow=new StackPanel{Orientation=Orientation.Horizontal};
  readonly Button start=new Button();
  readonly TextBlock finishedLabel=new TextBlock{FontSize=17,Height=25,HorizontalAlignment=HorizontalAlignment.Center,Foreground=Brushes.DarkGoldenrod};
  readonly bool countdown;
  readonly Action<bool,TimeSpan,bool> command;
  bool editingDuration;
  static Brush B(string hex){return (Brush)new BrushConverter().ConvertFromString(hex);}

  public LessonClockWindow(bool countdown,TimeSpan duration,Action<bool,TimeSpan,bool> callback){
   this.countdown=countdown;command=callback;
   SetDuration(duration);
   Title="课堂计时器";Width=Math.Min(680,SystemParameters.WorkArea.Width);Height=320;WindowStyle=WindowStyle.None;AllowsTransparency=true;ResizeMode=ResizeMode.NoResize;
   Topmost=true;WindowStartupLocation=WindowStartupLocation.CenterScreen;Background=Brushes.Transparent;FontFamily=new FontFamily("Microsoft YaHei UI");
   MinWidth=300;MinHeight=155;
   var windowLayout=new Grid();
   var frame=new Grid{Width=680,Height=320};
   var backdrop=new Border{Background=B("#D9F7F9FC"),BorderBrush=B("#A0263D62"),BorderThickness=new Thickness(2),CornerRadius=new CornerRadius(18),Cursor=Cursors.SizeAll,ToolTip="拖动移动 · Esc 关闭"};
   backdrop.MouseLeftButtonDown+=(s,e)=>DragMove();
   windowLayout.Children.Add(backdrop);
   windowLayout.Children.Add(new Viewbox{Child=frame,IsHitTestVisible=true});
   var close=new Button{Content="×",Width=34,Height=32,FontSize=21,Padding=new Thickness(0),HorizontalAlignment=HorizontalAlignment.Right,VerticalAlignment=VerticalAlignment.Top,Margin=new Thickness(0,12,14,0),ToolTip="关闭窗口（计时继续）"};
   StyleButton(close,false);close.Width=34;close.Height=32;close.FontSize=21;close.Margin=new Thickness(0,12,14,0);
   close.Click+=(s,e)=>Close();windowLayout.Children.Add(close);
   AddResizeHandles(windowLayout);
   Content=windowLayout;
   PreviewKeyDown+=(s,e)=>{if(e.Key==Key.Escape){Close();e.Handled=true;}};
   var root=new StackPanel{Width=620,VerticalAlignment=VerticalAlignment.Center};
   frame.Children.Add(root);
   root.Children.Add(new Viewbox{StretchDirection=StretchDirection.DownOnly,Width=600,Height=146,Child=digitRow});
   for(int i=0;i<digits.Length;i++){
    if(i>0)digitRow.Children.Add(new TextBlock{Text=":",FontFamily=new FontFamily("Segoe UI"),FontSize=128});
    int part=i;
    var digit=new TextBlock{FontFamily=new FontFamily("Segoe UI"),FontSize=128,Cursor=Cursors.SizeNS,
     ToolTip="滚轮调整"+new[]{"小时","分钟","秒钟"}[part]+" · Esc 关闭"};
    System.Windows.Documents.Typography.SetNumeralAlignment(digit,FontNumeralAlignment.Tabular);
    var hitArea=new Border{Child=digit,CornerRadius=new CornerRadius(12),Background=Brushes.Transparent};
    digits[i]=digit;digitRow.Children.Add(hitArea);
    hitArea.MouseEnter+=(s,e)=>hitArea.Background=B("#26356DE6");
    hitArea.MouseLeave+=(s,e)=>hitArea.Background=Brushes.Transparent;
    hitArea.MouseWheel+=(s,e)=>{AdjustDigits(part,e.Delta);e.Handled=true;};
   }
   root.Children.Add(finishedLabel);
   var actions=new StackPanel{Orientation=Orientation.Horizontal,HorizontalAlignment=HorizontalAlignment.Center,Margin=new Thickness(0,14,0,0)};
   root.Children.Add(actions);
   StyleButton(start,true);start.Click+=(s,e)=>Apply(false);actions.Children.Add(start);
   var reset=new Button{Content="重置"};StyleButton(reset,false);reset.Click+=(s,e)=>Apply(true);actions.Children.Add(reset);
   Update(countdown?duration:TimeSpan.Zero,false,false);
  }
  void AddResizeHandles(Grid layout){
   var template=(ControlTemplate)XamlReader.Parse("<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' TargetType='Thumb'><Border Background='#01000000'/></ControlTemplate>");
   for(int y=-1;y<=1;y++)for(int x=-1;x<=1;x++){
    if(x==0&&y==0)continue;
    int horizontal=x,vertical=y;
    var handle=new Thumb{Template=template,
     HorizontalAlignment=x<0?HorizontalAlignment.Left:x>0?HorizontalAlignment.Right:HorizontalAlignment.Stretch,
     VerticalAlignment=y<0?VerticalAlignment.Top:y>0?VerticalAlignment.Bottom:VerticalAlignment.Stretch,
     Cursor=x==0?Cursors.SizeNS:y==0?Cursors.SizeWE:x==y?Cursors.SizeNWSE:Cursors.SizeNESW};
    if(x!=0)handle.Width=y==0?8:16;
    if(y!=0)handle.Height=x==0?8:16;
    if(x==0)handle.Margin=new Thickness(16,0,16,0);
    if(y==0)handle.Margin=new Thickness(0,16,0,16);
    System.Drawing.Point origin=new System.Drawing.Point();
    Rect original=new Rect();
    handle.DragStarted+=(s,e)=>{origin=System.Windows.Forms.Cursor.Position;original=new Rect(Left,Top,ActualWidth,ActualHeight);};
    handle.DragDelta+=(s,e)=>{
     var pointer=System.Windows.Forms.Cursor.Position;
     var source=PresentationSource.FromVisual(this);
     if(source==null)return;
     var delta=source.CompositionTarget.TransformFromDevice.Transform(new Vector(pointer.X-origin.X,pointer.Y-origin.Y));
     Width=Math.Max(MinWidth,original.Width+horizontal*delta.X);
     Height=Math.Max(MinHeight,original.Height+vertical*delta.Y);
     if(horizontal<0)Left=original.Right-Width;
     if(vertical<0)Top=original.Bottom-Height;
    };
    layout.Children.Add(handle);
   }
  }
  static int Cycle(int current,int delta,int count){return (current+Math.Sign(delta)+count)%count;}
  public void SetDuration(TimeSpan duration){
   durationParts[0]=(int)duration.TotalHours;
   durationParts[1]=duration.Minutes;
   durationParts[2]=duration.Seconds;
   editingDuration=false;
  }
  void AdjustDigits(int part,int delta){
   if(!countdown)return;
   durationParts[part]=Cycle(durationParts[part],delta,part==0?100:60);
   if(SelectedDuration()==TimeSpan.Zero)durationParts[2]=1;
   editingDuration=true;
   Update(SelectedDuration(),false,false);
  }
  static void StyleButton(Button button,bool primary){
   button.Width=112;button.Height=42;button.Margin=new Thickness(6,0,6,0);button.FontSize=14;button.Cursor=Cursors.Hand;
   button.Foreground=primary?Brushes.White:B("#50617A");button.Background=B(primary?"#356DE6":"#E9EEF6");button.BorderThickness=new Thickness(0);
   button.Template=(ControlTemplate)XamlReader.Parse("<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' TargetType='Button'><Border Background='{TemplateBinding Background}' CornerRadius='10'><ContentPresenter HorizontalAlignment='Center' VerticalAlignment='Center'/></Border><ControlTemplate.Triggers><Trigger Property='IsMouseOver' Value='True'><Setter Property='Opacity' Value='0.85'/></Trigger></ControlTemplate.Triggers></ControlTemplate>");
  }
  TimeSpan SelectedDuration(){return new TimeSpan(durationParts[0],durationParts[1],durationParts[2]);}
  void Apply(bool reset){
   editingDuration=false;
   command(countdown,SelectedDuration(),reset);
  }
  public void Update(TimeSpan value,bool running,bool finished){
   if(editingDuration)value=SelectedDuration();
   digits[0].Text=((int)value.TotalHours).ToString("00");
   digits[1].Text=value.Minutes.ToString("00");
   digits[2].Text=value.Seconds.ToString("00");
   var foreground=B(finished&&!editingDuration?"#B45309":"#263D62");
   foreach(var digit in digits)digit.Foreground=foreground;
   foreach(var separator in digitRow.Children.OfType<TextBlock>())separator.Foreground=foreground;
   finishedLabel.Text=finished&&!editingDuration?"时间到":"";
   start.Content=running&&!editingDuration?"暂停":"开始";
  }
 }
 public static class SelfTest {
  static bool HasLaserPixels(LaserRipples effect){
   var bitmap=new RenderTargetBitmap(120,120,96,96,PixelFormats.Pbgra32);
   effect.UpdateLayout();bitmap.Render(effect);
   var pixels=new byte[120*120*4];bitmap.CopyPixels(pixels,120*4,0);
   return pixels.Any(value=>value!=0);
  }
  public static void Run(){
   var mouseStroke=new MouseInkStroke(new Point(10,10),new DrawingAttributes{Width=4,Height=4});
   mouseStroke.Append(new Point(72,10));mouseStroke.Append(new Point(72,64));mouseStroke.Complete(new Point(72,68));
   var samples=mouseStroke.InkStroke.StylusPoints;
   if(samples.Count<20||samples[0].X!=10||samples[samples.Count-1].Y!=68||!mouseStroke.InkStroke.HitTest(new Point(40,10),2))
    throw new Exception("Sparse mouse movement did not produce continuous ink");
   var tap=new MouseInkStroke(new Point(15,15),new DrawingAttributes());tap.Complete(new Point(15,15));
   if(tap.InkStroke.StylusPoints.Count!=1)throw new Exception("Mouse tap did not preserve a single dot");
   var effect=new LaserRipples();effect.Measure(new Size(120,120));effect.Arrange(new Rect(0,0,120,120));
   effect.Click(new Point(60,60));
   if(!HasLaserPixels(effect))throw new Exception("Laser ripple did not render");
   Desk.PumpAnimation(600);
   if(!HasLaserPixels(effect))throw new Exception("Laser dot disappeared after the ripple faded");
   effect.End();
   if(HasLaserPixels(effect))throw new Exception("Laser ripple did not disappear");
   var board=new Board();foreach(string shape in new[]{"line","arrow","rect","ellipse"})board.Ink.Strokes.Add(Board.Shape(shape,new Point(10,20),new Point(100,160),Colors.Coral,4));
   board.AddText("算法 <演示>\n你好 & Python",32,48,"#FF123456",24);board.SetWhite(true);string xml=Document.Save(board);var copy=new Board();Document.Load(copy,xml);
   if(copy.Ink.Strokes.Count!=4||copy.Ink.Children.Count!=1||!copy.White||((TextBlock)copy.Ink.Children[0]).Text!="算法 <演示>\n你好 & Python")throw new Exception("Round trip failed");
   if(Document.Save(copy)!=xml)throw new Exception("Serialization is unstable");
   var sample=BitmapSource.Create(2,2,96,96,PixelFormats.Bgr24,null,new byte[12],6);
   var png=new PngBitmapEncoder();png.Frames.Add(BitmapFrame.Create(sample));
   using(var stream=new MemoryStream()){
    png.Save(stream);
    copy.Ink.Children.Insert(0,Document.Picture(sample,Convert.ToBase64String(stream.ToArray()),20,30,120,80));
   }
   string imagePage=Document.Save(copy);Document.Load(copy,imagePage);
   var restored=copy.Ink.Children.OfType<Image>().Single();
   if(restored.Width!=120||InkCanvas.GetLeft(restored)!=20||Document.Save(copy)!=imagePage)throw new Exception("Screenshot did not round-trip");
   Document.Load(copy,xml);
   copy.SetSurface("green");Document.Load(copy,Document.Save(copy));
   if(copy.Surface!="green")throw new Exception("Chalkboard background was not preserved");
   Document.Load(copy,xml.Replace(" surface=\"white\"",""));
   if(!copy.White)throw new Exception("Legacy whiteboard background was not restored");
   copy.BeginText(new Point(100,120));var entry=copy.Ink.Children.OfType<TextBox>().Single();if(((SolidColorBrush)entry.Background).Color.A!=0)throw new Exception("Text editor background is not transparent");entry.Text="中文输入\nprint(123)";Document.Save(copy);copy.FinishText(true,true);
   if(copy.Ink.GetSelectedElements().Count!=1)throw new Exception("Committed text was not selected");
   var typed=copy.Ink.Children.OfType<TextBlock>().Last();if(typed.Text!="中文输入\nprint(123)"||InkCanvas.GetLeft(typed)!=100||InkCanvas.GetTop(typed)!=120||copy.Ink.Children.OfType<TextBox>().Any())throw new Exception("Inline text commit failed");
   string committed=Document.Save(copy);copy.BeginText(new Point(40,60));copy.Ink.Children.OfType<TextBox>().Single().Text="取消的文字";copy.FinishText(false);if(Document.Save(copy)!=committed)throw new Exception("Inline text cancellation changed board");Document.Load(copy,xml);
   bool rejected=false;try{Document.Load(copy,"<!DOCTYPE x [<!ENTITY e SYSTEM 'file:///no'>]><x>&e;</x>");}catch(XmlException){rejected=true;}if(!rejected)throw new Exception("DTD accepted");
   string path=Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"self-test.classink");Desk.AtomicWrite(path,xml);Desk.AtomicWrite(path,xml);File.Delete(path);
   var target=new RenderTargetBitmap(400,300,96,96,PixelFormats.Pbgra32);copy.Ink.Measure(new Size(400,300));copy.Ink.Arrange(new Rect(0,0,400,300));target.Render(copy.Ink);
   var encoder=new PngBitmapEncoder();encoder.Frames.Add(BitmapFrame.Create(target));using(var f=File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"self-test.png")))encoder.Save(f);
   File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"self-test.txt"),"PASS: four shapes, multilingual text, editable round trip, DTD rejection, atomic overwrite, PNG rendering.");
   var desk=new Desk();desk.VerifyInteractions();desk.VerifyLessonPages();desk.VerifyMotion();desk.RenderPreview(false,Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"orb-preview.png"));desk.RenderPreview(true,Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"toolbar-preview.png"));
   File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"self-test.txt"),"\nPASS: multi-page save/load, titles, independent duplicates, delete/restore, legacy import, per-page undo, annotation visibility and PNG/PDF export.");
   desk.RenderChoicePreview(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"choices-preview.png"));
   desk.VerifyCountdownAndRender(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"timer-preview.png"));
   File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"self-test.txt"),"\nPASS: transparent text editor, countdown completion/reset, large timer window rendering.");
   File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"self-test.txt"),"\nPASS: inline multilingual text commit/cancel and serialization, rail/orb equal width, side choice panel rendering.");
   File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"self-test.txt"),"\nPASS: animated slide/fade, continuous reversal, completion cancellation, collapsed hit testing.");
   File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"self-test.txt"),"\nPASS: clear/undo/redo with text and ink, orb position and docking, stopwatch pause/resume/reset and collapsed timing.");
   File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"self-test.txt"),"\nPASS: collapsed orb and vertical toolbar fit screen bounds.");
  }
 }
}
