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
using System.Globalization;

namespace ProjectRH.DumpInspector {
    /// <summary>
    /// Interaction logic for ScannerSettingsWindow.xaml
    /// </summary>
    public partial class ScannerSettingsWindow : Window {

        public IFirmwareDefinition FirmwareDefinition { get; private set; }

        public ScannerSettingsWindow(Window owner, IFirmwareDefinition fwd) {
            InitializeComponent();
            if (owner != null) { this.Owner = owner; }
            this.FirmwareDefinition = fwd;
            this.DataContext = this;

            this.UADVersionComboBox.ItemsSource = UADVersion.KnownVersions;
            this.UADVersionComboBox.DisplayMemberPath = "Key";
            this.UADVersionComboBox.SelectedValuePath = "Value";
            this.UADVersionComboBox.SelectedIndex = UADVersionComboBox.Items.Count;

            //this.UADVersionComboBox.Items = UADVersion.GetAllKnownVersions().Select(v => v.VersionNumber);
        }

    }

}
