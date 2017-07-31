using System;
using System.Threading.Tasks;
using UWPVersioningToolkit.Dialog;
using UWPVersioningToolkit.Models;
using UWPVersioningToolkit.Models.Source;
using UWPVersioningToolkit.ViewModels;
using Windows.UI.Xaml.Controls;

namespace UWPVersioningToolkit
{
    /// <summary>
    /// Helper for the UWP Versioning Toolkit, Replace the <see cref="Service"/> Property to Load Changelogs from other Sources, such as Online.
    /// </summary>
    public static class VersionHelper
    {
        /// <summary>
        /// Checks if the App Version is different from the Last Used Version, if it is, and the Dialog hasn't been shown yet, show the Dialog.
        /// </summary>
        public static async void CheckForUpdate()
        {
            await CheckForUpdateAsync();
        }

        /// <summary>
        /// Checks if the App Version is different from the Last Used Version, if it is, and the Dialog hasn't been shown yet, show the Dialog.
        /// </summary>
        public static async Task CheckForUpdateAsync()
        {
            try
            {
                var previousVersion = Service.GetPreviousVersion();
                var currentAppVersion = VersionCheck.GetVersion();

                var log = await FetchChangelog();

                if (previousVersion.LastUserVersion != currentAppVersion || !previousVersion.ViewedLog)
                {
                    await ShowChangelogAsync(log);
                    previousVersion.LastUserVersion = currentAppVersion;
                    previousVersion.ViewedLog = true;
                    Service.StoreVersion(previousVersion);
                }
            }
            catch (Exception ex) { if (!CatchExceptions) throw ex; }
        }

        /// <summary>
        /// Fetches the Changelog from <see cref="Service"/>, warns the User if the App Version doesn't match the Latest Changelog Version, if <see cref="AlertMissingVersionLog"/> is True.
        /// </summary>
        /// <returns></returns>
        private static async Task<Changelog> FetchChangelog()
        {
            var Changelog = await Service.GetChangelog();

            if (AlertMissingVersionLog)
            {
                var rawVersion = Changelog.CurrentVersion.Version;
                var ChangeVersion = new Version(rawVersion.Major, rawVersion.Minor, rawVersion.Build, rawVersion.Revision);

                var appVersion = VersionCheck.GetVersion();
                if (ChangeVersion != appVersion)
                {
                    var dlg = new ContentDialog
                    {
                        Title = "WARNING",
                        Content = $"Current Changelog Version ({ChangeVersion.ToString()}) does not match App Version ({appVersion.ToString()}).\nEnsure you update the Changelog before releasing the Update.\nYou can disable this Warning via VersionHelper.AlertMissingVersionLog",
                        PrimaryButtonText = "OK"
                    };
                    await DialogHandler.ShowDialog(dlg);
                }
            }

            return Changelog;
        }

        /// <summary>
        /// Shows the Changelog.
        /// </summary>
        public static async void ShowChangelog(Changelog Changelog = null)
        {
            await ShowChangelogAsync(Changelog);
        }

        /// <summary>
        /// Shows the Changelog.
        /// </summary>
        public static async Task ShowChangelogAsync(Changelog Changelog = null)
        {
            try
            {
                Changelog = Changelog ?? await Service.GetChangelog();
                await DialogHandler.ShowDialog(new ChangeDialog(Changelog));
            }
            catch (Exception ex) { if (!CatchExceptions) throw ex; }
        }

        /// <summary>
        /// The Service for IO for Change Information.
        /// </summary>
        public static IVersionToolkitService Service = new VersionToolkitDefaultSource();

        /// <summary>
        /// The Handler for Changelog events.
        /// </summary>
        public static ChangeDialogHandler DialogHandler = new ChangeDialogHandler();

        /// <summary>
        /// The Strings for Changelog UI.
        /// </summary>
        public static ChangelogStrings Strings = new ChangelogStrings();

        /// <summary>
        /// Enable this to prevent Exceptions caused by missing Changelog Information.
        /// </summary>
        public static bool CatchExceptions = false;

        /// <summary>
        /// Alerts the User that the App Version doesn't match the Changelog Lastest Version (For Development).
        /// </summary>
        public static bool AlertMissingVersionLog = true;
    }
}