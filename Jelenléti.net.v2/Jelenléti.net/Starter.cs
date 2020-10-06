using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Jelenléti.net
{
    public partial class Starter : Form
    {
        Form FormList;
        bool FormListOpen;
        public Starter()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Form pi = new Progress();
			try
            {				
				//pi.Show();
				//((ProgressBar)pi.Controls.Find("progressBar1", false)[0]).Value = 10;
				//((Label)pi.Controls.Find("label1", false)[0]).Text = "Betöltés";				
                label1.Text = "Jelenlegi felhasználó: " + System.Environment.UserName + "\nKérem válasszon funkciót!";
                toolStripStatusLabel2.Text = "";
				//((ProgressBar)pi.Controls.Find("progressBar1", false)[0]).Value = 20;
            }
            catch (Exception exc)
            {
                k.Uzi(exc.Message + "\nHely:Starter Form_Load, label1.text beállításakor.", "oo", "x");
            }
            string utvonal =Application.StartupPath + "\\settings.set";
            string g = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
            try
            {
				do
				{
					//((ProgressBar)pi.Controls.Find("progressBar1", false)[0]).Value = 30;
					if (!k.Letezik(utvonal, "F"))
					{
						Form beallitasok = new Beallitasok();
						beallitasok.ShowDialog();
					}
				}
				while (!k.Letezik(utvonal, "F"));
                ff.AdottSorIras(g, utvonal, 6, false);
				//((ProgressBar)pi.Controls.Find("progressBar1", false)[0]).Value = 50;
            }
            catch (Exception exc)
            {
                k.Uzi(exc.Message + "\nHely:Starter Form_Load, Settings.set ellenorzesekor.", "oo", "x");
            }
            try
            {
                k.Init();
				//((ProgressBar)pi.Controls.Find("progressBar1", false)[0]).Value = 70;

            }
            catch (Exception exc)
            {
                k.Uzi(exc.Message + "\nHely:Starter Form_Load Init hívásakor.", "oo", "x");
            }
            try
            {
				//((ProgressBar)pi.Controls.Find("progressBar1", false)[0]).Value = 90;
                if (k.Letezik(k.feldpath + "feldolg.txt", "F") && ff.Hanysor(k.feldpath + "feldolg.txt") > 0)
                {
					//((ProgressBar)pi.Controls.Find("progressBar1", false)[0]).Value = 100;					
					k.Uzi("Összesen " + ff.Hanysor(k.feldpath + "feldolg.txt") + " nev került eddig feldolgozásra.\nHa ezek a nevek az előző ciklusból maradtak akkor válassza\na \"feldolgozottak törlése\" menüpontot a nevek eltávolításához!", "oo", "i");
					//pi.Close();
					//pi.Dispose();
					//pi = null;
                }
                else
                {
					//((ProgressBar)pi.Controls.Find("progressBar1", false)[0]).Value = 100;					
                    k.Uzi("Egy név sincs még feldolgozva!", "oo", "i");
					//pi.Close();
					//pi.Dispose();
					//pi = null;
                }
				
            }
            catch (Exception exc)
            {
                k.Uzi(exc.Message + "\nHely:Starter Form_Load feldolg.txt lekérdezésekor.", "oo", "x");
            }
        }

		private void button1_Click(object sender, EventArgs e)
		{
			int i;
			try
			{
				ListBox ListBox1 = new ListBox();
				FormList = new Form();
				string svalasz;
				Button Button01 = new Button();
				FormList.StartPosition = FormStartPosition.Manual;
				FormList.Top = this.Top;
				if (this.Left + this.Width + FormList.Width < Screen.PrimaryScreen.WorkingArea.Right)
				{
					FormList.Left = this.Left + this.Width;
				}
				else
				{
					if (this.Left - FormList.Width > Screen.PrimaryScreen.WorkingArea.Left)
					{
						FormList.Left = this.Left - FormList.Width;
					}
				}
				ListBox1.Width = FormList.Width - 50;
				ListBox1.Height = FormList.Height - 70;
				ListBox1.Top = 1;
				ListBox1.Left = 20;
				Button01.Text = "Bezár";
				Button01.Top = ListBox1.Top + ListBox1.Height + 10;
				Button01.Left = ListBox1.Left + ListBox1.Width / 2 - Button01.Width / 2;
				FormList.Controls.Add(Button01);
				FormList.Controls.Add(ListBox1);
				FormList.CancelButton = Button01;
				FormList.FormClosing += new FormClosingEventHandler(FormList_FormClosing);
				FormList.Load += new EventHandler(FormList_Load);
				Button01.Click += new EventHandler(Button01_Click);
				FormList.Text = "Folyamatlista";
				FormList.Show();
				Outlook.MAPIFolder oInbox = olf.Inbox();
				Outlook.Items oItems = olf.Itemek(oInbox);
				int levelszam = oItems.Count;
				Outlook.MailItem oMsg = olf.Elsolevel(oItems);
				for (i = 1; i <= levelszam; i++)
				{
					if (oMsg.Subject == k.tema)
					{
						Application.DoEvents();
						if (oMsg.Attachments.Count > 0) oMsg.Attachments[1].SaveAsFile(@"C:\Temp.xls");
						if (k.Feldolgozas(oMsg.SenderName, @"C:\Temp.xls", this)) svalasz = " feldolgozva!";
						else svalasz = " nincs feldolgozva!";
						ListBox1.Items.Add(oMsg.SenderName + " " + svalasz);
					}
					oMsg = olf.Kovetkezolevel(oItems);
				}
			}
			catch (Exception exp)
			{
				k.Uzi(exp.Message + " \nHely:Starter button1_Click MAPI tesztelésekor", "oo", "x");
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Form ekez = new Excelkez();
			try
			{
				ekez.ShowDialog();
			}
			catch (Exception exp)
			{
				k.Uzi(exp.Message + " \nHely:Starter button2_Click", "oo", "x");
				ekez.Parent.Show();
			}
		}


		private void button3_Click(object sender, EventArgs e)
		{
			try
			{
				Form feldolg = new Feldolg();
				feldolg.ShowDialog();
			}
			catch (Exception exp)
			{
				k.Uzi(exp.Message + " \nHely:Starter button1_Click MAPI tesztelésekor", "oo", "x");
			}

		}

		private void button4_Click(object sender, EventArgs e)
		{
			try
			{
				Form beallitasok = new Beallitasok();
				beallitasok.ShowDialog();
			}
			catch (Exception exc)
			{
				k.Uzi(exc.Message + "\nHely:Starter button4_click.", "oo", "x");
			}
		}

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
            
        }

        private void Starter_FormClosing(object sender, FormClosingEventArgs e)
        {
			//Form pi = new Progress();
			if (k.Uzi("Biztosan kilép?", "yn", "?") == DialogResult.Yes)				
                try
                {
					//pi.Show((System.Windows.Forms.IWin32Window)this);
					//((Label)pi.Controls.Find("label1", false)[0]).Text = "Kilépés";
					//((ProgressBar)pi.Controls.Find("progressBar1", false)[0]).Value = 0;
                    {
                        
						if (k.EMjelen.m)
                        {
							//((Label)pi.Controls.Find("label1", false)[0]).Text = "Jelenléti lap bezárása";
                            k.EMjelen.ewb.Close(false, Missing.Value, Missing.Value);
                            k.EMjelen.ea.Quit();
                        }
						//((ProgressBar)pi.Controls.Find("progressBar1", false)[0]).Value = 20;
                        if (k.EMm2n.m)
                        {
							//((Label)pi.Controls.Find("label1", false)[0]).Text = "Nevek adatbázisa bezárása";
                            k.EMm2n.ewb.Close(false, Missing.Value, Missing.Value);
                            k.EMm2n.ea.Quit();
                        }
						//((ProgressBar)pi.Controls.Find("progressBar1", false)[0]).Value = 40;
                        if (k.EMmunka.m)
                        {
							//((Label)pi.Controls.Find("label1", false)[0]).Text = "Hétvégi munkanapok bezárása";
							k.EMmunka.ewb.Close(false, Missing.Value, Missing.Value);
                            k.EMmunka.ea.Quit();
                        }
						//((ProgressBar)pi.Controls.Find("progressBar1", false)[0]).Value = 60;
                        if (k.EMszabi.m)
                        {
							//((Label)pi.Controls.Find("label1", false)[0]).Text = "Szabadságos munkafüzet bezárása";
                            k.EMszabi.ewb.Close(false, Missing.Value, Missing.Value);
                            k.EMszabi.ea.Quit();
                        }
						//((ProgressBar)pi.Controls.Find("progressBar1", false)[0]).Value = 80;
                        if (k.EMunn.m)
                        {
							//((Label)pi.Controls.Find("label1", false)[0]).Text = "Hétközi szabadnapok bezárása";
							k.EMunn.ewb.Close(false, Missing.Value, Missing.Value);
                            k.EMunn.ea.Quit();
                        }
						//((ProgressBar)pi.Controls.Find("progressBar1", false)[0]).Value = 100;
						//pi.Hide();
						//((ProgressBar)pi.Controls.Find("progressBar1", false)[0]).Value = 0;
						//pi.Dispose();
						
						
						
						Application.Exit();
                    }
					
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Hiba a kilépés során: " + exp.Message, "Jelenléti", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            else e.Cancel = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            toolStripStatusLabel1.Text = DateTime.Now.ToString("yyyy.MM.dd");
            toolStripStatusLabel3.Text = DateTime.Now.ToString("HH:mm:ss");
            
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "Lekérdezi az Outlook Inbox mappát.";
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "";
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "Üzenetet küld, a beállítások teszteleléséhez.";
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "";
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "Lehetővé teszi a feldolgozási eredmények kezelését.";
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "";
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "Útvonalak és egyéb beállítások kezelése.";
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "";
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "Bezárja az alkalmazást.";
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "";
        }
        private void button2_MouseEnter_1(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "Adatbázisok karbantarása";
        }

        private void button2_MouseLeave_1(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "";
        }     


        void FormList_Load(object sender, EventArgs e)
        {
            FormListOpen = true;
        }

        void FormList_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormListOpen = false;
        }

        void Button01_Click(object sender, EventArgs e)
        {
            FormList.Close();
        }

        private void Starter_Activated(object sender, EventArgs e)
        {
            if ((k.EMjelen.m) && (k.EMjelen.ea.ActiveSheet == null)) k.EMjelen.m = false;
            if ((k.EMm2n.m) && (k.EMm2n.ea.ActiveSheet == null)) k.EMm2n.m = false;
            if ((k.EMmunka.m) && (k.EMmunka.ea.ActiveSheet == null)) k.EMmunka.m = false;
            if ((k.EMszabi.m) && (k.EMszabi.ea.ActiveSheet == null)) k.EMszabi.m = false;
            if ((k.EMunn.m) && (k.EMunn.ea.ActiveSheet == null)) k.EMunn.m = false;
        }

        private void Starter_Move(object sender, EventArgs e)
        {
            if (FormListOpen)
            {
                FormList.Top = this.Top;
                if (this.Left + this.Width + FormList.Width < Screen.PrimaryScreen.WorkingArea.Right)
                {
                    FormList.Left = this.Left + this.Width;
                }
                else
                {
                    if (this.Left - FormList.Width > Screen.PrimaryScreen.WorkingArea.Left)
                    {
                        FormList.Left = this.Left - FormList.Width;
                    }
                }
            }
        }          
    }    
}