using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
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
            else
                if(webBrowser1.Url == null)
                    webBrowser1.Navigate("http://code.google.com/p/wcsa");
        }

        private void saveCurrentReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowSaveAsDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }



        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }


        //Very bad implementation for links for this release
        private void bugFeatureRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process process = new Process();
            process.StartInfo.FileName = "http://code.google.com/p/wcsa/issues/list";
            process.Start();
        }

        private void top10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process process = new Process();
            process.StartInfo.FileName = "http://h71028.www7.hp.com/ERC/cache/571845-0-0-0-121.html";
            process.Start();
        }

        private void top10ApplicationSecurityVulnerabilitiesİnWebconfigFilesPartTwoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process process = new Process();
            process.StartInfo.FileName = "http://h71028.www7.hp.com/ERC/cache/571914-0-0-0-121.html";
            process.Start();
        }

        private void theASPNETWebconfigFileDemystifiedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process process = new Process();
            process.StartInfo.FileName = "http://articles.sitepoint.com/article/web-config-file-demystified";
            process.Start();
        }

        private void aSPNetWebconfigSecurityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process process = new Process();
            process.StartInfo.FileName = "http://liamb.com/2008/07/16/aspnet-webconfig-security/";
            process.Start();
        }

        private void encryptingConnectionStringsİnWebconfigFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process process = new Process();
            process.StartInfo.FileName = "http://www.beansoftware.com/ASP.NET-Tutorials/Encrypting-Connection-String.aspxl";
            process.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (File.Exists(Constants.REPORT_NAME))
                File.Delete(Constants.REPORT_NAME);
        }
    }
}
