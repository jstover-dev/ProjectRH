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


		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectRH.MainWindow"/> class.
		/// </summary>
        public MainWindow() {
            InitializeComponent();
            openFileDialog = createFileDialog();
        }


		/// <summary>
		/// Creates the open file dialog for the input NVRAM dump
		/// </summary>
		/// <returns>OpenFileDialog with NVRAM filter.</returns>
        private OpenFileDialog createFileDialog() {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = @"NVRAM Images (*.nv;*.nvram;*.dat)|*.nv;*.nvram;*.dat|All Files (*.*)|*.*";
            ofd.RestoreDirectory = true;
            if (!String.IsNullOrWhiteSpace(Properties.Settings.Default.LastFile))
                ofd.InitialDirectory = Path.GetDirectoryName(Properties.Settings.Default.LastFile);
            return ofd;
        }

		/// <summary>
		/// Input file 'Browse' button handler
		/// </summary>
        private void pictureBox1_Click(object sender, EventArgs e) {

            DialogResult r = openFileDialog.ShowDialog();
            if (r == DialogResult.OK) {

				Properties.Settings.Default.LastFile = openFileDialog.FileName;
				Properties.Settings.Default.Save();

                textBoxCurrentFile.Text = openFileDialog.FileName;

                nvInspector = new NVInspector(openFileDialog.FileName);
                labelSizeValue.Text = nvInspector.Bytes.Length.ToString();
				labelNameValue.Text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
				labelFormatValue.Text = "Unknown";
            }
        }




    }
}
