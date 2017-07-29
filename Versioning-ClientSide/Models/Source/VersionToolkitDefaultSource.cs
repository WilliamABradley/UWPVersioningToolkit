using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UWPVersioningToolkit.ViewModels;
using Windows.ApplicationModel;
using Windows.Storage;

namespace UWPVersioningToolkit.Models.Source
{
    /// <summary>
    /// The Default Service for getting Changelog Data, and Storing the Last Used Version Settings.
    /// </summary>
    public class VersionToolkitDefaultSource : IVersionToolkitService
    {
        /// <summary>
        /// Loads the Changelog Data from the provided File and Folder.
        /// </summary>
        public async Task<Changelog> GetChangelog()
        {
            var file = await ChangelogLocation.GetFileAsync(ChangelogFileName);
            var Logs = JsonConvert.DeserializeObject<List<VersionLog>>(await FileIO.ReadTextAsync(file))
                .OrderByDescending(v => v.Major)
                .ThenByDescending(v => v.Minor)
                .ThenByDescending(v => v.Build)
                .ThenByDescending(v => v.Revision);

            return new Changelog
            {
                CurrentVersion = new VersionModel(Logs.First()),
                OlderVersions = Logs.Skip(1).Select(item => new VersionModel(item)).ToList()
            };
        }

        /// <summary>
        /// Loads the Last Used Version from the Settings Cluster Provided.
        /// </summary>
        public virtual VersionCheck GetPreviousVersion()
        {
            var hasCheck = SettingsCluster.Values.TryGetValue(VersionKey, out object rawValue);
            var status = hasCheck ? JsonConvert.DeserializeObject<VersionCheck>(rawValue as string) : new VersionCheck();
            return status;
        }

        /// <summary>
        /// Stores the Last Used Version to the Settings Cluster Provided.
        /// </summary>
        public virtual void StoreVersion(VersionCheck version)
        {
            if (version != null)
            {
                SettingsCluster.Values[VersionKey] = JsonConvert.SerializeObject(version);
            }
            else SettingsCluster.Values.Remove(VersionKey);
        }

        /// <summary>
        /// FileName of Changelog File (Defaults to Changelog.json).
        /// </summary>
        public static string ChangelogFileName = "Changelog.json";

        /// <summary>
        /// Location where the Changelog File Exists (Defaults to App Install Folder).
        /// </summary>
        public static StorageFolder ChangelogLocation = Package.Current.InstalledLocation;

        /// <summary>
        /// Settings Location to Look for Last Used App Version (Defaults to RoamingSettings).
        /// </summary>
        public static ApplicationDataContainer SettingsCluster = ApplicationData.Current.RoamingSettings;

        /// <summary>
        /// Key for the Settings Container to find and save the Last Used Version Information (Defaults to CurrentVersion)
        /// </summary>
        protected string VersionKey = "CurrentVersion";
    }
}