using System.Linq;
using UWPVersioningToolkit.Models;
using UWPVersioningToolkit.ViewModels;
using Windows.UI.Xaml.Controls;

namespace UWPVersioningToolkit.Dialog
{
    /// <summary>
    /// The Default Presenter for Changelogs in the UWP Versioning Toolkit.
    /// </summary>
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

        /// <summary>
        /// Local Access to the Strings.
        /// </summary>
        public ChangelogStrings Strings { get { return VersionHelper.Strings; } }

        /// <summary>
        /// Are there Older Versions?
        /// </summary>
        public bool HasOlderVersions { get { return Changelog.OlderVersions.Any(); } }

        /// <summary>
        /// Current Changelog being Displayed.
        /// </summary>
        public Changelog Changelog { get; private set; }
    }
}