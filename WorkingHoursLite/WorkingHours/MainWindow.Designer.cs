namespace WorkingHours
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.dateTimePicker_StartDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_EndDate = new System.Windows.Forms.DateTimePicker();
            this.label_StartDate = new System.Windows.Forms.Label();
            this.label_EndDate = new System.Windows.Forms.Label();
            this.dataSet_Results = new System.Data.DataSet();
            this.button_SelectUsers = new System.Windows.Forms.Button();
            this.button_Start = new System.Windows.Forms.Button();
            this.toolStrip_Info = new System.Windows.Forms.ToolStrip();
            this.toolStripProgressBar_Info = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.labelInfo = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel_Sum = new System.Windows.Forms.ToolStripLabel();
            this.label_SpentOnDef = new System.Windows.Forms.Label();
            this.label_SpentOnOI = new System.Windows.Forms.Label();
            this.textBoxEffortDef = new System.Windows.Forms.TextBox();
            this.textBoxEffortOI = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Results)).BeginInit();
            this.toolStrip_Info.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimePicker_StartDate
            // 
            this.dateTimePicker_StartDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker_StartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker_StartDate.Location = new System.Drawing.Point(85, 15);
            this.dateTimePicker_StartDate.Name = "dateTimePicker_StartDate";
            this.dateTimePicker_StartDate.Size = new System.Drawing.Size(161, 20);
            this.dateTimePicker_StartDate.TabIndex = 0;
            // 
            // dateTimePicker_EndDate
            // 
            this.dateTimePicker_EndDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker_EndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker_EndDate.Location = new System.Drawing.Point(85, 41);
            this.dateTimePicker_EndDate.Name = "dateTimePicker_EndDate";
            this.dateTimePicker_EndDate.Size = new System.Drawing.Size(161, 20);
            this.dateTimePicker_EndDate.TabIndex = 1;
            // 
            // label_StartDate
            // 
            this.label_StartDate.AutoSize = true;
            this.label_StartDate.Location = new System.Drawing.Point(23, 21);
            this.label_StartDate.Name = "label_StartDate";
            this.label_StartDate.Size = new System.Drawing.Size(53, 13);
            this.label_StartDate.TabIndex = 2;
            this.label_StartDate.Text = "Start date";
            // 
            // label_EndDate
            // 
            this.label_EndDate.AutoSize = true;
            this.label_EndDate.Location = new System.Drawing.Point(23, 47);
            this.label_EndDate.Name = "label_EndDate";
            this.label_EndDate.Size = new System.Drawing.Size(50, 13);
            this.label_EndDate.TabIndex = 3;
            this.label_EndDate.Text = "End date";
            // 
            // dataSet_Results
            // 
            this.dataSet_Results.DataSetName = "NewDataSet";
            // 
            // button_SelectUsers
            // 
            this.button_SelectUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SelectUsers.Location = new System.Drawing.Point(265, 11);
            this.button_SelectUsers.Name = "button_SelectUsers";
            this.button_SelectUsers.Size = new System.Drawing.Size(75, 23);
            this.button_SelectUsers.TabIndex = 2;
            this.button_SelectUsers.Text = "Edit users";
            this.button_SelectUsers.UseVisualStyleBackColor = true;
            this.button_SelectUsers.Click += new System.EventHandler(this.button_SelectUsers_Click);
            // 
            // button_Start
            // 
            this.button_Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Start.Location = new System.Drawing.Point(265, 41);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(75, 23);
            this.button_Start.TabIndex = 3;
            this.button_Start.Text = "Start";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // toolStrip_Info
            // 
            this.toolStrip_Info.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip_Info.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip_Info.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar_Info,
            this.toolStripSeparator1,
            this.labelInfo,
            this.toolStripSeparator2,
            this.toolStripLabel_Sum});
            this.toolStrip_Info.Location = new System.Drawing.Point(0, 173);
            this.toolStrip_Info.Name = "toolStrip_Info";
            this.toolStrip_Info.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip_Info.Size = new System.Drawing.Size(364, 29);
            this.toolStrip_Info.TabIndex = 8;
            this.toolStrip_Info.Text = "toolStrip1";
            // 
            // toolStripProgressBar_Info
            // 
            this.toolStripProgressBar_Info.AutoToolTip = true;
            this.toolStripProgressBar_Info.Margin = new System.Windows.Forms.Padding(13, 2, 1, 5);
            this.toolStripProgressBar_Info.Name = "toolStripProgressBar_Info";
            this.toolStripProgressBar_Info.Size = new System.Drawing.Size(150, 22);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 29);
            // 
            // labelInfo
            // 
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(39, 26);
            this.labelInfo.Text = "Ready";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 29);
            // 
            // toolStripLabel_Sum
            // 
            this.toolStripLabel_Sum.Name = "toolStripLabel_Sum";
            this.toolStripLabel_Sum.Size = new System.Drawing.Size(0, 26);
            // 
            // label_SpentOnDef
            // 
            this.label_SpentOnDef.Location = new System.Drawing.Point(73, 90);
            this.label_SpentOnDef.Name = "label_SpentOnDef";
            this.label_SpentOnDef.Size = new System.Drawing.Size(103, 29);
            this.label_SpentOnDef.TabIndex = 11;
            this.label_SpentOnDef.Text = "Effort spent on DEFECT";
            this.label_SpentOnDef.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_SpentOnOI
            // 
            this.label_SpentOnOI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_SpentOnOI.Location = new System.Drawing.Point(192, 90);
            this.label_SpentOnOI.Name = "label_SpentOnOI";
            this.label_SpentOnOI.Size = new System.Drawing.Size(100, 28);
            this.label_SpentOnOI.TabIndex = 12;
            this.label_SpentOnOI.Text = "Effort spent on OPEN ISSUE";
            this.label_SpentOnOI.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxEffortDef
            // 
            this.textBoxEffortDef.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textBoxEffortDef.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxEffortDef.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxEffortDef.Location = new System.Drawing.Point(76, 121);
            this.textBoxEffortDef.Name = "textBoxEffortDef";
            this.textBoxEffortDef.Size = new System.Drawing.Size(100, 24);
            this.textBoxEffortDef.TabIndex = 13;
            this.textBoxEffortDef.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxEffortOI
            // 
            this.textBoxEffortOI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEffortOI.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textBoxEffortOI.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxEffortOI.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxEffortOI.Location = new System.Drawing.Point(192, 121);
            this.textBoxEffortOI.Name = "textBoxEffortOI";
            this.textBoxEffortOI.Size = new System.Drawing.Size(100, 24);
            this.textBoxEffortOI.TabIndex = 14;
            this.textBoxEffortOI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 202);
            this.Controls.Add(this.textBoxEffortOI);
            this.Controls.Add(this.textBoxEffortDef);
            this.Controls.Add(this.label_SpentOnOI);
            this.Controls.Add(this.label_SpentOnDef);
            this.Controls.Add(this.toolStrip_Info);
            this.Controls.Add(this.button_Start);
            this.Controls.Add(this.button_SelectUsers);
            this.Controls.Add(this.label_EndDate);
            this.Controls.Add(this.label_StartDate);
            this.Controls.Add(this.dateTimePicker_EndDate);
            this.Controls.Add(this.dateTimePicker_StartDate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(380, 240);
            this.Name = "MainWindow";
            this.Text = "Working hours checker";
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Results)).EndInit();
            this.toolStrip_Info.ResumeLayout(false);
            this.toolStrip_Info.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker_StartDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker_EndDate;
        private System.Windows.Forms.Label label_StartDate;
        private System.Windows.Forms.Label label_EndDate;
        private System.Data.DataSet dataSet_Results;
        private System.Windows.Forms.Button button_SelectUsers;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.ToolStrip toolStrip_Info;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar_Info;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel labelInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_Sum;
        private System.Windows.Forms.Label label_SpentOnDef;
        private System.Windows.Forms.Label label_SpentOnOI;
        private System.Windows.Forms.TextBox textBoxEffortDef;
        private System.Windows.Forms.TextBox textBoxEffortOI;
    }
}

