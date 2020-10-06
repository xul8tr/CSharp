#region Usings
using System;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
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
        private static IIdentityManagementService m_Ims;
        private static TeamFoundationIdentity m_group;
        private static WorkItemStore m_WorkItemStore;
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
        #endregion

        #region Public methods
        public static TeamFoundationIdentity GetIdentity(string name)
        {
            TeamFoundationIdentity identity = null;
            if (Ims != null && Group != null)
            {
                identity = Ims.ReadIdentity(IdentitySearchFactor.DisplayName, name, MembershipQuery.None, ReadIdentityOptions.None);
            }

            return identity;
        }

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
            WorkItemCollection witCollection = null;
            if (WorkItemStore != null)
            {
                witCollection = WorkItemStore.Query(wiqlQuery);
            }

            return witCollection;
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
