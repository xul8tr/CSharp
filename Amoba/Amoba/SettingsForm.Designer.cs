namespace Amoba
{
    partial class SettingsForm
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
            this.numericUpRow = new System.Windows.Forms.NumericUpDown();
            this.numericUpColumn = new System.Windows.Forms.NumericUpDown();
            this.labelsorok = new System.Windows.Forms.Label();
            this.labeloszlopok = new System.Windows.Forms.Label();
            this.buttonok = new System.Windows.Forms.Button();
            this.buttoncancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpColumn)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpRow
            // 
            this.numericUpRow.Location = new System.Drawing.Point(130, 15);
            this.numericUpRow.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpRow.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpRow.Name = "numericUpRow";
            this.numericUpRow.Size = new System.Drawing.Size(76, 20);
            this.numericUpRow.TabIndex = 0;
            this.numericUpRow.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // numericUpColumn
            // 
            this.numericUpColumn.Location = new System.Drawing.Point(130, 48);
            this.numericUpColumn.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpColumn.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpColumn.Name = "numericUpColumn";
            this.numericUpColumn.Size = new System.Drawing.Size(76, 20);
            this.numericUpColumn.TabIndex = 1;
            this.numericUpColumn.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // labelsorok
            // 
            this.labelsorok.AutoSize = true;
            this.labelsorok.Location = new System.Drawing.Point(12, 17);
            this.labelsorok.Name = "labelsorok";
            this.labelsorok.Size = new System.Drawing.Size(68, 13);
            this.labelsorok.TabIndex = 2;
            this.labelsorok.Text = "Sorok száma";
            // 
            // labeloszlopok
            // 
            this.labeloszlopok.AutoSize = true;
            this.labeloszlopok.Location = new System.Drawing.Point(12, 50);
            this.labeloszlopok.Name = "labeloszlopok";
            this.labeloszlopok.Size = new System.Drawing.Size(84, 13);
            this.labeloszlopok.TabIndex = 3;
            this.labeloszlopok.Text = "Oszlopok száma";
            // 
            // buttonok
            // 
            this.buttonok.Location = new System.Drawing.Point(21, 83);
            this.buttonok.Name = "buttonok";
            this.buttonok.Size = new System.Drawing.Size(75, 23);
            this.buttonok.TabIndex = 4;
            this.buttonok.Text = "Ok";
            this.buttonok.UseVisualStyleBackColor = true;
            this.buttonok.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttoncancel
            // 
            this.buttoncancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttoncancel.Location = new System.Drawing.Point(130, 83);
            this.buttoncancel.Name = "buttoncancel";
            this.buttoncancel.Size = new System.Drawing.Size(75, 23);
            this.buttoncancel.TabIndex = 5;
            this.buttoncancel.Text = "Cancel";
            this.buttoncancel.UseVisualStyleBackColor = true;
            this.buttoncancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // Settings
            // 
            this.AcceptButton = this.buttonok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttoncancel;
            this.ClientSize = new System.Drawing.Size(224, 122);
            this.Controls.Add(this.buttoncancel);
            this.Controls.Add(this.buttonok);
            this.Controls.Add(this.labeloszlopok);
            this.Controls.Add(this.labelsorok);
            this.Controls.Add(this.numericUpColumn);
            this.Controls.Add(this.numericUpRow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpColumn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpRow;
        private System.Windows.Forms.NumericUpDown numericUpColumn;
        private System.Windows.Forms.Label labelsorok;
        private System.Windows.Forms.Label labeloszlopok;
        private System.Windows.Forms.Button buttonok;
        private System.Windows.Forms.Button buttoncancel;
    }
}