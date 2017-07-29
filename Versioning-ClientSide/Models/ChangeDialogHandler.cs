using System;
using UWPVersioningToolkit.Dialog;
using Windows.System;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace UWPVersioningToolkit.Models
{
    public class ChangeDialogHandler
    {
        public virtual async Task ShowDialog(ContentDialog Dialog)
        {
            await Dialog.ShowAsync();
        }

        public virtual void OnDialogClosing(ContentDialog Dialog)
        {
        }

        public virtual async void ChangeDialogLinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                await Launcher.LaunchUriAsync(new Uri(e.Link));
            }
            catch { }
        }

        public virtual void ChangeDialogImageResolving(object sender, ImageResolvingEventArgs e)
        {
        }
    }
}