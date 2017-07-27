using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPVersioningToolkit.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UWPVersioningToolkit.Dialog
{
    public sealed partial class VersionLogUI : UserControl
    {
        public VersionLogUI()
        {
            this.InitializeComponent();
        }

        public ChangelogStrings Strings
        {
            get { return VersionHelper.Strings; }
        }

        public VersionModel Viewmodel
        {
            get { return (VersionModel)GetValue(VersionProperty); }
            set { SetValue(VersionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Version.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VersionProperty =
            DependencyProperty.Register(nameof(Viewmodel), typeof(VersionModel), typeof(VersionLogUI), new PropertyMetadata(null));

        private void MarkdownTextBlock_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            VersionHelper.DialogHandler.ChangeDialogLinkClicked(sender, e);
        }

        private void MarkdownTextBlock_ImageResolving(object sender, ImageResolvingEventArgs e)
        {
            VersionHelper.DialogHandler.ChangeDialogImageResolving(sender, e);
        }
    }
}