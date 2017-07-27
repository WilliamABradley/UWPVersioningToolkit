using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPVersioningToolkit;
using UWPVersioningToolkit.Models;

namespace UWPVersioningToolkit.ViewModels
{
    public class VersionEditor
    {
        public VersionEditor(VersionModel Model)
        {
            this.Model = Model;
            Version = Model?.Version?.Clone();
            Original = Version?.Clone();
        }

        public void Save()
        {
            Model.Update(Version);
            EditWaiter.TrySetResult(Version);
        }

        public void Revert()
        {
            Model.Update(Original);
            EditWaiter.TrySetResult(Original);
        }

        public VersionModel Model { get; }

        public VersionLog Original { get; }
        public VersionLog Version { get; }
        public TaskCompletionSource<VersionLog> EditWaiter { get; } = new TaskCompletionSource<VersionLog>();
    }
}