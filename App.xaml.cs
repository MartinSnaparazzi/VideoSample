using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace VideoSample;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
        base.OnStartup(e);
        MainWindow snapWindow = new MainWindow();
        snapWindow.Show();
    }
}

