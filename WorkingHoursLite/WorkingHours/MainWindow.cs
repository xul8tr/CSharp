#region Usings
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
#endregion

namespace WorkingHours
{
    public partial class MainWindow : Form
    {
        #region Fields
        private BackgroundWorker m_BackgroundWorker;
        private double effortOI, effortDefect;
        #endregion

        #region Properties
        public IList<string> Users { get; set; }       
        #endregion
               
        #region Public methods
      
        private delegate void EnableStartButtonDelegate();
        public void EnableStartButton()
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
 
        private delegate void UpdateSumValueDelegate(double itestEffort, bool isOpenIssue);
        public void UpdateSumValue(double itestEffort, bool isOpenIssue)
        {
            if (textBoxEffortDef.InvokeRequired)
            {
                UpdateSumValueDelegate del = new UpdateSumValueDelegate(UpdateSumValue);
                textBoxEffortDef.Invoke(del, new object[] {itestEffort, isOpenIssue });
            }
            else
            {
                effortDefect += isOpenIssue ? 0 : itestEffort;
                effortOI += isOpenIssue ? itestEffort : 0;
                UpdateTextBoxs();
            }
        }
        
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            InitializeUsersList();
            InitializeStartDateTimePicker();
        }
        #endregion

        #region Enventhandlers
        private void button_SelectUsers_Click(object sender, EventArgs e)
        {
            DisplaySelectUsersDialogAndUpdateUserList();
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            if (button_Start.Text == "Start")
            {
                ResetCounters();
                SetupWindowAppearenceForStartingToWork();
                SetAndStartBackgroundWorker();                
            }
            else
            {
                m_BackgroundWorker.CancelAsync();
            }

            button_Start.Text = button_Start.Text == "Start" ? "Stop" : "Start";
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            StartWorking();           
        }
        
        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {            
            if (e.ProgressPercentage < 0)
            {
                SetProgressbarAppearancForIndeterminate();
            }
            else
            {
                DisplayProgress(e);
            }

            DisplayInfoForTheUser(e);
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                SetStatusbarAppearanceForReady();
            }
            else
            {
                SetStatusbarAppearanceForError(e);
            }

            ResetWindowsAppearanceToDefault();
        }
        #endregion

        #region Method implementation
        private void InitializeStartDateTimePicker()
        {
            dateTimePicker_StartDate.Value = DateTime.Now.AddDays(-7);
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

        private void SetupWindowAppearenceForStartingToWork()
        {
            TaskbarProgress.SetState(this.Handle, TaskbarProgress.TaskbarStates.NoProgress);
            labelInfo.BackColor = System.Drawing.SystemColors.Control;
            labelInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            dateTimePicker_EndDate.Enabled = false;
            dateTimePicker_StartDate.Enabled = false;
            button_Start.Enabled = false;
        }
        
        private void SetAndStartBackgroundWorker()
        {
            m_BackgroundWorker = new BackgroundWorker();
            m_BackgroundWorker.WorkerSupportsCancellation = true;
            m_BackgroundWorker.WorkerReportsProgress = true;
            m_BackgroundWorker.DoWork += backgroundWorker_DoWork;
            m_BackgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            m_BackgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            m_BackgroundWorker.RunWorkerAsync();
        }

        private void StartWorking()
        {
            WorkerImplementation wi = new WorkerImplementation(this, m_BackgroundWorker);
            wi.DoWork(dateTimePicker_StartDate.Value, dateTimePicker_EndDate.Value);
        }

        private void DisplayInfoForTheUser(ProgressChangedEventArgs e)
        {
            OwnUserState userState = e.UserState as OwnUserState;
            labelInfo.Text = userState.Status;
            if (userState.UpdateNeeded == true)
            {
                userState.UpdateNeeded = false;
            }
        }

        private void DisplayProgress(ProgressChangedEventArgs e)
        {
            toolStripProgressBar_Info.Style = ProgressBarStyle.Blocks;
            toolStripProgressBar_Info.Value = e.ProgressPercentage;
            TaskbarProgress.SetValue(this.Handle, e.ProgressPercentage, 100);
        }

        private void SetProgressbarAppearancForIndeterminate()
        {
            toolStripProgressBar_Info.Style = ProgressBarStyle.Marquee;
            TaskbarProgress.SetState(this.Handle, TaskbarProgress.TaskbarStates.Indeterminate);
        }

        private void ResetWindowsAppearanceToDefault()
        {
            toolStripProgressBar_Info.Value = 0;
            button_Start.Text = "Start";
            dateTimePicker_EndDate.Enabled = true;
            dateTimePicker_StartDate.Enabled = true;
        }

        private void SetStatusbarAppearanceForError(RunWorkerCompletedEventArgs e)
        {
            labelInfo.BackColor = System.Drawing.Color.Red;
            labelInfo.ForeColor = System.Drawing.Color.White;
            labelInfo.Text += string.Format(" Exception: {0}. Operation incomplete.", e.Error.Message);
            TaskbarProgress.SetState(this.Handle, TaskbarProgress.TaskbarStates.Error);
        }

        private void SetStatusbarAppearanceForReady()
        {
            labelInfo.BackColor = System.Drawing.Color.Green;
            labelInfo.ForeColor = System.Drawing.Color.White;
            labelInfo.Text = "Ready";
            TaskbarProgress.SetState(this.Handle, TaskbarProgress.TaskbarStates.NoProgress);
        }

        private void DisplaySelectUsersDialogAndUpdateUserList()
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

        private void ResetCounters()
        {
            effortDefect = 0;
            effortOI = 0;
            UpdateTextBoxs();
        }

        private void UpdateTextBoxs()
        {
            textBoxEffortDef.Text = effortDefect.ToString();
            textBoxEffortOI.Text = effortOI.ToString();
        }
        #endregion
    }
}