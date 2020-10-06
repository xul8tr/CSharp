namespace Dijkstra.GUI
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
            this.m_DrawPanel = new System.Windows.Forms.Panel();
            this.m_InfoListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // m_DrawPanel
            // 
            this.m_DrawPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_DrawPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_DrawPanel.Location = new System.Drawing.Point(0, 0);
            this.m_DrawPanel.Name = "m_DrawPanel";
            this.m_DrawPanel.Size = new System.Drawing.Size(284, 194);
            this.m_DrawPanel.TabIndex = 0;
            this.m_DrawPanel.Click += new System.EventHandler(this.m_DrawPanel_Click);
            // 
            // m_InfoListBox
            // 
            this.m_InfoListBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.m_InfoListBox.FormattingEnabled = true;
            this.m_InfoListBox.Location = new System.Drawing.Point(0, 193);
            this.m_InfoListBox.Name = "m_InfoListBox";
            this.m_InfoListBox.Size = new System.Drawing.Size(284, 69);
            this.m_InfoListBox.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.m_InfoListBox);
            this.Controls.Add(this.m_DrawPanel);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel m_DrawPanel;
        private System.Windows.Forms.ListBox m_InfoListBox;
    }
}

