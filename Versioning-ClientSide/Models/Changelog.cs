using System.Collections.Generic;
using UWPVersioningToolkit.ViewModels;

namespace UWPVersioningToolkit.Models
{
    /// <summary>
    /// The Changelog Container for Displaying the ChangeLog, can be instantiated to create Changelogs during Runtime.
    /// </summary>
    public class Changelog
    {
        /// <summary>
        /// Current App Version Changelog.
        /// </summary>
        public VersionModel CurrentVersion { get; set; }

        /// <summary>
        /// List of Older Versions logs.
        /// </summary>
        public List<VersionModel> OlderVersions { get; set; } = new List<VersionModel>();
    }
}