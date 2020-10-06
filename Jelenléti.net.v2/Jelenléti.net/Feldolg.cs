using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Jelenléti.net
{
    public partial class Feldolg : Form
    {
        bool entered = false;
        Form InputBox = new Form();
        TextBox IBTextbox = new TextBox();
        ComboBox IBCombo = new ComboBox();
        public Feldolg()
        {
            InitializeComponent();
        }

        private void Feldolg_Load(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel1.Text  = "";
                string[] Lista = k.Feldolgozottak.Nevek.Lista();
                if (Lista.Length > 0)
                {
                    listBox1.Items.AddRange(Lista);
                }
            }
            catch (Exception exp)
            {
                k.Uzi(exp.Message + " \nKivétel törént a feldolgozottak listájának kitöltésekot.","oo","*");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {                   
                if (!k.EMm2n.m)
                {
                    k.EMm2n.ea = ef.Exapp ();
                    k.EMm2n.ewb = ef.Megnyit(k.mail2nev, k.EMm2n.ea);
                    k.EMm2n.m = true;
                }
                int vege=ef.listavege(k.EMm2n.ewb,1);
                string[] nevsor=new string[vege-2];
                for (int i = 2; i < vege; i++)
                {
                    nevsor[i-2] = ef.cella(k.EMm2n.ewb, i, 1);
                }
                IBCombo.Items.AddRange(nevsor);
                Label IBLabel = new Label();
                Button IBButton = new Button();
                Button IBButton2 = new Button();
                EventHandler IBButtonClick = new EventHandler(IBButton_Click);
                EventHandler IBButton2Click = new EventHandler(IBButton2_Click);
                InputBox.Controls.Add(IBLabel);                
                InputBox.Controls.Add(IBButton);
                InputBox.Controls.Add(IBButton2);
                InputBox.Controls.Add(IBCombo);                
                IBCombo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                IBCombo.AutoCompleteSource = AutoCompleteSource.ListItems;
                IBCombo.Sorted = true;
                InputBox.Height = 150;
                InputBox.Width = 350;
                InputBox.Text = "Új név hozzáadása";
                InputBox.BackColor = System.Drawing.Color.White;
                InputBox.FormBorderStyle = FormBorderStyle.FixedSingle;
                IBLabel.AutoSize = true;
                IBLabel.Top = 22;
                IBLabel.Left = 21;                
                IBButton.Top = 81;
                IBButton.Left = 241;
                IBButton.Height = 23;
                IBButton.Width = 75;
                IBButton2.Top = 81;
                IBButton2.Left = 241 - 80;
                IBButton2.Height = 23;
                IBButton2.Width = 75;
                IBButton2.Text = "Be&zár";
                IBButton.Text = "Hozzá&ad";
                IBButton.Click += IBButtonClick;
                IBButton2.Click += IBButton2Click;
                IBLabel.Text = "Adja meg az új nevet!";
                InputBox.AcceptButton = IBButton;
                InputBox.CancelButton = IBButton2;
                IBCombo.Left = 21;
                IBCombo.Top = 48;
                IBCombo.Width = 293;                
                InputBox.ShowDialog();
                IBCombo.Focus();
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:Feldolgott() Button1_Click"));
            }
        }
            
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void IBButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (k.GetParams(IBCombo.Text).nev == "")
                {
                    if (k.Uzi("A név: " + IBCombo.Text + " nem található a nevek adatbázisában.\nBiztosan hozzáadja a feldolgozottak listájához?", "yn", "?") == DialogResult.Yes)
                    {
                        listBox1.Items.Add(IBCombo.Text);
                        listBox1.SelectedIndex=listBox1.FindStringExact(IBCombo.Text);
                    }
                }
                else
                {
                    listBox1.Items.Add(IBCombo.Text);
                    listBox1.SelectedIndex=listBox1.FindStringExact(IBCombo.Text);
                }
                
                IBCombo.Text = "";
                InputBox.Close();
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:Feldolgott() IBButton_Click"));
            }
        }
        private void IBButton2_Click(object sender, EventArgs e)
        {
            try
            {
                IBCombo.Text = "";
                InputBox.Close();
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:Feldolgott() IBButton2_Click"));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex > -1)
                {
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                }
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:Feldolgott() button2_click"));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (k.Uzi("Biztosan törli a feldolgozottak listáját?\n(A legutolsó listát mindig visszaállthatja a:\n" + k.feldpath + "feldolg.bak\nhelyről)", "yn", "?") == DialogResult.Yes)
                {
                    listBox1.Items.Clear();
                }
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:Feldolgott() button3_Click"));
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.Items.Count < 1)
                {
                    k.Feldolgozottak.fajl.Torles();
                }
                else
                {
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        if (i == 0)
                        {
                            k.Feldolgozottak.fajl.Torles();
                            ff.SorIras(listBox1.Items[i].ToString(), k.feldpath + "feldolg.txt", "o");
                        }
                        else ff.SorIras(listBox1.Items[i].ToString(), k.feldpath + "feldolg.txt", "a");
                    }
                }
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:Feldolgott() button5_Click"));
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Feldolg_Load(null, null);
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex > -1)
                {
                    IBTextbox.KeyDown += new KeyEventHandler(IBTextbox_KeyDown);
                    IBTextbox.KeyPress += new KeyPressEventHandler(IBTextbox_KeyPress);
                    IBTextbox.LostFocus += new EventHandler(IBTextbox_LostFocus);
                    entered = false;
                    IBTextbox.BorderStyle = BorderStyle.None;
                    IBTextbox.Left = 0;
                    IBTextbox.Top = listBox1.ItemHeight * listBox1.SelectedIndex;
                    IBTextbox.Width = listBox1.Width;
                    IBTextbox.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
                    listBox1.Controls.Add(IBTextbox);
                    IBTextbox.Focus();
                    IBTextbox.Select(0, IBTextbox.Text.Length);
                }
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:listBox1_MouseDoubleClick"));
            }
        }

        void IBTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        void IBTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.KeyCode == Keys.Enter) && !entered)
                {
                    listBox1.Items[listBox1.SelectedIndex] = IBTextbox.Text;
                    entered = true;
                    IBTextbox.Text = "";
                    listBox1.Controls.Remove(IBTextbox);
                }
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:IBTextbox_KeyDown"));
            }
        }

        void IBTextbox_LostFocus(object sender, EventArgs e)
        {
            IBTextbox.Text = "";
            listBox1.Controls.Remove(IBTextbox);
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex > -1)
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        IBTextbox.KeyDown += new KeyEventHandler(IBTextbox_KeyDown);
                        IBTextbox.KeyPress += new KeyPressEventHandler(IBTextbox_KeyPress);
                        IBTextbox.LostFocus += new EventHandler(IBTextbox_LostFocus);
                        entered = false;
                        IBTextbox.BorderStyle = BorderStyle.None;
                        IBTextbox.Left = 0;
                        IBTextbox.Top = listBox1.ItemHeight * listBox1.SelectedIndex;
                        IBTextbox.Width = listBox1.Width;
                        IBTextbox.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
                        listBox1.Controls.Add(IBTextbox);
                        IBTextbox.Focus();
                        IBTextbox.Select(0, IBTextbox.Text.Length);
                    }
                    if (e.KeyCode == Keys.Delete)
                    {
                        listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                    }
                }
                if (e.KeyCode == Keys.Insert)
                {
                    button1_Click(null, null);
                }
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:listBox1_KeyDown"));
            }
        }

        private void listBox1_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Megmutatja az eddig beérkezett és feldolgozott nevek listáját";
        }

        private void listBox1_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Hozzáadhat egy nevet (Insert)";
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Letörli a kijelölt nevet (Del)";
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Törli a teljes listát (Alt+L)";
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button6_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Frissíti a listát (újra olvassa) (Alt+R)";
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "A változtatások csak mentés után érvényesülnek (Alt+S)";
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Bezárja az ablakot (Esc)";
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }
    }
}