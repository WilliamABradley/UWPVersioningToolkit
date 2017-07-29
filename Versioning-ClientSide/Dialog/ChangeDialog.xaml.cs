using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UWPVersioningToolkit.Models;
using UWPVersioningToolkit.ViewModels;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace UWPVersioningToolkit.Dialog
{
    public sealed partial class ChangeDialog : ContentDialog
    {
        public ChangeDialog(Changelog Changelog)
        {
            this.InitializeComponent();
            this.Changelog = Changelog;
            CurrentVersionTitle.Text = Strings.NewInVersion + $" ({Changelog.CurrentVersion.VersionName})";
            this.Closing += delegate
            {
                VersionHelper.DialogHandler.OnDialogClosing(this);
            };
        }

        public ChangelogStrings Strings { get { return VersionHelper.Strings; } }
        public bool HasOlderVersions { get { return Changelog.OlderVersions.Any(); } }

        public Changelog Changelog { get; private set; }
    }
}