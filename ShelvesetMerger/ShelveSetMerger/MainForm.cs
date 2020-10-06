using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Proxy;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace ShelveSetMerger
{
    public partial class MainForm : Form
    {
        #region Fields
        private static TfsTeamProjectCollection m_tfs;
        private static IIdentityManagementService m_Ims;
        private static TeamFoundationIdentity m_group;
        private static TeamFoundationIdentity[] m_Identities;
        private static VersionControlServer m_Vcs;
        private static IList<WorkingFolder> m_Branches;
        private static string m_Commandline;
        private static bool m_BackgroundFillInProgress;
        #endregion

        #region properties

        private static TfsTeamProjectCollection Tfs
        {
            get
            {
                if (m_tfs == null)
                {
                    DomainProjectPicker picker = new DomainProjectPicker();
                    DialogResult result = picker.ShowDialog();
                    if (result == DialogResult.OK && picker.SelectedServer != null)
                    {
                        m_tfs = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(TfsTeamProjectCollection.GetFullyQualifiedUriForName(picker.SelectedServer.Name));
                    }
                    else
                    {
                        MessageBox.Show("Please select at least a server!");
                    }
                }

                return m_tfs;
            }
        }

        private static IIdentityManagementService Ims
        {
            get
            {
                if (m_Ims == null && Tfs != null)
                {
                    m_Ims = Tfs.GetService<IIdentityManagementService>();
                }

                return m_Ims;
            }
        }

        private static TeamFoundationIdentity Group
        {
            get
            {
                if (m_group == null && Ims != null)
                {
                    m_group = Ims.ReadIdentity(GroupWellKnownDescriptors.EveryoneGroup, MembershipQuery.Expanded, ReadIdentityOptions.None);
                }

                return m_group;
            }
        }

        private static TeamFoundationIdentity[] Identities
        {
            get
            {
                if (m_Identities == null && Ims != null && Group != null)
                {
                    m_Identities = Ims.ReadIdentities(Group.Members, MembershipQuery.Direct, ReadIdentityOptions.None);
                }

                return m_Identities;
            }
        }

        private static VersionControlServer Vcs
        {
            get
            {
                if (m_Vcs == null && Tfs != null)
                {
                    m_Vcs = Tfs.GetService<VersionControlServer>();
                }

                return m_Vcs;
            }
        }

        private static IList<WorkingFolder> Branches
        {
            get
            {
                if (m_Branches == null && Vcs !=null)
                {
                    IList<Workspace> workspaces = Vcs.QueryWorkspaces(null, Vcs.AuthorizedUser, Environment.MachineName);
                    m_Branches = new List<WorkingFolder>();
                    foreach (Workspace workspace in workspaces)
                    {
                        if (workspace != null)
                        {
                            foreach (WorkingFolder folder in workspace.Folders)
                            {
                                m_Branches.Add(folder);
                            }
                        }
                    }
                }

                return m_Branches;
            }
        }
        #endregion

        #region Constructor
        public MainForm()
        {
            InitializeComponent();
            GetCommandLine();            
        }
        #endregion

        #region Private methods
        private void FillUsersButton_Click(object sender, EventArgs e)
        {
            if (m_Identities == null && !m_BackgroundFillInProgress)
            {
                DialogResult result = MessageBox.Show("This can take a while.\nAre you sure?", "Are you sure?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    m_BackgroundFillInProgress = true;
                    m_FillUsersButton.Enabled = false;
                    StartReporting("Getting the list of TFS users. Please be patient!");
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += DoWork;
                    bw.RunWorkerCompleted += RunWorkerCompleted;
                    bw.RunWorkerAsync();
                }
            }
            else
            {
                RunWorkerCompleted(null, null);
            }
        }

        void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StartReporting("Feeding the list of TFS users in the combo box. Please be patient!");
            m_UserComboBox.DataSource = Identities;
            ResetReporting();
            m_UserComboBox.DisplayMember = "DisplayName";
            m_FillUsersButton.Enabled = true;
            m_BackgroundFillInProgress = false;
            ListShevesetsButton_Click(null, null);
        }

        void DoWork(object sender, DoWorkEventArgs e)
        {
            var a = Identities;
        }

        private void ListShevesetsButton_Click(object sender, EventArgs e)
        {
            string user;
            TeamFoundationIdentity identity = m_UserComboBox.SelectedItem as TeamFoundationIdentity;
            if (identity != null)
            {
                user = identity.UniqueName;
            }
            else
            {
                user = m_UserComboBox.Text;
            }

            m_ShelveComboBox.DataSource = GetShelvesetsForUser(user);
            m_ShelveComboBox.DisplayMember = "Name";
            if (m_ShelveComboBox.Items.Count > 0)
            {
                m_ShelveComboBox.SelectedItem = m_ShelveComboBox.Items[0];
            }
            else
            {
                m_ShelveComboBox.Text = string.Empty;
                m_SourceTextBox.Text = string.Empty;
            }
        }

        private static IList<Shelveset> GetShelvesetsForUser(string user)
        {
            IList<Shelveset> retval = new List<Shelveset>();
            if (!string.IsNullOrEmpty(user) && Vcs != null)
            {
                retval = Vcs.QueryShelvesets(null, user);
            }

            return retval;
        }

        private void ShelveComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Shelveset shelveset = m_ShelveComboBox.SelectedItem as Shelveset;
            if (shelveset != null && Vcs != null)
            {
                IList<PendingSet> pendingSet = Vcs.QueryShelvedChanges(shelveset);
                if (pendingSet.Count > 0 && pendingSet[0].PendingChanges[0] != null)
                {
                    string serverItem = pendingSet[0].PendingChanges[0].ServerItem;
                    m_SourceTextBox.Text = serverItem.Substring(0, serverItem.IndexOf("/src/"));
                }
            }

            GetCommandLine();
        }

        private void FillTargetBranches()
        {
            m_TagetBranchComboBox.DataSource = Branches;
            m_TagetBranchComboBox.DisplayMember = "ServerItem";
        }

        private void UserComboBox_TextChanged(object sender, EventArgs e)
        {
            GetCommandLine();
        }

        private void SourceTextBox_TextChanged(object sender, EventArgs e)
        {
            GetCommandLine();
        }

        private void TagetBranchComboBox_TextChanged(object sender, EventArgs e)
        {
            GetCommandLine();
        }

        private void GetCommandLine()
        {
            string commandline = string.Format("tfpt unshelve {0};{1} /migrate /source:{2} /target:{3}",
                string.IsNullOrEmpty(m_ShelveComboBox.Text) ? "empty" : GetShelveSetName(),
                string.IsNullOrEmpty(m_UserComboBox.Text) ? "empty" : GetUserName(),
                string.IsNullOrEmpty(m_SourceTextBox.Text) ? "empty" : m_SourceTextBox.Text,
                string.IsNullOrEmpty(m_TagetBranchComboBox.Text) ? "empty" : GetTargetBranchName());
            m_Commandline = commandline;
            m_CommandLineTextBox.Text = m_Commandline;
        }

        private string GetUserName()
        {
            string retVal;
            TeamFoundationIdentity identity = m_UserComboBox.SelectedItem as TeamFoundationIdentity;
            if (identity != null)
            {
                retVal = identity.UniqueName;
            }
            else
            {
                retVal = m_UserComboBox.Text;
            }

            return retVal;
        }

        private string GetShelveSetName()
        {
            string retVal;
            Shelveset shelveset = m_ShelveComboBox.SelectedItem as Shelveset;
            if (shelveset != null)
            {
                retVal = string.Format("\"{0}\"", shelveset.Name);
            }
            else
            {
                retVal = string.Format("\"{0}\"", m_ShelveComboBox.Text);
            }

            return retVal;
        }

        private string GetTargetBranchName()
        {
            string retVal;
            WorkingFolder folder = m_TagetBranchComboBox.SelectedItem as WorkingFolder;
            if (folder != null)
            {
                retVal = folder.ServerItem;
            }
            else
            {
                retVal =  m_ShelveComboBox.Text;
            }

            return retVal;
        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            if (CheckParameters())
            {
                string filename = CreateTempCommandFile();
                ExecuteCommandFile(filename);
                DeleteTempFile(filename);
            }
        }

        private void DeleteTempFile(string filename)
        {
            System.IO.File.Delete(filename);
        }

        private string CreateTempCommandFile()
        {
            string filename = System.IO.Path.GetTempFileName();
            System.IO.File.Move(filename, filename.Replace(".tmp", ".cmd"));
            filename = filename.Replace(".tmp", ".cmd");
            IList<string> lines = new List<string>();
            lines.Add("@echo off");
            lines.Add("call \"%VS100COMNTOOLS%..\\..\\VC\\vcvarsall.bat\"");
            lines.Add(string.Format("cd /d {0}", (m_TagetBranchComboBox.SelectedItem as WorkingFolder).LocalItem));
            lines.Add(m_CommandLineTextBox.Text);
            lines.Add("pause");
            System.IO.File.WriteAllLines(filename, lines.ToArray<string>());
            return filename;
        }

        private void ExecuteCommandFile(string path)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.FileName = path;
            p.Start();
            p.WaitForExit();
        }

        private bool CheckParameters()
        {
            bool condition = !string.IsNullOrEmpty(m_UserComboBox.Text) &&
                !string.IsNullOrEmpty(m_ShelveComboBox.Text) &&
                !string.IsNullOrEmpty(m_SourceTextBox.Text) &&
                m_TagetBranchComboBox.SelectedItem is WorkingFolder;

            if (!condition)
            {
                DialogResult result = MessageBox.Show("Some parameters are either empty or invalid. Do you want to execute the commandline?\nThe current commandline wont be validated.",
                    "Difference in parameters", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                condition = true;
            }
            else if (m_Commandline != m_CommandLineTextBox.Text)
            {
                DialogResult result = MessageBox.Show("The selected parameters, and the parameters in the commandline are not match.\nThe current commandline wont be validated.\n\nDo you want to continue?",
                    "Difference in parameters",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question);
                condition &= result == DialogResult.Yes;
            }

            return condition;
        }

        private void m_CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            StartReporting("Loading your mapped branches...");
            FillTargetBranches();
            ResetReporting();
        }

        private void StartReporting(string text)
        {
            m_ToolStripStatusLabel.Text = text;
            m_ToolStripProgressBar.Style = ProgressBarStyle.Marquee;
            m_ToolStripProgressBar.MarqueeAnimationSpeed = 30;
            Application.DoEvents();
        }

        private void ResetReporting()
        {
            m_ToolStripStatusLabel.Text = "Ready";
            m_ToolStripProgressBar.Style = ProgressBarStyle.Continuous;
            m_ToolStripProgressBar.MarqueeAnimationSpeed = 0;
            Application.DoEvents();
        }
        #endregion
    }
}
