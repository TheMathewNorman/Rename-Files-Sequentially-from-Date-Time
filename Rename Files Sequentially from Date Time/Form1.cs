using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Rename_Files_Sequentially_from_Date_Time
{
    public partial class frmRenameSequentially : Form
    {
        public frmRenameSequentially()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            fbdSelectedDir.ShowDialog();
            txtSelectedDir.Text = fbdSelectedDir.SelectedPath;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(fbdSelectedDir.SelectedPath))
            {
                this.Enabled = false;
                lblState.Visible = true;

                // Create an enumerable list of files and order by creation date
                DirectoryInfo di = new DirectoryInfo(fbdSelectedDir.SelectedPath);
                IEnumerable<FileInfo> iEnFi = di.EnumerateFiles();
                iEnFi = iEnFi.OrderBy(x => x.LastWriteTime);
 
                try
                {                    
                    int fileCount = iEnFi.Count();
                    int currentFileNameCount = 1;

                    //Rename each file in the directory sequentially by chronological order
                    foreach (FileInfo fi in iEnFi)
                    {
                        double currentProgressPercent = Math.Round((double)currentFileNameCount / fileCount * 100, 2);
                        lblState.Text = "Working (" + currentProgressPercent.ToString() +"%)";

                        // Rename the file
                        File.Move(fi.FullName, fbdSelectedDir.SelectedPath + '\\' + currentFileNameCount + fi.Extension);

                        currentFileNameCount++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                    Console.WriteLine(ex.Message);
                }

                this.Enabled = true;
                lblState.Text = "Done";
                lblState.ForeColor = Color.Green;
            }
        }
    }
}
