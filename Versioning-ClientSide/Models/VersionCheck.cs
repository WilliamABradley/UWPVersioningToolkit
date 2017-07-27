using System;
using Windows.ApplicationModel;

namespace UWPVersioningToolkit.Models
{
    public class VersionCheck
    {
        public VersionCheck()
        {
        }

        public static VersionCheck Create()
        {
            return new VersionCheck
            {
                LastUserVersion = GetVersion(),
                ViewedLog = false
            };
        }

        public static Version GetVersion()
        {
            var appversion = Package.Current.Id.Version;
            return new Version(appversion.Major, appversion.Minor, appversion.Build, appversion.Revision);
        }

        public Version LastUserVersion { get; set; }
        public bool ViewedLog { get; set; }
    }
}