using System;
using System.Text.RegularExpressions;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace UWPVersioningToolkit.Controls
{
    /// <summary>
    /// A Modified TextBox that only accepts Numeric Text.
    /// </summary>
    public sealed class NumericTextBox : TextBox
    {
        public NumericTextBox()
        {
            this.DefaultStyleKey = typeof(TextBox);
            InputScope scope = new InputScope();
            scope.Names.Add(new InputScopeName { NameValue = InputScopeNameValue.Number });
            InputScope = scope;
            KeyDown += NumericTextBox_KeyDown;
            TextChanged += NumericTextBox_TextChanged;
        }

        private void NumericTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex rgx = new Regex("[^0-9 -]");
            Text = rgx.Replace(Text, "");

            if (!string.IsNullOrWhiteSpace(Text))
            {
                LongValue = Convert.ToInt64(Text);
                DoubleValue = Convert.ToDouble(Text);
            }
            else
            {
                LongValue = null;
                DoubleValue = null;
            }
        }

        private void NumericTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if ((e.Key < VirtualKey.NumberPad0 || e.Key > VirtualKey.NumberPad9) & (e.Key < VirtualKey.Number0 || e.Key > VirtualKey.Number9) && e.Key != VirtualKey.Tab)
            {
                e.Handled = true;
            }
        }

        public long? LongValue
        {
            get { return (long?)GetValue(LongValueProperty); }
            set { SetValue(LongValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LongValueProperty =
            DependencyProperty.Register(nameof(LongValue), typeof(long?), typeof(NumericTextBox), new PropertyMetadata(null, LongValueChanged));

        public double? DoubleValue
        {
            get { return (double?)GetValue(DoubleValueProperty); }
            set { SetValue(DoubleValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DoubleValueProperty =
            DependencyProperty.Register(nameof(DoubleValue), typeof(double?), typeof(NumericTextBox), new PropertyMetadata(null, DoubleValueChanged));

        public static void LongValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is NumericTextBox box)
            {
                box.Text = box.LongValue.HasValue ? box.LongValue.ToString() : "";
            }
        }

        public static void DoubleValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is NumericTextBox box)
            {
                box.Text = box.DoubleValue.HasValue ? box.DoubleValue.ToString() : "";
            }
        }
    }
}