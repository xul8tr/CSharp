#region Usings
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Utils;
#endregion

namespace WorkingHours
{
    public partial class EditUsersDialog : Form
    {
        #region Fields
        private bool changed;
        #endregion

        #region Properties
        public IList<string> Users { get; set; }
        #endregion

        #region Constructor
        public EditUsersDialog(IList<string> users)
        {
            InitializeComponent();
            if (users != null)
            {
                Users = users;
            }

            if (Users == null)
            {
                Users = new List<string>();
            }

            UpdateUserBox();           
        }
        #endregion

        #region Methods
        private void UpdateUserBox()
        {
            listBox_SelectedUsers.Items.Clear();
            listBox_SelectedUsers.Items.AddRange(Users.ToArray<string>());
        }

        private bool CloseDialog()
        {
            bool retVal = true;
            this.DialogResult = System.Windows.Forms.DialogResult.Abort;
            if (changed)
            {
                DialogResult dr = MessageBox.Show("Do you want to save the changes?", "Close dialog", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    retVal = false;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else if (dr == DialogResult.No)
                {
                    retVal = false;
                }
            }

            return retVal && changed;
        }

        #endregion

        #region Eventhandlers
        private void button_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditUsersDialog_Load(object sender, EventArgs e)
        {
            this.FormClosing += EditUsersDialog_FormClosing;           
        }

        void EditUsersDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = CloseDialog();            
        }        

        private void button_SaveList_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "Please select a file! The file will be overwritten!";
                saveFileDialog.ValidateNames = true;
                DialogResult dr = saveFileDialog.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    using (FileStream stream = File.Open(saveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        using (TextWriter tw = new StreamWriter(stream))
                        {
                            foreach (string user in Users)
                            {
                                tw.WriteLine(user);
                            }

                            tw.Flush();
                        }
                    }
                }
            }
        }

        private void button_LoadList_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select list of users. The file won't be validated!";
                openFileDialog.ValidateNames = true;
                DialogResult dr = openFileDialog.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    if (!File.Exists(openFileDialog.FileName))
                    {
                        MessageBox.Show("The selected file does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (Users.Count > 0)
                        {
                            dr = MessageBox.Show("Do you want to extend the list?\nIf you press 'No', the list will be replaced.", "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            if (dr == System.Windows.Forms.DialogResult.No)
                            {
                                Users.Clear();
                            }
                        }

                        if (dr != DialogResult.Cancel)
                        {
                            using (FileStream stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                            {
                                using (TextReader reader = new StreamReader(stream))
                                {
                                    string line;
                                    while ((line = reader.ReadLine()) != null)
                                    {
                                        Users.Add(line);
                                    }
                                }
                            }

                            UpdateUserBox();
                            changed = true;
                        }
                    }
                }
            }
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            string item = textBox_NameToAdd.Text;
            if (!string.IsNullOrEmpty(item))
            {
                DialogResult dr = System.Windows.Forms.DialogResult.Yes;                
                if (Users.Contains(item))
                {
                    dr = MessageBox.Show("This user already added to the list!\nDo you wish to duplicate the entry?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                }

                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    listBox_SelectedUsers.Items.Add(item);
                    Users.Add(item);
                    changed = true;
                    textBox_NameToAdd.Clear();
                }
            }            
        }

        private void button_Remove_Click(object sender, EventArgs e)
        {
            if (listBox_SelectedUsers.Items != null && listBox_SelectedUsers.Items.Count > 0 && listBox_SelectedUsers.SelectedIndices.Count > 0)
            {
                foreach (int item in listBox_SelectedUsers.SelectedIndices)
                {
                    Users.Remove(listBox_SelectedUsers.Items[item].ToString());
                    listBox_SelectedUsers.Items.RemoveAt(item);
                    changed = true;
                }
            }
        }

        private void button_ValidateNames_Click(object sender, EventArgs e)
        {
            if (listBox_SelectedUsers.Items != null && listBox_SelectedUsers.Items.Count > 0)
            {
                bool valid = true;
                foreach (object item in listBox_SelectedUsers.Items)
                {
                    if (TfsUtils.GetIdentity((string)item) == null)
                    {
                        DialogResult dr = MessageBox.Show(string.Format("The username: {0} could not be validated as a valid TFS user.\nContinue the validation?", item), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                        valid = false;
                        if (dr == System.Windows.Forms.DialogResult.No)
                        {
                            break;
                        }
                    }
                }

                if (valid)
                {
                    MessageBox.Show("All names are valid.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (listBox_SelectedUsers.Items != null && listBox_SelectedUsers.Items.Count > 0 && listBox_SelectedUsers.SelectedIndices.Count > 0)
            {
                textBox_NameToAdd.Text = listBox_SelectedUsers.SelectedItem.ToString();
                Users.Remove(listBox_SelectedUsers.SelectedItem.ToString());
                listBox_SelectedUsers.Items.Remove(listBox_SelectedUsers.SelectedItem);
                listBox_SelectedUsers.SelectedItem = null;
                textBox_NameToAdd.Focus();
            }
        }
        #endregion

    }
}
