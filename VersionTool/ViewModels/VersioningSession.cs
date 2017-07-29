using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using UWPVersioningToolkit.Models;
using UWPVersioningToolkit.Views;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPVersioningToolkit.ViewModels
{
    /// <summary>
    /// Handles the Current Session of Changelog Data, where it will be saved, and the ability to Create New or Load other Files.
    /// </summary>
    public class VersioningSession : INotifyPropertyChanged
    {
        /// <summary>
        /// Current Session, Singleton.
        /// </summary>
        public static VersioningSession Current = new VersioningSession();

        /// <summary>
        /// This Session should be Instatiated once, to allow for x:binding.
        /// </summary>
        private VersioningSession()
        {
            Current = this;
            Versions.CollectionChanged += delegate { UpdateProperties(); };
        }

        /// <summary>
        /// Creates a new Document for Changelog Information.
        /// </summary>
        /// <returns></returns>
        public VersioningSession Create()
        {
            Current.Versions.Clear();
            Current.ChangelogFile = null;
            return Current;
        }

        /// <summary>
        /// Opens the File Picker to Load a Changelog File.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Converts Plain Text To Markdown Formatting.
        /// </summary>
        private string ConvertToMarkdown(string raw)
        {
            return raw.Insert(0, "\r").Replace("\r", "\r\r").Replace("\r-", "\r- ").Remove(0, 2).Trim();
        }

        /// <summary>
        /// Upgrades old Changelog Style to new Style.
        /// </summary>
        /// <param name="OldLogs"></param>
        /// <returns></returns>
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
                    newLog.New = ConvertToMarkdown(parts[0]);
                    try
                    {
                        newLog.Fixed = ConvertToMarkdown(parts[1]);
                    }
                    catch { }
                }
                catch { }

                NewLogs.Add(newLog);
            }
            return NewLogs;
        }

        /// <summary>
        /// Creates a new Version Log, and opens the Editor.
        /// </summary>
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

        /// <summary>
        /// Requests the Editor to begin Editing a Version Log.
        /// </summary>
        public async Task<VersionLog> Edit(VersionModel Model)
        {
            var editor = new VersionEditor(Model);
            EditRequested?.Invoke(this, editor);
            return await editor.EditWaiter.Task;
        }

        /// <summary>
        /// Saves Changelog Modifications to File, or Creates a File to Save the New Changelog.
        /// </summary>
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
                    ChangelogFile = await Picker.PickSaveFileAsync();
                }
                await FileIO.WriteTextAsync(ChangelogFile, GetJson());
            }
            catch (Exception ex)
            {
                await new ContentDialog { Title = "Couldn't Save Changlog", Content = $"{ex.GetType().Name}: {ex.Message}", PrimaryButtonText = "OK" }.ShowAsync();
            }
        }

        /// <summary>
        /// Converts the Current Session to JSON.
        /// </summary>
        /// <returns></returns>
        private string GetJson()
        {
            var raw = Versions.Select(item => item.Version).OrderByDescending(k => k.Major).ThenByDescending(k => k.Minor).ThenByDescending(k => k.Build).ThenByDescending(k => k.Revision);
            return JsonConvert.SerializeObject(raw, Formatting.Indented);
        }

        /// <summary>
        /// Displays the Changelog Data in a Content Dialog.
        /// </summary>
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

        /// <summary>
        /// Allows you to Preview the Changelog, using the Client Side UI.
        /// </summary>
        public void Preview()
        {
            var changelog = new Changelog
            {
                CurrentVersion = Versions.First(),
                OlderVersions = Versions.Skip(1).ToList()
            };

            VersionHelper.ShowChangelog(changelog);
        }

        /// <summary>
        /// Updates the UI about whether there is Data to Save or Preview or not.
        /// </summary>
        private void UpdateProperties()
        {
            Current.PropertyChanged?.Invoke(Current, new PropertyChangedEventArgs(nameof(Current.HasData)));
        }

        /// <summary>
        /// Check to see if there is any Version Information.
        /// </summary>
        public bool HasData
        {
            get { return Versions.Any(); }
        }

        /// <summary>
        /// The Versions List.
        /// </summary>
        public ObservableCollection<VersionModel> Versions { get; } = new ObservableCollection<VersionModel>();

        /// <summary>
        /// The Current Document being Edited in this Session, will be null if Created New.
        /// </summary>
        public StorageFile ChangelogFile { get; private set; }

        /// <summary>
        /// Request to the UI to Navigate to the Editor.
        /// </summary>
        public event EventHandler<VersionEditor> EditRequested;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}