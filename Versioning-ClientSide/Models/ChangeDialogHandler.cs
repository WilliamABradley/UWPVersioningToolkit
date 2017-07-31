using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace UWPVersioningToolkit.Models
{
    /// <summary>
    /// The Handler for functions involving the Changelog. <para/> Override and Replace this class in <see cref="VersionHelper.DialogHandler"/> in order to Modify handling of Displaying the Dialog, when it Closes, and Internal MarkDownTextBlock handles for Link Clicking and Image Resolving.
    /// </summary>
    public class ChangeDialogHandler
    {
        /// <summary>
        /// Allows you to Override the Logic for Showing the ChangeDialog, and Alert Dialog, so you can ensure that you won't receive any exceptions from multiple Dialogs being open.
        /// </summary>
        public virtual async Task ShowDialog(ContentDialog Dialog)
        {
            await Dialog.ShowAsync();
        }

        /// <summary>
        /// Called after the ContentDialog Closed Event Fires.
        /// </summary>
        /// <param name="Dialog"></param>
        public virtual void OnDialogClosing(ContentDialog Dialog)
        {
        }

        /// <summary>
        /// Override for Link Clicking for MarkdownTextBlock from UWP Community Toolkit.
        /// </summary>
        public virtual async void ChangeDialogLinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                await Launcher.LaunchUriAsync(new Uri(e.Link));
            }
            catch { }
        }

        /// <summary>
        /// Override for Image Resolving for MarkdownTextBlock from UWP Community Toolkit.
        /// </summary>
        public virtual void ChangeDialogImageResolving(object sender, ImageResolvingEventArgs e)
        {
        }
    }
}