namespace CopyMappedDriveLocation
{
    partial class Form1
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
            this.buttonCopy = new System.Windows.Forms.Button();
            this.listViewNwDrives = new System.Windows.Forms.ListView();
            this.Drive = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Path = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.checkBoxAutoClose = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonCopy
            // 
            this.buttonCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopy.Location = new System.Drawing.Point(341, 12);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(75, 23);
            this.buttonCopy.TabIndex = 1;
            this.buttonCopy.Text = "&Copy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // listViewNwDrives
            // 
            this.listViewNwDrives.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewNwDrives.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Drive,
            this.Path});
            this.listViewNwDrives.Location = new System.Drawing.Point(0, 0);
            this.listViewNwDrives.Name = "listViewNwDrives";
            this.listViewNwDrives.Size = new System.Drawing.Size(335, 264);
            this.listViewNwDrives.TabIndex = 2;
            this.listViewNwDrives.UseCompatibleStateImageBehavior = false;
            this.listViewNwDrives.View = System.Windows.Forms.View.Details;
            // 
            // Drive
            // 
            this.Drive.Text = "Drive";
            // 
            // Path
            // 
            this.Path.Text = "Path";
            // 
            // checkBoxAutoClose
            // 
            this.checkBoxAutoClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAutoClose.AutoSize = true;
            this.checkBoxAutoClose.Location = new System.Drawing.Point(341, 43);
            this.checkBoxAutoClose.Name = "checkBoxAutoClose";
            this.checkBoxAutoClose.Size = new System.Drawing.Size(102, 17);
            this.checkBoxAutoClose.TabIndex = 3;
            this.checkBoxAutoClose.Text = "Close after copy";
            this.checkBoxAutoClose.UseVisualStyleBackColor = true;
            this.checkBoxAutoClose.CheckedChanged += new System.EventHandler(this.checkBoxAutoClose_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 262);
            this.Controls.Add(this.checkBoxAutoClose);
            this.Controls.Add(this.listViewNwDrives);
            this.Controls.Add(this.buttonCopy);
            this.Name = "Form1";
            this.Text = "Copy Network Path";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.ListView listViewNwDrives;
        private System.Windows.Forms.ColumnHeader Drive;
        private System.Windows.Forms.ColumnHeader Path;
        private System.Windows.Forms.CheckBox checkBoxAutoClose;
    }
}

