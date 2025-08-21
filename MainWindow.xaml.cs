using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace VideoSample;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private static string EnvSuffix => Environment.Is64BitProcess ? "x64" : "x86";
    private static readonly string LogFilePath = $"fpsLog-{EnvSuffix}.txt";

    private int framesRendered = 0;
    private List<SpareWindow> videoWindows = new List<SpareWindow>();
    private Stopwatch stopwatch = Stopwatch.StartNew();
    public MainWindow()
    {
        InitializeComponent();
    }

    private void StartTest(string videoPath, int windowCount)
    {
        File.WriteAllText(LogFilePath, "");

        PrintEnvironmentInfo();
        var screens = Screen.AllScreens;
        int screenCount = screens.Length;

        for (int i = 0; i < windowCount; i++)
        {
            var win = new SpareWindow(videoPath);
            var screen = screens[i % screenCount];
            var area = screen.WorkingArea;
            win.WindowStartupLocation = WindowStartupLocation.Manual;
            win.Left = area.Left;
            win.Top = area.Top;
            win.Loaded += (s, e) =>
            {
                win.WindowState = WindowState.Maximized;
            };

            win.Closed += StopTest;
            win.Show();
            videoWindows.Add(win);
        }

        stopwatch.Restart();
        CompositionTarget.Rendering += CaptureFrameInfo;
    }

    private void CaptureFrameInfo(object? sender, EventArgs e)
    {
        framesRendered++;
        if (stopwatch.ElapsedMilliseconds >= 1000)
        {
            long workingSetMB = Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024;
            long privateMB = Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024;

            Log($"FPS: {framesRendered} | Memory: {workingSetMB} MB WS / {privateMB} MB Private | IsX64={Environment.Is64BitProcess}");
            framesRendered = 0;
            stopwatch.Restart();
        }
    }

    private void StopTest(object? sender, EventArgs e)
    {
        foreach (var win in videoWindows)
        {
            win.Closed -= StopTest;
            win.Close();
        }

        videoWindows.Clear();
        this.Show();
        CompositionTarget.Rendering -= CaptureFrameInfo;

        LogTextBox.Text = File.ReadAllText(LogFilePath);
    }
    private void Button_Click(object sender, RoutedEventArgs e)
    {

        string videoPath = "sample-file-HQ.mp4";

        if (VideoQualityCombo.SelectedValue is ComboBoxItem comboBoxItem)
        {
            if (comboBoxItem.Content is string selectedPath)
            {
                videoPath = selectedPath;
            }
        }

        int windowCount;

        if (int.TryParse(ExtraWindowsBox.Text, out var n))
        {
            windowCount = n;
        }
        else
        {
            System.Windows.MessageBox.Show("Couldnt parse count, using 2");
            windowCount = 2;
        }
        StartTest(videoPath, windowCount);
        this.Hide();
    }

    public static void Log(string message)
    {
        try
        {
            Debug.WriteLine(message);
            File.AppendAllText(LogFilePath, $"{DateTime.Now:HH:mm:ss.fff} {message}{Environment.NewLine}");
        }
        catch { /* ignore logging failures */ }
    }

    void PrintEnvironmentInfo()
    {
        Log($".NET Runtime: {RuntimeInformation.FrameworkDescription}");
        Log($"Runtime Identifier: {RuntimeInformation.RuntimeIdentifier}");
        Log($"OS Version: {Environment.OSVersion}");
        Log($"OS Architecture: {RuntimeInformation.OSArchitecture}");
        Log($"Process Architecture: {RuntimeInformation.ProcessArchitecture}");
        Log($"Is 64-bit OS: {Environment.Is64BitOperatingSystem}");
        Log($"Is 64-bit Process: {Environment.Is64BitProcess}");
        Log($"Processor Count (logical): {Environment.ProcessorCount}");
        Log($"Render tier: {RenderCapability.Tier >> 16}");
        Log("");

        try
        {
            using var searcher = new ManagementObjectSearcher("select * from Win32_Processor");
            foreach (var obj in searcher.Get())
            {
                Log("=== CPU Info ===");
                Log($"Name: {obj["Name"]}");
                Log($"Cores: {obj["NumberOfCores"]}");
                Log($"Logical Processors: {obj["NumberOfLogicalProcessors"]}");
                Log($"Max Clock Speed (MHz): {obj["MaxClockSpeed"]}");
                Log($"Manufacturer: {obj["Manufacturer"]}");
                Log("");
            }
        }
        catch (Exception ex)
        {
            Log("Could not query CPU info: " + ex.Message);
        }

        try
        {
            using var searcher = new System.Management.ManagementObjectSearcher("select * from Win32_VideoController");
            foreach (var obj in searcher.Get())
            {
                Log("GPU: " + obj["Name"]);
                Log("Driver Version: " + obj["DriverVersion"]);
                Log("Driver Date: " + obj["DriverDate"]);
                Log("");
            }
        }
        catch (Exception ex)
        {
            Log("Could not query GPU info: " + ex.Message);
        }
    }
}