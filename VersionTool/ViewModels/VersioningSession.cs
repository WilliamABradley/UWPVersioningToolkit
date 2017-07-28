using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using UWPVersioningToolkit.Dialog;
using UWPVersioningToolkit.Models;
using UWPVersioningToolkit.Views;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPVersioningToolkit.ViewModels
{
    public class VersioningSession : INotifyPropertyChanged
    {
        public static VersioningSession Current = new VersioningSession();

        public event PropertyChangedEventHandler PropertyChanged;

        private VersioningSession()
        {
            Current = this;
            Versions.CollectionChanged += delegate { UpdateProperties(); };
        }

        public VersioningSession Create()
        {
            Current.Versions.Clear();
            Current.ChangelogFile = null;
            return Current;
        }

        public async void AddVersion()
        {
            var version = await VersionCreation.CreateVersion();
            if (version != null)
            {
                var model = new VersionModel(version);
                //launch editor
                var log = await Edit(model);
                if (log != null)
                {
                    Versions.Insert(0, model);
                }
            }
        }

        public async Task<VersionLog> Edit(VersionModel Model)
        {
            var editor = new VersionEditor(Model);
            EditRequested?.Invoke(this, editor);
            return await editor.EditWaiter.Task;
        }

        public async Task<VersioningSession> Load()
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".json");
            try
            {
                var file = await picker.PickSingleFileAsync();

                Current.Versions.Clear();
                Current.ChangelogFile = file;

                var raw = await FileIO.ReadTextAsync(file);
                var logs = JsonConvert.DeserializeObject<List<VersionLog>>(raw);

                if (logs.Any(item => string.IsNullOrWhiteSpace(item.New) && string.IsNullOrWhiteSpace(item.Fixed)))
                {
                    var oldLogs = JsonConvert.DeserializeObject<List<V1VersionLog>>(raw);
                    logs = Upgrade(oldLogs);
                }

                foreach (var log in logs.OrderByDescending(k => k.Major).ThenByDescending(k => k.Minor).ThenByDescending(k => k.Build).ThenByDescending(k => k.Revision))
                {
                    Current.Versions.Add(new VersionModel(log));
                }
            }
            catch
            {
                await new ContentDialog { Title = "Error", Content = "Couldn't load Changlog Json", PrimaryButtonText = "OK" }.ShowAsync();
                Create();
            }

            UpdateProperties();
            return Current;
        }

        public List<VersionLog> Upgrade(List<V1VersionLog> OldLogs)
        {
            List<VersionLog> NewLogs = new List<VersionLog>();
            foreach (var log in OldLogs)
            {
                var newLog = new VersionLog { Major = log.Major, Minor = log.Minor, Build = log.Build, Revision = log.Revision };

                try
                {
                    var text = log.Log;
                    //hopeful
                    var parts = text.Split(new string[] { "Fixed:\r" }, StringSplitOptions.RemoveEmptyEntries);
                    newLog.New = CleanForMarkdown(parts[0]);
                    newLog.Fixed = CleanForMarkdown(parts[1]);
                }
                catch { }

                NewLogs.Add(newLog);
            }
            return NewLogs;
        }

        private string CleanForMarkdown(string raw)
        {
            return raw.Insert(0, "\r").Replace("\r", "\r\r").Replace("\r-", "\r- ").Remove(0, 2);
        }

        public async Task Save()
        {
            try
            {
                if (ChangelogFile == null)
                {
                    FileSavePicker Picker = new FileSavePicker
                    {
                        DefaultFileExtension = ".json",
                        SuggestedFileName = "Changelog"
                    };
                    Picker.FileTypeChoices.Add("JSON", new string[] { ".json" });
                    await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        ChangelogFile = await Picker.PickSaveFileAsync();
                    });
                }
                await FileIO.WriteTextAsync(ChangelogFile, GetJson());
            }
            catch (Exception ex)
            {
                await new ContentDialog { Title = "Couldn't Save Changlog", Content = $"{ex.GetType().Name}: {ex.Message}", PrimaryButtonText = "OK" }.ShowAsync();
            }
        }

        private string GetJson()
        {
            var raw = Versions.Select(item => item.Version).OrderByDescending(k => k.Major).ThenByDescending(k => k.Minor).ThenByDescending(k => k.Build).ThenByDescending(k => k.Revision);
            return JsonConvert.SerializeObject(raw, Formatting.Indented);
        }

        public async void DisplayJson()
        {
            var viewer = new ScrollViewer()
            {
                Content = new TextBox
                {
                    AcceptsReturn = true,
                    IsReadOnly = true,
                    Text = GetJson(),
                    Height = 200,
                    TextWrapping = TextWrapping.Wrap
                }
            };
            await new ContentDialog { Title = "Processed Json Changelog", Content = viewer, PrimaryButtonText = "OK" }.ShowAsync();
        }

        public async void Preview()
        {
            var dialog = new ChangeDialog(Versions.ToList());
            await dialog.ShowAsync();
        }

        private void UpdateProperties()
        {
            Current.PropertyChanged?.Invoke(Current, new PropertyChangedEventArgs(nameof(Current.HasData)));
        }

        public bool HasData
        {
            get { return Versions.Any(); }
        }

        public ObservableCollection<VersionModel> Versions { get; } = new ObservableCollection<VersionModel>();
        public StorageFile ChangelogFile { get; private set; }

        public event EventHandler<VersionEditor> EditRequested;
    }
}