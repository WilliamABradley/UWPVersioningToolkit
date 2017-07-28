using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPVersioningToolkit.Models;

namespace UWPVersioningToolkit.ViewModels
{
    public class VersionModel : INotifyPropertyChanged
    {
        public VersionModel()
        {
        }

        public VersionModel(VersionLog Version)
        {
            this.Version = Version;
        }

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

        public void Update(VersionLog Version)
        {
            this.Version = Version;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        public override string ToString()
        {
            return VersionName;
        }

        public string VersionName
        {
            get { return $"V{Version?.Major}.{Version?.Minor}.{Version?.Build}.{Version?.Revision}"; }
        }

        public bool HasNew
        {
            get { return !string.IsNullOrWhiteSpace(Version?.New); }
        }

        public bool HasFixed
        {
            get { return !string.IsNullOrWhiteSpace(Version?.Fixed); }
        }

        public VersionLog Version { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}