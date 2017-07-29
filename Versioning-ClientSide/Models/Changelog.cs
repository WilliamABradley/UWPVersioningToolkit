using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPVersioningToolkit.ViewModels;

namespace UWPVersioningToolkit.Models
{
    public class Changelog
    {
        public VersionModel CurrentVersion { get; set; }
        public List<VersionModel> OlderVersions { get; set; } = new List<VersionModel>();
    }
}