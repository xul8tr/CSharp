using System.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyMappedDriveLocation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IDictionary <string, string> nwdrives = FindUNCPaths();
            foreach (KeyValuePair<string,string> item in nwdrives)
            {
                ListViewItem lvi = listViewNwDrives.Items.Add(item.Key);
                lvi.SubItems.Add(item.Value);
            }

            listViewNwDrives.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewNwDrives.Select();
            if (listViewNwDrives.Items.Count > 0)
            {
                listViewNwDrives.Items[0].Selected = true;
            }

            checkBoxAutoClose.Checked = Properties.Settings.Default.AutoClose;
        }

        public IDictionary<string, string> FindUNCPaths()
        {
            IList <DriveInfo> dis = DriveInfo.GetDrives();
            IDictionary <string, string> nwdrives = new Dictionary<string, string>();
            foreach (DriveInfo di in dis)
            {
                if (di.DriveType == DriveType.Network)
                {
                    string drive = di.RootDirectory.FullName.Substring(0, 2);
                    nwdrives.Add(drive, GetUNCPath(drive));
                }
            }

            return nwdrives;
        }

        public string GetUNCPath(string path)
        {
            if (path.StartsWith(@"\\")) return path;

            ManagementObject mo = new ManagementObject();
            mo.Path = new ManagementPath(string.Format("Win32_LogicalDisk='{0}'", path));

            //DriveType 4 = Network Drive
            if (Convert.ToUInt32(mo["DriveType"]) == 4) return Convert.ToString(mo["ProviderName"]);
            else return path;
        }

        private void checkBoxAutoClose_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoClose = ((CheckBox)sender).Checked;
            Properties.Settings.Default.Save();
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if (listViewNwDrives.Items.Count > 0 && listViewNwDrives.SelectedItems.Count > 0 && listViewNwDrives.SelectedItems[0].SubItems.Count > 1)
            {
                System.Windows.Forms.Clipboard.SetText(listViewNwDrives.SelectedItems[0].SubItems[1].Text);                
            }

            if (checkBoxAutoClose.Checked)
            {
                this.Close();
            }
        }
    }
}
