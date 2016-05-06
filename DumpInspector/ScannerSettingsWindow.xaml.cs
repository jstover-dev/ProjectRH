using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace ProjectRH.DumpInspector {
    /// <summary>
    /// Interaction logic for ScannerSettingsWindow.xaml
    /// </summary>
    public partial class ScannerSettingsWindow {

        public IFirmwareDefinition FirmwareDefinition { get; private set; }
        public IEnumerable<UadVersion> UadVersions { get; private set; }

        public ScannerSettingsWindow(Window owner, IFirmwareDefinition fwd) {
            InitializeComponent();
            if (owner != null) { Owner = owner; }
            DataContext = this; 

            FirmwareDefinition = fwd;
            UadVersions = UadVersion.KnownVersions;

        }

        private void RescanButton_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
        }

    }


    public class ComboBoxToLongestItemWidthConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            var versions = value as IEnumerable<UadVersion>;
            if (versions != null) {
                return versions.Max(u => u.ToString().Length) * 12;
            }
            return value.ToString().Length * 12;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

}
