using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using Microsoft.Win32;
using Microsoft.TeamFoundation.Proxy;
using Microsoft.TeamFoundation.Common;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    public partial class DeleteWorkItemUtility : Form
    {
        private string _toolsPath;
        private readonly Dictionary<string, TfsTeamProjectCollection> _servers = new Dictionary<string, TfsTeamProjectCollection>();

        public DeleteWorkItemUtility()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show(this, string.Format("Are you sure you want to delete the ({0}) selected work items?  This action cannot be undone.", this.lvResults.SelectedItems.Count), "Delete Selected Work Items", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes) && (MessageBox.Show(this, string.Format("Please confirm your decision to delete the ({0}) selected work items.", this.lvResults.SelectedItems.Count), "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes))
            {
                this.DeleteSelectedWorkItems(this.cboTfsServer.Text);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.SearchWorkItems();
        }

        private void btnServers_Click(object sender, EventArgs e)
        {
            this.DisplayServerDialog();
        }

        private void cboProject_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.LoadSavedQueries(this.cboTfsServer.Text, this.cboProject.Text);
            this.txtIds.Clear();
            this.lvResults.Items.Clear();
            this.btnDelete.Enabled = false;
        }

        private void cboTfsServer_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.LoadProjectList(this.cboTfsServer.Text);
        }

        private void DeleteSelectedWorkItems(string tfsServer)
        {
            string format = "destroywi /collection:{0} /id:{1} /noprompt";
            char ch = ',';
            string str2 = string.Empty;
            foreach (ListViewItem item in this.lvResults.SelectedItems)
            {
                if (item.Selected)
                {
                    str2 = str2 + item.Text + ch;
                }
            }
            str2 = str2.TrimEnd(new char[] { ch });
            format = string.Format(format, tfsServer, str2);
            ProcessStartInfo info2 = new ProcessStartInfo(this.PowerToolsPath, format);
            info2.CreateNoWindow = true;
            info2.WindowStyle = ProcessWindowStyle.Hidden;
            ProcessStartInfo startInfo = info2;
            Process process = Process.Start(startInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
            this.SearchWorkItems();
        }

        private void DeleteWorkItemForm_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + string.Format(" [v{0}]", Assembly.GetExecutingAssembly().GetName().Version);
            this.LocatePowerTools();
            this.LoadServerList();
            if (this.cboTfsServer.SelectedIndex >= 0)
            {
                this.LoadProjectList(this.cboTfsServer.Text);
            }
        }

        private void DisplayServerDialog()
        {
            TeamProjectPicker picker = new TeamProjectPicker(TeamProjectPickerMode.SingleProject, false);
            if (picker.ShowDialog() == DialogResult.OK)
            {
                this.LoadServerList();
            }
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (this.components != null))
        //    {
        //        this.components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        private TfsTeamProjectCollection GetServer(string serverName)
        {
            //TfsTeamProjectCollection tfsProj = tpp.SelectedTeamProjectCollection;
            TfsTeamProjectCollection server = null;
            try
            {
                if (!this._servers.ContainsKey(serverName))
                {
                    server = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(serverName));
                    //server = TeamFoundationServerFactory.GetServer(serverName, new UICredentialsProvider());
                    server.EnsureAuthenticated();
                    this._servers.Add(serverName, server);
                    return server;
                }
                return this._servers[serverName];
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return null;
            }
        }

        private void LoadProjectList(string tfsServer)
        {
            TfsTeamProjectCollection server = this.GetServer(tfsServer);
            if (server != null)
            {
                this.cboProject.Items.Clear();
                WorkItemStore service = (WorkItemStore)server.GetService(typeof(WorkItemStore));
                foreach (Project project in service.Projects)
                {
                    this.cboProject.Items.Add(project.Name);
                }
                if (this.cboProject.Items.Count > 0)
                {
                    this.cboProject.SelectedIndex = 0;
                    this.LoadSavedQueries(tfsServer, this.cboProject.Text);
                }
            }
        }

        private void LoadSavedQueries(string tfsServer, string teamProject)
        {
            TfsTeamProjectCollection server = this.GetServer(tfsServer);
            if (server != null)
            {
                this.cboQueries.Items.Clear();
                WorkItemStore service = (WorkItemStore)server.GetService(typeof(WorkItemStore));
                if (service.Projects.Contains(teamProject))
                {
                    foreach (QueryFolder folder in service.Projects[teamProject].QueryHierarchy)
                    {
                        foreach (QueryItem query in folder)
                        {
                            this.cboQueries.Items.Add(query.Name);
                        }
                    }
                    if (this.cboQueries.Items.Count > 0)
                    {
                        this.cboQueries.SelectedIndex = 0;
                    }
                }
            }
        }

        private void LoadServerList()
        {
            this.cboTfsServer.DataSource = RegisteredTfsConnections.GetProjectCollections();
            this.cboTfsServer.DisplayMember = "Uri";
        }

        private void LocatePowerTools()
        {
            string path = string.Empty;
            if (MsiQueryProductState("{B6DC31D8-A303-4D14-9C88-59F183F55BEC}") == 5)
            {
                //if (MsiQueryFeatureState("{E8085D3C-7185-4a58-A6DD-27C4507CF179}", "CLI") == 3)
                //{
                    path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft Visual Studio 10.0\\Common7\\IDE");
                    if (Directory.Exists(path))
                    {
                        path = Path.Combine(path, "witadmin.exe");
                    }
                    if (!File.Exists(path))
                    {
                        object obj2 = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\UserData\S-1-5-18\Components\493D0E7B3A56F23469F3DA82DF2DDA8E", "C3D5808E581785A46ADD724C05C71F97", string.Empty);
                        if (obj2 != null)
                        {
                            path = obj2.ToString();
                        }
                    }
                //}
                //else
                //{
                //    MessageBox.Show("The Team Foundation Server 2010 Power Tools have been located.  However, the Command-Line Interface feature has not been installed.\r\n\r\nPlease install the missing feature before running this utility.", "Command-Line Interface", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    base.Close();
                //}
            }
            if (File.Exists(path))
            {
                this.PowerToolsPath = path;
            }
            else
            {
                MessageBox.Show("The Team Foundation Server 2008 Power Tools could not be located.  Please install the Power Tools before running this utility.", "TFS Power Tools", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                base.Close();
            }
        }

        private void lvResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnDelete.Enabled = this.lvResults.SelectedItems.Count > 0;
        }

        [DllImport("msi.dll", CharSet = CharSet.Unicode)]
        private static extern int MsiGetProductInfo(string product, string property, [Out] StringBuilder valueBuf, ref int len);
        [DllImport("msi.dll")]
        public static extern int MsiQueryFeatureState(string szProduct, string szFeature);
        [DllImport("msi.dll")]
        public static extern int MsiQueryProductState(string szProduct);
        private void SearchBySavedQuery(string tfsServer, string teamProject, string queryName)
        {
            TfsTeamProjectCollection server = this.GetServer(tfsServer);
            if (server != null)
            {
                this.lvResults.BeginUpdate();
                this.lvResults.Items.Clear();
                WorkItemStore service = (WorkItemStore)server.GetService(typeof(WorkItemStore));
                string str = "";
                foreach (QueryFolder folder in service.Projects[teamProject].QueryHierarchy)
                {
                    foreach (QueryItem query in folder)
                    {
                        if (query is QueryDefinition)
                        {
                            if (query.Name.Equals(queryName, StringComparison.InvariantCultureIgnoreCase))
                            {
                                str = ((QueryDefinition)query).QueryText.Replace("@project", "'" + teamProject + "'");
                                break;
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(str))
                {
                    foreach (WorkItem item in service.Query(str))
                    {
                        this.lvResults.Items.Add(new ListViewItem(new string[] { item.Id.ToString(), item.Type.Name, item.Title, item.State }));
                    }
                }
                this.lvResults.EndUpdate();
            }
        }

        private void SearchByWorkItemId(string tfsServer, string teamProject, string[] ids)
        {
            TfsTeamProjectCollection server = this.GetServer(tfsServer);
            if (server != null)
            {
                this.lvResults.BeginUpdate();
                this.lvResults.Items.Clear();
                string str = string.Empty;
                for (int i = 0; i < ids.Length; i++)
                {
                    str = str + ids[i] + ",";
                }
                str = str.TrimEnd(new char[] { ',' });
                string wiql = string.Format("SELECT [System.Id], [System.WorkItemType], [System.Title], [System.State] FROM WorkItems WHERE [System.TeamProject] = '{0}'  AND  [System.Id] IN ({1}) ORDER BY [System.Id]", teamProject, str);
                WorkItemStore service = (WorkItemStore)server.GetService(typeof(WorkItemStore));
                foreach (WorkItem item in service.Query(wiql))
                {
                    this.lvResults.Items.Add(new ListViewItem(new string[] { item.Id.ToString(), item.Type.Name, item.Title, item.State }));
                }
                this.lvResults.EndUpdate();
            }
        }

        private void SearchWorkItems()
        {
            this.Cursor = Cursors.WaitCursor;
            if (this.optSavedQuery.Checked)
            {
                this.SearchBySavedQuery(this.cboTfsServer.Text, this.cboProject.Text, this.cboQueries.Text);
            }
            else if (this.optWorkItemIds.Checked)
            {
                this.SearchByWorkItemId(this.cboTfsServer.Text, this.cboProject.Text, this.txtIds.Text.Split(new char[] { ',' }));
            }
            this.btnDelete.Enabled = false;
            this.Cursor = Cursors.Default;
        }

        // Properties
        private string PowerToolsPath
        {
            
            get
            {
                return this._toolsPath;
            }
        
            set
            {
                this._toolsPath = value;
            }
        }



    }
}
