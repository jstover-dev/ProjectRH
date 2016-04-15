﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace ProjectRH.DumpInspector {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private FirmwareFile firmware { get; set; }
        private Settings settings { get; set; }

        private static readonly string VersionString = "1.04";

        public MainWindow() {
            InitializeComponent();
            InitializeSettings();
            this.Closed += (s, e) => OnApplicationExit();
            this.Title = String.Format("{0} [{1}]", Title, VersionString);
        }

        private void InitializeSettings() {
            settings = new Settings();
            if (settings.Load() == SettingsResult.LoadError){
                statusMessage.Content = settings.LastException.Message;
            }
        }

        private void OnApplicationExit() {
            settings.Save();
        }

        private void OnApplicationStart() {
            this.OpenFile();
        }

        private OpenFileDialog ShowFileOpenDialog(){
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "NVRAM dump (*.nv, *.nvram)|*.nv;*.nvram|All Files (*.*)|*.*";
            ofd.InitialDirectory = System.IO.Directory.Exists(settings.LastOpenPath) ? settings.LastOpenPath : null ;
            bool success = false;
            try {
                success = ofd.ShowDialog(this) ?? false;
            } catch (System.ComponentModel.Win32Exception) {
                ofd.InitialDirectory = null;
                success = ofd.ShowDialog(this) ?? false;
            }
            return success ? ofd : null ;
        }

        private SaveFileDialog ShowFileExportDialog() {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "NVRAM Dump (*.nv)|*.nv";
            sfd.InitialDirectory = System.IO.Directory.Exists(settings.LastExportPath) ? settings.LastExportPath : null ;
            bool success = false;
            try {
                success = sfd.ShowDialog(this) ?? false;
            } catch (System.ComponentModel.Win32Exception) {
                sfd.InitialDirectory = null;
                success = sfd.ShowDialog(this) ?? false;
            }
            return success ? sfd : null;
        }

        private void OpenFile() {
            OpenFileDialog ofd;
            if ((ofd = ShowFileOpenDialog()) != null) {
                settings.LastOpenPath = System.IO.Path.GetDirectoryName(ofd.FileName);
                this.firmware = new FirmwareFile(ofd.FileName);
                this.statusMessage.Content = firmware.Length.AsHumanFileSize();
                this.firmwareMessage.Content = String.Format("{0}", firmware.FirmwareString);
                dataGrid.ItemsSource = firmware.GetPasswords();
                dataGrid.Items.Refresh();
            }
        }

        private void ExportFile() {
            SaveFileDialog sfd;
            if ((sfd = ShowFileExportDialog()) != null) {
                settings.LastExportPath = System.IO.Path.GetDirectoryName(sfd.FileName);
                firmware.WriteFile(sfd.FileName);
            }
        }

        private void ClearPasswords() {
            foreach (AdministratorLogin x in dataGrid.Items) {
                x.Password = "";
            }
            dataGrid.Items.Refresh();
        }

        private void EditPassword(AdministratorLogin pw) {
            PasswordEditWindow editor = new PasswordEditWindow(pw, this);
            if (editor.ShowDialog()??false) {
                pw.Username = editor.Username.Text;
                pw.Password = editor.Password.Text;
                dataGrid.Items.Refresh();
            }
        }


/* MenuItem Click Handlers */

        // File -> Open
        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            this.OpenFile();
        }

        // File -> Export
        private void ExportCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            this.ExportFile();
        }

        // File -> Exit
        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        // Edit -> Clear Passwords
        private void ClearCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            this.ClearPasswords();
        }

        // Password Edit Button
        private void EditPassword(object sender, RoutedEventArgs e) {
            this.EditPassword((sender as Button).DataContext as AdministratorLogin);
        }

/* Window Events */

        // Open file on application start
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            this.OnApplicationStart();
        }

        // Adjust the datagrid after auto column generation
        private void dataGrid_AutoGeneratedColumns(object sender, EventArgs e) {
            dataGrid.Columns[0].DisplayIndex = dataGrid.Columns.Count - 1;
            dataGrid.Columns[1].Width = 10;
            dataGrid.Columns[2].Width = DataGridLength.Auto;
        }


/* Command CanExecute Functions */

        // Enable command if a file has been opened
        private void ExecuteIfFileOpen(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = (firmware != null);
        }

        // disable command
        private void ExecutionDisabled(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = false;
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
    }


}
