using System.Windows;

namespace VideoSample;

/// <summary>
/// Interaction logic for SpareWindow.xaml
/// </summary>
public partial class SpareWindow : Window
{
    public SpareWindow(string videoPath)
    {
        InitializeComponent();
        VideoPlayer.Source = new Uri(videoPath, UriKind.RelativeOrAbsolute);
    }

}