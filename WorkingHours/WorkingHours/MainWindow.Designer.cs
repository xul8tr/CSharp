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
            this.dataGridView_Result = new System.Windows.Forms.DataGridView();
            this.dataSet_Results = new System.Data.DataSet();
            this.button_Start = new System.Windows.Forms.Button();
            this.buttonToggle = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.toolStrip_Info = new System.Windows.Forms.ToolStrip();
            this.toolStripProgressBar_Info = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.labelInfo = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel_Sum = new System.Windows.Forms.ToolStripLabel();
            this.label_Note = new System.Windows.Forms.Label();
            this.button_ClearSetting = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Result)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Results)).BeginInit();
            this.toolStrip_Info.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimePicker_StartDate
            // 
            this.dateTimePicker_StartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker_StartDate.Location = new System.Drawing.Point(72, 15);
            this.dateTimePicker_StartDate.Name = "dateTimePicker_StartDate";
            this.dateTimePicker_StartDate.Size = new System.Drawing.Size(139, 20);
            this.dateTimePicker_StartDate.TabIndex = 0;
            // 
            // dateTimePicker_EndDate
            // 
            this.dateTimePicker_EndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker_EndDate.Location = new System.Drawing.Point(72, 41);
            this.dateTimePicker_EndDate.Name = "dateTimePicker_EndDate";
            this.dateTimePicker_EndDate.Size = new System.Drawing.Size(139, 20);
            this.dateTimePicker_EndDate.TabIndex = 1;
            // 
            // label_StartDate
            // 
            this.label_StartDate.AutoSize = true;
            this.label_StartDate.Location = new System.Drawing.Point(13, 21);
            this.label_StartDate.Name = "label_StartDate";
            this.label_StartDate.Size = new System.Drawing.Size(53, 13);
            this.label_StartDate.TabIndex = 2;
            this.label_StartDate.Text = "Start date";
            // 
            // label_EndDate
            // 
            this.label_EndDate.AutoSize = true;
            this.label_EndDate.Location = new System.Drawing.Point(13, 47);
            this.label_EndDate.Name = "label_EndDate";
            this.label_EndDate.Size = new System.Drawing.Size(50, 13);
            this.label_EndDate.TabIndex = 3;
            this.label_EndDate.Text = "End date";
            // 
            // dataGridView_Result
            // 
            this.dataGridView_Result.AllowUserToAddRows = false;
            this.dataGridView_Result.AllowUserToDeleteRows = false;
            this.dataGridView_Result.AllowUserToOrderColumns = true;
            this.dataGridView_Result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_Result.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_Result.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dataGridView_Result.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Result.Location = new System.Drawing.Point(12, 84);
            this.dataGridView_Result.Name = "dataGridView_Result";
            this.dataGridView_Result.ReadOnly = true;
            this.dataGridView_Result.RowHeadersWidth = 10;
            this.dataGridView_Result.Size = new System.Drawing.Size(440, 248);
            this.dataGridView_Result.TabIndex = 6;
            this.dataGridView_Result.SelectionChanged += new System.EventHandler(this.dataGridView_Result_SelectionChanged);
            // 
            // dataSet_Results
            // 
            this.dataSet_Results.DataSetName = "NewDataSet";
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(298, 38);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(75, 23);
            this.button_Start.TabIndex = 3;
            this.button_Start.Text = "Start";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // buttonToggle
            // 
            this.buttonToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonToggle.Location = new System.Drawing.Point(377, 38);
            this.buttonToggle.Name = "buttonToggle";
            this.buttonToggle.Size = new System.Drawing.Size(75, 23);
            this.buttonToggle.TabIndex = 5;
            this.buttonToggle.Text = "Summary ->";
            this.buttonToggle.UseVisualStyleBackColor = true;
            this.buttonToggle.Click += new System.EventHandler(this.buttonToggle_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.Location = new System.Drawing.Point(377, 12);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(75, 23);
            this.buttonExport.TabIndex = 4;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
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
            this.toolStrip_Info.Location = new System.Drawing.Point(0, 333);
            this.toolStrip_Info.Name = "toolStrip_Info";
            this.toolStrip_Info.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip_Info.Size = new System.Drawing.Size(464, 29);
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
            // label_Note
            // 
            this.label_Note.AutoEllipsis = true;
            this.label_Note.AutoSize = true;
            this.label_Note.Location = new System.Drawing.Point(71, 68);
            this.label_Note.Name = "label_Note";
            this.label_Note.Size = new System.Drawing.Size(391, 13);
            this.label_Note.TabIndex = 9;
            this.label_Note.Text = "The timestamp of the change might be diffrent in the DB then the displayed value." +
    "";
            // 
            // button_ClearSetting
            // 
            this.button_ClearSetting.Location = new System.Drawing.Point(217, 38);
            this.button_ClearSetting.Name = "button_ClearSetting";
            this.button_ClearSetting.Size = new System.Drawing.Size(75, 23);
            this.button_ClearSetting.TabIndex = 10;
            this.button_ClearSetting.Text = "Clear setting";
            this.button_ClearSetting.UseVisualStyleBackColor = true;
            this.button_ClearSetting.Click += new System.EventHandler(this.button_ClearSetting_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 362);
            this.Controls.Add(this.button_ClearSetting);
            this.Controls.Add(this.label_Note);
            this.Controls.Add(this.toolStrip_Info);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.buttonToggle);
            this.Controls.Add(this.button_Start);
            this.Controls.Add(this.dataGridView_Result);
            this.Controls.Add(this.label_EndDate);
            this.Controls.Add(this.label_StartDate);
            this.Controls.Add(this.dateTimePicker_EndDate);
            this.Controls.Add(this.dateTimePicker_StartDate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(480, 400);
            this.Name = "MainWindow";
            this.Text = "Working hours checker";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Result)).EndInit();
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
        private System.Windows.Forms.DataGridView dataGridView_Result;
        private System.Data.DataSet dataSet_Results;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.Button buttonToggle;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.ToolStrip toolStrip_Info;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar_Info;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel labelInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_Sum;
        private System.Windows.Forms.Label label_Note;
        private System.Windows.Forms.Button button_ClearSetting;
    }
}

