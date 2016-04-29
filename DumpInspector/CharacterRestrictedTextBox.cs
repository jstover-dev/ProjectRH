using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Text.RegularExpressions;

namespace ProjectRH.DumpInspector {
    public class CharacterRestrictedTextBox : TextBox {

        public event Action<TextChangedEventArgs> TextChangedByUser;

        private bool _isTextUpdating = false;
        public new string Text {
            set {
                _isTextUpdating = true;
                base.Text = value;
                _isTextUpdating = false;
            }
            get { return base.Text; }
        }

        protected override void OnTextChanged(TextChangedEventArgs e) {
            base.OnTextChanged(e);
            if (!_isTextUpdating && TextChangedByUser != null) {
                TextChangedByUser(e);
            }
        }

        protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e) {
            string regex = (string)GetValue(CharacterRestrictionRegexProperty);
            e.Handled = !(String.IsNullOrEmpty(regex) || Regex.IsMatch(e.Text, regex));
            base.OnPreviewTextInput(e);
        }

        public static readonly DependencyProperty CharacterRestrictionRegexProperty = DependencyProperty.RegisterAttached(
            "CharacterRestrictionRegex",
            typeof(string),
            typeof(CharacterRestrictedTextBox),
            new FrameworkPropertyMetadata("")
        );

        public static void SetCharacterRestrictionRegex(UIElement e, String value) {
            e.SetValue(CharacterRestrictionRegexProperty, value);
        }

        public static string GetCharacterRestrictionRegex(UIElement e) {
            return (string)e.GetValue(CharacterRestrictionRegexProperty);
        }

        public string ToAscii() {
            if (String.IsNullOrEmpty(this.Text)) {
                return String.Empty;
            }
            try {
                string hex = Text.Substring(0, Math.Min(Text.Length, 2));
                string text = Convert.ToChar(Convert.ToByte(hex, 16)).ToString();
                return Convert.ToChar(Convert.ToByte(Text.Substring(0, Math.Min(Text.Length, 2)), 16)).ToString();
            } catch (FormatException) {
                return "?";
            }
        }


    }
}
