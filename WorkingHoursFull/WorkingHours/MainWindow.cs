#region Usings
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Utils;
using System.Text.RegularExpressions;
#endregion

namespace WorkingHours
{
    public partial class MainWindow : Form
    {
        #region Nested class
        private class OwnUserState
        {
            public string Status { get; set; }
            public bool UpdateNeeded { get; set; }
        }
        #endregion

        #region Fields
        private string m_DecimalSeparator;
        private DataTable m_ResultDataTable;
        private DataTable m_SummaryDataTable;
        private IDictionary<string, int> m_SumDictionary;
        private BackgroundWorker m_BackgroundWorker;
        private BindingSource m_GridBindingSource;
        private int m_NumberOfWorkdays;
        private IList<DateTime> m_Holidays;
        private DateTime m_EasterDay = DateTime.MinValue;
        private IDictionary<string, IList<ColumnSettings>> m_session;
        private IList<string> m_regexPatternList;
        private int selRowRes = 0, selColRes = 0, selRowSum = 0, selColSum = 0;
        const string ITESTREGEX = @"itest\.?.?e(f+)ort?:.?(?<hours>(\d+[\.|\,]?)\d*)";
        const string DEVREGEX = @"dev\.?.?e(f+)ort?:.?(?<hours>(\d+[\.|\,]?)\d*)";

        #endregion

        #region Properties
        public IList<string> Users { get; set; }

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

        public BindingSource GridBindingSource
        {
            get
            {
                if (m_GridBindingSource == null)
                {
                    m_GridBindingSource = new BindingSource();
                }

                m_GridBindingSource.DataSource = ResultDataSet;
                return m_GridBindingSource;
            }
        }

        private DataSet ResultDataSet
        {
            get
            {
                if (dataSet_Results == null)
                {
                    dataSet_Results = new DataSet("Result");
                }

                return dataSet_Results;
            }
        }

        private DataTable SummaryDataTable
        {
            get
            {
                if (m_SummaryDataTable == null)
                {
                    CreateSummaryTable();
                }

                return m_SummaryDataTable;
            }
        }
        
        private DataTable ResultDataTable
        {
            get
            {
                if (m_ResultDataTable == null)
                {
                    CreateResultTable();
                }

                return m_ResultDataTable;
            }
        }

        private IList<DateTime> Holidays
        {
            get
            {
                if (m_Holidays == null)
                {
                    int year = DateTime.Now.Year;
                    m_Holidays = new List<DateTime>();
                    // 1956
                    m_Holidays.Add(new DateTime(year, 10, 23)); 
                    // Halloween
                    m_Holidays.Add(new DateTime(year, 11, 1));
                    // Christmas
                    m_Holidays.Add(new DateTime(year, 12, 25));
                    m_Holidays.Add(new DateTime(year, 12, 26));
                    // Newyears day
                    // Checking from December forward
                    m_Holidays.Add(new DateTime(year + 1, 1, 1));
                    // Checking from January backward
                    m_Holidays.Add(new DateTime(year, 1, 1));
                    // 1848/49
                    m_Holidays.Add(new DateTime(year, 3, 15));
                    // Easter
                    m_Holidays.Add(EasterDay);
                    // The day of labour
                    m_Holidays.Add(new DateTime(year, 5, 1));
                    // Whitsuntide (monday)
                    // 7 Weeks after Easter
                    m_Holidays.Add(EasterDay.AddDays(7 * 7));
                    // St. Stephan's day
                    m_Holidays.Add(new DateTime(year, 8, 20));
                }

                return m_Holidays;
            }
        }

        // Easter Monday!!!
        private DateTime EasterDay
        {
            get
            {
                if (m_EasterDay == DateTime.MinValue)
                {
                    // Gaussian methode. See Wikipedia
                    int year = DateTime.Now.Year;
                    int a = year % 19;
                    int b = year % 4;
                    int c = year % 7;
                    int d = (19 * a + 24) % 30;
                    int e = (2 * b + 4 * c + 6*d + 5) % 7;
                    int day = 22 + d + e;
                    int month = 3;
                    if (e == 6 && d == 29)
                    {
                        day = 50;
                    }

                    if (e == 6 && d == 28 && a > 10)
                    {
                        day = 49;
                    }

                    if (day > 31)
                    {
                        day -= 31;
                        month++;
                    }

                    m_EasterDay = new DateTime(year, month, day + 1);
                }

                return m_EasterDay;
            }
        }

        private int NumberOfWorkdays
        {
            get 
            {
                if (m_NumberOfWorkdays == 0)
                {
                    DateTime start = dateTimePicker_StartDate.Value.Date;
                    DateTime end = dateTimePicker_EndDate.Value.Date;
                    m_NumberOfWorkdays = CalculateNumberOfWorkdays(start, end);
                }

                return m_NumberOfWorkdays; 
            }
        }        
        #endregion
               
        #region Methods
        private int CalculateNumberOfWorkdays(DateTime firstDay, DateTime lastDay)
        {
            TimeSpan span = lastDay - firstDay;
            int workingDays = span.Days + 1;
            int fullWeekCount = workingDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (workingDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = firstDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)firstDay.DayOfWeek;
                int lastDayOfWeek = lastDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        workingDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        workingDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    workingDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            workingDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            foreach (DateTime holiday in Holidays)
            {
                DateTime bh = holiday.Date;
                if ((firstDay <= bh && bh <= lastDay) && !(bh.DayOfWeek == DayOfWeek.Sunday || bh.DayOfWeek == DayOfWeek.Saturday))
                {
                    --workingDays;
                }
            }

            return workingDays;
        }

        private void CreateSummaryTable()
        {
            if (ResultDataSet.Tables["Summary"] == null)
            {
                m_SummaryDataTable = ResultDataSet.Tables.Add("Summary");
                m_SummaryDataTable.Columns.Add("Name", typeof(string));
                m_SummaryDataTable.Columns.Add("Sum of completed work [h]", typeof(double));
                m_SummaryDataTable.Columns.Add("Defect analysis hours [h]", typeof(double));
                m_SummaryDataTable.Columns.Add("Open Issue analysis hours [h]", typeof(double));
                m_SummaryDataTable.Columns.Add("Percentage of RQ analysis [%]", typeof(double));
                m_SummaryDataTable.Columns.Add("Daily average [h]", typeof(double));
            }
        }

        private void CreateResultTable()
        {
            if (ResultDataSet.Tables["Result"] == null)
            {
                m_ResultDataTable = ResultDataSet.Tables.Add("Result");
                m_ResultDataTable.Columns.Add("Changed Date", typeof (DateTime));
                m_ResultDataTable.Columns.Add("Changed By");
                m_ResultDataTable.Columns.Add("ID", typeof(int));
                m_ResultDataTable.Columns.Add("Title");
                m_ResultDataTable.Columns.Add("Workitem Type");
                m_ResultDataTable.Columns.Add("Iteration Path");
                m_ResultDataTable.Columns.Add("Stack Rank");
                m_ResultDataTable.Columns.Add("Original Completed Work");
                m_ResultDataTable.Columns.Add("Completed Work", typeof (double));
                m_ResultDataTable.Columns.Add("Original Remaining Work");
                m_ResultDataTable.Columns.Add("Remaining Work");
                m_ResultDataTable.Columns.Add("Effort on Defect");
                m_ResultDataTable.Columns.Add("Effort on OpenIssue");
                m_ResultDataTable.Columns.Add("Delta Completed Work", typeof (double));
                m_ResultDataTable.Columns.Add("Delta Remaining Work");
                m_ResultDataTable.Columns.Add("Sum of delta", typeof(double));
            }
        }

        private void ClearTableSet()
        {
            m_SummaryDataTable.Clear();
            m_ResultDataTable.Clear();
            m_SumDictionary = new Dictionary<string, int>();
        }

        private delegate void EnableStartButtonDelegate();
        private void EnableStartButton()
        {
            if (button_Start.InvokeRequired)
            {
                EnableStartButtonDelegate enableStartButtonDelegate = new EnableStartButtonDelegate(EnableStartButton);
                button_Start.Invoke(enableStartButtonDelegate);
            }
            else
            {
                button_Start.Enabled = true;
            }
        }

        private void DoDataBind(string tableName)
        {
            GridBindingSource.ResetBindings(false);
            GridBindingSource.DataMember = tableName;
            dataGridView_Result.DataSource = null;
            dataGridView_Result.DataSource = GridBindingSource;
            dataGridView_Result.Update();
        }

        private WorkItemCollection GetWorkItemCollection()
        {
            // It is needed, to get all the WIs from the start date because it is possible, that somebody made a change out of the selected time period for the WI.
            string wiql = "SELECT * FROM WorkItems WHERE [System.TeamProject] ='TIA' AND " + 
                          "(([System.WorkItemType] = 'Task'  AND" + "[System.AreaPath] UNDER 'TIA\\Development\\HMI Runtime Innovation Line' ) OR "+ 
                          "([System.WorkItemType] = 'Request' AND [Siemens.Industry.Common.Component] CONTAINS 'PLF')) AND " +
                          "[System.ChangedDate] >= '" + dateTimePicker_StartDate.Value.Date.ToString() + "'";
            OwnUserState userState = new OwnUserState() { Status = "Getting WorkItems..." };
            m_BackgroundWorker.ReportProgress(-1, userState);
            WorkItemCollection workItemCollection = Utils.TfsUtils.QueryWorkItems(wiql);
            return workItemCollection;
        }

        private void ParseTFSHistory(WorkItemCollection workItemCollection)
        {
            if (workItemCollection != null)
            { 
                DataRow dr;
                WorkItem workItem;
                OwnUserState userState = new OwnUserState();
                double savedCompletedWorkOriginalValue, completedDelta, remainingDelta, rqEffort = 0;
                Field savedRemainingWorkField, remainingWorkField, completedWorkField;
                DateTime changeDate, startDate = dateTimePicker_StartDate.Value.Date, endDate = dateTimePicker_EndDate.Value.Date.AddHours(23.9999);
                bool hackInProgress, isRequest, isOpenIssue = false, rqEffortFound = false;
                int foundItems = 0, lastrow = 0, workItemCount = workItemCollection.Count;
                string changedBy, history;
                // Completed(Original)Value, Remaining...
                object COV, CV, ROV, RV;

                userState.Status = "Starting to parse...";
                m_BackgroundWorker.ReportProgress(0, userState);

                for (int i = 0; i < workItemCount; i++)
                {
                    if (m_BackgroundWorker.CancellationPending)
                    {
                        i = workItemCount;
                        continue;
                    }

                    workItem = workItemCollection[i];
                    isRequest = workItem.Type.Name.ToUpperInvariant().Equals("REQUEST");
                    isOpenIssue = false;
                    userState.Status = string.Format("Checking {0}. Found: {1}", workItem.Id, foundItems);
                    m_BackgroundWorker.ReportProgress((i * 100) / workItemCount, userState);
                    savedCompletedWorkOriginalValue = 0;
                    savedRemainingWorkField = null;
                    remainingDelta = 0;                    
                    dr = null;
                    hackInProgress = false;
                    
                    foreach (Revision workItemRevision in workItem.Revisions)
                    {
                        changeDate = (DateTime)workItemRevision.Fields["Changed Date"].Value;
                        changedBy = workItemRevision.Fields["Changed By"].Value.ToString();

                        if (Users.Contains(changedBy) && changeDate >= startDate && changeDate <= endDate)
                        {
                            rqEffort = 0;
                            rqEffortFound = false; 
							history = workItemRevision.Fields["History"].Value.ToString().ToLowerInvariant();
							
                            if (isRequest)
                            {
                                isOpenIssue = workItem.Fields["Type Of Request"].Value.ToString().ToUpperInvariant() == ("OPEN ISSUE");
                                foreach (string regexPattern in m_regexPatternList)
                                {
                                    Regex regex = new Regex(regexPattern);
                                    var match = regex.Match(history);
                                    string hours = match.Groups["hours"].Value;
                                    if (!string.IsNullOrEmpty(hours))
                                    {
                                        if (rqEffort != 0)
                                        {
                                            MessageBox.Show(string.Format("An RQ effort: {0} in Work Item {1}, in revision {2} is already parsed either for ITEST or DEV.\nDuplicate entry in one revision?", rqEffort, workItem.Id, workItemRevision.Index),
                                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }

                                        rqEffortFound = true;
                                        hours = hours.Replace(".", DecimalSeparator);
                                        hours = hours.Replace(",", DecimalSeparator);
                                        if (!double.TryParse(hours, out rqEffort))
                                        {
                                            MessageBox.Show(string.Format("Unable to parse RQ effort: {0} in Work Item {1}, in revision {2}", hours, workItem.Id, workItemRevision.Index),
                                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                            }

                            remainingWorkField = workItemRevision.Fields["Remaining Work"];
                            completedWorkField = workItemRevision.Fields["Completed Work"];
                            COV = completedWorkField.OriginalValue;
                            CV = completedWorkField.Value;
                            ROV = remainingWorkField.OriginalValue;
                            RV = remainingWorkField.Value;
                            completedDelta = 0;

                            if (((COV != null || hackInProgress || isRequest) &&
                                CV != COV) || rqEffortFound)
                            {
                                if (!rqEffortFound && CV != null)
                                {
                                    if (COV != null)
                                    {
                                        completedDelta = (double)CV - (double)COV;
                                    }
                                    else if (hackInProgress)
                                    {
                                        completedDelta = (double)CV - savedCompletedWorkOriginalValue;
                                        remainingWorkField = savedRemainingWorkField;
                                        savedCompletedWorkOriginalValue = 0;
                                        savedRemainingWorkField = null;
                                        hackInProgress = false;
                                    }
                                    else if (isRequest)
                                    {
                                        completedDelta = (double)CV;
                                    }
                                }
                                else if (!rqEffortFound)
                                {
                                    // Hack: the user changed the the origial meaningful value to 'null'. Probably the user cleared the field accidentally, and in a next revision it is fixed.
                                    if (workItemRevision.Index == workItem.Revisions.Count - 1)
                                    {
                                        // This is the last revision: not yet fixed. 
                                        // Add error line.
                                        dr = AddResultTableRow(workItem, workItemRevision, changedBy, remainingWorkField, completedWorkField, null, null, null, "completedWorkField.Value == null and this is the last revision", false);
                                        // Skip this.
                                        continue;
                                    }
                                    else
                                    {
                                        hackInProgress = true;
                                        savedCompletedWorkOriginalValue = (double)COV;
                                        savedRemainingWorkField = remainingWorkField;
                                        continue;
                                    }
                                }
                                else if (rqEffortFound)
                                {
                                    completedDelta = rqEffort;
                                }

                                foundItems++;
                                userState.UpdateNeeded = true;

                                if (!isRequest && !rqEffortFound)
                                {
                                    if (ROV != null && RV != null)
                                    {
                                        remainingDelta = (double)RV - (double)ROV;
                                    }
                                    else if (RV != null)
                                    {
                                        remainingDelta = (double)RV;
                                    }
                                    else if (ROV != null)
                                    {
                                        remainingDelta = (double)ROV;
                                    }
                                    else
                                    {
                                        remainingDelta = 0;
                                    }
                                }

                                dr = AddResultTableRow(workItem, workItemRevision, changedBy, remainingWorkField, completedWorkField, completedDelta, remainingDelta, rqEffort, string.Empty, isOpenIssue);
                                lastrow = UpdateSumTable(lastrow, changedBy, completedDelta, rqEffort, isOpenIssue);
                            }
                        }
                    }
                    if (hackInProgress)
                    {
                        // Hack not finished. Add error line.
                        dr = AddResultTableRow(workItem, null, null, null, null, null, null, null, string.Format("The CompletedWork Value was nulled, although there was a CompletedWork OriginalValue with {0} ", savedCompletedWorkOriginalValue), false);
                        hackInProgress = false;
                    }
                }
            }
        }

        private delegate int UpdateSumTableDelegate(int lastrow, string changedBy, double completedDelta, double rqEffort, bool isOpenIssue);
        private int UpdateSumTable(int lastrow, string changedBy, double completedDelta, double rqEffort, bool isOpenIssue)
        {
            if (dataGridView_Result.InvokeRequired)
            {
                UpdateSumTableDelegate ustd = new UpdateSumTableDelegate(UpdateSumTable);
                lastrow = (int)dataGridView_Result.Invoke(ustd, new object[] { lastrow, changedBy, completedDelta, rqEffort, isOpenIssue });
            }
            else
            {
                if (!m_SumDictionary.ContainsKey(changedBy))
                {
                    m_SumDictionary.Add(changedBy, lastrow);
                    SummaryDataTable.Rows.Add(changedBy, completedDelta, isOpenIssue ? 0 : rqEffort, isOpenIssue ? rqEffort : 0, Math.Round(rqEffort / completedDelta * 100, 2), Math.Round(completedDelta / NumberOfWorkdays, 2));
                    lastrow++;
                }
                else
                {
                    DataRow row = SummaryDataTable.Rows[m_SumDictionary[changedBy]];
                    double completedDeltaValue = double.TryParse(row.ItemArray[1].ToString(), out completedDeltaValue) ? completedDeltaValue + completedDelta : completedDelta;
                    double rqEffortValue = double.TryParse(row.ItemArray[2].ToString(), out rqEffortValue) && !isOpenIssue ? rqEffortValue + rqEffort : rqEffortValue;
                    double rqEffortValueOnOpenIssue = double.TryParse(row.ItemArray[3].ToString(), out rqEffortValueOnOpenIssue) && isOpenIssue ? rqEffortValueOnOpenIssue + rqEffort : rqEffortValueOnOpenIssue;
                    row.ItemArray = new object[] { changedBy, completedDeltaValue, rqEffortValue, rqEffortValueOnOpenIssue, Math.Round((rqEffortValue + rqEffortValueOnOpenIssue) / completedDeltaValue * 100, 2), Math.Round(completedDeltaValue / NumberOfWorkdays, 2) };
                }
            }

            return lastrow;
        }

        private DataRow AddResultTableRow(WorkItem workItem, Revision workItemRevisions, string changedBy, Field remainingWorkField, Field completedWorkField, double? completedDelta, double? remainingDelta, double? rqEffort, string error, bool isOpenIssue)
        {
            DataRow dr = ResultDataTable.NewRow();
            dr.ItemArray = new object[] {
                                workItemRevisions == null ? workItem.RevisedDate : workItemRevisions.Fields["Changed Date"].Value,
                                string.IsNullOrEmpty(changedBy) ? workItem.Fields["System.AssignedTo"].Value : changedBy,
                                workItemRevisions == null ? workItem.Id : workItemRevisions.Fields["ID"].Value,
                                workItemRevisions == null ? workItem.Title : workItemRevisions.Fields["Title"].Value,
                                workItem.Type.Name,
                                workItemRevisions == null ? workItem.IterationPath : workItemRevisions.Fields["Iteration Path"].Value,
                                workItemRevisions == null ? string.Empty : workItemRevisions.Fields["Stack Rank"].Value ?? string.Empty,
                                completedWorkField == null ? 0d : completedWorkField.OriginalValue ?? 0d,
                                completedWorkField == null ? 0d : completedWorkField.Value,
                                remainingWorkField == null ? 0d : remainingWorkField.OriginalValue ?? 0d,
                                remainingWorkField == null ? 0d : remainingWorkField.Value ?? 0d,
                                rqEffort == null || isOpenIssue ? 0d : rqEffort.Value,
                                !isOpenIssue || rqEffort == null ? 0d : rqEffort.Value,
                                completedDelta == null ? 0d : completedDelta.Value,
                                remainingDelta == null ? 0d : remainingDelta.Value,
                                (remainingDelta == null || completedDelta == null) ? 0d : (completedDelta.Value+remainingDelta.Value)
                                };
            dr.RowError = error;
            AddRowToTable(dr);
            return dr;
        }

        private delegate void AddRowToTableDelegate(DataRow dr);
        private void AddRowToTable(DataRow dr)
        {
            if (dataGridView_Result.InvokeRequired)
            {
                AddRowToTableDelegate arttd = new AddRowToTableDelegate(AddRowToTable);
                dataGridView_Result.Invoke(arttd, new object[] { dr });
            }
            else
            {
                ResultDataTable.Rows.Add(dr);
            }
        }

        private void CreateCSVFile(DataTable dt, string strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath, false);
            int iColCount = dt.Columns.Count;
            for (int i = 0; i < iColCount; i++)
            {
                sw.Write(dt.Columns[i]);
                if (i < iColCount - 1)
                {
                    sw.Write(";");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        sw.Write(dr[i].ToString());
                    }
                    if (i < iColCount - 1)
                    {
                        sw.Write(";");
                    }
                }

                sw.Write(sw.NewLine);
            }

            sw.Close();
        }

        private void InitializeUsersList()
        {
            Users = new List<string>();
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Users))
            {
                string[] loadedUserList = Properties.Settings.Default.Users.Split(new char[] { ';' });
                (Users as List<string>).AddRange(loadedUserList);
            }
        }

        private void SaveUserList()
        {
            StringBuilder saveUserList = new StringBuilder();
            for (int i = 0; i < Users.Count; i++)
            {
                string user = Users[i];
                saveUserList.Append(user);
                if (i != Users.Count - 1)
                {
                    saveUserList.Append(";");
                }
            }

            Properties.Settings.Default.Users = saveUserList.ToString();
            Properties.Settings.Default.Save();
        }

        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();            
            InitializeUsersList();
            CreateResultTable();
            CreateSummaryTable();
            DoDataBind("Result");
            InitializeFields();
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            this.Load += MainWindow_Load;
            this.FormClosing += MainWindow_FormClosing;
        }

        private void InitializeFields()
        {
            m_SumDictionary = new Dictionary<string, int>();
            dateTimePicker_StartDate.Value = DateTime.Now.AddDays(-7);
            dataGridView_Result.DoubleBuffered(true);
            m_session = new Dictionary<string, IList<ColumnSettings>>();
            m_regexPatternList = new List<string>();
            m_regexPatternList.Add(ITESTREGEX);
            m_regexPatternList.Add(DEVREGEX);
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            RestoreUserSettings();
        }

        private void RestoreUserSettings()
        {
            m_session = GridViewSessionSettings.Default.SessionSettings;
            this.Size = GridViewSessionSettings.Default.WindowSize;
            this.Location = GridViewSessionSettings.Default.WindowLocation;
            if (this.Location.X < 0 || this.Location.Y < 0) this.Location = new System.Drawing.Point(0, 0);
            this.WindowState = GridViewSessionSettings.Default.WindowState;
            RestoreSessionSettings();
        }

        void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveUserSettings();
        }

        private void SaveUserSettings()
        {
            SaveSessionSettings();
            GridViewSessionSettings.Default.SessionSettings = m_session;
            GridViewSessionSettings.Default.WindowSize = this.Size;
            GridViewSessionSettings.Default.WindowLocation = this.Location;
            if (this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;
            GridViewSessionSettings.Default.WindowState = this.WindowState;
            GridViewSessionSettings.Default.Save();
        }

        private void ClearUserSettings()
        {
            DoDataBind(GridBindingSource.DataMember);
            m_session = new Dictionary<string, IList<ColumnSettings>>();            
            this.Size = this.MinimumSize;
            this.Location = new System.Drawing.Point(0, 0);
            this.WindowState = FormWindowState.Normal;
            selRowRes = 0;
            selColRes = 0;
            selRowSum = 0;
            selColSum = 0;
            SaveSessionSettings();
        }

        private void SaveSessionSettings()
        {
            string currentView = GridBindingSource.DataMember.ToString();
            IList<ColumnSettings> sessionSettings = new List<ColumnSettings>();            
            foreach (DataGridViewColumn DGVcolumn in dataGridView_Result.Columns)
            {
                ColumnSettings cs = new ColumnSettings()
                {
                    ColumnIndex = DGVcolumn.Index,
                    DisplayIndex = DGVcolumn.DisplayIndex,
                    Visible = DGVcolumn.Visible,
                    Width = DGVcolumn.Width,
                    SortOrder = dataGridView_Result.SortOrder,
                    SortedByThisColumn = dataGridView_Result.SortedColumn == DGVcolumn
                };
                sessionSettings.Add(cs);
            }
            m_session[currentView] = sessionSettings;

            if (dataGridView_Result.SelectedCells.Count > 0)
            {                
                int selCol = dataGridView_Result.SelectedCells.Count > 0 ? dataGridView_Result.SelectedCells[0].ColumnIndex : 0;
                int selRow = dataGridView_Result.SelectedCells.Count > 0 ? dataGridView_Result.SelectedCells[0].RowIndex : 0;
                switch (currentView)
                {
                    case "Result":
                        selColRes = selCol;
                        selRowRes = selRow;
                        break;
                    case "Summary":
                        selColSum = selCol;
                        selRowSum = selRow;
                        break;
                }
            }
            else
            {

            }
        }

        private void RestoreSessionSettings()
        {
            string currentView = GridBindingSource.DataMember.ToString();
            if (m_session.ContainsKey(currentView))
            {
                IList<ColumnSettings> sessionSettings = m_session[currentView];
                foreach (ColumnSettings cs in sessionSettings)
                {
                    if (dataGridView_Result.Columns.Count >= cs.ColumnIndex)
                    {
                        DataGridViewColumn DGVcolumn = dataGridView_Result.Columns[cs.ColumnIndex];
                        DGVcolumn.DisplayIndex = cs.DisplayIndex;
                        DGVcolumn.Visible = cs.Visible;
                        DGVcolumn.Width = cs.Width;
                        if (cs.SortedByThisColumn)
                        {
                            ListSortDirection listSort;
                            switch (cs.SortOrder)
                            {
                                case SortOrder.Ascending:
                                    listSort = ListSortDirection.Ascending;
                                    break;

                                case SortOrder.Descending:
                                    listSort = ListSortDirection.Descending;
                                    break;

                                default:
                                    listSort = ListSortDirection.Descending;
                                    break;
                            }

                            dataGridView_Result.Sort(dataGridView_Result.Columns[cs.ColumnIndex], listSort);
                        }
                    }
                }
            }

            int selCol = 0, selRow = 0;
            dataGridView_Result.ClearSelection();
            switch (currentView)
            {
                case "Result":
                    selCol = selColRes;
                    selRow = selRowRes;
                    break;
                case "Summary":
                    selCol = selColSum;
                    selRow = selRowSum;
                    break;
            }

            if (dataGridView_Result.Columns.Count >= selCol+1 && dataGridView_Result.RowCount>= selRow+1)
            {
                dataGridView_Result.Rows[selRow].Cells[selCol].Selected = true;
            }
        }
        
        #endregion

        #region Enventhandlers
        private void button_SelectUsers_Click(object sender, EventArgs e)
        {
            using (EditUsersDialog selectUserDialog = new EditUsersDialog(Users))
            {
                DialogResult dr = selectUserDialog.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    Users = selectUserDialog.Users;
                    SaveUserList();
                }
            }
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            if (button_Start.Text == "Start")
            {
                TaskbarProgress.SetState(this.Handle, TaskbarProgress.TaskbarStates.NoProgress);
                labelInfo.BackColor = System.Drawing.SystemColors.Control;
                labelInfo.ForeColor = System.Drawing.SystemColors.ControlText;
                dateTimePicker_EndDate.Enabled = false;
                dateTimePicker_StartDate.Enabled = false;
                ClearTableSet();
                dataGridView_Result.Focus();
                m_BackgroundWorker = new BackgroundWorker();
                m_BackgroundWorker.WorkerSupportsCancellation = true;
                m_BackgroundWorker.WorkerReportsProgress = true;
                m_BackgroundWorker.DoWork += backgroundWorker_DoWork;
                m_BackgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
                m_BackgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
                m_BackgroundWorker.RunWorkerAsync();
                button_Start.Enabled = false;
            }
            else
            {
                m_BackgroundWorker.CancelAsync();
            }

            button_Start.Text = button_Start.Text == "Start" ? "Stop" : "Start";
        }        

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            WorkItemCollection workItemCollection = GetWorkItemCollection();
            EnableStartButton();
            ParseTFSHistory(workItemCollection);
        }
        
        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {            
            if (e.ProgressPercentage < 0)
            {
                toolStripProgressBar_Info.Style = ProgressBarStyle.Marquee;
                TaskbarProgress.SetState(this.Handle, TaskbarProgress.TaskbarStates.Indeterminate);
            }
            else
            {
                toolStripProgressBar_Info.Style = ProgressBarStyle.Blocks;
                toolStripProgressBar_Info.Value = e.ProgressPercentage;
                TaskbarProgress.SetValue(this.Handle, e.ProgressPercentage, 100);
            }

            OwnUserState userState = e.UserState as OwnUserState;
            labelInfo.Text = userState.Status;
            if (userState.UpdateNeeded == true)
            {
                dataGridView_Result.Refresh();
                userState.UpdateNeeded = false;
            }
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                labelInfo.BackColor = System.Drawing.Color.Green;
                labelInfo.ForeColor = System.Drawing.Color.White;
                labelInfo.Text = "Ready";
                TaskbarProgress.SetState(this.Handle, TaskbarProgress.TaskbarStates.NoProgress);
            }
            else
            {
                labelInfo.BackColor = System.Drawing.Color.Red;
                labelInfo.ForeColor = System.Drawing.Color.White;
                labelInfo.Text += string.Format(" Exception: {0}. Operation incomplete.", e.Error.Message);
                TaskbarProgress.SetState(this.Handle, TaskbarProgress.TaskbarStates.Error);
            }
            toolStripProgressBar_Info.Value = 0;
            button_Start.Text = "Start";
            dataGridView_Result.Refresh();
            m_NumberOfWorkdays = 0;
            dateTimePicker_EndDate.Enabled = true;
            dateTimePicker_StartDate.Enabled = true;
        }

        private void buttonToggle_Click(object sender, EventArgs e)
        {
            string tableName = GridBindingSource.DataMember == "Result" ? "Summary" : "Result";
            buttonToggle.Text = buttonToggle.Text == "<- Details" ? "Summary ->" : "<- Details";
            SaveSessionSettings();
            DoDataBind(tableName);
            RestoreSessionSettings();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            DataTable dt = ResultDataSet.Tables[GridBindingSource.DataMember];
            if (dt != null)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Title = "Please select a file! The file will be overwritten!";
                    saveFileDialog.ValidateNames = true;
                    DialogResult dr = saveFileDialog.ShowDialog();
                    if (dr == System.Windows.Forms.DialogResult.OK)
                    {
                        CreateCSVFile(dt, saveFileDialog.FileName);
                    }
                }
            }
        }

        private void dataGridView_Result_SelectionChanged(object sender, EventArgs e)
        {
            double sum = 0;
            double parsed = 0;
            foreach (DataGridViewCell item in dataGridView_Result.SelectedCells)
            {
                if (double.TryParse(item.Value.ToString(), out parsed))
                {
                    sum += parsed;
                }
            }

            toolStripLabel_Sum.Text = string.Format("Sum: {0}", sum);
        }

        private void button_ClearSetting_Click(object sender, EventArgs e)
        {
            ClearUserSettings();
        }
     
        #endregion
    }
    
    [Serializable]
    public sealed class ColumnSettings
    {
        public int DisplayIndex { get; set; }
        public int Width { get; set; }
        public bool Visible { get; set; }
        public int ColumnIndex { get; set; }
        public SortOrder SortOrder {get; set;}
        public bool SortedByThisColumn { get; set; }
    }
}