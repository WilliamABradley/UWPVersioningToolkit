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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace JsonVersioningGenerator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<VersionLog> versions = new ObservableCollection<VersionLog>();
        bool loadedfromfile = false;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void CreateVersion_Click(object sender, RoutedEventArgs e)
        {
            versions.Insert(0, new VersionLog());
        }

        private async void Convert_Click(object sender, RoutedEventArgs e)
        {
            var viewer = new ScrollViewer();
            viewer.Content = new TextBox
            {
                Text = JsonConvert.SerializeObject(versions.OrderByDescending(k => k.Major).ThenByDescending(k => k.Minor).ThenByDescending(k => k.Build).ThenByDescending(k => k.Revision)),
                Height = 200,
                TextWrapping = TextWrapping.Wrap
            };
            await new ContentDialog { Title = "Processed Json Changelog", Content =  viewer, PrimaryButtonText = "OK" }.ShowAsync();
        }

        private void RichEditBox_TextChanged(object sender, RoutedEventArgs e)
        {
            var source = sender as RichEditBox;
            var model = source.DataContext as VersionLog;
            if(model != null)
            {
                string value = "";
                source.Document.GetText(Windows.UI.Text.TextGetOptions.None, out value);
                var ending = value.Substring(value.Length - 1);
                if (ending == "\r") value = value.Substring(0, value.Length - 1);
                model.log = value;
            }
        }

        private async void LoadJson_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.FileTypeFilter.Add(".json");
            try
            {
                versions = new ObservableCollection<VersionLog>(JsonConvert.DeserializeObject<List<VersionLog>>(await FileIO.ReadTextAsync((await picker.PickSingleFileAsync()))).OrderByDescending(k => k.Major).ThenByDescending(k => k.Minor).ThenByDescending(k => k.Build).ThenByDescending(k => k.Revision));
                loadedfromfile = true;
                Bindings.Update();
            }
            catch { await new ContentDialog { Title = "Error", Content = new TextBlock { Text = "Couldn't load Changlog Json" }, PrimaryButtonText = "OK" }.ShowAsync(); }
            
        }

        private void Versionsarea_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (loadedfromfile)
            {
                ((args.ItemContainer.ContentTemplateRoot as StackPanel).Children[2] as RichEditBox).Document.SetText(Windows.UI.Text.TextSetOptions.None, (args.Item as VersionLog).log);
            }
            if (sender.ItemsPanelRoot.Children.Count == versions.Count) loadedfromfile = false;
        }
    }
}
