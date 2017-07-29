using System;
using Windows.ApplicationModel;

namespace UWPVersioningToolkit.Models
{
    /// <summary>
    /// Information about What was the Last Used Version, and If the Changelog has been viewed.
    /// </summary>
    public class VersionCheck
    {
        /// <summary>
        /// Creates a new VersionCheck, with the Current App Version, and the Viewed Log set to False.
        /// </summary>
        public VersionCheck()
        {
            LastUserVersion = GetVersion();
            ViewedLog = false;
        }

        /// <summary>
        /// Gets the Current App Version.
        /// </summary>
        public static Version GetVersion()
        {
            var appversion = Package.Current.Id.Version;
            return new Version(appversion.Major, appversion.Minor, appversion.Build, appversion.Revision);
        }

        /// <summary>
        /// Last Used App Version.
        /// </summary>
        public Version LastUserVersion { get; set; }

        /// <summary>
        /// Has the User Viewed the Changelog?
        /// </summary>
        public bool ViewedLog { get; set; }
    }
}