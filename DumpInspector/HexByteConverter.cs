using System;
using System.Windows.Data;
using System.Globalization;

namespace ProjectRH.DumpInspector {

    public class HexByteConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return String.Format("{0:x2}", (byte)value).ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return System.Convert.ToByte(value as string, 16);
        }

    }
}
