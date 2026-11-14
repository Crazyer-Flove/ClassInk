#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")/.."
mkdir -p dist
work=$(mktemp -d)
trap 'rm -rf "$work"' EXIT
app="$work/image/ClassInk.app"
mkdir -p "$app/Contents/MacOS" "$app/Contents/Resources"
for arch in arm64 x86_64; do
  xcrun swiftc -O -target "$arch-apple-macos13.0" macos/main.swift -o "$work/$arch"
done
lipo -create "$work/arm64" "$work/x86_64" -output "$app/Contents/MacOS/ClassInk"
cp assets/support-*.jpg LICENSE "$app/Contents/Resources/"
cp macos/Info.plist "$app/Contents/Info.plist"
codesign --force --sign - "$app"
codesign --verify --strict "$app"
ln -s /Applications "$work/image/Applications"
hdiutil create -volname ClassInk -srcfolder "$work/image" -format UDZO -ov dist/ClassInk-macOS.dmg
hdiutil verify dist/ClassInk-macOS.dmg
