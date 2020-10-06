namespace ELTE.Algorithms.GraphDisplay
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
            this._MenuStrip = new System.Windows.Forms.MenuStrip();
            this._MenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this._MenuFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this._MenuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this._MenuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this._MenuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this._MenuAlgorithm = new System.Windows.Forms.ToolStripMenuItem();
            this._MenuAlgorithmBFS = new System.Windows.Forms.ToolStripMenuItem();
            this._MenuAlgorithmBFSStartStop = new System.Windows.Forms.ToolStripMenuItem();
            this.dijkstraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._MenuAlgorithmDijkstraStartStop = new System.Windows.Forms.ToolStripMenuItem();
            this._StatusStrip = new System.Windows.Forms.StatusStrip();
            this._StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this._Panel = new System.Windows.Forms.Panel();
            this._ButtonRedrawGraph = new System.Windows.Forms.Button();
            this._ListAlgorithmResults = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.autoStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._MenuStrip.SuspendLayout();
            this._StatusStrip.SuspendLayout();
            this._Panel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _MenuStrip
            // 
            this._MenuStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._MenuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._MenuFile,
            this._MenuAlgorithm});
            this._MenuStrip.Location = new System.Drawing.Point(0, 0);
            this._MenuStrip.Name = "_MenuStrip";
            this._MenuStrip.Size = new System.Drawing.Size(118, 24);
            this._MenuStrip.TabIndex = 0;
            this._MenuStrip.Text = "menuStrip1";
            // 
            // _MenuFile
            // 
            this._MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._MenuFileNew,
            this._MenuFileOpen,
            this._MenuFileSave,
            this.toolStripMenuItem1,
            this._MenuFileExit});
            this._MenuFile.Name = "_MenuFile";
            this._MenuFile.Size = new System.Drawing.Size(37, 20);
            this._MenuFile.Text = "File";
            // 
            // _MenuFileNew
            // 
            this._MenuFileNew.Name = "_MenuFileNew";
            this._MenuFileNew.Size = new System.Drawing.Size(131, 22);
            this._MenuFileNew.Text = "New";
            this._MenuFileNew.Click += new System.EventHandler(this.MenuFileNew_Click);
            // 
            // _MenuFileOpen
            // 
            this._MenuFileOpen.Name = "_MenuFileOpen";
            this._MenuFileOpen.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this._MenuFileOpen.Size = new System.Drawing.Size(131, 22);
            this._MenuFileOpen.Text = "Open...";
            this._MenuFileOpen.Click += new System.EventHandler(this.MenuFileOpen_Click);
            // 
            // _MenuFileSave
            // 
            this._MenuFileSave.Name = "_MenuFileSave";
            this._MenuFileSave.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this._MenuFileSave.Size = new System.Drawing.Size(131, 22);
            this._MenuFileSave.Text = "Save...";
            this._MenuFileSave.Click += new System.EventHandler(this.MenuFileSave_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(128, 6);
            // 
            // _MenuFileExit
            // 
            this._MenuFileExit.Name = "_MenuFileExit";
            this._MenuFileExit.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this._MenuFileExit.Size = new System.Drawing.Size(131, 22);
            this._MenuFileExit.Text = "Exit";
            this._MenuFileExit.Click += new System.EventHandler(this.MenuFileExit_Click);
            // 
            // _MenuAlgorithm
            // 
            this._MenuAlgorithm.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._MenuAlgorithmBFS,
            this.dijkstraToolStripMenuItem});
            this._MenuAlgorithm.Name = "_MenuAlgorithm";
            this._MenuAlgorithm.Size = new System.Drawing.Size(73, 20);
            this._MenuAlgorithm.Text = "Algorithm";
            // 
            // _MenuAlgorithmBFS
            // 
            this._MenuAlgorithmBFS.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._MenuAlgorithmBFSStartStop});
            this._MenuAlgorithmBFS.Name = "_MenuAlgorithmBFS";
            this._MenuAlgorithmBFS.Size = new System.Drawing.Size(171, 22);
            this._MenuAlgorithmBFS.Text = "Breath First Search";
            // 
            // _MenuAlgorithmBFSStartStop
            // 
            this._MenuAlgorithmBFSStartStop.Name = "_MenuAlgorithmBFSStartStop";
            this._MenuAlgorithmBFSStartStop.Size = new System.Drawing.Size(98, 22);
            this._MenuAlgorithmBFSStartStop.Text = "Start";
            this._MenuAlgorithmBFSStartStop.Click += new System.EventHandler(this.MenuAlgorithmStartStop_Click);
            // 
            // dijkstraToolStripMenuItem
            // 
            this.dijkstraToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._MenuAlgorithmDijkstraStartStop,
            this.autoStartToolStripMenuItem});
            this.dijkstraToolStripMenuItem.Name = "dijkstraToolStripMenuItem";
            this.dijkstraToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.dijkstraToolStripMenuItem.Text = "Dijkstra";
            // 
            // _MenuAlgorithmDijkstraStartStop
            // 
            this._MenuAlgorithmDijkstraStartStop.Name = "_MenuAlgorithmDijkstraStartStop";
            this._MenuAlgorithmDijkstraStartStop.Size = new System.Drawing.Size(152, 22);
            this._MenuAlgorithmDijkstraStartStop.Text = "Start";
            this._MenuAlgorithmDijkstraStartStop.Click += new System.EventHandler(this.MenuAlgorithmStartStop_Click);
            // 
            // _StatusStrip
            // 
            this._StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._StatusLabel});
            this._StatusStrip.Location = new System.Drawing.Point(0, 530);
            this._StatusStrip.Name = "_StatusStrip";
            this._StatusStrip.Size = new System.Drawing.Size(744, 22);
            this._StatusStrip.TabIndex = 1;
            this._StatusStrip.Text = "statusStrip1";
            // 
            // _StatusLabel
            // 
            this._StatusLabel.Name = "_StatusLabel";
            this._StatusLabel.Size = new System.Drawing.Size(42, 17);
            this._StatusLabel.Text = "Ready.";
            // 
            // _Panel
            // 
            this._Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._Panel.BackColor = System.Drawing.SystemColors.Window;
            this._Panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._Panel.Controls.Add(this._ButtonRedrawGraph);
            this._Panel.Location = new System.Drawing.Point(0, 27);
            this._Panel.Name = "_Panel";
            this._Panel.Size = new System.Drawing.Size(744, 400);
            this._Panel.TabIndex = 2;
            // 
            // _ButtonRedrawGraph
            // 
            this._ButtonRedrawGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._ButtonRedrawGraph.Location = new System.Drawing.Point(648, 372);
            this._ButtonRedrawGraph.Name = "_ButtonRedrawGraph";
            this._ButtonRedrawGraph.Size = new System.Drawing.Size(91, 23);
            this._ButtonRedrawGraph.TabIndex = 0;
            this._ButtonRedrawGraph.Text = "Redraw graph";
            this._ButtonRedrawGraph.UseVisualStyleBackColor = true;
            this._ButtonRedrawGraph.Click += new System.EventHandler(this._ButtonRedrawGraph_Click);
            // 
            // _ListAlgorithmResults
            // 
            this._ListAlgorithmResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ListAlgorithmResults.FormattingEnabled = true;
            this._ListAlgorithmResults.Location = new System.Drawing.Point(3, 16);
            this._ListAlgorithmResults.Name = "_ListAlgorithmResults";
            this._ListAlgorithmResults.Size = new System.Drawing.Size(738, 69);
            this._ListAlgorithmResults.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this._ListAlgorithmResults);
            this.groupBox1.Location = new System.Drawing.Point(0, 433);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(744, 94);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Algorithm results";
            // 
            // _OpenFileDialog
            // 
            this._OpenFileDialog.FileName = "openFileDialog1";
            // 
            // autoStartToolStripMenuItem
            // 
            this.autoStartToolStripMenuItem.CheckOnClick = true;
            this.autoStartToolStripMenuItem.Name = "autoStartToolStripMenuItem";
            this.autoStartToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.autoStartToolStripMenuItem.Text = "AutoStart";
            this.autoStartToolStripMenuItem.Click += new System.EventHandler(this.autoStartToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 552);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._Panel);
            this.Controls.Add(this._StatusStrip);
            this.Controls.Add(this._MenuStrip);
            this.MainMenuStrip = this._MenuStrip;
            this.Name = "MainForm";
            this.Text = "Graph Display and Algorithm Running";
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.VisibleChanged += new System.EventHandler(this.MainForm_VisibleChanged);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this._MenuStrip.ResumeLayout(false);
            this._MenuStrip.PerformLayout();
            this._StatusStrip.ResumeLayout(false);
            this._StatusStrip.PerformLayout();
            this._Panel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip _MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _MenuFile;
        private System.Windows.Forms.ToolStripMenuItem _MenuFileNew;
        private System.Windows.Forms.ToolStripMenuItem _MenuFileOpen;
        private System.Windows.Forms.ToolStripMenuItem _MenuFileSave;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem _MenuFileExit;
        private System.Windows.Forms.StatusStrip _StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel _StatusLabel;
        private System.Windows.Forms.Panel _Panel;
        private System.Windows.Forms.ToolStripMenuItem _MenuAlgorithm;
        private System.Windows.Forms.ToolStripMenuItem _MenuAlgorithmBFS;
        private System.Windows.Forms.ToolStripMenuItem _MenuAlgorithmBFSStartStop;
        private System.Windows.Forms.ListBox _ListAlgorithmResults;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SaveFileDialog _SaveFileDialog;
        private System.Windows.Forms.OpenFileDialog _OpenFileDialog;
        private System.Windows.Forms.ToolStripMenuItem dijkstraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _MenuAlgorithmDijkstraStartStop;
        private System.Windows.Forms.Button _ButtonRedrawGraph;
        private System.Windows.Forms.ToolStripMenuItem autoStartToolStripMenuItem;
    }
}

