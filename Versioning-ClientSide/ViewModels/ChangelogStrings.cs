using Windows.UI.Xaml;

namespace UWPVersioningToolkit.ViewModels
{
    public class ChangelogStrings : DependencyObject
    {
        // Using a DependencyProperty as the backing store for Changelog.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChangelogProperty =
            DependencyProperty.Register(nameof(Changelog), typeof(string), typeof(ChangelogStrings), new PropertyMetadata("Changelog"));

        // Using a DependencyProperty as the backing store for NewInVersion.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewInVersionProperty =
            DependencyProperty.Register(nameof(NewInVersion), typeof(string), typeof(ChangelogStrings), new PropertyMetadata("New in this Version"));

        // Using a DependencyProperty as the backing store for WhatsNew.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WhatsNewProperty =
            DependencyProperty.Register(nameof(WhatsNew), typeof(string), typeof(ChangelogStrings), new PropertyMetadata("What's New"));

        // Using a DependencyProperty as the backing store for Fixed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FixedProperty =
            DependencyProperty.Register(nameof(Fixed), typeof(string), typeof(ChangelogStrings), new PropertyMetadata("Fixed"));

        // Using a DependencyProperty as the backing store for OlderVersions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OlderVersionsProperty =
            DependencyProperty.Register(nameof(OlderVersions), typeof(string), typeof(ChangelogStrings), new PropertyMetadata("Older Versions"));

        // Using a DependencyProperty as the backing store for Close.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseProperty =
            DependencyProperty.Register(nameof(Close), typeof(string), typeof(ChangelogStrings), new PropertyMetadata("Close"));

        public string Changelog
        {
            get { return (string)GetValue(ChangelogProperty); }
            set { SetValue(ChangelogProperty, value); }
        }

        public string NewInVersion
        {
            get { return (string)GetValue(NewInVersionProperty); }
            set { SetValue(NewInVersionProperty, value); }
        }

        public string WhatsNew
        {
            get { return (string)GetValue(WhatsNewProperty); }
            set { SetValue(WhatsNewProperty, value); }
        }

        public string Fixed
        {
            get { return (string)GetValue(FixedProperty); }
            set { SetValue(FixedProperty, value); }
        }

        public string OlderVersions
        {
            get { return (string)GetValue(OlderVersionsProperty); }
            set { SetValue(OlderVersionsProperty, value); }
        }

        public string Close
        {
            get { return (string)GetValue(CloseProperty); }
            set { SetValue(CloseProperty, value); }
        }
    }
}