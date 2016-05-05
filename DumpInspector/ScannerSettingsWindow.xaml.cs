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
        public IEnumerable<UADVersion> UADVersions { get; private set; }

        public ScannerSettingsWindow(Window owner, IFirmwareDefinition fwd) {
            InitializeComponent();
            if (owner != null) { this.Owner = owner; }
            this.DataContext = this; 

            this.FirmwareDefinition = fwd;
            this.UADVersions = UADVersion.KnownVersions;

        }

        private void RescanButton_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
        }

    }


    public class ComboBoxToLongestItemWidthConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {

            if (value is IEnumerable<UADVersion>) {
                return (value as IEnumerable<UADVersion>).Max(u => u.ToString().Length) * 12;
            } else {
                return value.ToString().Length * 12;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

}
