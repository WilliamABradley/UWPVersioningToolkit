using System;
using System.Threading.Tasks;
using UWPVersioningToolkit.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPVersioningToolkit.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Editor : Page
    {
        public VersionEditor Viewmodel { get; set; }

        public Editor()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Viewmodel = e.Parameter as VersionEditor;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (e.SourcePageType != typeof(Editor) && e.NavigationMode == NavigationMode.Back && !Viewmodel.EditWaiter.Task.IsCompleted)
            {
                Viewmodel.Revert();
            }
            base.OnNavigatedFrom(e);
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Summary.OverLimit)
            {
                var result = await new ContentDialog { Title = "Warning", Content = "The Store Summary is currently over the 1500 Character Limit, do you want to Continue?", PrimaryButtonText = "OK", SecondaryButtonText = "Cancel" }.ShowAsync();
                if (result != ContentDialogResult.Primary) return;
            }

            Viewmodel.Save();
            Frame.GoBack();
        }
    }
}