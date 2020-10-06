using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Amoba
{
    public partial class GameForm : Form
    {
        int buttonsize = 30;
        string buttontext;
        string[,] mirror;
        int step = 0;
        bool vege = false;
        public GameForm()
        {
            InitializeComponent();
        }


        private void táblaméretToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm mySettings = new SettingsForm();
            DialogResult myResult = mySettings.ShowDialog();
            if (myResult == DialogResult.OK)
            {
                myResult = MessageBox.Show("Kezdjünk új játékot a megváltozott beállításokkal?", "A beállítások megváltoztak", MessageBoxButtons.YesNo);
                if (myResult == DialogResult.Yes)
                {
                    DrawTable();
                }
            }
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            DrawTable();
            buttontext = "O";
            this.Text = "Amőba - " + buttontext + " kezd";
            //this.kovetkezoToolStripMenuItem.Text = buttontext + " kezd";
        }

        private void újJátékToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult myResult;

            myResult = MessageBox.Show("Kezdjünk új játékot?", "Új játék", MessageBoxButtons.YesNo);
            if (myResult == DialogResult.Yes)
            {
                DrawTable();
            }
        }

        private void DrawTable()
        {
            this.buttonPanel.Enabled = true;
            for (int c = this.buttonPanel.Controls.Count; c > 0; c--)
            {
                Control myControl = this.buttonPanel.Controls[c - 1];
                if (myControl is Button)
                {
                    myControl.Hide();
                    myControl.Dispose();
                }
            }
            for (int i = 0; i < Program.columns; i++)
            {
                for (int j = 0; j < Program.rows; j++)
                {
                    Button myButton = new Button();
                    myButton.Size = new Size(buttonsize, buttonsize);
                    myButton.Left = i * buttonsize + 40;
                    myButton.Top = j * buttonsize + 40;
                    myButton.Show();
                    myButton.Name = "button_" + i + "_" + j;
                    myButton.Click += new EventHandler(myButton_Click);
                    myButton.Disposed += new EventHandler(myButton_Disposed);
                    this.buttonPanel.Controls.Add(myButton);
                    if (i == Program.columns - 1 && j == Program.rows - 1)
                    {
                        this.Width = myButton.Right + 60;
                        this.Height = myButton.Bottom + menuStrip.Height + 60;
                    }
                }
            }            
            mirror = new string[Program.columns, Program.rows];
            step = 0;
            vege = false;
        }

        void myButton_Disposed(object sender, EventArgs e)
        {
            Button myButton = sender as Button;
            if (myButton != null)
            {
                myButton.Click -= new EventHandler(myButton_Click);
            }
        }

        void myButton_Click(object sender, EventArgs e)
        {
            Button myButton = sender as Button;
            if (myButton != null)
            {
                if (myButton.Text == string.Empty)
                {
                    myButton.Text = buttontext;
                    string[] nevtomb = myButton.Name.Split(new char[] { '_' });
                    SetMirror(nevtomb);
                    Changetext();
                    step++;
                }
                else
                {
                    MessageBox.Show("A mező már foglalt");
                }
            }
        }
        void Changetext()
        {
            if (buttontext == "O")
            {
                buttontext = "X";
            }
            else
            {
                buttontext = "O";
            }
            if (!vege)
            {
                //this.Text = "Amőba - " + buttontext + " következik";
                this.kovetkezoToolStripMenuItem.Text = buttontext + " következik";
            }
            else
            {
                this.kovetkezoToolStripMenuItem.Text = buttontext + " kezd";
                //this.Text = "Amőba - " + buttontext + " kezd";
            }
        }
        void SetMirror(string[] nevtomb)
        {
            int x = int.Parse(nevtomb[1]);
            int y = int.Parse(nevtomb[2]);
            mirror[x, y] = buttontext;
            CheckMirror(x, y);
        }

        void CheckMirror(int x, int y)
        {
            int[,] toe = new int[2, 2];

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (!(i == 0 && j == 0))
                    {
                        if (x + i >= 0 && y + j >= 0 && x + i < Program.columns && y + j < Program.rows)
                        {
                            int testX = x;
                            int testY = y;
                            while (SameInDir(testX, testY, i, j))
                            {
                                testX = testX + i;
                                testY = testY + j;
                                //A logigákan az volt a hiba, hogy ha volt egy azonos jel -1,1, egy 1,-1, egy 1,1 és még egy -1,-1 felé, akkor volt egy szép X, de az abszolút értékek miatt a program 5db-nak számolta
                                //Mivel a [0,0] nem kerül felhasználásra, az ellentétes előjelű koordinátákra fektethető egyenesen lévő azonos jelek száma ebbe a rekeszbe kerül.
                                if ((i != j) && (Math.Abs(i) == Math.Abs(j)))
                                {
                                    toe[0, 0] = toe[0, 0] + 1;
                                }
                                else
                                {
                                    toe[Math.Abs(i), Math.Abs(j)] = toe[Math.Abs(i), Math.Abs(j)] + 1;

                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < 2 && !vege; i++)
            {
                for (int j = 0; j < 2 && !vege; j++)
                {
                    if (toe[i, j] >= 4)
                    {
                        MessageBox.Show(buttontext + " nyert!");
                        this.buttonPanel.Enabled = false;
                        vege = true;
                        string[] buttonNames = GetButtonNames(x, y, i, j, toe[i, j]);
                        HighlightButtons(buttonNames);
                    }
                }
            }
            if (step == Program.columns * Program.rows - 1 && !vege)
            {
                MessageBox.Show("Nincs több hely a táblán.\nDöntetlen!");
                this.buttonPanel.Enabled = false;
            }
        }

        private void HighlightButtons(string[] buttonNames)
        {
            foreach (Control c in buttonPanel.Controls)
            {
                foreach (string s in buttonNames)
                {
                    if (c is Button)
                    {
                        if (c.Name == s)
                        {
                            c.BackColor = Color.Blue;
                        }
                    }
                }
            }
        }

        private string[] GetButtonNames(int x, int y, int i, int j, int count)
        {
            if (i == 0 && j == 0)
            {
                i = 1;
                j = -1;
            }
            string[] mystring = new string[count + 1];
            int testX = x;
            int testY = y;
            int k=0;
            mystring[k] = "button_" + testX + "_" + testY;
            k++;
            while (SameInDir(testX, testY, i, j))
            {                
                testX = testX + i;
                testY = testY + j;
                mystring[k] = "button_" + testX + "_" + testY;
                k++;
            }
            testX = x;
            testY = y;
            while (SameInDir(testX, testY, -i, -j))
            {                
                testX = testX - i;
                testY = testY - j;
                mystring[k] = "button_" + testX + "_" + testY;
                k++;
            }
            return mystring;
        }
        
        bool SameInDir(int StartX, int StartY, int DirX, int DirY)
        {
            return StartX + DirX >= 0 && StartY + DirY >= 0 && StartX + DirX < Program.columns && StartY + DirY < Program.rows && mirror[StartX + DirX, StartY + DirY] == buttontext;
        }
    }
}
