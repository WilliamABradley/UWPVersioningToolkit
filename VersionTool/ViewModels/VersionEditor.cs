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
            New = Model.Version.New;
            Fixed = Model.Version.Fixed;
            StoreSummary = Model.Version.StoreVersionSummary;
        }

        public string New { get; set; }
        public string Fixed { get; set; }
        public string StoreSummary { get; set; }

        public void Save()
        {
            Model.Version.New = New?.Remove(New.Length - 1);
            Model.Version.Fixed = Fixed?.Remove(Fixed.Length - 1);
            Model.Version.StoreVersionSummary = StoreSummary?.Remove(StoreSummary.Length - 1);
            EditWaiter.TrySetResult(Model.Version);
        }

        public void Revert()
        {
            EditWaiter.TrySetResult(null);
        }

        public VersionModel Model { get; }
        public TaskCompletionSource<VersionLog> EditWaiter { get; } = new TaskCompletionSource<VersionLog>();
    }
}