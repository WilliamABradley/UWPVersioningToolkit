using UWPVersioningToolkit.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPVersioningToolkit.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VersionList : Page
    {
        public VersioningSession Viewmodel { get { return VersioningSession.Current; } }

        public VersionList()
        {
            this.InitializeComponent();
        }

        private async void VersionsArea_ItemClick(object sender, ItemClickEventArgs e)
        {
            var model = e.ClickedItem as VersionModel;
            var log = await Viewmodel.Edit(model);
        }
    }
}