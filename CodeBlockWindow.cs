using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
namespace ClassInk {
 public sealed class CodeBlockWindow : Window {
  bool light,composing;
  readonly TextBox editor=new TextBox{AcceptsReturn=true,AcceptsTab=true,FontFamily=new FontFamily("Consolas"),FontSize=16,HorizontalScrollBarVisibility=ScrollBarVisibility.Auto,VerticalScrollBarVisibility=ScrollBarVisibility.Auto,TextWrapping=TextWrapping.NoWrap,Padding=new Thickness(12),BorderThickness=new Thickness(0),Text="// Hello, ClassInk!\nfunction greet(name) {\n  return \"Hello, \" + name;\n}"};
  readonly ComboBox language=new ComboBox{Width=150,Margin=new Thickness(0,0,12,0)};
  readonly TextBlock code=new TextBlock{FontFamily=new FontFamily("Consolas"),FontSize=16,Foreground=Brushes.White};
  readonly Border card=new Border{Background=new SolidColorBrush(Color.FromRgb(24,30,42)),Padding=new Thickness(24),CornerRadius=new CornerRadius(12),HorizontalAlignment=HorizontalAlignment.Left,VerticalAlignment=VerticalAlignment.Top};
  readonly System.Windows.Threading.DispatcherTimer refresh=new System.Windows.Threading.DispatcherTimer{Interval=TimeSpan.FromMilliseconds(180)};
  static Brush ColorBrush(string value){return (Brush)new BrushConverter().ConvertFromString(value);}
  public CodeBlockWindow(){
   Title="课堂笔 · 代码块";Width=960;Height=640;MinWidth=640;MinHeight=420;WindowStartupLocation=WindowStartupLocation.CenterScreen;Background=ColorBrush("#181E2A");
   var root=new DockPanel{Margin=new Thickness(24)};Content=root;
   var bar=new StackPanel{Orientation=Orientation.Horizontal,Margin=new Thickness(0,0,0,24)};var dots=EditorChrome.Dots();dots.Margin=new Thickness(0,0,24,0);dots.VerticalAlignment=VerticalAlignment.Center;bar.Children.Add(dots);DockPanel.SetDock(bar,Dock.Top);root.Children.Add(bar);
   foreach(string name in new[]{"JavaScript","TypeScript","Python","C#","C++","Java","JSON","HTML","CSS","SQL","纯文本"})language.Items.Add(name);language.SelectedIndex=0;bar.Children.Add(language);
   var export=new Button{Content="导出 PNG",Padding=new Thickness(16,6,16,6)};bar.Children.Add(export);export.Click+=(s,e)=>Export();
   EditorChrome.Style(export);export.ToolTip="导出完整代码图片";
   var theme=new Button{Content="◐",ToolTip="切换黑白风格",Padding=new Thickness(12,6,12,6),Margin=new Thickness(10,0,0,0)};EditorChrome.Style(theme);bar.Children.Add(theme);
   export.Foreground=Brushes.White;theme.Foreground=Brushes.White;
   var editLayer=new Grid();code.Margin=new Thickness(2,0,2,0);code.IsHitTestVisible=false;editLayer.Children.Add(code);
   editor.Padding=new Thickness(0);editor.Background=Brushes.Transparent;editor.Foreground=Brushes.Transparent;editor.CaretBrush=Brushes.White;editor.SelectionOpacity=0.35;editor.HorizontalScrollBarVisibility=ScrollBarVisibility.Disabled;editor.VerticalScrollBarVisibility=ScrollBarVisibility.Disabled;editor.MinWidth=480;editor.MinHeight=300;editor.AcceptsTab=false;
   editor.PreviewKeyDown+=(s,e)=>{if(e.Key==System.Windows.Input.Key.Tab){int pos=editor.SelectionStart;editor.SelectedText="    ";editor.SelectionStart=pos+4;e.Handled=true;}};
   editor.Text=editor.Text.Replace("\t","    ");editLayer.Children.Add(editor);
   var preview=new ScrollViewer{Content=editLayer,HorizontalScrollBarVisibility=ScrollBarVisibility.Auto,VerticalScrollBarVisibility=ScrollBarVisibility.Auto};root.Children.Add(preview);EditorChrome.EnableTextZoom(this,editLayer);preview.ToolTip="Ctrl + 滚轮调整显示大小（50%–300%）";
   theme.Click+=(s,e)=>{light=!light;Background=card.Background=ColorBrush(light?"#FAFBFD":"#181E2A");export.Foreground=theme.Foreground=ColorBrush(light?"#24292F":"#E6EDF3");code.Foreground=ColorBrush(light?"#24292F":"#E6EDF3");editor.CaretBrush=code.Foreground;if(composing)editor.Foreground=code.Foreground;Highlight();};
      System.Windows.Input.TextCompositionManager.AddPreviewTextInputStartHandler(editor,(s,e)=>{composing=true;editor.Foreground=code.Foreground;code.Opacity=0;});
   System.Windows.Input.TextCompositionManager.AddPreviewTextInputUpdateHandler(editor,(s,e)=>{composing=true;editor.Foreground=code.Foreground;code.Opacity=0;});
   System.Windows.Input.TextCompositionManager.AddPreviewTextInputHandler(editor,(s,e)=>{composing=false;Dispatcher.BeginInvoke(new Action(()=>{Highlight();editor.Foreground=Brushes.Transparent;code.Opacity=1;}));});
   editor.LostKeyboardFocus+=(s,e)=>{composing=false;Highlight();editor.Foreground=Brushes.Transparent;code.Opacity=1;};
   editor.TextChanged+=(s,e)=>{if(!composing)Highlight();};language.SelectionChanged+=(s,e)=>Highlight();refresh.Tick+=(s,e)=>{refresh.Stop();Highlight();};Closed+=(s,e)=>refresh.Stop();Highlight();
  }
  public void Highlight(){
   string text=editor.Text.Replace("\t","    ");code.Inlines.Clear();string lang=(string)language.SelectedItem;
   if(lang=="纯文本"){code.Inlines.Add(new Run(text));return;}
   string comment=lang=="Python"?@"\#[^\r\n]*":lang=="SQL"?@"--[^\r\n]*":lang=="HTML"?@"<!--[\s\S]*?-->":lang=="JSON"?@"(?!)":@"//[^\r\n]*|/\*[\s\S]*?\*/";
   string strings="\"(?:\\\\.|[^\"\\\\])*\"|'(?:\\\\.|[^'\\\\])*'|`(?:\\\\.|[^`\\\\])*`";
   string words=lang=="Python"?"def class return if elif else for while in import from as True False None and or not try except finally with lambda pass yield async await":lang=="SQL"?"SELECT FROM WHERE JOIN LEFT RIGHT INNER ON AS INSERT INTO VALUES UPDATE SET DELETE CREATE TABLE DROP GROUP BY ORDER ASC DESC NULL AND OR NOT LIMIT":lang=="C#"||lang=="C++"||lang=="Java"?"public private protected static void int double float bool boolean string char class struct namespace using new return if else for while switch case break continue null true false const auto include try catch throw extends implements import package": "function const let var return if else for while class new true false null undefined import export from async await interface type extends switch case break";
   var keywords=new System.Collections.Generic.HashSet<string>(words.Split(' '),lang=="SQL"?StringComparer.OrdinalIgnoreCase:StringComparer.Ordinal);
   var regex=new Regex("(?<comment>"+comment+")|(?<str>"+strings+")|(?<num>\\b\\d+(?:\\.\\d+)?\\b)|(?<word>[A-Za-z_$][\\w$]*)",RegexOptions.None,TimeSpan.FromMilliseconds(300));
   int position=0;
   try{foreach(Match m in regex.Matches(text)){
    if(m.Index>position)code.Inlines.Add(new Run(text.Substring(position,m.Index-position)));
    string color=m.Groups["comment"].Success?"#8EAA91":m.Groups["str"].Success?"#A8E6A2":m.Groups["num"].Success?"#FFC58F":keywords.Contains(m.Value)?"#C9A7FF":(lang=="HTML"||lang=="CSS")?"#83D6FF":"#E6EDF3";
    if(light)color=m.Groups["comment"].Success?"#638366":m.Groups["str"].Success?"#216B36":m.Groups["num"].Success?"#995B00":keywords.Contains(m.Value)?"#7944B5":(lang=="HTML"||lang=="CSS")?"#0969A2":"#24292F";
    code.Inlines.Add(new Run(m.Value){Foreground=ColorBrush(color)});position=m.Index+m.Length;
   }code.Inlines.Add(new Run(text.Substring(position)));}catch(RegexMatchTimeoutException){code.Inlines.Clear();code.Inlines.Add(new Run(text));}
  }
  void Export(){
   var dialog=new SaveFileDialog{Filter="PNG 图片|*.png",FileName="代码块-"+DateTime.Now.ToString("yyyyMMdd-HHmmss")};if(dialog.ShowDialog(this)!=true)return;
   try{
    Highlight();var exported=new TextBlock{FontFamily=code.FontFamily,FontSize=code.FontSize,Foreground=code.Foreground};foreach(Run run in code.Inlines)exported.Inlines.Add(new Run(run.Text){Foreground=run.Foreground});var output=new StackPanel();output.Children.Add(EditorChrome.Dots());output.Children.Add(exported);card.Child=output;card.Measure(new Size(double.PositiveInfinity,double.PositiveInfinity));Size size=card.DesiredSize;
    int width=(int)Math.Ceiling(size.Width*2),height=(int)Math.Ceiling(size.Height*2);
    if(width<1||height<1||width>16000||height>16000||(long)width*height>40000000)throw new InvalidOperationException("代码图片过大，请减少代码或缩短长行后导出。");
    card.Arrange(new Rect(size));card.UpdateLayout();var bitmap=new RenderTargetBitmap(width,height,192,192,PixelFormats.Pbgra32);bitmap.Render(card);var encoder=new PngBitmapEncoder();encoder.Frames.Add(BitmapFrame.Create(bitmap));using(var file=File.Create(dialog.FileName))encoder.Save(file);

   }catch(Exception error){MessageBox.Show(this,error.Message,"导出未完成");}finally{card.Child=null;}
  }
 }
}
