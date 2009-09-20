using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WCSA;

namespace WCSAGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            openAnotherWebConfirForAnalyzeToolStripMenuItem_Click(sender, e);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void openAnotherWebConfirForAnalyzeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath = "";
            System.IO.Stream myStream;

            OpenFileDialog ofd = new OpenFileDialog();
            StreamReader streamReader;
            ofd.Filter = "web.config Files (*.config)|*.config|Text Files (*.txt)|*.txt| All Files (*.*)|*.*";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = ofd.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            try
                            {
                                ScanEngine scanEngine = new ScanEngine();
                                Reporter reporter = new Reporter();
                                scanEngine.StartScan(ofd.FileName);
                                filePath = reporter.GenerateReport(scanEngine.vulns, ofd.FileName);
                            }
                            catch (WCSAException wcsaexception)
                            {
                                MessageBox.Show(Constants.FILE_NOT_FOUND_MESSAGE);
                                Application.Exit();
                            }

                            streamReader = new StreamReader(filePath);
                            filePath = Application.StartupPath + @"\" + filePath;
                            streamReader.Close();
                            webBrowser1.Navigate(filePath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
    }
}
