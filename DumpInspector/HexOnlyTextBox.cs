using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace ProjectRH.DumpInspector {
    public class HexOnlyTextBox : UpdateableTextBox {
        private static readonly Regex allowedCharacters = new Regex("^[0-9a-fA-F]+$");

        protected override void OnPreviewTextInput(TextCompositionEventArgs e) {
            if (allowedCharacters.IsMatch(e.Text)) {
                e.Handled = true;
            }
            base.OnPreviewTextInput(e);
        }
    }
}
