using System.Diagnostics;
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

        int framesRendered = 0;
        Stopwatch stopwatch = Stopwatch.StartNew();

        int renderTier = (RenderCapability.Tier >> 16);
        System.Diagnostics.Debug.WriteLine($"Render tier: {renderTier}");

        CompositionTarget.Rendering += (s, e) =>
        {
            framesRendered++;
            if (stopwatch.ElapsedMilliseconds >= 1000)
            {
                System.Diagnostics.Debug.WriteLine($"FPS: {framesRendered}");
                framesRendered = 0;
                stopwatch.Restart();
            }
        };
        base.OnStartup(e);


        MainWindow snapWindow = new MainWindow();
        snapWindow.Show();

        SpareWindow spareWindow = new SpareWindow();
        spareWindow.Show();
    }
}

