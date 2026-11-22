using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Markup;
namespace ClassInk {
 static class EditorChrome {
  public static void EnableTextZoom(Window window,FrameworkElement surface){
   var transform=new ScaleTransform(1,1);surface.LayoutTransform=transform;
   window.AddHandler(System.Windows.Input.Mouse.PreviewMouseWheelEvent,new System.Windows.Input.MouseWheelEventHandler((sender,e)=>{
    if((System.Windows.Input.Keyboard.Modifiers&System.Windows.Input.ModifierKeys.Control)==0||e.Delta==0)return;
    double zoom=Math.Max(0.5,Math.Min(3,transform.ScaleX*Math.Pow(1.1,e.Delta/120.0)));
    transform.ScaleX=transform.ScaleY=zoom;
    e.Handled=true;
   }),true);
  }
  public static void Style(Button b){b.Background=Brushes.Transparent;b.BorderThickness=new Thickness(0);b.Foreground=new SolidColorBrush(Color.FromRgb(60,70,86));b.Cursor=System.Windows.Input.Cursors.Hand;b.Template=(ControlTemplate)XamlReader.Parse("<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' TargetType='Button'><Border x:Name='surface' Background='{TemplateBinding Background}' CornerRadius='8' Padding='{TemplateBinding Padding}'><ContentPresenter HorizontalAlignment='Center' VerticalAlignment='Center'/></Border><ControlTemplate.Triggers><Trigger Property='IsMouseOver' Value='True'><Setter TargetName='surface' Property='Background' Value='#E8EDF4'/></Trigger><Trigger Property='IsEnabled' Value='False'><Setter Property='Opacity' Value='0.4'/></Trigger></ControlTemplate.Triggers></ControlTemplate>");}
  public static StackPanel Dots(){var row=new StackPanel{Orientation=Orientation.Horizontal,Margin=new Thickness(0,0,0,20)};foreach(string c in new[]{"#FF5F57","#28C840","#FEBC2E"})row.Children.Add(new System.Windows.Shapes.Ellipse{Width=12,Height=12,Fill=(Brush)new BrushConverter().ConvertFromString(c),Margin=new Thickness(0,0,8,0)});return row;}
 }
}

