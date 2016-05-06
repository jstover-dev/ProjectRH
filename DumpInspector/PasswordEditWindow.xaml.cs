using System.Windows;

namespace ProjectRH.DumpInspector {
    /// <summary>
    /// Interaction logic for PasswordEditWindow.xaml
    /// </summary>
    public partial class PasswordEditWindow {

        public PasswordEditWindow(AdministratorLogin p, Window owner = null) {
            InitializeComponent();
            if (owner != null) { Owner = owner; }
            Username.Text = p.Username;
            Password.Text = p.Password;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
        }

    }
}
