using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ProjectRH
{
    public partial class MainWindow : Form
    {

        private OpenFileDialog openFileDialog;
        private NVInspector nvInspector;

        public MainWindow() {
            InitializeComponent();
            openFileDialog = createFileDialog();
        }

        private OpenFileDialog createFileDialog() {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = @"NVRAM Images (*.nv;*.nvram;*.dat)|*.nv;*.nvram;*.dat|All Files (*.*)|*.*";
            ofd.RestoreDirectory = true;
            if (!String.IsNullOrWhiteSpace(Properties.Settings.Default.LastFile))
                ofd.InitialDirectory = Path.GetDirectoryName(Properties.Settings.Default.LastFile);
            return ofd;
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
            DialogResult r = openFileDialog.ShowDialog();
            if (r == DialogResult.OK) {
                textBoxCurrentFile.Text = openFileDialog.FileName;
                Properties.Settings.Default.LastFile = openFileDialog.FileName;
                Properties.Settings.Default.Save();

                nvInspector = new NVInspector(openFileDialog.FileName);
                Console.WriteLine("Read {0} bytes", nvInspector.Bytes.Length);
            }
        }



    }
}
