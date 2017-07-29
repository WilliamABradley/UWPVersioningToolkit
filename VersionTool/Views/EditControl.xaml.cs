using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPVersioningToolkit.Views
{
    /// <summary>
    /// The Internal Editor for each of Whats New, Fixed and Store Summary, providing Text Bindings with the RichEditBox, and associated TextToolbar.
    /// </summary>
    public partial class EditControl : UserControl, INotifyPropertyChanged
    {
        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(EditControl), new PropertyMetadata("", TextChanged));

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(EditControl), new PropertyMetadata(""));

        // Using a DependencyProperty as the backing store for MaxLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register(nameof(MaxLength), typeof(int), typeof(EditControl), new PropertyMetadata(-1));

        // Using a DependencyProperty as the backing store for AutomaticGeneration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutomaticGenerationProperty =
            DependencyProperty.Register(nameof(AutomaticGeneration), typeof(bool), typeof(EditControl), new PropertyMetadata(true));

        public EditControl()
        {
            this.InitializeComponent();
            this.Loaded += EditControl_Loaded;
            Editor.KeyDown += Editor_KeyDown;
        }

        /// <summary>
        /// Prevents Automatic Generation after a Key Press is registered.
        /// </summary>
        private void Editor_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (AutomaticGeneration)
            {
                AutomaticGeneration = false;
            }
        }

        /// <summary>
        /// Sets Automatic Generation on, if the Field is empty, once loaded.
        /// </summary>
        private async void EditControl_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(120);
            if (string.IsNullOrWhiteSpace(Text)) AutomaticGeneration = true;
        }

        /// <summary>
        /// Updates the RichEditBox, when the Text Property is updated.
        /// </summary>
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

        /// <summary>
        /// Updates Counters and TextProperty, when the Text Changes.
        /// </summary>
        private void Editor_TextChanged(object sender, RoutedEventArgs e)
        {
            Editor.Document.GetText(Windows.UI.Text.TextGetOptions.None, out string doc);
            InternalSet = true;
            Text = doc;
            Length = doc.Trim().Length;
            InternalSet = false;
            if (!IsStoreSummary) Previewer.Text = doc;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Limiter)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OverLimit)));
        }

        /// <summary>
        /// Handler for Links in the Previews.
        /// </summary>
        private async void Previewer_LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
        {
            try
            {
                await Launcher.LaunchUriAsync(new Uri(e.Link));
            }
            catch { }
        }

        /// <summary>
        /// How many characters the Current RichEditBox is at.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Max Length the Text should be.
        /// </summary>
        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        /// <summary>
        /// Presentation of the length Limitation.
        /// </summary>
        public string Limiter
        {
            get
            {
                return $"{Length}/{MaxLength}";
            }
        }

        /// <summary>
        /// Is the Text Limited?
        /// </summary>
        public bool HasLimit
        {
            get { return MaxLength > -1; }
        }

        /// <summary>
        /// Is the Text over the Limit?
        /// </summary>
        public bool OverLimit
        {
            get { return Length > MaxLength; }
        }

        /// <summary>
        /// Automatically Generate the Text? (Store Summary Only)
        /// </summary>
        public bool AutomaticGeneration
        {
            get { return (bool)GetValue(AutomaticGenerationProperty); }
            set { SetValue(AutomaticGenerationProperty, value); }
        }

        /// <summary>
        /// Is it the Store Summary?
        /// </summary>
        public bool IsStoreSummary { get; set; }

        /// <summary>
        /// Is the Text Markdown? For showing the Previewer.
        /// </summary>
        public bool IsMarkdown { get { return !IsStoreSummary; } }

        /// <summary>
        /// Section Header Presenter.
        /// </summary>
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// The Current Text.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Prevents a Set Loop for Changing the Text Property.
        /// </summary>
        private bool InternalSet = false;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}