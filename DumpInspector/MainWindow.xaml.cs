using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace ProjectRH.DumpInspector {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {

        private FirmwareFile FirmwareFile { get; set; }
        private Settings Settings { get; set; }

        private static readonly string VersionString = "1.05*";

        public MainWindow() {
            InitializeComponent();
            Closed += (s, e) => OnApplicationExit();
            Title = String.Format("{0} [{1}]", Title, VersionString);
            FirmwareFile = new FirmwareFile();
            Settings = new Settings();
            if (Settings.Load() == SettingsResult.LoadError) {
                StatusMessage.Content = Settings.LastException.Message;
            }
        }

        private void OnApplicationExit() {
            Settings.Save();
        }

        private void OnApplicationStart() {
            OpenFile();
        }

        private OpenFileDialog ShowFileOpenDialog(){
            OpenFileDialog ofd = new OpenFileDialog {
                Filter = "NVRAM dump (*.nv, *.nvram)|*.nv;*.nvram|All Files (*.*)|*.*",
                InitialDirectory = System.IO.Directory.Exists(Settings.LastOpenPath) ? Settings.LastOpenPath : null
            };
            bool success;
            try {
                success = (bool) ofd.ShowDialog(this);
            } catch (System.ComponentModel.Win32Exception) {
                ofd.InitialDirectory = null;
                success = (bool) ofd.ShowDialog(this);
            }
            return success ? ofd : null ;
        }

        private SaveFileDialog ShowFileExportDialog() {
            SaveFileDialog sfd = new SaveFileDialog {
                Filter = "NVRAM Dump (*.nv)|*.nv",
                InitialDirectory = System.IO.Directory.Exists(Settings.LastExportPath) ? Settings.LastExportPath : null
            };
            bool success;
            try {
                success = (bool) sfd.ShowDialog(this);
            } catch (System.ComponentModel.Win32Exception) {
                sfd.InitialDirectory = null;
                success = (bool) sfd.ShowDialog(this);
            }
            return success ? sfd : null;
        }

        private void OpenFile() {
            OpenFileDialog ofd;
            if ((ofd = ShowFileOpenDialog()) != null) {
                Settings.LastOpenPath = System.IO.Path.GetDirectoryName(ofd.FileName);
                FirmwareFile = new FirmwareFile(ofd.FileName);
                StatusMessage.Content = FirmwareFile.Length.AsHumanFileSize();
                FirmwareMessage.Content = String.Format("{0}", FirmwareFile.FirmwareString);
                DataGrid.ItemsSource = FirmwareFile.GetPasswords();
                DataGrid.Items.Refresh();
            }
        }

        private void ExportFile() {
            SaveFileDialog sfd;
            if ((sfd = ShowFileExportDialog()) != null) {
                Settings.LastExportPath = System.IO.Path.GetDirectoryName(sfd.FileName);
                FirmwareFile.WriteFile(sfd.FileName);
            }
        }

        private void ClearPasswords() {
            foreach (AdministratorLogin x in DataGrid.Items) {
                x.Password = "";
            }
            DataGrid.Items.Refresh();
        }

        private void ShowScannerSettings() {
            var w = new ScannerSettingsWindow(this, FirmwareFile.FirmwareDefinition);
            if (w.ShowDialog() == true) {
                DataGrid.ItemsSource = FirmwareFile.GetPasswords(w.FirmwareDefinition);
                DataGrid.Items.Refresh();
            }
        }

        private void EditPassword(AdministratorLogin pw) {
            var w = new PasswordEditWindow(pw, this);
            if (w.ShowDialog() == true) {
                pw.Username = w.Username.Text;
                pw.Password = w.Password.Text;
                DataGrid.Items.Refresh();
            }
        }


/* MenuItem Click Handlers */

        // File -> Open
        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            OpenFile();
        }

        // File -> Export
        private void ExportCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            ExportFile();
        }

        // File -> Exit
        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        // Edit -> Clear Passwords
        private void ClearCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            ClearPasswords();
        }

        // Edit -> Scanner Settings
        private void ScannerSettingsCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            ShowScannerSettings();
        }

        // Password Edit Button
        private void EditPassword(object sender, RoutedEventArgs e) {
            EditPassword(((Button)sender).DataContext as AdministratorLogin);
        }


/* Window Events */

        // Open file on application start
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            OnApplicationStart();
        }

        // Adjust the datagrid after auto column generation
        private void dataGrid_AutoGeneratedColumns(object sender, EventArgs e) {
            DataGrid.Columns[0].DisplayIndex = DataGrid.Columns.Count - 1;
            DataGrid.Columns[1].Width = 10;
            DataGrid.Columns[2].Width = DataGridLength.Auto;
        }


/* Command CanExecute Functions */

        // Enable command if a file has been opened
        private void ExecuteIfFileOpen(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = FirmwareFile != null;
        }

        // disable command
        private void ExecutionDisabled(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = false;
        }

/*
        private void ExecutionEnabled(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }
*/
    }


    /// <summary>
    /// DumpInspector application commands
    /// </summary>
    public static class Commands {
        public static RoutedUICommand Open = new RoutedUICommand(
            "Open NVRAM...",
            "Open",
            typeof(MenuItem),
            new KeyGesture(Key.O, ModifierKeys.Control).AsCollection()
        );
        public static RoutedUICommand Export = new RoutedUICommand(
            "Export...",
            "Export",
            typeof(MenuItem),
            new KeyGesture(Key.E, ModifierKeys.Control).AsCollection()
        );
        public static RoutedUICommand Exit = new RoutedUICommand(
            "Exit",
            "Exit",
            typeof(MenuItem),
            new KeyGesture(Key.X, ModifierKeys.Alt).AsCollection()
        );
        public static RoutedUICommand Clear = new RoutedUICommand(
            "Clear Passwords",
            "Clear",
            typeof(MenuItem),
            new KeyGesture(Key.C, ModifierKeys.Alt).AsCollection()
        );
        public static RoutedCommand ScannerSettings = new RoutedUICommand(
            "Scanner Settings",
            "ScannerSettings",
            typeof(MenuItem),
            new KeyGesture(Key.S, ModifierKeys.Alt).AsCollection()
        );
    }


}
