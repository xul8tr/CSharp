namespace ShelveSetMerger
{
    partial class MainForm
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
            this.m_ShelveSetNameLabel = new System.Windows.Forms.Label();
            this.m_UserNameLabel = new System.Windows.Forms.Label();
            this.m_SourceTextBox = new System.Windows.Forms.TextBox();
            this.m_SourceBranchLabel = new System.Windows.Forms.Label();
            this.m_TargetBranchLabel = new System.Windows.Forms.Label();
            this.m_UserComboBox = new System.Windows.Forms.ComboBox();
            this.m_FillUsersButton = new System.Windows.Forms.Button();
            this.m_ListButton = new System.Windows.Forms.Button();
            this.m_ShelveComboBox = new System.Windows.Forms.ComboBox();
            this.m_TagetBranchComboBox = new System.Windows.Forms.ComboBox();
            this.m_CommandLineTextBox = new System.Windows.Forms.TextBox();
            this.m_CommandLineLabel = new System.Windows.Forms.Label();
            this.m_ExecuteButton = new System.Windows.Forms.Button();
            this.m_CloseButton = new System.Windows.Forms.Button();
            this.m_statusStrip = new System.Windows.Forms.StatusStrip();
            this.m_ToolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.m_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_CopyrightLabel = new System.Windows.Forms.Label();
            this.m_statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_ShelveSetNameLabel
            // 
            this.m_ShelveSetNameLabel.AutoSize = true;
            this.m_ShelveSetNameLabel.Location = new System.Drawing.Point(12, 57);
            this.m_ShelveSetNameLabel.Name = "m_ShelveSetNameLabel";
            this.m_ShelveSetNameLabel.Size = new System.Drawing.Size(113, 13);
            this.m_ShelveSetNameLabel.TabIndex = 4;
            this.m_ShelveSetNameLabel.Text = "Name of the &shelveset";
            // 
            // m_UserNameLabel
            // 
            this.m_UserNameLabel.AutoSize = true;
            this.m_UserNameLabel.Location = new System.Drawing.Point(12, 9);
            this.m_UserNameLabel.Name = "m_UserNameLabel";
            this.m_UserNameLabel.Size = new System.Drawing.Size(88, 13);
            this.m_UserNameLabel.TabIndex = 1;
            this.m_UserNameLabel.Text = "Name of the &user";
            // 
            // m_SourceTextBox
            // 
            this.m_SourceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_SourceTextBox.Location = new System.Drawing.Point(15, 125);
            this.m_SourceTextBox.Name = "m_SourceTextBox";
            this.m_SourceTextBox.Size = new System.Drawing.Size(430, 20);
            this.m_SourceTextBox.TabIndex = 8;
            this.m_SourceTextBox.TextChanged += new System.EventHandler(this.SourceTextBox_TextChanged);
            // 
            // m_SourceBranchLabel
            // 
            this.m_SourceBranchLabel.AutoSize = true;
            this.m_SourceBranchLabel.Location = new System.Drawing.Point(12, 109);
            this.m_SourceBranchLabel.Name = "m_SourceBranchLabel";
            this.m_SourceBranchLabel.Size = new System.Drawing.Size(77, 13);
            this.m_SourceBranchLabel.TabIndex = 7;
            this.m_SourceBranchLabel.Text = "Source &branch";
            // 
            // m_TargetBranchLabel
            // 
            this.m_TargetBranchLabel.AutoSize = true;
            this.m_TargetBranchLabel.Location = new System.Drawing.Point(12, 159);
            this.m_TargetBranchLabel.Name = "m_TargetBranchLabel";
            this.m_TargetBranchLabel.Size = new System.Drawing.Size(74, 13);
            this.m_TargetBranchLabel.TabIndex = 9;
            this.m_TargetBranchLabel.Text = "&Target branch";
            // 
            // m_UserComboBox
            // 
            this.m_UserComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_UserComboBox.FormattingEnabled = true;
            this.m_UserComboBox.Location = new System.Drawing.Point(15, 25);
            this.m_UserComboBox.Name = "m_UserComboBox";
            this.m_UserComboBox.Size = new System.Drawing.Size(430, 21);
            this.m_UserComboBox.TabIndex = 2;
            this.m_UserComboBox.TextChanged += new System.EventHandler(this.UserComboBox_TextChanged);
            // 
            // m_FillUsersButton
            // 
            this.m_FillUsersButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_FillUsersButton.Location = new System.Drawing.Point(462, 25);
            this.m_FillUsersButton.Name = "m_FillUsersButton";
            this.m_FillUsersButton.Size = new System.Drawing.Size(75, 23);
            this.m_FillUsersButton.TabIndex = 3;
            this.m_FillUsersButton.Text = "&Fill ...";
            this.m_FillUsersButton.UseVisualStyleBackColor = true;
            this.m_FillUsersButton.Click += new System.EventHandler(this.FillUsersButton_Click);
            // 
            // m_ListButton
            // 
            this.m_ListButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ListButton.Location = new System.Drawing.Point(462, 73);
            this.m_ListButton.Name = "m_ListButton";
            this.m_ListButton.Size = new System.Drawing.Size(75, 23);
            this.m_ListButton.TabIndex = 6;
            this.m_ListButton.Text = "&List ...";
            this.m_ListButton.UseVisualStyleBackColor = true;
            this.m_ListButton.Click += new System.EventHandler(this.ListShevesetsButton_Click);
            // 
            // m_ShelveComboBox
            // 
            this.m_ShelveComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ShelveComboBox.FormattingEnabled = true;
            this.m_ShelveComboBox.Location = new System.Drawing.Point(15, 73);
            this.m_ShelveComboBox.Name = "m_ShelveComboBox";
            this.m_ShelveComboBox.Size = new System.Drawing.Size(430, 21);
            this.m_ShelveComboBox.TabIndex = 5;
            this.m_ShelveComboBox.SelectedIndexChanged += new System.EventHandler(this.ShelveComboBox_SelectedIndexChanged);
            // 
            // m_TagetBranchComboBox
            // 
            this.m_TagetBranchComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_TagetBranchComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_TagetBranchComboBox.FormattingEnabled = true;
            this.m_TagetBranchComboBox.Location = new System.Drawing.Point(15, 175);
            this.m_TagetBranchComboBox.Name = "m_TagetBranchComboBox";
            this.m_TagetBranchComboBox.Size = new System.Drawing.Size(430, 21);
            this.m_TagetBranchComboBox.TabIndex = 10;
            this.m_TagetBranchComboBox.TextChanged += new System.EventHandler(this.TagetBranchComboBox_TextChanged);
            // 
            // m_CommandLineTextBox
            // 
            this.m_CommandLineTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_CommandLineTextBox.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.m_CommandLineTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.m_CommandLineTextBox.Location = new System.Drawing.Point(15, 227);
            this.m_CommandLineTextBox.Multiline = true;
            this.m_CommandLineTextBox.Name = "m_CommandLineTextBox";
            this.m_CommandLineTextBox.Size = new System.Drawing.Size(430, 57);
            this.m_CommandLineTextBox.TabIndex = 12;
            // 
            // m_CommandLineLabel
            // 
            this.m_CommandLineLabel.AutoSize = true;
            this.m_CommandLineLabel.Location = new System.Drawing.Point(12, 211);
            this.m_CommandLineLabel.Name = "m_CommandLineLabel";
            this.m_CommandLineLabel.Size = new System.Drawing.Size(70, 13);
            this.m_CommandLineLabel.TabIndex = 11;
            this.m_CommandLineLabel.Text = "&Commandline";
            // 
            // m_ExecuteButton
            // 
            this.m_ExecuteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ExecuteButton.Location = new System.Drawing.Point(462, 221);
            this.m_ExecuteButton.Name = "m_ExecuteButton";
            this.m_ExecuteButton.Size = new System.Drawing.Size(75, 23);
            this.m_ExecuteButton.TabIndex = 13;
            this.m_ExecuteButton.Text = "&Execute";
            this.m_ExecuteButton.UseVisualStyleBackColor = true;
            this.m_ExecuteButton.Click += new System.EventHandler(this.ExecuteButton_Click);
            // 
            // m_CloseButton
            // 
            this.m_CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_CloseButton.Location = new System.Drawing.Point(462, 261);
            this.m_CloseButton.Name = "m_CloseButton";
            this.m_CloseButton.Size = new System.Drawing.Size(75, 23);
            this.m_CloseButton.TabIndex = 14;
            this.m_CloseButton.Text = "Cl&ose";
            this.m_CloseButton.UseVisualStyleBackColor = true;
            this.m_CloseButton.Click += new System.EventHandler(this.m_CloseButton_Click);
            // 
            // m_statusStrip
            // 
            this.m_statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_ToolStripProgressBar,
            this.m_ToolStripStatusLabel});
            this.m_statusStrip.Location = new System.Drawing.Point(0, 302);
            this.m_statusStrip.Name = "m_statusStrip";
            this.m_statusStrip.Size = new System.Drawing.Size(548, 22);
            this.m_statusStrip.TabIndex = 21;
            this.m_statusStrip.Text = "statusStrip1";
            // 
            // m_ToolStripProgressBar
            // 
            this.m_ToolStripProgressBar.Name = "m_ToolStripProgressBar";
            this.m_ToolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // m_ToolStripStatusLabel
            // 
            this.m_ToolStripStatusLabel.Name = "m_ToolStripStatusLabel";
            this.m_ToolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.m_ToolStripStatusLabel.Text = "Ready";
            // 
            // m_CopyrightLabel
            // 
            this.m_CopyrightLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_CopyrightLabel.AutoSize = true;
            this.m_CopyrightLabel.Location = new System.Drawing.Point(12, 287);
            this.m_CopyrightLabel.Name = "m_CopyrightLabel";
            this.m_CopyrightLabel.Size = new System.Drawing.Size(317, 13);
            this.m_CopyrightLabel.TabIndex = 22;
            this.m_CopyrightLabel.Text = "Created by andras.varro@evosoft.com. For internal use only. V0.1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 324);
            this.Controls.Add(this.m_CopyrightLabel);
            this.Controls.Add(this.m_statusStrip);
            this.Controls.Add(this.m_CloseButton);
            this.Controls.Add(this.m_ExecuteButton);
            this.Controls.Add(this.m_CommandLineLabel);
            this.Controls.Add(this.m_CommandLineTextBox);
            this.Controls.Add(this.m_TagetBranchComboBox);
            this.Controls.Add(this.m_ShelveComboBox);
            this.Controls.Add(this.m_ListButton);
            this.Controls.Add(this.m_FillUsersButton);
            this.Controls.Add(this.m_UserComboBox);
            this.Controls.Add(this.m_TargetBranchLabel);
            this.Controls.Add(this.m_SourceTextBox);
            this.Controls.Add(this.m_SourceBranchLabel);
            this.Controls.Add(this.m_UserNameLabel);
            this.Controls.Add(this.m_ShelveSetNameLabel);
            this.MinimumSize = new System.Drawing.Size(564, 362);
            this.Name = "MainForm";
            this.Text = "Shelveset merge helper";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.m_statusStrip.ResumeLayout(false);
            this.m_statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_ShelveSetNameLabel;
        private System.Windows.Forms.Label m_UserNameLabel;
        private System.Windows.Forms.TextBox m_SourceTextBox;
        private System.Windows.Forms.Label m_SourceBranchLabel;
        private System.Windows.Forms.Label m_TargetBranchLabel;
        private System.Windows.Forms.ComboBox m_UserComboBox;
        private System.Windows.Forms.Button m_FillUsersButton;
        private System.Windows.Forms.Button m_ListButton;
        private System.Windows.Forms.ComboBox m_ShelveComboBox;
        private System.Windows.Forms.ComboBox m_TagetBranchComboBox;
        private System.Windows.Forms.TextBox m_CommandLineTextBox;
        private System.Windows.Forms.Label m_CommandLineLabel;
        private System.Windows.Forms.Button m_ExecuteButton;
        private System.Windows.Forms.Button m_CloseButton;
        private System.Windows.Forms.StatusStrip m_statusStrip;
        private System.Windows.Forms.ToolStripProgressBar m_ToolStripProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel m_ToolStripStatusLabel;
        private System.Windows.Forms.Label m_CopyrightLabel;
    }
}

