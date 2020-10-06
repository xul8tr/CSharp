using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Jelenléti.net
{
    public partial class Nevhozzadasa : Form
    {
        public Nevhozzadasa()
        {
            InitializeComponent();
        }
        public Nevhozzadasa(string nev)
        {
            InitializeComponent();
            textBox2.Text = nev;            
        }
        private void Nevhozzadasa_Load(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "")
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            
            Microsoft.Office.Interop.Excel.Application Exapp;
            Microsoft.Office.Interop.Excel.Workbook eWb;
            
            try
            {
                if (!k.EMm2n.m)
                {
                    k.EMm2n.ea = ef.Exapp();
                    k.EMm2n.ewb = ef.Megnyit(k.mail2nev, k.EMm2n.ea);
                    k.EMm2n.m = true;
                }
                Exapp = k.EMm2n.ea;
                eWb = k.EMm2n.ewb;
                k.Felado locfelado;
                locfelado = k.GetParams(textBox2.Text);
                if (locfelado.nev == "")
                {
                    int vege;
                    vege = ef.listavege(eWb, 1);
                    ef.cella(eWb, vege, 1, textBox2.Text);
                    ef.cella(eWb, vege, 2, textBox1.Text);
                    ef.cella(eWb, vege, 3, comboBox1.Text);
                    ef.cella(eWb, vege, 4, comboBox2.Text);
                    eWb.Save();
                }
                else
                {
                    k.Uzi("A nev: " + textBox2.Text + " már létezik!", "oo", "i");
                }


            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:GetUserParams cella olvasásakor"));
            }
            finally
            {
                eWb = null;
                Exapp = null;
            }

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "")
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "")
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Ez a név jelenik meg a jelenléti listában";
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void textBox2_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Az Outlook szerinti feladó";
        }

        private void textBox2_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void comboBox1_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "A jelenléti ezen lapján jelenik meg a fenti személy";
        }

        private void comboBox1_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void comboBox2_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Válassza az Igent, ha a túlórát el kell számolni!";
        }

        private void comboBox2_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Hozzáadja a személyt az adatbázishoz";
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Hozzáadás nélkül bezárja a panelt";
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }
    }
}