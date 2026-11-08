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
[assembly: System.Reflection.AssemblyVersion("1.0.0.0")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.0.0")]

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
    case "save":path="M4,3 L14,3 17,6 17,17 3,17 3,3 Z M7,3 L7,8 13,8 13,3 M6,17 L6,12 14,12 14,17";break;
    case "open":path="M3,7 L3,4 8,4 10,7 17,7 M3,7 L17,7 15,17 3,17 Z";break;
    case "export":path="M4,12 L4,17 16,17 16,12 M10,3 L10,13 M6,7 L10,3 14,7";break;
    case "exit":path="M8,4 L3,4 3,17 8,17 M8,10 L18,10 M14,6 L18,10 14,14";break;
    case "settings":path="M8,3 L12,3 13,5 16,5 18,8 17,10 18,12 16,15 13,15 12,17 8,17 7,15 4,15 2,12 3,10 2,8 4,5 7,5 Z M13,10 A3,3 0 1 1 7,10 A3,3 0 1 1 13,10";break;
   }
   if(path.Length>0)dc.DrawGeometry(null,new Pen(brush,1.5){StartLineCap=PenLineCap.Round,EndLineCap=PenLineCap.Round,LineJoin=PenLineJoin.Round},Geometry.Parse(path));
   if(smallArrow)dc.Pop();
  }
 }
 public static class Program {
  [STAThread] public static void Main(string[] args) {
   if(args.Contains("--self-test")) { SelfTest.Run(); return; }
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
  sealed class Pulse {public Point Center;public double Started;}
  Pulse pulse;
  readonly System.Diagnostics.Stopwatch clock=System.Diagnostics.Stopwatch.StartNew();
  readonly System.Windows.Threading.DispatcherTimer animation=new System.Windows.Threading.DispatcherTimer();
  const double Lifetime=0.42;
  Point point;
  bool isPointing;
  public LaserRipples(){
   IsHitTestVisible=false;
   animation.Interval=TimeSpan.FromMilliseconds(16);
   animation.Tick+=(s,e)=>{
    double now=clock.Elapsed.TotalSeconds;
    if(pulse!=null&&now-pulse.Started>=Lifetime)pulse=null;
    if(pulse==null)animation.Stop();
    InvalidateVisual();
   };
  }
  public void Click(Point location){Move(location);pulse=new Pulse{Center=location,Started=clock.Elapsed.TotalSeconds};animation.Start();}
  public void Move(Point location){point=location;isPointing=true;InvalidateVisual();}
  public void End(){isPointing=false;InvalidateVisual();}
  public void Clear(){isPointing=false;pulse=null;animation.Stop();InvalidateVisual();}
  protected override void OnRender(DrawingContext dc){
   double now=clock.Elapsed.TotalSeconds;
   if(pulse!=null){
    double age=Math.Min(1,(now-pulse.Started)/Lifetime);
    double radius=10+25*(1-Math.Pow(1-age,2));
    byte alpha=(byte)(160*(1-age)*(1-age));
    var brush=new SolidColorBrush(Color.FromArgb(alpha,255,0,0));
    dc.DrawEllipse(null,new Pen(brush,2.8-age*1.5),pulse.Center,radius,radius);
   }
   if(isPointing){
    dc.DrawEllipse(null,new Pen(new SolidColorBrush(Color.FromArgb(155,255,0,0)),1.5),point,17,17);
    dc.DrawEllipse(Brushes.Red,new Pen(Brushes.White,1.25),point,9,9);
   }
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
  Point start; Stroke preview; bool drawing; string before;
  TextBox editor;
  Point textPosition;
  double textResizeScale=1;
  public string Surface {get;private set;}
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
   Ink.LostMouseCapture+=(s,e)=>{if(drawing)Finish();};
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
   if(Tool=="laser"&&tool!="laser")laser.Clear();
   FinishText();
   Tool=tool; ApplyPen();
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
   before=Document.Save(this);
   if(Tool=="text") { e.Handled=true;BeginText(e.GetPosition(Ink));return; }
   if(Tool=="line"||Tool=="arrow"||Tool=="rect"||Tool=="ellipse") { start=e.GetPosition(Ink); drawing=true; Ink.CaptureMouse(); e.Handled=true; }
  }
  void Move(object s,MouseEventArgs e) {
   if(Tool=="laser"){laser.Move(e.GetPosition(Ink));return;}
   if(!drawing) return; if(preview!=null) Ink.Strokes.Remove(preview);
   preview=Shape(Tool,start,e.GetPosition(Ink),Color,Size); Ink.Strokes.Add(preview);
  }
  void Up(object s,MouseButtonEventArgs e) { if(Tool=="laser"){e.Handled=true;return;} if(drawing) { Move(s,e); Finish(); e.Handled=true; } else if(before!=Document.Save(this)) Changed(); }
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
 public static class Document {
  public static string Save(Board b) {
   var doc=new XmlDocument(); var root=doc.CreateElement("ClassInk"); root.SetAttribute("version","1");root.SetAttribute("white",b.White.ToString());root.SetAttribute("surface",b.Surface);doc.AppendChild(root);
   var ink=doc.CreateElement("Strokes"); using(var ms=new MemoryStream()) { b.Ink.Strokes.Save(ms);ink.InnerText=Convert.ToBase64String(ms.ToArray()); }root.AppendChild(ink);
   foreach(TextBlock t in b.Ink.Children.OfType<TextBlock>()) { var n=doc.CreateElement("Text");n.SetAttribute("x",InkCanvas.GetLeft(t).ToString(CultureInfo.InvariantCulture));n.SetAttribute("y",InkCanvas.GetTop(t).ToString(CultureInfo.InvariantCulture));n.SetAttribute("size",t.FontSize.ToString(CultureInfo.InvariantCulture));n.SetAttribute("color",t.Foreground.ToString());n.InnerText=t.Text;root.AppendChild(n); }
   return doc.OuterXml;
  }
  public static void Load(Board b,string xml) {
   var doc=new XmlDocument { XmlResolver=null }; using(var reader=XmlReader.Create(new StringReader(xml),new XmlReaderSettings { DtdProcessing=DtdProcessing.Prohibit, MaxCharactersInDocument=20000000, XmlResolver=null })) doc.Load(reader);
   var root=doc.DocumentElement; if(root.Name!="ClassInk"||root.GetAttribute("version")!="1") throw new InvalidDataException("不是支持的课堂笔文件。");
   StrokeCollection strokes; using(var ms=new MemoryStream(Convert.FromBase64String(root["Strokes"].InnerText))) strokes=new StrokeCollection(ms);
   var texts=new List<Tuple<string,double,double,string,double>>();
   foreach(XmlElement n in root.SelectNodes("Text")) { double x=Double.Parse(n.GetAttribute("x"),CultureInfo.InvariantCulture),y=Double.Parse(n.GetAttribute("y"),CultureInfo.InvariantCulture),size=Double.Parse(n.GetAttribute("size"),CultureInfo.InvariantCulture); if(Double.IsNaN(x)||Double.IsInfinity(x)||Double.IsNaN(y)||Double.IsInfinity(y)||size<1||size>500||Double.IsNaN(size)) throw new InvalidDataException("文字数据无效。"); new BrushConverter().ConvertFromString(n.GetAttribute("color")); texts.Add(Tuple.Create(n.InnerText,x,y,n.GetAttribute("color"),size)); }
   b.FinishText(false);b.Ink.Strokes=strokes; b.Ink.Children.Clear();foreach(var t in texts)b.AddText(t.Item1,t.Item2,t.Item3,t.Item4,t.Item5);
   if(root.HasAttribute("surface"))b.SetSurface(root.GetAttribute("surface"));else b.SetWhite(root.GetAttribute("white")=="True");
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
  Board board=new Board(); StackPanel panel; Border card; Button orb; TextBlock status; readonly List<string> history=new List<string>(); int index; bool collapsed; bool exiting; bool saved=true; string savedState;
  readonly Dictionary<string,Button> tools=new Dictionary<string,Button>();
  static readonly string[] boardSurfaces={"screen","white","black","green"};
  int themeIndex=ReadTheme();
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
   var row=new StackPanel {Orientation=Orientation.Horizontal};
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
   row.Children.Add(Button("计时音效",()=>ShowSoundSettings(anchor)));
   OpenFlyout(anchor,row);
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
  static string CountdownPreferencePath(){return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"ClassInk","countdown-duration.txt");}
  static TimeSpan ReadCountdownDuration(){
   try{
    int seconds;
    if(Int32.TryParse(File.ReadAllText(CountdownPreferencePath()),out seconds)&&seconds>0&&seconds<360000)return TimeSpan.FromSeconds(seconds);
   }catch(IOException){}catch(UnauthorizedAccessException){}
   return TimeSpan.FromMinutes(5);
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
   public void Refresh(){Main.Content=new ToolIcon(Icons[Index]);Main.ToolTip=Names[Index]+" · 点击或右键展开选项";bool active=Immediate&&Desk.board.Tool==Icons[Index];Main.Background=active?Desk.ActiveFill:Brushes.Transparent;Main.Foreground=active?Desk.ActiveInk:Desk.UiInk;}
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
   var row=new Grid();group.Main=Button("",()=>ShowChoice(group));group.Main.MinWidth=30;row.Children.Add(group.Main);
   row.Children.Add(new TextBlock{Text="›",FontSize=10,Foreground=B("#94A3B8"),HorizontalAlignment=HorizontalAlignment.Right,VerticalAlignment=VerticalAlignment.Bottom,Margin=new Thickness(0,0,3,4),IsHitTestVisible=false});
   group.Main.MouseRightButtonUp+=(s,e)=>{ShowChoice(group);e.Handled=true;};
   if(icons[0]=="thin"){
    group.Main.MouseWheel+=(s,e)=>{
     if(board.Tool!="pen"&&board.Tool!="highlight")return;
     group.Step(Math.Sign(e.Delta));e.Handled=true;
    };
   }else{
    var hover=new System.Windows.Threading.DispatcherTimer {Interval=TimeSpan.FromMilliseconds(180)};
    hover.Tick+=(s,e)=>{hover.Stop();if(IsLoaded&&!collapsed&&group.Main.IsMouseOver)ShowChoice(group);};
    group.Main.MouseEnter+=(s,e)=>hover.Start();group.Main.MouseLeave+=(s,e)=>hover.Stop();
   }
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
   var widths=new[]{2.0,4.0,6.0,10.0,16.0};
   AddChoice(new[]{"thin","medium","bold","thick","heavy"},widths.Select(width=>width+" px · 滚轮调节").ToArray(),i=>{board.Size=widths[i];board.ApplyPen();},true,1);
   Separator();
   AddChoice(new[]{"undo","redo"},new[]{"撤销","重做"},i=>Travel(i==0?-1:1),false);
   AddChoice(new[]{"screen","board","blackboard","greenboard"},new[]{"屏幕标注","白板","黑板","仿真绿板"},i=>{board.SetSurface(boardSurfaces[i]);Commit();},false);
   AddIcon("trash","一键清空 · 可撤销",ClearBoard);
   AddChoice(new[]{"save","open","export"},new[]{"保存笔迹","打开笔迹","导出 PNG"},i=>{if(i==0)Save();else if(i==1)Open();else Export();},false);
   Separator();
   clockText=new TextBlock {Text="00:00",FontFamily=new FontFamily("Consolas"),FontSize=10,HorizontalAlignment=HorizontalAlignment.Center};
   var clockFace=new StackPanel {Orientation=Orientation.Vertical};clockFace.Children.Add(new ToolIcon("clock"){HorizontalAlignment=HorizontalAlignment.Center});clockFace.Children.Add(clockText);
   clockButton=Button("",ShowClock);clockButton.Content=clockFace;clockButton.ToolTip="打开课堂计时器";clockButton.Height=44;panel.Children.Add(clockButton);
   clockButton.MouseRightButtonUp+=(s,e)=>{ResetClock();e.Handled=true;};
   AddIcon("reset","重置计时",ResetClock);
   Button settings=null;
   settings=Button("",()=>ShowSettings(settings));settings.Content=new ToolIcon("settings");settings.ToolTip="设置外观和计时音效";panel.Children.Add(settings);
   AddIcon("exit","直接退出",Exit);
   clockTick.Interval=TimeSpan.FromMilliseconds(200);clockTick.Tick+=(s,e)=>UpdateClock();clockTick.Start();
   SetCollapsed(true);
   ApplyTheme(themeIndex);
   noticeTimer.Tick+=(s,e)=>CloseNotice();
   PreviewKeyDown+=(s,e)=>{if(e.Key==Key.Escape&&board.Tool!="mouse"){if(flyout!=null)flyout.IsOpen=false;ActivateTool("mouse");e.Handled=true;}};
   Loaded+=(s,e)=>{board.Show();Owner=board;board.SetTool("mouse");savedState=Document.Save(board);history.Add(savedState);board.Changed=Commit;Refresh();var h=new WindowInteropHelper(this).Handle;HwndSource.FromHwnd(h).AddHook(Hook);if(!Native.RegisterHotKey(h,1,0x4003,0x44))orb.ToolTip+="\n快捷键已占用，请用鼠标按钮切换。";Activate();};
   Closing+=(s,e)=>{if(exiting)return;e.Cancel=true;Exit();};
  }
  static ControlTemplate SoftButtonTemplate(){return (ControlTemplate)XamlReader.Parse("<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' TargetType='Button'><Grid><Border Background='{TemplateBinding Background}' BorderBrush='{TemplateBinding BorderBrush}' BorderThickness='{TemplateBinding BorderThickness}' CornerRadius='8'/><Border x:Name='hover' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' Background='#0C356DE6' CornerRadius='8' Opacity='0'/><ContentPresenter HorizontalAlignment='Center' VerticalAlignment='Center' Margin='{TemplateBinding Padding}'/></Grid><ControlTemplate.Triggers><Trigger Property='IsMouseOver' Value='True'><Setter TargetName='hover' Property='Opacity' Value='1'/></Trigger><Trigger Property='IsPressed' Value='True'><Setter TargetName='hover' Property='Background' Value='#20356DE6'/></Trigger><Trigger Property='IsEnabled' Value='False'><Setter Property='Opacity' Value='0.35'/></Trigger></ControlTemplate.Triggers></ControlTemplate>");}
  static ControlTemplate SliderTemplate(){return (ControlTemplate)XamlReader.Parse("<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' TargetType='Slider'><Grid Height='24'><Border Height='3' Background='#E3E9F1' CornerRadius='2' Margin='5,0'/><Track Name='PART_Track' Minimum='{TemplateBinding Minimum}' Maximum='{TemplateBinding Maximum}' Value='{Binding Value, RelativeSource={RelativeSource TemplatedParent}, Mode=TwoWay}'><Track.DecreaseRepeatButton><RepeatButton Command='Slider.DecreaseLarge' Opacity='0'/></Track.DecreaseRepeatButton><Track.Thumb><Thumb Width='12' Height='12'><Thumb.Template><ControlTemplate TargetType='Thumb'><Ellipse Fill='#356DE6' Stroke='White' StrokeThickness='2'/></ControlTemplate></Thumb.Template></Thumb></Track.Thumb><Track.IncreaseRepeatButton><RepeatButton Command='Slider.IncreaseLarge' Opacity='0'/></Track.IncreaseRepeatButton></Track></Grid></ControlTemplate>");}
  void Separator(){panel.Children.Add(new Border {Height=1,Background=B("#E4E9F0"),Margin=new Thickness(4,7,4,7),VerticalAlignment=VerticalAlignment.Center});}
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
   history.Clear();index=0;savedState=Document.Save(board);history.Add(savedState);
   board.Ink.Strokes.Add(Board.Shape("arrow",new Point(10,20),new Point(100,100),Colors.Coral,4));board.AddText("课堂测试",20,30,"#FF123456",24);Commit();ClearBoard();
   if(board.Ink.Strokes.Count!=0||board.Ink.Children.Count!=0)throw new Exception("One-click clear failed");Travel(-1);
   if(board.Ink.Strokes.Count!=1||board.Ink.Children.Count!=1)throw new Exception("Undo clear failed");Travel(1);
   if(board.Ink.Strokes.Count!=0||board.Ink.Children.Count!=0)throw new Exception("Redo clear failed");
   BeginOrbDrag(new Point(0,0));MoveOrb(new Point(160,120));EndOrbDrag();var position=floatPosition;
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
  Button Button(string text,Action action){var b=new Button {Content=text,MinWidth=30,Height=34,Margin=new Thickness(2),Foreground=UiInk,Background=Brushes.Transparent,BorderThickness=new Thickness(0),FontSize=11,FontFamily=new FontFamily("Microsoft YaHei UI"),Cursor=Cursors.Hand,Padding=new Thickness(2)};if(text=="undo"||text=="redo")b.Content=new ToolIcon(text);b.Template=SoftButtonTemplate();b.Click+=(s,e)=>{try{action();}catch(Exception ex){MessageBox.Show(ex.Message,"课堂笔 · 操作未完成");}};return b;}
  void AddTool(Panel grid,string key,string text){var b=Button(text,()=>{board.SetTool(key);Refresh();});b.Content=new ToolIcon(key);b.ToolTip=text;tools[key]=b;grid.Children.Add(b);}
  void Refresh(){foreach(var group in choices){int selected=group.Icons[0]=="screen"?Array.IndexOf(boardSurfaces,board.Surface):Array.IndexOf(group.Icons,board.Tool);if(selected>=0)group.Index=selected;group.Refresh();}foreach(var kv in tools){bool active=kv.Key==board.Tool;kv.Value.Background=active?ActiveFill:Brushes.Transparent;kv.Value.Foreground=active?ActiveInk:UiInk;((ToolIcon)kv.Value.Content).InvalidateVisual();}saved=Document.Save(board)==savedState;orb.ToolTip=(board.Tool=="mouse"?"● 鼠标穿透 · 操作下层软件":"● 标注中 · Esc 返回鼠标")+(saved?"":"  · 未保存");ReportToolChange();}
  void Commit(){string state=Document.Save(board);if(history.Count==0||state!=history[index]){if(noticeOffersUndo)CloseNotice();if(index+1<history.Count)history.RemoveRange(index+1,history.Count-index-1);history.Add(state);if(history.Count>60)history.RemoveAt(0);index=history.Count-1;}Refresh();}
  void Travel(int step){int n=index+step;if(n<0||n>=history.Count)return;CloseNotice();Document.Load(board,history[n]);index=n;Refresh();}
  IntPtr Hook(IntPtr h,int msg,IntPtr w,IntPtr l,ref bool handled){if(msg==0x312&&w.ToInt32()==1){board.SetTool(board.Tool=="mouse"?"pen":"mouse");Refresh();handled=true;}return IntPtr.Zero;}
  bool Confirm(){if(Document.Save(board)==savedState)return true;var r=MessageBox.Show("是否保存当前笔迹？","课堂笔",MessageBoxButton.YesNoCancel);if(r==MessageBoxResult.Cancel)return false;if(r==MessageBoxResult.Yes){Save();return Document.Save(board)==savedState;}return true;}
  void Save(){board.FinishText();var d=new SaveFileDialog {Filter="课堂笔可编辑文件|*.classink",FileName="课堂板书-"+DateTime.Now.ToString("yyyyMMdd-HHmmss")};if(d.ShowDialog(this)!=true)return;string xml=Document.Save(board);AtomicWrite(d.FileName,xml);savedState=xml;Refresh();}
  internal static void AtomicWrite(string path,string content){string temp=path+"."+Guid.NewGuid().ToString("N")+".tmp";try{File.WriteAllText(temp,content,System.Text.Encoding.UTF8);if(File.Exists(path))File.Replace(temp,path,null);else File.Move(temp,path);}finally{if(File.Exists(temp))File.Delete(temp);}}
  void Open(){if(!Confirm())return;var d=new OpenFileDialog {Filter="课堂笔可编辑文件|*.classink"};if(d.ShowDialog(this)!=true)return;Document.Load(board,File.ReadAllText(d.FileName));Commit();savedState=Document.Save(board);Refresh();}
  async void Export(){
   var d=new SaveFileDialog {Filter="PNG 图片|*.png",FileName="课堂板书-"+DateTime.Now.ToString("yyyyMMdd-HHmmss")};if(d.ShowDialog(this)!=true)return;
   string tool=board.Tool;try { CloseNotice();Hide();board.SetTool("mouse");await System.Threading.Tasks.Task.Delay(240);
    var bounds=System.Windows.Forms.SystemInformation.VirtualScreen;
    using(var bmp=new System.Drawing.Bitmap(bounds.Width,bounds.Height)) {using(var g=System.Drawing.Graphics.FromImage(bmp))g.CopyFromScreen(bounds.Left,bounds.Top,0,0,bmp.Size);bmp.Save(d.FileName,System.Drawing.Imaging.ImageFormat.Png);}
   }catch(Exception ex){MessageBox.Show(ex.Message,"导出失败");}finally{Show();board.SetTool(tool);Refresh();}
  }
  void Exit(){exiting=true;CloseNotice();clockTick.Stop();countdownAudio.Dispose();Native.UnregisterHotKey(new WindowInteropHelper(this).Handle,1);Application.Current.Shutdown();}
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
   durationParts[0]=(int)duration.TotalHours;durationParts[1]=duration.Minutes;durationParts[2]=duration.Seconds;
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
   var desk=new Desk();desk.VerifyInteractions();desk.VerifyMotion();desk.RenderPreview(false,Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"orb-preview.png"));desk.RenderPreview(true,Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"toolbar-preview.png"));
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
