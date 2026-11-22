using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
namespace ClassInk {
 public sealed class TextNoteWindow : Window {
  readonly RichTextBox editor=new RichTextBox{AcceptsTab=true,VerticalScrollBarVisibility=ScrollBarVisibility.Auto,BorderThickness=new Thickness(0),Padding=new Thickness(18),FontFamily=new FontFamily("Microsoft YaHei UI"),FontSize=18};
  string path;
  TextRange All {get{return new TextRange(editor.Document.ContentStart,editor.Document.ContentEnd);}}
  Button ActionButton(string title,Action action){var button=new Button{Content=title,Width=34,Height=34,ToolTip=title,Padding=new Thickness(6),Margin=new Thickness(0,0,6,6),Focusable=false};EditorChrome.Style(button);string glyph=title=="打开"?"\uE8E5":title=="保存"?"\uE74E":title=="导出图片"?"\uE91B":title=="左对齐"?"\uE8E4":title=="居中"?"\uE8E3":title=="右对齐"?"\uE8E2":"";button.Content=new TextBlock{Text=glyph,FontFamily=new FontFamily("Segoe MDL2 Assets"),FontSize=16};button.Click+=(s,e)=>action();return button;}
  public TextNoteWindow(){
   Title="课堂笔 · 文本便笺";Width=720;Height=560;MinWidth=480;MinHeight=360;Background=Brushes.White;WindowStartupLocation=WindowStartupLocation.CenterScreen;
   var root=new DockPanel{Margin=new Thickness(14)};Content=root;
   var bar=new WrapPanel();DockPanel.SetDock(bar,Dock.Top);root.Children.Add(bar);
   bar.Children.Add(ActionButton("打开",Open));bar.Children.Add(ActionButton("保存",()=>Save()));bar.Children.Add(ActionButton("导出图片",ExportImage));
   var sizes=new ComboBox{Width=70,Height=34,VerticalAlignment=VerticalAlignment.Center,VerticalContentAlignment=VerticalAlignment.Center,Padding=new Thickness(8,0,4,0),FontSize=14,Margin=new Thickness(0,0,10,6),ToolTip="字号（选中文字后设置）"};foreach(double value in new[]{12.0,16,18,20,24,28,32,40,48})sizes.Items.Add(value);sizes.SelectedItem=18.0;bar.Children.Add(sizes);
   sizes.SelectionChanged+=(s,e)=>{if(sizes.SelectedItem==null)return;editor.Selection.ApplyPropertyValue(TextElement.FontSizeProperty,(double)sizes.SelectedItem);editor.Focus();};
   string[] colors={"#243047","#DC3545","#2563EB","#16834A","#8B5CF6"};string[] names={"黑","红","蓝","绿","紫"};
   for(int i=0;i<colors.Length;i++){var brush=(Brush)new BrushConverter().ConvertFromString(colors[i]);var button=ActionButton(names[i],()=>{editor.Selection.ApplyPropertyValue(TextElement.ForegroundProperty,brush);editor.Focus();});button.Content=new System.Windows.Shapes.Ellipse{Width=16,Height=16,Fill=brush};button.Foreground=brush;bar.Children.Add(button);}
   bar.Children.Add(ActionButton("左对齐",()=>Align(TextAlignment.Left)));bar.Children.Add(ActionButton("居中",()=>Align(TextAlignment.Center)));bar.Children.Add(ActionButton("右对齐",()=>Align(TextAlignment.Right)));
   var note=new TextBlock{Text="Ctrl + 滚轮缩放显示  ·  TXT 纯文本 / RTF 保留格式",FontSize=11,Foreground=Brushes.SlateGray,TextWrapping=TextWrapping.Wrap,Margin=new Thickness(0,0,0,8)};DockPanel.SetDock(note,Dock.Bottom);root.Children.Add(note);
   editor.Document=new FlowDocument{PagePadding=new Thickness(0),FontFamily=new FontFamily("Microsoft YaHei UI"),FontSize=18};var style=new Style(typeof(Paragraph));style.Setters.Add(new Setter(Block.MarginProperty,new Thickness(0,0,0,10)));editor.Document.Resources.Add(typeof(Paragraph),style);root.Children.Add(editor);EditorChrome.EnableTextZoom(this,editor);
   Loaded+=(s,e)=>editor.Focus();
  }
  void Align(TextAlignment alignment){editor.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty,alignment);editor.Focus();}
  void Open(){
   var dialog=new OpenFileDialog{Filter="文本文件|*.txt;*.rtf|TXT 文本|*.txt|RTF 格式文本|*.rtf"};if(dialog.ShowDialog(this)!=true)return;
   try{var document=new FlowDocument();var range=new TextRange(document.ContentStart,document.ContentEnd);if(Path.GetExtension(dialog.FileName).Equals(".rtf",StringComparison.OrdinalIgnoreCase)){using(var file=File.OpenRead(dialog.FileName))range.Load(file,DataFormats.Rtf);}else range.Text=File.ReadAllText(dialog.FileName,Encoding.UTF8);document.FontFamily=editor.FontFamily;document.FontSize=18;editor.Document=document;path=dialog.FileName;Title="文本便笺 · "+Path.GetFileName(path);}catch(Exception error){MessageBox.Show(this,error.Message,"打开失败");}
  }
  void ExportImage(){
   var dialog=new SaveFileDialog{Filter="PNG 图片|*.png",FileName="文本便笺-"+DateTime.Now.ToString("yyyyMMdd-HHmmss")};if(dialog.ShowDialog(this)!=true)return;
   try{RenderImage(dialog.FileName);}catch(Exception error){MessageBox.Show(this,error.Message,"图片导出失败");}
  }
  public void RenderImage(string filename){
   var document=new FlowDocument();
   using(var stream=new MemoryStream()){All.Save(stream,DataFormats.XamlPackage);stream.Position=0;new TextRange(document.ContentStart,document.ContentEnd).Load(stream,DataFormats.XamlPackage);}
   document.FontFamily=editor.Document.FontFamily;document.FontSize=editor.Document.FontSize;document.Foreground=editor.Document.Foreground;document.PagePadding=new Thickness(0);
   foreach(object key in editor.Document.Resources.Keys)document.Resources[key]=editor.Document.Resources[key];
   var surface=new RichTextBox{Document=document,Width=Math.Max(320,editor.ActualWidth),Padding=editor.Padding,BorderThickness=new Thickness(0),Background=Brushes.White,IsReadOnly=true,IsHitTestVisible=false,VerticalScrollBarVisibility=ScrollBarVisibility.Disabled,HorizontalScrollBarVisibility=ScrollBarVisibility.Disabled};
   surface.Measure(new Size(surface.Width,double.PositiveInfinity));var size=surface.DesiredSize;
   int width=(int)Math.Ceiling(size.Width*2),height=(int)Math.Ceiling(size.Height*2);
   if(width<1||height<1||width>16000||height>16000||(long)width*height>40000000)throw new InvalidOperationException("文本图片过大，请分段导出。");
   surface.Arrange(new Rect(size));surface.UpdateLayout();
   var bitmap=new RenderTargetBitmap(width,height,192,192,PixelFormats.Pbgra32);bitmap.Render(surface);var encoder=new PngBitmapEncoder();encoder.Frames.Add(BitmapFrame.Create(bitmap));using(var file=File.Create(filename))encoder.Save(file);
  }
  bool Save(){
   var dialog=new SaveFileDialog{Filter="TXT 纯文本|*.txt|RTF 保留格式|*.rtf",FileName=path==null?"文本便笺.txt":Path.GetFileName(path),FilterIndex=path!=null&&Path.GetExtension(path).Equals(".rtf",StringComparison.OrdinalIgnoreCase)?2:1};if(dialog.ShowDialog(this)!=true)return false;
   try{using(var data=new MemoryStream()){if(Path.GetExtension(dialog.FileName).Equals(".rtf",StringComparison.OrdinalIgnoreCase))All.Save(data,DataFormats.Rtf);else{byte[] text=new UTF8Encoding(true).GetBytes(All.Text);data.Write(text,0,text.Length);}File.WriteAllBytes(dialog.FileName,data.ToArray());}path=dialog.FileName;Title="文本便笺 · "+Path.GetFileName(path);return true;}catch(Exception error){MessageBox.Show(this,error.Message,"保存失败");return false;}
  }
 }
}
