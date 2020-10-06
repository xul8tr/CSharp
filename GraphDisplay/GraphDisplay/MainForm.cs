using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ELTE.Algorithms.GraphDisplay.DataStructure;
using ELTE.Algorithms.GraphDisplay.Algorithm;


namespace ELTE.Algorithms.GraphDisplay
{
    public partial class MainForm : Form
    {
        private readonly Graph _Graph; // gráf
        private GraphAlgorithm _Algorithm; // gráf algoritmus

        private readonly Timer _Timer; // időzítő az algoritmus futtatáshoz
        private Point _MouseLocation; // egérpozíció elmentése az él rajzoláshoz
        private Bitmap _DrawBitmap; // kép, amire rajzolunk
        private ToolStripMenuItem _SelectedMenuItem; // a menu item, ami alapján elindítjuk az elemzést
        private bool _GraphChanged; //változtattunk valamit a gráfon?
        private bool _AutoStart; //start looking for path automatically as soon as the second node is selected
        public MainForm()
        {
            InitializeComponent();

            _Graph = new Graph(); // létrehozzuk a gráfot
            _DrawBitmap = new Bitmap(_Panel.Width, _Panel.Height); // létrehozzuk a képet, amire rajzolunk
            _MouseLocation = Point.Empty;
            _GraphChanged = false;

            _Timer = new Timer(); // létrehozzuk  az időzítőt
            _Timer.Interval = 3000; // 3 másodpercenként fog eseményt kiváltani
            _Timer.Tick += Timer_Tick; // időzített esemény eseménykezelő társítása

            // panel egérmozgatásának eseménykezelése:
            _Panel.MouseDown += Panel_MouseDown;
            _Panel.MouseMove += Panel_MouseMove;
            _Panel.MouseUp += Panel_MouseUp;

            _ListAlgorithmResults.MouseWheel += MainForm_MouseWheel; 
            // teljes ablak eseménykezelése:
            // MouseWheel += MainForm_MouseWheel;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool retVal = true;
            switch (keyData)
            {
                case Keys.Delete: // Delete gomb a törlés
                    if (_Graph.NodeSelected) // ha van csúcs kiválasztva
                    {
                        _Graph.RemoveNode();
                        _StatusLabel.Text = "Node and connected arcs removed.";
                        _GraphChanged = true;
                    }
                    else if (_Graph.ArcSelected) // ha van él kiválasztva
                    {
                        _Graph.RemoveArc();
                        _StatusLabel.Text = "Arc removed.";
                        _GraphChanged = true;
                    }
                    else // különben nincs mit törölni
                        _StatusLabel.Text = "Cannot delete, nothing selected.";
                    break;
                case Keys.Up: // ha felfelé gombot nyomunk, akkor növeljük eggyel az élsúlyt
                    if (_Graph.ArcSelected) // már ha van él kiválasztva
                    {
                        _Graph.ModifyArc(1);
                        _StatusLabel.Text = "Arc weight increased by 1.";
                        _GraphChanged = true;
                    }
                    break;
                case Keys.Down: // ha lefelé gombot nyomunk, csökkentjük az élsúlyt
                    if (_Graph.ArcSelected)
                    {
                        _Graph.ModifyArc(-1);
                        _StatusLabel.Text = "Arc weight decreased by 1.";
                        _GraphChanged = true;
                    }
                    break;
                default:
                    retVal = base.ProcessCmdKey(ref msg, keyData);
                    break;
            }
            DrawGraph();
            return retVal;
        }

        protected override void OnClosing(CancelEventArgs e)
            // felülírjuk a bezáró eseménykezelőt, hogy megkérdezhessük a felhasználót, tényleg be akarja-e zárni a programot
        {
            if (_GraphChanged && MessageBox.Show("Are you sure, you wish to exit the program?" + Environment.NewLine + "All unsaved changes will be lost.", "Exit Program", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                // ha nemmel válaszol, töröljük az eseményt
                e.Cancel = true;

            base.OnClosing(e);
        }

        private void MenuFileNew_Click(object sender, EventArgs e)
        {
            if (!_GraphChanged ||
                MessageBox.Show(
                    "Are you sure, you wish to clear the graph?" + Environment.NewLine +
                    "All unsaved changes will be lost.", "Clear Graph", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes)
                // előbb megkérdezzük, hogy biztosan törölni akarja-e a gráfot
            {
                _Graph.Clear();
                _GraphChanged = false;
                DrawGraph();
            }
        }

        private void MenuFileOpen_Click(object sender, EventArgs e)
        {
            _OpenFileDialog.Filter = "Graph Files (*.grf)|*.grf";
            if (!_GraphChanged ||
                MessageBox.Show(
                    "Are you sure, you wish to load a graph?" + Environment.NewLine +
                    "All unsaved changes will be lost.", "Open", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes)
            {
                if (_OpenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (_Graph.Load(_OpenFileDialog.FileName))
                    {
                        _StatusLabel.Text = "Graph loaded from " + _OpenFileDialog.FileName + ".";
                    }

                    else
                        _StatusLabel.Text = "Failed to open " + _OpenFileDialog.FileName + ".";
                    DrawGraph();
                }
            }
        }
        private void MenuFileSave_Click(object sender, EventArgs e)
        {
            _SaveFileDialog.Filter = "Graph Files (*.grf)|*.grf";
            if (_SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (_Graph.Save(_SaveFileDialog.FileName))
                {
                    _StatusLabel.Text = "Graph saved as " + _SaveFileDialog.FileName + ".";
                    _GraphChanged = false;
                }
                else
                    _StatusLabel.Text = "Failed to save " + _SaveFileDialog.FileName + ".";
            }
        }
        private void MenuFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MenuAlgorithmStartStop_Click(object sender, EventArgs e)
        {
            _SelectedMenuItem = sender as ToolStripMenuItem;
            if (_SelectedMenuItem != null)
            {
                if (_SelectedMenuItem.Name.Contains("Dijkstra"))
                {
                    _Algorithm = new DijkstraAlgorithm(_Graph);
                }
                else if (_SelectedMenuItem.Name.Contains("BFS"))
                {
                    _Algorithm = new BreathFirstSearchAlgorithm(_Graph);
                }
            }
            StartAlogorithm();

        }

        private void StartAlogorithm()
        {
            if (_SelectedMenuItem != null)
            {
                if (_SelectedMenuItem.Text == "Start")
                {
                    _SelectedMenuItem.Text = "Stop";
                    _ListAlgorithmResults.Items.Clear(); // listbox eleminek törlése
                    _ListAlgorithmResults.Items.Insert(0, "Algorithm started."); // beszúrás a listbox tetejére
                    _Timer.Start(); // időzítő indítása
                }
                else
                {
                    _SelectedMenuItem.Text = "Start";
                    _ListAlgorithmResults.Items.Insert(0, "Algorithm stopped.");
                    _Timer.Stop(); // időzítő leállítása
                    _Algorithm = null;
                }
            }
            else
            {
                _ListAlgorithmResults.Items.Insert(0, "Error! No Menu selected, but the algorithm is begin to start.");
            }
        }

        private void Timer_Tick(object sender, EventArgs e) // időzített esemény
        {
            if (_Algorithm != null) // ha van algoritmus
            {
                if (!_Algorithm.Running) // és még nem fut
                {
                    _Algorithm.Start(); // akkor elindítjuk
                    if (_Algorithm.Running) // ha sikerült elindítani
                    {
                        _ListAlgorithmResults.Items.Insert(0, _Algorithm.Status);
                        // a listára felvesszük az algoritmus státuszát
                        _StatusLabel.Text = "Algorithm running...";
                    }
                    else // ha nem sikerült elindítani
                    {
                        _ListAlgorithmResults.Items.Insert(0, "Algorithm finished.");
                        _SelectedMenuItem.Text = "Start";
                        _StatusLabel.Text = "Algorithm stopped.";
                        _Timer.Interval = 3000;
                        _Timer.Stop(); // leállítjuk az időzítőt
                        _Algorithm = null;
                    }
                    DrawGraph();
                }
                else // ha már fut
                {
                    _Algorithm.Step(); // léptetjük
                    _ListAlgorithmResults.Items.Insert(0, _Algorithm.Status);
                    if (!_Algorithm.Running) // ha befejeződött
                    {
                        _ListAlgorithmResults.Items.Insert(0, "Algorithm finished.");
                        _SelectedMenuItem.Text = "Start";
                        _StatusLabel.Text = "Algorithm stopped.";
                        _Timer.Interval = 3000;
                        _Timer.Stop();
                        //nem rajzolom ki a gráfot, hogy az algoritmus futásának eredménye megmaradjon.
                        _ButtonRedrawGraph.Visible = true;
                    }
                    else
                    {
                        DrawGraph();
                    }
                }
            }
        }
        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            // ha mozgatjuk az egérgörgőt, akkor a mozgatás irányával arányosan változtatjuk az élsúlyt
            _Graph.ModifyArc(Math.Sign(e.Delta));
            _GraphChanged = true;
            DrawGraph();
        }

        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            // amikor lenyomjuk az egérgombot
            switch (e.Button)
            {
                case MouseButtons.Left: // ha bal gomb, akkor kiválasztás
                    _Graph.SelectNode(e.Location); // megpróbálunk csúcsot kiválasztani
                    if (!_Graph.NodeSelected) // ha ez nem sikerül
                    {
                        _Graph.SelectArc(e.Location); // megpróbálunk élet kiválasztani
                    }
                    else
                    {
                        if (_Graph.PreviousNode !=null && _AutoStart)
                        {
                            _Timer.Interval = 10;
                            _Algorithm = _Algorithm = new DijkstraAlgorithm(_Graph);
                            _SelectedMenuItem = _MenuAlgorithmDijkstraStartStop;
                            StartAlogorithm();
                        }
                    }
                    break;
                case MouseButtons.Right: // ha jobb gomb, akkor létrehozás
                    _Graph.SelectNode(e.Location);
                    if (_Graph.NodeSelected) // ha egy csúcson vagyunk, akkor élet hozunk létre
                    {
                        _MouseLocation = e.Location;
                    }
                    else // különben új csúcsot hozunk létre
                    {
                        _Graph.AddNode(e.Location);
                        _GraphChanged = true;
                    }
                    break;
            }
            DrawGraph();
        }
        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            // ha mozgatjuk az egeret, akkor nem mindegy, melyik gomb van lenyomva közben
            switch (e.Button)
            {
                case MouseButtons.Left: // ha bal gomb, akkor csúcsot mozgatunk
                    if (_Graph.NodeSelected) // feltéve, hogy van csúcs kijelölve
                    {
                        _Graph.MoveNode(e.Location);
                        _GraphChanged = true;
                        DrawGraph();
                    }
                    break;
                case MouseButtons.Right: // ha jobb gomb, akkor az élet húzzuk
                    if (_MouseLocation != Point.Empty) // ha már van kezdőpont kijelölve az élnek
                    {
                        DrawGraph();

                        Pen drawPen = new Pen(Color.Green, 2);
                        _Panel.CreateGraphics().DrawLine(drawPen, _MouseLocation, e.Location);
                        // az új élet zölddel rárajzoljuk a panelra
                    }
                    break;
            }
        }
        private void Panel_MouseUp(object sender, MouseEventArgs e)
        {
            // ha felengedtük az egeret, és volt kezdőpont kijelölve, akkor élrajzolás van
            if (_MouseLocation != Point.Empty)
            {
                _Graph.AddArc(_MouseLocation, e.Location, 1);
                // felvesszük az új élet a gráfon
                _MouseLocation = Point.Empty;
                _GraphChanged = true;
                DrawGraph();
            }
        }

        private void DrawGraph() // gráf, kirajzolása
        {
            // létrehozzuk a rajzobjektumot a képre
            Graphics gr = Graphics.FromImage(_DrawBitmap);
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gr.FillRectangle(Brushes.White, 0, 0, _DrawBitmap.Width, _DrawBitmap.Height);
            // kitöltjük fehérrel a hátteret

            // kirajzoljuk vagy az algoritmust, vagy a gráfot:
            if (_Algorithm != null && _Algorithm.Running)
                _Algorithm.Draw(gr);
            else
            {
                _ButtonRedrawGraph.Visible = false;
                _Graph.Draw(gr);
            }

            // végül kirajzoljuk a képet a panelra:
            _Panel.CreateGraphics().DrawImage(_DrawBitmap, 0, 0);
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            //mozgatás után eltűnik a kép, ha kívülre kerül a képernyőn, majd vissza
            DrawGraph();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //átméretezés után létre kell hozni az új bitmapot a megváltozott méretekkel
            if (_Panel.Width > 0 && _Panel.Height > 0)
            {
                if (_DrawBitmap != null)
                {
                    _DrawBitmap.Dispose();
                }
                _DrawBitmap = new Bitmap(_Panel.Width, _Panel.Height);
            }
            DrawGraph();
        }

        private void MainForm_VisibleChanged(object sender, EventArgs e)
        {
            DrawGraph();
        }

        private void _ButtonRedrawGraph_Click(object sender, EventArgs e)
        {
            DrawGraph();
            _ButtonRedrawGraph.Visible = false;
        }

        private void autoStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _AutoStart = autoStartToolStripMenuItem.Checked;
        }
    }
}
