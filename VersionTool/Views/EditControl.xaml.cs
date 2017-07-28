using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UWPVersioningToolkit.Views
{
    public sealed partial class EditControl : UserControl
    {
        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(EditControl), new PropertyMetadata("", TextChanged));

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(EditControl), new PropertyMetadata(""));

        public EditControl()
        {
            this.InitializeComponent();
        }

        private static void TextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is EditControl control)
            {
                if (control.InternalSet) return;
                if (args.NewValue != args.OldValue)
                {
                    control.Editor.Document.SetText(Windows.UI.Text.TextSetOptions.None, control.Text);
                }
            }
        }

        private void Editor_TextChanged(object sender, RoutedEventArgs e)
        {
            Editor.Document.GetText(Windows.UI.Text.TextGetOptions.None, out string doc);
            InternalSet = true;
            Text = doc;
            InternalSet = false;
            if (!RegularText) Previewer.Text = doc;
        }

        private async void Previewer_LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
        {
            try
            {
                await Launcher.LaunchUriAsync(new Uri(e.Link));
            }
            catch { }
        }

        public bool RegularText { get; set; }
        public bool IsMarkdown { get { return !RegularText; } }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        private bool InternalSet = false;
    }
}