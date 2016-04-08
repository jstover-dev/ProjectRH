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
    /// Interaction logic for PasswordEditWindow.xaml
    /// </summary>
    public partial class PasswordEditWindow : Window {
        public PasswordEditWindow(AdministratorLogin p) {
            InitializeComponent();
            this.Username.Text = p.Username;
            this.Password.Text = p.Password;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
        }

    }
}
