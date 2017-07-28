using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Windows.Storage;
using UWPVersioningToolkit.Models;
using UWPVersioningToolkit.ViewModels;
using UWPVersioningToolkit.Views;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPVersioningToolkit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
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