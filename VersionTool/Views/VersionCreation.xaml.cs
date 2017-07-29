using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace UWPVersioningToolkit.Views
{
    /// <summary>
    /// A Dialog Prompt for Entering the new Version Number for a Version Log you would like to create.
    /// </summary>
    public sealed partial class VersionCreation : ContentDialog
    {
        private VersionCreation()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Creates a Version from a Dialog Prompt.
        /// </summary>
        public static async Task<Version> CreateVersion()
        {
            var creator = new VersionCreation();
            var result = await creator.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                return new Version((int)(creator.Major.LongValue ?? 0), (int)(creator.Minor.LongValue ?? 0), (int)(creator.Build.LongValue ?? 0), (int)(creator.Revision.LongValue ?? 0));
            }
            else return null;
        }
    }
}