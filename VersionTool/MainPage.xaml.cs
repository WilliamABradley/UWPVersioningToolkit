using UWPVersioningToolkit.ViewModels;
using UWPVersioningToolkit.Views;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UWPVersioningToolkit
{
    /// <summary>
    /// The Main Page in Charge of Navigation
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public VersioningSession Viewmodel { get { return VersioningSession.Current; } }
        private SystemNavigationManager Nav = SystemNavigationManager.GetForCurrentView();

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            Viewmodel.EditRequested += Viewmodel_EditRequested;
            Nav.BackRequested += Nav_BackRequested;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(VersionList));
            MainFrame.Navigated += Frame_Navigated;
        }

        private void Nav_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (MainFrame.CanGoBack) MainFrame.GoBack();
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            Nav.AppViewBackButtonVisibility = MainFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        private void Viewmodel_EditRequested(object sender, VersionEditor e)
        {
            MainFrame.Navigate(typeof(Editor), e);
        }
    }
}