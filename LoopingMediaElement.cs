using System.Windows;
using System.Windows.Controls;

namespace VideoSample
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Snap.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Snap.Controls;assembly=Snap.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:LoopingMediaElement/>
    ///
    /// </summary>
    public class LoopingMediaElement : MediaElement
    {
        public LoopingMediaElement() : base()
        {
            this.Source = new Uri("sample-file.mp4", UriKind.Relative);
            this.MediaEnded += LoopingMediaElement_MediaEnded;
        }

        private void LoopingMediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            // Note this starts at 100ms because you get a black flicker if its sooner
            this.Position = TimeSpan.FromMilliseconds(100);
        }
    }
}
