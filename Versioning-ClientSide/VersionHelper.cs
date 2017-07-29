using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UWPVersioningToolkit.Dialog;
using UWPVersioningToolkit.Models;
using UWPVersioningToolkit.ViewModels;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace UWPVersioningToolkit
{
    public static class VersionHelper
    {
        public static async void CheckForUpdate()
        {
            await CheckForUpdateAsync();
        }

        public static async Task CheckForUpdateAsync()
        {
            try
            {
                var previousVersion = GetPreviousVersion();
                var currentAppVersion = VersionCheck.GetVersion();

                var log = await GetChangelog();

                if (previousVersion.LastUserVersion != currentAppVersion || !previousVersion.ViewedLog)
                {
                    await ShowChangelogAsync(log);
                    previousVersion.LastUserVersion = currentAppVersion;
                    previousVersion.ViewedLog = true;
                    StoreVersion(previousVersion);
                }
            }
            catch (Exception ex) { if (!CatchExceptions) throw ex; }
        }

        public static async Task<Changelog> GetChangelog()
        {
            var file = await ChangelogLocation.GetFileAsync(ChangelogFileName);
            var Logs = JsonConvert.DeserializeObject<List<VersionLog>>(await FileIO.ReadTextAsync(file))
                .OrderByDescending(v => v.Major)
                .ThenByDescending(v => v.Minor)
                .ThenByDescending(v => v.Build)
                .ThenByDescending(v => v.Revision);

            var Changelog = new Changelog
            {
                CurrentVersion = new VersionModel(Logs.First()),
                OlderVersions = Logs.Skip(1).Select(item => new VersionModel(item)).ToList()
            };

            if (AlertMissingVersionLog)
            {
                var rawVersion = Changelog.CurrentVersion.Version;
                var ChangeVersion = new Version(rawVersion.Major, rawVersion.Minor, rawVersion.Build, rawVersion.Revision);

                var appVersion = VersionCheck.GetVersion();
                if (ChangeVersion != appVersion)
                {
                    var dlg = new ContentDialog
                    {
                        Title = "WARNING:",
                        Content = $"Current Changelog Version ({ChangeVersion.ToString()}) does not match App Version ({appVersion.ToString()}).\nEnsure you update the Changelog before releasing the Update.\nYou can disable this Warning via VersionHelper.AlertMissingVersionLog",
                        PrimaryButtonText = "OK"
                    };
                    await DialogHandler.ShowDialog(dlg);
                }
            }

            return Changelog;
        }

        public static async void ShowChangelog(Changelog Changelog = null)
        {
            await ShowChangelogAsync(Changelog);
        }

        public static async Task ShowChangelogAsync(Changelog Changelog = null)
        {
            try
            {
                Changelog = Changelog ?? await GetChangelog();
                await DialogHandler.ShowDialog(new ChangeDialog(Changelog));
            }
            catch (Exception ex) { if (!CatchExceptions) throw ex; }
        }

        public static VersionCheck GetPreviousVersion()
        {
            var hasCheck = SettingsCluster.Values.TryGetValue(VersionKey, out object rawValue);
            var status = hasCheck ? JsonConvert.DeserializeObject<VersionCheck>(rawValue as string) : VersionCheck.Create();
            return status;
        }

        public static void StoreVersion(VersionCheck version)
        {
            if (version != null)
            {
                SettingsCluster.Values[VersionKey] = JsonConvert.SerializeObject(version);
            }
            else SettingsCluster.Values.Remove(VersionKey);
        }

        public static ChangeDialogHandler DialogHandler = new ChangeDialogHandler();
        public static ChangelogStrings Strings = new ChangelogStrings();

        public static bool CatchExceptions = false;
        public static bool AlertMissingVersionLog = true;

        public static string ChangelogFileName = "Changelog.json";
        public static StorageFolder ChangelogLocation = Package.Current.InstalledLocation;
        public static ApplicationDataContainer SettingsCluster = ApplicationData.Current.RoamingSettings;

        private const string VersionKey = "CurrentVersion";
    }
}