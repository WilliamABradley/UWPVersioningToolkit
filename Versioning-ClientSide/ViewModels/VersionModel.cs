using System;
using System.ComponentModel;
using UWPVersioningToolkit.Models;

namespace UWPVersioningToolkit.ViewModels
{
    /// <summary>
    /// Viewmodel for the VersionLog, and Associated Data.
    /// </summary>
    public class VersionModel : INotifyPropertyChanged
    {
        /// <summary>
        /// For Presenting a Current VersionLog.
        /// </summary>
        public VersionModel(VersionLog Version)
        {
            this.Version = Version;
        }

        /// <summary>
        /// Used during the Creation of a new Version Log.
        /// </summary>
        public VersionModel(Version Version)
        {
            this.Version = new VersionLog
            {
                Major = Version.Major,
                Minor = Version.Minor,
                Build = Version.Build,
                Revision = Version.Revision
            };
        }

        /// <summary>
        /// Updates the Internal Log.
        /// </summary>
        public void Update(VersionLog Version)
        {
            this.Version = Version;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        /// <summary>
        /// Presents the Version Name to the UI.
        /// </summary>
        public override string ToString()
        {
            return VersionName;
        }

        /// <summary>
        /// The Version Presented Nicely.
        /// </summary>
        public string VersionName
        {
            get { return $"V{Version?.Major}.{Version?.Minor}.{Version?.Build}.{Version?.Revision}"; }
        }

        /// <summary>
        /// Does the Log have Whats New Data?
        /// </summary>
        public bool HasNew
        {
            get { return !string.IsNullOrWhiteSpace(Version?.New); }
        }

        /// <summary>
        /// Does the Log have Fixed Data?
        /// </summary>
        public bool HasFixed
        {
            get { return !string.IsNullOrWhiteSpace(Version?.Fixed); }
        }

        /// <summary>
        /// The Version Log.
        /// </summary>
        public VersionLog Version { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}