namespace WorkingHours
{
    partial class EditUsersDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox_SelectedUsers = new System.Windows.Forms.ListBox();
            this.button_Add = new System.Windows.Forms.Button();
            this.button_Remove = new System.Windows.Forms.Button();
            this.button_Close = new System.Windows.Forms.Button();
            this.button_SaveList = new System.Windows.Forms.Button();
            this.textBox_NameToAdd = new System.Windows.Forms.TextBox();
            this.label_NameToAdd = new System.Windows.Forms.Label();
            this.button_ValidateNames = new System.Windows.Forms.Button();
            this.button_LoadList = new System.Windows.Forms.Button();
            this.button_Edit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox_SelectedUsers
            // 
            this.listBox_SelectedUsers.Dock = System.Windows.Forms.DockStyle.Right;
            this.listBox_SelectedUsers.FormattingEnabled = true;
            this.listBox_SelectedUsers.Location = new System.Drawing.Point(334, 0);
            this.listBox_SelectedUsers.Name = "listBox_SelectedUsers";
            this.listBox_SelectedUsers.Size = new System.Drawing.Size(250, 127);
            this.listBox_SelectedUsers.TabIndex = 2;
            // 
            // button_Add
            // 
            this.button_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Add.Location = new System.Drawing.Point(253, 10);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(75, 23);
            this.button_Add.TabIndex = 1;
            this.button_Add.Text = "Add ->";
            this.button_Add.UseVisualStyleBackColor = true;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // button_Remove
            // 
            this.button_Remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Remove.Location = new System.Drawing.Point(253, 39);
            this.button_Remove.Name = "button_Remove";
            this.button_Remove.Size = new System.Drawing.Size(75, 23);
            this.button_Remove.TabIndex = 3;
            this.button_Remove.Text = "<- Remove";
            this.button_Remove.UseVisualStyleBackColor = true;
            this.button_Remove.Click += new System.EventHandler(this.button_Remove_Click);
            // 
            // button_Close
            // 
            this.button_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Close.Location = new System.Drawing.Point(172, 97);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(75, 23);
            this.button_Close.TabIndex = 8;
            this.button_Close.Text = "Close";
            this.button_Close.UseVisualStyleBackColor = true;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // button_SaveList
            // 
            this.button_SaveList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_SaveList.Location = new System.Drawing.Point(91, 97);
            this.button_SaveList.Name = "button_SaveList";
            this.button_SaveList.Size = new System.Drawing.Size(75, 23);
            this.button_SaveList.TabIndex = 7;
            this.button_SaveList.Text = "Save list";
            this.button_SaveList.UseVisualStyleBackColor = true;
            this.button_SaveList.Click += new System.EventHandler(this.button_SaveList_Click);
            // 
            // textBox_NameToAdd
            // 
            this.textBox_NameToAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_NameToAdd.Location = new System.Drawing.Point(89, 12);
            this.textBox_NameToAdd.Name = "textBox_NameToAdd";
            this.textBox_NameToAdd.Size = new System.Drawing.Size(150, 20);
            this.textBox_NameToAdd.TabIndex = 0;
            // 
            // label_NameToAdd
            // 
            this.label_NameToAdd.AutoSize = true;
            this.label_NameToAdd.Location = new System.Drawing.Point(25, 15);
            this.label_NameToAdd.Name = "label_NameToAdd";
            this.label_NameToAdd.Size = new System.Drawing.Size(58, 13);
            this.label_NameToAdd.TabIndex = 7;
            this.label_NameToAdd.Text = "New name";
            // 
            // button_ValidateNames
            // 
            this.button_ValidateNames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ValidateNames.Location = new System.Drawing.Point(253, 97);
            this.button_ValidateNames.Name = "button_ValidateNames";
            this.button_ValidateNames.Size = new System.Drawing.Size(75, 23);
            this.button_ValidateNames.TabIndex = 5;
            this.button_ValidateNames.Text = "Validate";
            this.button_ValidateNames.UseVisualStyleBackColor = true;
            this.button_ValidateNames.Click += new System.EventHandler(this.button_ValidateNames_Click);
            // 
            // button_LoadList
            // 
            this.button_LoadList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_LoadList.Location = new System.Drawing.Point(10, 97);
            this.button_LoadList.Name = "button_LoadList";
            this.button_LoadList.Size = new System.Drawing.Size(75, 23);
            this.button_LoadList.TabIndex = 6;
            this.button_LoadList.Text = "Load list";
            this.button_LoadList.UseVisualStyleBackColor = true;
            this.button_LoadList.Click += new System.EventHandler(this.button_LoadList_Click);
            // 
            // button_Edit
            // 
            this.button_Edit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Edit.Location = new System.Drawing.Point(253, 68);
            this.button_Edit.Name = "button_Edit";
            this.button_Edit.Size = new System.Drawing.Size(75, 23);
            this.button_Edit.TabIndex = 4;
            this.button_Edit.Text = "Edit";
            this.button_Edit.UseVisualStyleBackColor = true;
            this.button_Edit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // EditUsersDialog
            // 
            this.AcceptButton = this.button_Add;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Close;
            this.ClientSize = new System.Drawing.Size(584, 127);
            this.Controls.Add(this.button_Edit);
            this.Controls.Add(this.button_LoadList);
            this.Controls.Add(this.button_ValidateNames);
            this.Controls.Add(this.label_NameToAdd);
            this.Controls.Add(this.textBox_NameToAdd);
            this.Controls.Add(this.button_SaveList);
            this.Controls.Add(this.button_Close);
            this.Controls.Add(this.button_Remove);
            this.Controls.Add(this.button_Add);
            this.Controls.Add(this.listBox_SelectedUsers);
            this.MinimumSize = new System.Drawing.Size(600, 165);
            this.Name = "EditUsersDialog";
            this.Text = "Edit Users";
            this.Load += new System.EventHandler(this.EditUsersDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_SelectedUsers;
        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.Button button_Remove;
        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.Button button_SaveList;
        private System.Windows.Forms.TextBox textBox_NameToAdd;
        private System.Windows.Forms.Label label_NameToAdd;
        private System.Windows.Forms.Button button_ValidateNames;
        private System.Windows.Forms.Button button_LoadList;
        private System.Windows.Forms.Button button_Edit;
    }
}