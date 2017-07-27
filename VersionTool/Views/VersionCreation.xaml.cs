using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPVersioningToolkit.Views
{
    public sealed partial class VersionCreation : ContentDialog
    {
        private VersionCreation()
        {
            this.InitializeComponent();
        }

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