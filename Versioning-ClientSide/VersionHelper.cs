using Newtonsoft.Json;
using System;
using UWPVersioningToolkit.Dialog;
using UWPVersioningToolkit.Models;
using UWPVersioningToolkit.ViewModels;
using Windows.Storage;

namespace UWPVersioningToolkit
{
    public static class VersionHelper
    {
        public static void CheckForUpdate()
        {
            try
            {
                var previousVersion = GetPreviousVersion();
                var currentAppVersion = VersionCheck.GetVersion();

                if (previousVersion.LastUserVersion != currentAppVersion || !previousVersion.ViewedLog)
                {
                    ShowChangelog();
                    previousVersion.LastUserVersion = currentAppVersion;
                    previousVersion.ViewedLog = true;
                    StoreVersion(previousVersion);
                }
            }
            catch (Exception ex) { if (!CatchExceptions) throw ex; }
        }

        public static void ShowChangelog()
        {
            DialogHandler.ShowDialog(new ChangeDialog());
        }

        public static VersionCheck GetPreviousVersion()
        {
            var hasCheck = ApplicationData.Current.RoamingSettings.Values.TryGetValue("CurrentVersion", out object rawValue);
            var status = hasCheck ? JsonConvert.DeserializeObject<VersionCheck>(rawValue as string) : VersionCheck.Create();
            return status;
        }

        public static void StoreVersion(VersionCheck version)
        {
            ApplicationData.Current.RoamingSettings.Values["CurrentVersion"] = JsonConvert.SerializeObject(version);
        }

        public static ChangeDialogHandler DialogHandler = new ChangeDialogHandler();
        public static ChangelogStrings Strings = new ChangelogStrings();
        public static bool CatchExceptions = false;
        public static bool AlertMissingVersionLog = true;
    }
}