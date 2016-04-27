using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectRH.DumpInspector {
    /// <summary>
    /// Interaction logic for ScannerSettingsWindow.xaml
    /// </summary>
    public partial class ScannerSettingsWindow : Window {

        public ScannerSettingsWindow(Window owner = null) {
            InitializeComponent();

            if (owner != null) { this.Owner = owner; }


            // this is really lazy, error-prone, copy-paste code
            LoginMajorByteAscii.TextChangedByUser += e =>
                LoginMajorByteHex.Text = LoginMajorByteAscii.ToHex();
            LoginMajorByteHex.TextChangedByUser += e =>
                LoginMajorByteAscii.Text = LoginMajorByteHex.ToAscii();

            LoginMinorByte1Ascii.TextChangedByUser += e =>
                LoginMinorByte1Hex.Text = LoginMinorByte1Ascii.ToHex();
            LoginMinorByte1Hex.TextChangedByUser += e =>
                LoginMinorByte1Ascii.Text = LoginMinorByte1Hex.ToAscii();

            LoginMinorByte2Ascii.TextChangedByUser += e =>
                LoginMinorByte2Hex.Text = LoginMinorByte2Ascii.ToHex();
            LoginMinorByte2Hex.TextChangedByUser += e =>
                LoginMinorByte2Ascii.Text = LoginMinorByte2Hex.ToAscii();

            LoginMinorByte3Ascii.TextChangedByUser += e =>
                LoginMinorByte3Hex.Text = LoginMinorByte3Ascii.ToHex();
            LoginMinorByte3Hex.TextChangedByUser += e =>
                LoginMinorByte3Ascii.Text = LoginMinorByte3Hex.ToAscii();

            LoginMinorByte4Ascii.TextChangedByUser += e =>
                LoginMinorByte4Hex.Text = LoginMinorByte4Ascii.ToHex();
            LoginMinorByte4Hex.TextChangedByUser += e =>
                LoginMinorByte4Ascii.Text = LoginMinorByte4Hex.ToAscii();

            LoginMinorByte5Ascii.TextChangedByUser += e =>
             LoginMinorByte5Hex.Text = LoginMinorByte5Ascii.ToHex();
            LoginMinorByte5Hex.TextChangedByUser += e =>
                LoginMinorByte5Ascii.Text = LoginMinorByte5Hex.ToAscii();

        }

        /*
        private void LoginMajorByteHex_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
            if (!char.IsControl(e.Key)
        }
         * */

   }
}
