using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Linq;

namespace ProjectRH.DumpInspector {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {

        private FirmwareFile FirmwareFile { get; set; }
        private Settings Settings { get; set; }

        public ObservableCollection<AdministratorLogin> logins { get; set; }

        private static readonly string VersionString = "1.06";

        public MainWindow() {
            DataContext = this;
            InitializeComponent();
            Closed += (s, e) => OnApplicationExit();
            Title = String.Format("{0} [{1}]", Title, VersionString);
            Settings = new Settings();
            if (Settings.Load() == SettingsResult.LoadError) {
                StatusMessage.Content = Settings.LastException.Message;
            }
            logins = new ObservableCollection<AdministratorLogin>();
            DataGrid.ItemsSource = logins;
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
                StatusMessage.Content = String.Format("Scanning {0} ...", System.IO.Path.GetFileName(ofd.FileName));
                Settings.LastOpenPath = System.IO.Path.GetDirectoryName(ofd.FileName);
                FirmwareFile = new FirmwareFile(ofd.FileName);
                StatusMessage.Content = FirmwareFile.Length.AsHumanFileSize();
                FirmwareMessage.Content = FirmwareFile.FirmwareString;
                logins.Clear();
                foreach (var a in FirmwareFile.Rescan()) {
                    logins.Add(a);
                }
            }
        }

        private void ExportFile() {
            SaveFileDialog sfd;
            if ((sfd = ShowFileExportDialog()) != null && FirmwareFile!=null) {
                Settings.LastExportPath = System.IO.Path.GetDirectoryName(sfd.FileName);
                FirmwareFile.AdministratorLogins = logins;
                FirmwareFile.Write(sfd.FileName);
            }
        }

        private void ClearPasswords() {
            foreach (var x in logins) {
                x.Password = string.Empty;
            }
        }

        private void ShowScannerSettings() {
            var w = new ScannerSettingsWindow(this, FirmwareFile.FirmwareDefinition);
            if (w.ShowDialog() == true) {
                logins.Clear();
                foreach( var a in FirmwareFile.Rescan(w.FirmwareDefinition)) {
                    logins.Add(a);
                }
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

        // Edit -> Allow Editing
        private void AllowEditing_Executed(object sender, ExecutedRoutedEventArgs e) {
            DataGrid.IsReadOnly = !AllowEditingCheckbox.IsChecked;
        }

        // Edit -> Clear Passwords
        private void ClearCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            ClearPasswords();
        }

        // Edit -> Scanner Settings
        private void ScannerSettingsCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            ShowScannerSettings();
        }


/* Window Events */
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            OnApplicationStart();
        }


/* Command CanExecute Functions */

        private void ExecuteIfFileOpen(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = FirmwareFile != null;
        }

        private void ScannerSettingsCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = FirmwareFile != null;
        }

        private void ClearPasswordsCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = FirmwareFile != null && AllowEditingCheckbox.IsChecked;
        }

        private void ExecutionDisabled(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = false;
        }

        private void ExecutionEnabled(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }
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
        public static RoutedCommand AllowEditing = new RoutedUICommand(
            "Allow Editing",
            "AllowEditing",
            typeof(MenuItem),
            new KeyGesture(Key.E, ModifierKeys.Alt).AsCollection()
        );
    }


}
