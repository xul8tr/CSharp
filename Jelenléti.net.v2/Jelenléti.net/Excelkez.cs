using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Jelenléti.net
{
    
    public partial class Excelkez : Form
    {        
        private void Excelkez_Activated(object sender, EventArgs e)
        {
			try
			{
				initbuttex();
			}
			catch (Exception exp)
			{

				throw (new Exception(exp.Message + "\nHely:Exckez Exckez_Activated"));
				this.Close();
			}
        }
        
		public Excelkez()
        {
            InitializeComponent();
        }        

        

        private void Excelkez_Load(object sender, EventArgs e)
        {
			try
			{
				initbuttex();
				toolStripStatusLabel1.Text = "";
			}
			catch (Exception exp)
			{
				
				throw (new Exception(exp.Message + "\nHely:Exckez Exckez_Load"));
				this.Close();
			}


        }
		private void initbuttex()
		{
			try
			{
				if ((k.EMjelen.m) && (k.EMjelen.ea.ActiveSheet == null)) k.EMjelen.m = false;
				if ((k.EMm2n.m) && (k.EMm2n.ea.ActiveSheet == null)) k.EMm2n.m = false;
				if ((k.EMmunka.m) && (k.EMmunka.ea.ActiveSheet == null)) k.EMmunka.m = false;
				if ((k.EMszabi.m) && (k.EMszabi.ea.ActiveSheet == null)) k.EMszabi.m = false;
				if ((k.EMunn.m) && (k.EMunn.ea.ActiveSheet == null)) k.EMunn.m = false;

				if (k.Letezik(k.mentpath + "jelenleti.xls", "f"))
				{
					if (k.EMjelen.m)
					{
						if (k.EMjelen.ea.Visible) button1.Text = "Jelenleti.xls elrejtése";
					}
					else button1.Text = "Jelenleti.xls megnyitása";
				}
				else
				{
					button1.Text = "Jelenléti.xls elkészítése és megnyitása";
				}

				if (k.Letezik(k.munkapath, "F"))
				{
					if (!k.EMmunka.m)
					{
						button2.Text = "Hétvégére eső munkanapokat tartalmazó munkafüzet megnyitása";
					}
					else
					{
						if (!k.EMmunka.ea.Visible)
						{
							button2.Text = "Hétvégére eső munkanapokat tartalmazó munkafüzet megnyitása";
						}
						else
						{						
							button2.Text = "Hétvégére eső munkanapokat tartalmazó munkafüzet elrejtése";
						}
					}

				}
				else
				{					
					button2.Text = "Új hétvégére eső munkanapokat tartalmazó munkafüzet készítése";
				}
				if (k.Letezik(k.unnepath, "F"))
				{
					if (!k.EMunn.m)
					{
						button3.Text = "Ünnepnapokat tartalmazó munkafüzet megnyitása";						
					}
					else
					{
						if (k.EMunn.ea.Visible)
						{
							button3.Text = "Ünnepnapokat tartalmazó munkafüzet elrejtése";
						}
						else
						{
							button3.Text = "Ünnepnapokat tartalmazó munkafüzet megnyitása";
						}
					}
				}
				else
				{
					button3.Text = "Új ünnepnapokat tartalmazó munkafüzet létrehozása";
				}
			}
			catch (Exception exp)
			{
				throw (new Exception(exp.Message + "\nHely:Excelkez initbuttex"));
				this.Close();
			}


		}
        private void button1_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Megnyitja az erre a hónapra készített jelenléti lapot";
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Megnyitja a szabadnapos lapot";
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Megnyitja a munkanapos lapot";

        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Megnyitja a nevek adatbázisát";

        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Megnyitja a tavalyi szabikat tartalamzó lapot";
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";   
        }

        private void button6_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Bezárja ezt az ablakot";
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }
        private void button7_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Létrehozza a 'fizikai' lapot";
        }

        private void button7_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
				if (k.Letezik(k.mentpath + "jelenleti.xls", "F"))
				{
					if (k.EMjelen.m)
					{
						if (!k.EMjelen.ea.Visible)
						{
							k.EMjelen.ea.Visible = true;
							button1.Text = "Jelenleti.xls elrejtése";
						}
						else
						{
							k.EMjelen.ea.Visible = false;
							button1.Text = "Jelenleti.xls megnyitása";
						}
					}
					else
					{

						k.EMjelen.ea = ef.Exapp();
						k.EMjelen.ewb = ef.Megnyit(k.mentpath + "jelenleti.xls", k.EMjelen.ea);
						k.EMjelen.m = true;
						k.EMjelen.ea.Visible = true;
						button1.Text = "Jelenleti.xls elrejtése";
					}
				}
				else
				{
					//k.Uzi("A jelenléti lap nem nyitható meg, mivel még nincs létrehozva.", "oo", "i");
					if (!k.EMm2n.m)
					{
						k.EMm2n.ea = ef.Exapp();
						k.EMm2n.ewb = ef.Megnyit(k.mail2nev, k.EMm2n.ea);
						k.EMm2n.m = true;
					}
					int maxsor = ef.listavege(k.EMm2n.ewb, 1);
					k.Felado locfelado;
					for (int i = 2; i < maxsor; i++)
					{
						locfelado.emailnev = ef.cella(k.EMm2n.ewb, i, 1);
						locfelado.nev = ef.cella(k.EMm2n.ewb, i, 2);
						locfelado.tipus = ef.cella(k.EMm2n.ewb, i, 3);
						string tulora = (ef.cella(k.EMm2n.ewb, i, 4));
						locfelado.tulora = ((tulora == "Igen") || (tulora == "igen"));
						k.GetJelenletiExcel(locfelado);
					}
					k.EMjelen.ewb.Save();
					button1.Text = "Jelenleti.xls elrejtése";
					k.EMjelen.ea.Visible = true;					
				}
			}		
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:Excel kezelö() button1_Click"));
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
			try
			{
				if (k.Letezik(k.munkapath, "F"))
				{
					if (!k.EMmunka.m)
					{
						k.EMmunka.ea = ef.Exapp();
						k.EMmunka.ewb = ef.Megnyit(k.munkapath, k.EMmunka.ea);
						k.EMmunka.m = true;
						k.EMmunka.ea.Visible = true;
						button2.Text = "Hétvégére eső munkanapokat tartalmazó munkafüzet elrejtése";

					}
					else
					{
						if (k.EMmunka.ea.Visible)
						{
							k.EMmunka.ea.Visible = false;
							button2.Text = "Hétvégére eső munkanapokat tartalmazó munkafüzet megnyitása";
						}
						else
						{
							k.EMmunka.ea.Visible = true;
							button2.Text = "Hétvégére eső munkanapokat tartalmazó munkafüzet elrejtése";
						}
					}

				}
				else
				{
					k.EMmunka.ea = ef.Exapp();
					k.EMmunka.ewb = k.EMmunka.ea.Workbooks.Add(System.Reflection.Missing.Value);
					k.EMmunka.ewb.SaveAs(k.munkapath, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
					k.EMmunka.m = true;
					k.EMmunka.ea.Visible = true;
					button2.Text = "Hétvégére eső munkanapokat tartalmazó munkafüzet elrejtése";
				}
			}
			catch (Exception exp)
			{
				throw (new Exception(exp.Message + " \nHely:Excel kezelö() button2_Click"));
			}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
				if (k.Letezik(k.unnepath, "F"))
				{
					if (!k.EMunn.m)
					{
						k.EMunn.ea = ef.Exapp();
						k.EMunn.ewb = ef.Megnyit(k.unnepath, k.EMunn.ea);
						k.EMunn.m = true;
						k.EMunn.ea.Visible = true;
						button3.Text = "Ünnepnapokat tartalmazó munkafüzet elrejtése";
					}
					else
					{
						if (k.EMunn.ea.Visible)
						{
							k.EMunn.ea.Visible = false;
							button3.Text = "Ünnepnapokat tartalmazó munkafüzet megnyitása";
						}
						else
						{
							k.EMunn.ea.Visible = true;
							button3.Text = "Ünnepnapokat tartalmazó munkafüzet elrejtése";
						}
					}
				}
				else
				{
					k.EMunn.ea = ef.Exapp();
					k.EMunn.ewb = k.EMmunka.ea.Workbooks.Add(System.Reflection.Missing.Value);
					k.EMunn.ewb.SaveAs(k.unnepath, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
					k.EMunn.m = true;
					k.EMunn.ea.Visible = true;
					button3.Text = "Ünnepnapokat tartalmazó munkafüzet elrejtése";
				}
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:Excel kezelö() button3_Click"));
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
				if (k.Letezik(k.mail2nev, "F"))
				{					
					if (!k.EMm2n.m)
					{
						k.EMm2n.ea = ef.Exapp();
						k.EMm2n.ewb = ef.Megnyit(k.mail2nev, k.EMm2n.ea);
						k.EMm2n.m = true;
					}
					k.EMm2n.ea.Visible = true;
				}
				else
				{
					k.EMm2n.ea = ef.Exapp();
					k.EMm2n.ewb = k.EMmunka.ea.Workbooks.Add(System.Reflection.Missing.Value);
					k.EMm2n.ewb.SaveAs(k.mail2nev, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
					k.EMm2n.m = true;
					k.EMm2n.ea.Visible = true;
				}
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:Excel kezelö() button4_Click"));
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
				if (k.Letezik(k.szabipath, "F"))
				{
					if (!k.EMszabi.m)
					{
						k.EMszabi.ea = ef.Exapp();
						k.EMszabi.ewb = ef.Megnyit(k.szabipath, k.EMszabi.ea);
						k.EMszabi.m = true;
					}
					k.EMszabi.ea.Visible = true;
				}
				else
				{
					k.Uzi("A szabadságos fájl nem található!", "oo", "*");
				}
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:Excel kezelö() button5_Click"));
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            Excelkez.ActiveForm.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
			try
			{
				if (k.Letezik(k.mentpath + "jelenleti.xls", "F"))
				{
					if (!k.EMjelen.m)
					{
						k.EMjelen.ea = ef.Exapp();
						k.EMjelen.ewb = ef.Megnyit(k.mentpath + "jelenleti.xls", k.EMjelen.ea);
						k.EMjelen.m = true;
					}

					if (k.EMjelen.ea.Visible) k.EMjelen.ea.Visible = false;
					Microsoft.Office.Interop.Excel.Worksheet eWs;
					eWs = k.GetSzabiSheet(k.EMjelen.ewb, "Túlóra");
					if (eWs == null)
					{
						k.Felado locfelado = new k.Felado(true);
						locfelado.tipus = "Túlóra";
						eWs = k.CreateSheet(locfelado, k.EMjelen.ewb);
						string[] nevek ={ "Dolg.", "", "", "", "", "", "" };
						IFormatProvider culture = new System.Globalization.CultureInfo("hu-HU", true);
						DateTime dt;
						string datum = DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + ".1";
						dt = DateTime.Parse(datum, culture);
						Microsoft.Office.Interop.Excel.Range eR;
						int napokszama = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
						int i = napokszama;
						foreach (string nev in nevek)
						{
							i++;
							ef.cella(k.EMjelen.ewb, 1 + k.ExcElsoSorYElt, i + k.ExcNapXElt, nev);
							eR = ef.GetColRange(k.EMjelen.ewb, i + k.ExcNapXElt);
							eR.EntireColumn.ColumnWidth = 6.29;
						}
						((Microsoft.Office.Interop.Excel.Worksheet)k.EMjelen.ewb.ActiveSheet).PageSetup.CenterHeader = "&\"Arial,Bold\"&14TÚLÓRA JELENTÉS&\"Arial,Regular\"&10\n" + dt.ToString("yyyy.MMMM", culture);
						eWs.Activate();
						string[] tnevek = k.Tulorasok();
						for (int il = 0; tnevek[il] != null; il++)
						{
							locfelado.nev = tnevek[il];
							k.AddtoJelenleti(k.EMjelen.ea, k.EMjelen.ewb, eWs, locfelado);
						}
						k.EMjelen.ea.Visible = true;
						button1.Text = "Jelenleti.xls elrejtése";
					}
					else
					{
						eWs.Activate();
						k.EMjelen.ea.Visible = true;
						button1.Text = "Jelenleti.xls elrejtése";
					}
					k.EMjelen.ewb.Save();
				}

				else
				{
					k.Uzi("A jelenleti.xls még nem létezik!\nA fizikai lap csak akkor készíthető el, ha a jelenléti.xls már létezik.", "oo", "i");
				}

			}
			catch (Exception exp)
			{
				throw (new Exception(exp.Message + " \nHely:Excel kezelö() button5_Click"));
			}
        }
    }
}