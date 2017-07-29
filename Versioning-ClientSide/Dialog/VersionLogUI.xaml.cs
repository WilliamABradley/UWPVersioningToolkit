using Microsoft.Toolkit.Uwp.UI.Controls;
using UWPVersioningToolkit.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPVersioningToolkit.Dialog
{
    /// <summary>
    /// The Internal Presenter for each Version Log, for the Default Changelog Presenter.
    /// </summary>
    public partial class VersionLogUI : UserControl
    {
        public VersionLogUI()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Passes off the Link Clicked Event to the Dialog Handler.
        /// </summary>
        private void MarkdownTextBlock_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            VersionHelper.DialogHandler.ChangeDialogLinkClicked(sender, e);
        }

        /// <summary>
        /// Passes off the Image Resolving Event to the Dialog Handler.
        /// </summary>
        private void MarkdownTextBlock_ImageResolving(object sender, ImageResolvingEventArgs e)
        {
            VersionHelper.DialogHandler.ChangeDialogImageResolving(sender, e);
        }

        /// <summary>
        /// Local Access to the Strings.
        /// </summary>
        public ChangelogStrings Strings
        {
            get { return VersionHelper.Strings; }
        }

        /// <summary>
        /// The Current Version Log.
        /// </summary>
        public VersionModel Viewmodel
        {
            get { return (VersionModel)GetValue(VersionProperty); }
            set { SetValue(VersionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Version.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VersionProperty =
            DependencyProperty.Register(nameof(Viewmodel), typeof(VersionModel), typeof(VersionLogUI), new PropertyMetadata(null));
    }
}