using System;
using UWPVersioningToolkit.Dialog;
using Windows.System;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace UWPVersioningToolkit.Models
{
    public class ChangeDialogHandler
    {
        public virtual async void ShowDialog(ChangeDialog Dialog)
        {
            await Dialog.ShowAsync();
        }

        public virtual void OnDialogClosing(ChangeDialog Dialog)
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