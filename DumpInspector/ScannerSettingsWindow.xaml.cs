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

        public ScannerSettingsWindow(Window owner = null, IFirmwareDefinition fwd) {
            InitializeComponent();
            if (owner != null) { this.Owner = owner; }
            this.LoginMajorByteHex.Text = String.Format("{0:x2}", fwd.LoginMajorByte).ToUpper();
            this.LoginMinorByte1Hex.Text = String.Format("{0:x2}", fwd.LoginMinorBytes[0]).ToUpper();
            this.LoginMinorByte2Hex.Text = String.Format("{0:x2}", fwd.LoginMinorBytes[1]).ToUpper();
            this.LoginMinorByte3Hex.Text = String.Format("{0:x2}", fwd.LoginMinorBytes[2]).ToUpper();
            this.LoginMinorByte4Hex.Text = String.Format("{0:x2}", fwd.LoginMinorBytes[3]).ToUpper();
            this.LoginMinorByte5Hex.Text = String.Format("{0:x2}", fwd.LoginMinorBytes[4]).ToUpper();
        }

   }
}
