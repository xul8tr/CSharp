namespace Amoba
{
    partial class GameForm
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.beállításToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.táblaméretToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.újJátékToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kovetkezoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.beállításToolStripMenuItem,
            this.újJátékToolStripMenuItem,
            this.kovetkezoToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(284, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // beállításToolStripMenuItem
            // 
            this.beállításToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.táblaméretToolStripMenuItem});
            this.beállításToolStripMenuItem.Name = "beállításToolStripMenuItem";
            this.beállításToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.beállításToolStripMenuItem.Text = "Beállítások";
            // 
            // táblaméretToolStripMenuItem
            // 
            this.táblaméretToolStripMenuItem.Name = "táblaméretToolStripMenuItem";
            this.táblaméretToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.táblaméretToolStripMenuItem.Text = "Táblaméret";
            this.táblaméretToolStripMenuItem.Click += new System.EventHandler(this.táblaméretToolStripMenuItem_Click);
            // 
            // újJátékToolStripMenuItem
            // 
            this.újJátékToolStripMenuItem.Name = "újJátékToolStripMenuItem";
            this.újJátékToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.újJátékToolStripMenuItem.Text = "Új játék";
            this.újJátékToolStripMenuItem.Click += new System.EventHandler(this.újJátékToolStripMenuItem_Click);
            // 
            // kovetkezoToolStripMenuItem
            // 
            this.kovetkezoToolStripMenuItem.Name = "kovetkezoToolStripMenuItem";
            this.kovetkezoToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.kovetkezoToolStripMenuItem.Text = "Kovetkezo";
            // 
            // buttonPanel
            // 
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPanel.Location = new System.Drawing.Point(0, 24);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(284, 238);
            this.buttonPanel.TabIndex = 1;
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "GameForm";
            this.Text = "Amőba";
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem beállításToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem táblaméretToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem újJátékToolStripMenuItem;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.ToolStripMenuItem kovetkezoToolStripMenuItem;
    }
}

