#region Usings
using System;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using WorkingHours.Properties;
#endregion

namespace Utils
{
    internal class TfsUtils
    {
        #region Fields
        private static TfsTeamProjectCollection m_tfs;
        private static WorkItemStore m_WorkItemStore;
        private static VersionControlServer m_Vcs;
        private static string m_AuthorizedUser;
        #endregion

        #region Properties
        private static TfsTeamProjectCollection Tfs
        {
            get
            {
                if (m_tfs == null)
                {
                    string tfsUrl = Settings.Default.TfsUrl;
                    if (!string.IsNullOrEmpty(tfsUrl))
                    {
                        try
                        {
                            m_tfs = new TfsTeamProjectCollection(new Uri(tfsUrl));
                            if (m_tfs.ConnectivityFailureOnLastWebServiceCall)
                            {
                                // No connection available. Maybe URL is wrong. Try to get a new from TeamProjectPicker
                                m_tfs.Dispose();
                                m_tfs = null;
                            }
                        }
                        catch (UriFormatException)
                        {
                            Settings.Default.TfsUrl = string.Empty;
                            Settings.Default.Save();
                        }
                    }

                    if (m_tfs == null)
                    {
                        TeamProjectPicker picker = new TeamProjectPicker();
                        DialogResult result = picker.ShowDialog();
                        if (result == DialogResult.OK && picker.SelectedTeamProjectCollection != null)
                        {
                            m_tfs = picker.SelectedTeamProjectCollection;
                            Settings.Default.TfsUrl = m_tfs.Uri.ToString();
                            Settings.Default.Save();
                        }
                        else
                        {
                            MessageBox.Show("Please select at least a server!");
                        }
                    }
                }

                return m_tfs;
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

        public static string AuthorizedUser
        {
            get
            {
                if (m_AuthorizedUser == null && Vcs != null)
                {
                    Tfs.EnsureAuthenticated();
                    TeamFoundationIdentity tfi = Vcs.AuthorizedIdentity;
                    m_AuthorizedUser = tfi.DisplayName;
                }

                return m_AuthorizedUser;
            }
        }
        #endregion

        #region Public methods
        public static WorkItemStore WorkItemStore
        {
            get
            {
                if (Tfs != null)
                {
                    m_WorkItemStore = Tfs.GetService<WorkItemStore>();
                }

                return m_WorkItemStore;
            }
        }

        public static WorkItemCollection QueryWorkItems(string wiqlQuery)
        {
            WorkItemCollection wiCollection = null;
            if (WorkItemStore != null)
            {
                wiCollection = WorkItemStore.Query(wiqlQuery);
            }

            return wiCollection;
        }
        #endregion
    }

    internal static class Extensions
    {
        #region Public methods
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
        #endregion
    }
}
