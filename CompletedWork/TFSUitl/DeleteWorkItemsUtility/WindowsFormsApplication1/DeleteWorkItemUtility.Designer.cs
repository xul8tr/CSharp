namespace WindowsFormsApplication1
{
    partial class DeleteWorkItemUtility
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
            this.serverLabel = new System.Windows.Forms.Label();
            this.cboTfsServer = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.projectLabel = new System.Windows.Forms.Label();
            this.cboProject = new System.Windows.Forms.ComboBox();
            this.optSavedQuery = new System.Windows.Forms.RadioButton();
            this.optWorkItemIds = new System.Windows.Forms.RadioButton();
            this.cboQueries = new System.Windows.Forms.ComboBox();
            this.txtIds = new System.Windows.Forms.TextBox();
            this.deleteLabel = new System.Windows.Forms.Label();
            this.lvResults = new System.Windows.Forms.ListView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.colId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(22, 21);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(38, 13);
            this.serverLabel.TabIndex = 0;
            this.serverLabel.Text = "Server";
            // 
            // cboTfsServer
            // 
            this.cboTfsServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTfsServer.FormattingEnabled = true;
            this.cboTfsServer.Location = new System.Drawing.Point(67, 16);
            this.cboTfsServer.Name = "cboTfsServer";
            this.cboTfsServer.Size = new System.Drawing.Size(200, 21);
            this.cboTfsServer.TabIndex = 1;
            this.cboTfsServer.SelectionChangeCommitted += new System.EventHandler(this.cboTfsServer_SelectionChangeCommitted);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(268, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnServers_Click);
            // 
            // projectLabel
            // 
            this.projectLabel.AutoSize = true;
            this.projectLabel.Location = new System.Drawing.Point(346, 20);
            this.projectLabel.Name = "projectLabel";
            this.projectLabel.Size = new System.Drawing.Size(43, 13);
            this.projectLabel.TabIndex = 3;
            this.projectLabel.Text = "Project:";
            // 
            // cboProject
            // 
            this.cboProject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProject.FormattingEnabled = true;
            this.cboProject.Location = new System.Drawing.Point(396, 15);
            this.cboProject.Name = "cboProject";
            this.cboProject.Size = new System.Drawing.Size(247, 21);
            this.cboProject.TabIndex = 4;
            this.cboProject.SelectionChangeCommitted += new System.EventHandler(this.cboProject_SelectionChangeCommitted);
            // 
            // optSavedQuery
            // 
            this.optSavedQuery.AutoSize = true;
            this.optSavedQuery.Location = new System.Drawing.Point(25, 61);
            this.optSavedQuery.Name = "optSavedQuery";
            this.optSavedQuery.Size = new System.Drawing.Size(87, 17);
            this.optSavedQuery.TabIndex = 5;
            this.optSavedQuery.TabStop = true;
            this.optSavedQuery.Text = "Saved Query";
            this.optSavedQuery.UseVisualStyleBackColor = true;
            // 
            // optWorkItemIds
            // 
            this.optWorkItemIds.AutoSize = true;
            this.optWorkItemIds.Location = new System.Drawing.Point(25, 95);
            this.optWorkItemIds.Name = "optWorkItemIds";
            this.optWorkItemIds.Size = new System.Drawing.Size(41, 17);
            this.optWorkItemIds.TabIndex = 6;
            this.optWorkItemIds.TabStop = true;
            this.optWorkItemIds.Text = "IDs";
            this.optWorkItemIds.UseVisualStyleBackColor = true;
            // 
            // cboQueries
            // 
            this.cboQueries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboQueries.FormattingEnabled = true;
            this.cboQueries.Location = new System.Drawing.Point(140, 61);
            this.cboQueries.Name = "cboQueries";
            this.cboQueries.Size = new System.Drawing.Size(503, 21);
            this.cboQueries.TabIndex = 7;
            // 
            // txtIds
            // 
            this.txtIds.Location = new System.Drawing.Point(140, 95);
            this.txtIds.Name = "txtIds";
            this.txtIds.Size = new System.Drawing.Size(503, 20);
            this.txtIds.TabIndex = 8;
            // 
            // deleteLabel
            // 
            this.deleteLabel.AutoSize = true;
            this.deleteLabel.Location = new System.Drawing.Point(25, 142);
            this.deleteLabel.Name = "deleteLabel";
            this.deleteLabel.Size = new System.Drawing.Size(188, 13);
            this.deleteLabel.TabIndex = 9;
            this.deleteLabel.Text = "Select one or more items to be deleted";
            // 
            // lvResults
            // 
            this.lvResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colId,
            this.colType,
            this.colTitle,
            this.colState});
            this.lvResults.FullRowSelect = true;
            this.lvResults.HideSelection = false;
            this.lvResults.Location = new System.Drawing.Point(28, 159);
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new System.Drawing.Size(615, 226);
            this.lvResults.TabIndex = 10;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.View = System.Windows.Forms.View.Details;
            this.lvResults.SelectedIndexChanged += new System.EventHandler(this.lvResults_SelectedIndexChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(377, 392);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(83, 23);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(466, 392);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 23);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "Find";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(557, 392);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(86, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // colId
            // 
            this.colId.Text = "ID";
            // 
            // colType
            // 
            this.colType.Text = "Type";
            // 
            // colTitle
            // 
            this.colTitle.Text = "Title";
            // 
            // colState
            // 
            this.colState.Text = "State";
            // 
            // DeleteWorkItemUtility
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 427);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.lvResults);
            this.Controls.Add(this.deleteLabel);
            this.Controls.Add(this.txtIds);
            this.Controls.Add(this.cboQueries);
            this.Controls.Add(this.optWorkItemIds);
            this.Controls.Add(this.optSavedQuery);
            this.Controls.Add(this.cboProject);
            this.Controls.Add(this.projectLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cboTfsServer);
            this.Controls.Add(this.serverLabel);
            this.Name = "DeleteWorkItemUtility";
            this.Text = "Delete WorkItem Utility";
            this.Load += new System.EventHandler(this.DeleteWorkItemForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.ComboBox cboTfsServer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label projectLabel;
        private System.Windows.Forms.ComboBox cboProject;
        private System.Windows.Forms.RadioButton optSavedQuery;
        private System.Windows.Forms.RadioButton optWorkItemIds;
        private System.Windows.Forms.ComboBox cboQueries;
        private System.Windows.Forms.TextBox txtIds;
        private System.Windows.Forms.Label deleteLabel;
        private System.Windows.Forms.ListView lvResults;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ColumnHeader colId;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colTitle;
        private System.Windows.Forms.ColumnHeader colState;
    }
}

