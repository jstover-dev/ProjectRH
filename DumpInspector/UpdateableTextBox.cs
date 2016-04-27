using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace ProjectRH.DumpInspector {
    public class UpdateableTextBox : TextBox {

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

        public static readonly DependencyProperty CharacterRestrictionRegexProperty = DependencyProperty.RegisterAttached(
            "CharacterRestrictionRegex",
            typeof(string),
            typeof(UpdateableTextBox),
            new FrameworkPropertyMetadata("")
        );

        public static void SetCharacterRestrictionRegex(UIElement e, String value) {
            e.SetValue(CharacterRestrictionRegexProperty, value);
        }

        public static string GetCharacterRestrictionRegex(UIElement e) {
            return (string)e.GetValue(CharacterRestrictionRegexProperty);
        }


        public string ToHex() {
            if (String.IsNullOrEmpty(this.Text)) {
                return String.Empty;
            }
            return String.Format("{0:x2}", (int)this.Text[0]).ToUpper();
        }

        public string ToAscii() {
            if (String.IsNullOrEmpty(this.Text)) {
                return String.Empty;
            }
            string hex = Text.Substring(0, Math.Min(Text.Length, 2));
            //string text = Convert.ToChar(Convert.ToByte(hex, 16)).ToString();
            //return Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(Convert.ToByte(hex, 16)));

            try {
                return Encoding.ASCII.GetString(new[] { Convert.ToByte(hex, 16) });
            } catch (FormatException) {
                return String.Empty;
            }
        }


    }
}
