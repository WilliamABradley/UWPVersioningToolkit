using System;
using UWPVersioningToolkit.ViewModels;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace UWPVersioningToolkit.Views
{
    /// <summary>
    /// A list of the Versions in the current Changelog
    /// </summary>
    public sealed partial class VersionList : Page
    {
        /// <summary>
        /// Current Session.
        /// </summary>
        public VersioningSession Viewmodel { get { return VersioningSession.Current; } }

        public VersionList()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Opens the Editor when clicking on a version.
        /// </summary>
        private async void VersionsArea_ItemClick(object sender, ItemClickEventArgs e)
        {
            var model = e.ClickedItem as VersionModel;
            var log = await Viewmodel.Edit(model);
        }

        /// <summary>
        /// Context Menu for each Version.
        /// </summary>
        private void Version_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var source = sender as FrameworkElement;
            var model = source.DataContext as VersionModel;

            var menu = new MenuFlyout();

            var GetSummary = new MenuFlyoutItem { Text = "Copy Store Summary" };
            GetSummary.Click += delegate { CopyStoreSummary(model); };
            menu.Items.Add(GetSummary);

            menu.ShowAt(source, e.GetPosition(source));
        }

        /// <summary>
        /// Copies the Store Summary Text to Clipboard for a version.
        /// </summary>
        private async void CopyStoreSummary(VersionModel Version)
        {
            string text = Version.Version.StoreVersionSummary;
            if (string.IsNullOrWhiteSpace(text))
            {
                var dlg = new ContentDialog
                {
                    Title = "WARNING",
                    Content = $"Changelog {Version.VersionName.ToString()} does not have a Store Summary.",
                    PrimaryButtonText = "OK"
                };
                await dlg.ShowAsync();
                return;
            }

            var dataPackage = new DataPackage();
            dataPackage.SetText(text);
            Clipboard.SetContent(dataPackage);
        }
    }
}