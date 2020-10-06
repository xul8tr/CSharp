using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkingHours
{
    class WorkerImplementation
    {
        #region Fields
        private string m_DecimalSeparator;
        #endregion

        #region Properties
        private MainWindow Parent { get; set; }
        private BackgroundWorker BackgroundWorker { get; set; }

        public string DecimalSeparator
        {
            get
            {
                if (string.IsNullOrEmpty(m_DecimalSeparator))
                {
                    m_DecimalSeparator = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
                }

                return m_DecimalSeparator;
            }
        }
        #endregion

        #region Constructor
        public WorkerImplementation(MainWindow mainWindow, BackgroundWorker backgroundWorker)
        {
            Parent = mainWindow;
            BackgroundWorker = backgroundWorker;
        }
        #endregion

        #region Internal methods
        internal void DoWork(DateTime startDate, DateTime endDate)
        {
            WorkItemCollection workItemCollection = GetWorkItemCollection(startDate.Date.ToString());
            Parent.EnableStartButton();
            ParseTFSHistory(workItemCollection, startDate, endDate);
        }

        internal WorkItemCollection GetWorkItemCollection(string startDate)
        {
            // It is needed, to get all the WIs from the start date because it is possible, that somebody made a change out of the selected time period for the WI.
            string wiql = "SELECT * FROM WorkItems WHERE [System.TeamProject] ='TIA' AND " +
                          "([System.WorkItemType] = 'Request' AND [Siemens.Industry.Common.Component] CONTAINS 'PLF') AND " +
                          "[System.ChangedDate] >= '" + startDate + "'";
            OwnUserState userState = new OwnUserState() { Status = "Getting WorkItems..." };
            BackgroundWorker.ReportProgress(-1, userState);
            WorkItemCollection workItemCollection = Utils.TfsUtils.QueryWorkItems(wiql);
            return workItemCollection;
        }
        #endregion

        #region Methods
        // TODO: This should be done in some nicer way :S LINQ maybe?
        private void ParseTFSHistory(WorkItemCollection workItemCollection, DateTime startDate, DateTime endDate)
        {
            if (workItemCollection != null)
            {
                WorkItem workItem;
                OwnUserState userState = new OwnUserState();
                double itestEffort = 0;
                DateTime changeDate;
                endDate = endDate.AddHours(23.9999);
                bool isOpenIssue = false;
                int foundItems = 0, workItemCount = workItemCollection.Count;
                string changedBy, history;
                userState.Status = "Starting to parse...";
                BackgroundWorker.ReportProgress(0, userState);

                for (int i = 0; i < workItemCount; i++)
                {
                    if (BackgroundWorker.CancellationPending)
                    {
                        i = workItemCount;
                        continue;
                    }

                    workItem = workItemCollection[i];
                    isOpenIssue = false;
                    userState.Status = string.Format("Checking {0}. Found: {1}", workItem.Id, foundItems);
                    BackgroundWorker.ReportProgress((i * 100) / workItemCount, userState);

                    foreach (Revision workItemRevision in workItem.Revisions)
                    {
                        changeDate = (DateTime)workItemRevision.Fields["Changed Date"].Value;
                        changedBy = workItemRevision.Fields["Changed By"].Value.ToString();

                        if (Parent.Users.Contains(changedBy) && changeDate >= startDate && changeDate <= endDate)
                        {
                            itestEffort = 0;
                            history = workItemRevision.Fields["History"].Value.ToString().ToLowerInvariant();

                            isOpenIssue = workItem.Fields["Type Of Request"].Value.ToString().ToUpperInvariant() == ("OPEN ISSUE");
                            Regex regex = new Regex(@"itest.e(f+)ort?:.?(?<hours>(\d+[\.|\,]?)\d*)");
                            var match = regex.Match(history);
                            string hours = match.Groups["hours"].Value;
                            if (!string.IsNullOrEmpty(hours))
                            {
                                hours = hours.Replace(".", DecimalSeparator);
                                hours = hours.Replace(",", DecimalSeparator);
                                if (!double.TryParse(hours, out itestEffort))
                                {
                                    MessageBox.Show(string.Format("Unable to parse ITEST effort: {0} in Work Item {1}, in revision {2}", hours, workItem.Id, workItemRevision.Index),
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    foundItems++;
                                    userState.UpdateNeeded = true;
                                }

                                Parent.UpdateSumValue(itestEffort, isOpenIssue);
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
