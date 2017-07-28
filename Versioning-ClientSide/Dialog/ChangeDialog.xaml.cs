using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UWPVersioningToolkit.Models;
using UWPVersioningToolkit.ViewModels;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace UWPVersioningToolkit.Dialog
{
    public sealed partial class ChangeDialog : ContentDialog
    {
        public VersionModel CurrentVersion { get; private set; }

        public ChangeDialog(List<VersionModel> Versions = null)
        {
            this.InitializeComponent();

            if (Versions == null)
            {
                GetVersions();
            }
            else
            {
                CurrentVersion = Versions.First();
                OlderVersions = Versions.Skip(1).ToList();
            }

            CurrentVersionTitle.Text = Strings.NewInVersion + $" ({CurrentVersion.VersionName})";
            this.Closing += delegate
            {
                VersionHelper.DialogHandler.OnDialogClosing(this);
            };
        }

        private async void GetVersions()
        {
            try
            {
                var file = await ChangelogLocation.GetFileAsync("Changelog.json");
                var Logs = JsonConvert.DeserializeObject<List<VersionLog>>(await FileIO.ReadTextAsync(file))
                    .OrderByDescending(v => v.Major)
                    .ThenByDescending(v => v.Minor)
                    .ThenByDescending(v => v.Build)
                    .ThenByDescending(v => v.Revision);

                CurrentVersion = new VersionModel(Logs.First());
                OlderVersions = Logs.Skip(1).Select(item => new VersionModel(item)).ToList();
            }
            catch (Exception ex) { if (!VersionHelper.CatchExceptions) throw ex; }
        }

        public ChangelogStrings Strings { get { return VersionHelper.Strings; } }
        public bool HasOlderVersions { get { return OlderVersions.Any(); } }

        public static StorageFolder ChangelogLocation = Package.Current.InstalledLocation;
        private List<VersionModel> OlderVersions = new List<VersionModel>();
    }
}