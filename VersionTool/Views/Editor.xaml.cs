using System;
using UWPVersioningToolkit.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UWPVersioningToolkit.Views
{
    /// <summary>
    /// The Editor page for Editing and Creating Version Logs.
    /// </summary>
    public sealed partial class Editor : Page
    {
        /// <summary>
        /// Editor Viewmodel Instance.
        /// </summary>
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

        /// <summary>
        /// Saves the Updated Version Log.
        /// </summary>
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