using System.Windows;
using System.Windows.Forms;

namespace VideoSample;

/// <summary>
/// Interaction logic for SpareWindow.xaml
/// </summary>
public partial class SpareWindow : Window
{
    public SpareWindow()
    {
        InitializeComponent();

        var targetScreen = Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();
        if (targetScreen != null)
        {
            var workingArea = targetScreen.WorkingArea;
            this.Left = workingArea.Left;
            this.Top = workingArea.Top;
        }

        if (this.IsLoaded)
        {
            this.WindowState = WindowState.Maximized;
        }
        else
        {
            this.Loaded += Window_Loaded;
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var senderWindow = sender as Window;
        senderWindow.WindowState = WindowState.Maximized;
    }
}