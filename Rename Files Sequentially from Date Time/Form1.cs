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

                // Create an enumerable list of files and order by creation date
                DirectoryInfo di = new DirectoryInfo(fbdSelectedDir.SelectedPath);
                IEnumerable<FileInfo> iEnFi = di.EnumerateFiles();
                iEnFi = iEnFi.OrderBy(x => x.CreationTime);
 
                try
                {                    
                    int fileCount = iEnFi.Count();
                    int currentFileNameCount = 1;

                    // Rename each file in the directory sequentially by chronological order
                    foreach (FileInfo fi in iEnFi)
                    {
                        // Get current file's extension
                        string fileExt = '.' + fi.Name.Split('.')[fi.Name.Split('.').Length - 1];

                        double currentProgressPercent = currentFileNameCount / fileCount * 100;

                        // Log current progress to console window
                        Console.WriteLine("Progress: {0}% ({1}/{2})", currentProgressPercent, currentFileNameCount, fileCount);
                        Console.WriteLine("{0} to {1} ({2})", fi.Name, currentFileNameCount, fi.CreationTime);

                        // Rename the file
                        File.Move(fi.FullName, fbdSelectedDir.SelectedPath + '\\' + currentFileNameCount + fileExt);

                        currentFileNameCount++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                this.Enabled = true;
            }
        }
    }
}
