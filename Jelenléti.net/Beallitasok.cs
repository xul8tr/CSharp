using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Jelenléti.net
{
    public partial class Beallitasok : Form
    {
        public string a, b, c, d, f, g, h, i, j, l;
        public Beallitasok()
        {
            InitializeComponent();
        }

        private void Beallitasok_Load(object sender, EventArgs e)
        {
            string utvonal = Application.StartupPath + "\\settings.set";
            try
            {
                toolStripStatusLabel1.Text = "";
                if (k.Letezik(utvonal, "F"))
                {
                    textBox1.Text = ff.SorOlvas(utvonal, 1);
                    textBox2.Text = ff.SorOlvas(utvonal, 2);
                    textBox3.Text = ff.SorOlvas(utvonal, 3);
                    textBox5.Text = ff.SorOlvas(utvonal, 4);
                    textBox6.Text = ff.SorOlvas(utvonal, 5);
                    radioButton1.Checked = ff.SorOlvas(utvonal, 7)=="True";
                    radioButton2.Checked = !radioButton1.Checked;
                    textBox4.Text = ff.SorOlvas(utvonal, 8);
                    textBox7.Text = ff.SorOlvas(utvonal, 9);
                    textBox8.Text = ff.SorOlvas(utvonal, 10);
                }
                else
                {
                    alapertelmezes();
                }
                a = textBox1.Text;
                b = textBox2.Text;
                c = textBox3.Text;
                d = textBox5.Text;
                f = textBox6.Text;
                h = radioButton1.Checked.ToString();
                i = textBox4.Text;
                j = textBox7.Text;
                l = textBox8.Text;
            }
            catch (Exception exc)
            {
                k.Uzi(exc.Message + " kivétel történt a Beállítások form betöltésekor\nHibakód", "oo", "x");
            }

        }
        private void alapertelmezes()
        {
            try
            {
                textBox1.Text = Application.StartupPath + "\\mail2nev.xls";
                textBox2.Text = k.Endslash(Application.StartupPath);
                textBox3.Text = "Jelenléti";
                textBox5.Text = k.Endslash(Application.StartupPath);
                textBox6.Text = Application.StartupPath + "\\unnepek.xls";
                radioButton1.Checked = true;
                radioButton2.Checked = false;
                textBox4.Text = Application.StartupPath + "\\szabik.xls";
                textBox7.Text = k.Endslash(Application.StartupPath);
                textBox8.Text = Application.StartupPath + "\\hmunknap.xls";

                a = textBox1.Text;
                b = textBox2.Text;
                c = textBox3.Text;
                d = textBox5.Text;
                f = textBox6.Text;
                h = radioButton1.Checked.ToString();
                i = textBox4.Text;
                j = textBox7.Text;
                l = textBox8.Text;
            }
            catch (Exception exc)
            {
                throw (new Exception(exc.Message + "\nHely: alapertelmezes()"));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {

                openFileDialog1.Filter = "Microsoft Excel | *.xls";
                openFileDialog1.FileName = @textBox1.Text;
                if (openFileDialog1.ShowDialog() == DialogResult.OK) textBox1.Text = openFileDialog1.FileName;
                a = textBox1.Text;               
            }
            catch (Exception exc)
            {
                k.Uzi(exc.Message + "\nHely: beallitasok button1_Click", "oo", "x");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = label2.Text;
            folderBrowserDialog1.SelectedPath = @textBox2.Text;
            folderBrowserDialog1.ShowDialog();
            textBox2.Text = k.Endslash(folderBrowserDialog1.SelectedPath);
            b = textBox2.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {

                openFileDialog1.Filter = "Microsoft Excel | *.xls";
                openFileDialog1.FileName = @textBox4.Text;
                if (openFileDialog1.ShowDialog() == DialogResult.OK) textBox4.Text = openFileDialog1.FileName;
                i = textBox4.Text;
            }
            catch (Exception exc)
            {
                k.Uzi(exc.Message + "\nHely: beallitasok button1_Click", "oo", "x");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = label8.Text;
            folderBrowserDialog1.SelectedPath = @textBox7.Text;
            folderBrowserDialog1.ShowDialog();
            textBox7.Text = k.Endslash(folderBrowserDialog1.SelectedPath);
            j = textBox7.Text; 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = label5.Text;
            folderBrowserDialog1.SelectedPath = @textBox5.Text;
            folderBrowserDialog1.ShowDialog();
            textBox5.Text = k.Endslash(folderBrowserDialog1.SelectedPath);
            d = textBox5.Text;       
        }

        private void button6_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Microsoft Excel | *.xls";
            openFileDialog1.FileName = @textBox6.Text;
            if (openFileDialog1.ShowDialog() == DialogResult.OK) textBox6.Text = openFileDialog1.FileName;
            f = textBox6.Text;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Microsoft Excel | *.xls";
            openFileDialog1.FileName = @textBox8.Text;
            if (openFileDialog1.ShowDialog() == DialogResult.OK) textBox8.Text = openFileDialog1.FileName;
            l = textBox8.Text;
        } 

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                Beallitasok_Load(null, null);
            }
            catch (Exception exc)
            {
                k.Uzi(exc.Message + "\nHely: beallitasok button8_Click-viszavonás", "oo", "x");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string utvonal = Application.StartupPath + "\\settings.set";
            try
            {
                if (k.Letezik(utvonal + ".bak", "F")) System.IO.File.Delete(utvonal + ".bak");  //ha létezik előző settings.set tartalékmentés letörli
                if (k.Letezik(utvonal, "F")) System.IO.File.Copy(utvonal, utvonal + ".bak");     //a jelenlegi beállításokat eltartaékolja

                string g = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
                ff.SorIras(textBox1.Text, utvonal, "F");
                ff.SorIras(textBox2.Text, utvonal, "A");
                ff.SorIras(textBox3.Text, utvonal, "A");
                ff.SorIras(textBox5.Text, utvonal, "A");
                ff.SorIras(textBox6.Text, utvonal, "A");
                ff.SorIras(g, utvonal, "A");
                ff.SorIras(radioButton1.Checked.ToString(), utvonal, "A");
                ff.SorIras(textBox4.Text, utvonal, "A");
                ff.SorIras(textBox7.Text, utvonal, "A");
                ff.SorIras(textBox8.Text, utvonal, "A");
            }
            catch (Exception exc)
            {
                k.Uzi(exc.Message + "\nHely: beallitasok button9_Click-settings.set írása", "oo", "x");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            
            string utvonal = Application.StartupPath + "\\settings.set";
            if (k.Letezik(utvonal, "F"))
            {
                if ((ff.SorOlvas(utvonal, 1) != a || ff.SorOlvas(utvonal, 2) != b || ff.SorOlvas(utvonal, 3) != c || ff.SorOlvas(utvonal, 4) != d || ff.SorOlvas(utvonal, 5) != f || ff.SorOlvas(utvonal, 7) != h || ff.SorOlvas(utvonal, 8) != i || ff.SorOlvas(utvonal, 9) != j || ff.SorOlvas(utvonal, 10) != l) && (k.Uzi("Menti a változtatásokat?", "yn", "?") == DialogResult.Yes)) button9_Click(null, null);
            }
            else
                button9_Click(null, null);
            k.Init();
            this.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                alapertelmezes();
            }
            catch (Exception exc)
            {
                k.Uzi(exc.Message + "\nHely: beallitasok button11_Click-alapértelmezések betöltése", "oo", "x");
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            h = radioButton1.Checked.ToString();
        }
      
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text != a)
            {
                if (!k.Letezik(textBox1.Text, "F"))
                {

                    k.Uzi("Az útvonal: " + @textBox1.Text + " nem létezik!", "oo", "!");
                    textBox1.Text = a;
                }
                else
                {
                    a = textBox1.Text;
                }
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text != b)
            {
                if (!k.Letezik(textBox2.Text, "M"))
                {
                    k.Uzi("Az útvonal: " + @textBox2.Text + " nem létezik!", "oo", "!");
                    textBox2.Text = b;
                }
                else
                {
                    b = textBox2.Text;
                }
            }

        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text != c)
            {
                if (k.Uzi("Biztosan megváltoztatja a témaszűrés feltételét?", "yn", "?") == DialogResult.No) textBox3.Text = c;
                else c = textBox3.Text;
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (textBox4.Text != i)
            {
                if (!k.Letezik(textBox4.Text, "F"))
                {
                    k.Uzi("Az útvonal: " + @textBox4.Text + " nem létezik!", "oo", "!");
                    textBox4.Text = i;
                }
                else
                {
                    i = textBox4.Text;
                }
            }
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            if (textBox5.Text != d)
            {
                if (!k.Letezik(textBox5.Text, "M"))
                {
                    k.Uzi("Az útvonal: " + @textBox5.Text + " nem létezik!", "oo", "!");
                    textBox5.Text = d;
                }
                else
                {
                    d = textBox5.Text;
                }
            }
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            if (textBox6.Text != f)
            {
                if (!k.Letezik(textBox6.Text, "F"))
                {
                    k.Uzi("Az útvonal: " + @textBox6.Text + " nem létezik!", "oo", "!");
                    textBox6.Text = f;
                }
                else
                {
                    f = textBox6.Text;
                }
            }
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            if (textBox7.Text != j)
            {
                if (!k.Letezik(textBox7.Text, "M"))
                {
                    k.Uzi("Az útvonal: " + @textBox7.Text + " nem létezik!", "oo", "!");
                    textBox7.Text = j;
                }
                else
                {
                    j = textBox7.Text;
                }
            }
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            if (textBox8.Text != l)
            {
                if (!k.Letezik(textBox8.Text, "F"))
                {
                    k.Uzi("Az útvonal: " + @textBox8.Text + " nem létezik!", "oo", "!");
                    textBox8.Text = l;
                }
                else
                {
                    l = textBox8.Text;
                }
            }
        }        

        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Itt adhatja meg a nevek adatbázisát";
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Kiválaszthatja a fájl megnyitás ablak segítségével a nevek adatbázisát";
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void textBox2_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Beállíthatja a log fájlok elérési útját";
        }

        private void textBox2_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Pár kattintással megadhatja a szükséges útvonalat";
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void textBox3_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "A mezőben megadott témájú leveleket fogja feldolgozni a program";
        }

        private void textBox3_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void textBox5_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Beállíthatja a feldgozási adatbázis útvonalát";
        }

        private void textBox5_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Pár kattintással megadhatja a szükséges útvonalat";
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void textBox6_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Itt lehet megadni az ünnepek adatbázis elérési útvonalát";
        }

        private void textBox6_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button6_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Kiválaszthatja a fájl megnyitás ablak segítségével az ünnepek adatbázisát";
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button8_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Visszaállítja az értékeket a legutóbbi mentett állásra";
        }

        private void button8_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button9_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Elmenti a jelenlegi beállításokat";
        }

        private void button9_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button10_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Bezárja az ablakot";
        }

        private void button10_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button11_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Visszaállít minden értéket a program alapértelmezett útvonalaira";
        }

        private void button11_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void radioButton1_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Válassza ezt az opciót, ha a jelentés csatolt munkafüzetben jön";
        }

        private void radioButton1_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void radioButton2_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Válasza ezt az opciót, ha a jelentés a levél törzsében jön";
        }

        private void radioButton2_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }
               
        private void textBox4_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Itt adhatja meg az éves szabadságok adatbázisát";
        }

        private void textBox4_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Kiválaszthatja a fájl megnyitás ablak segítségével az éves szabadságok adatbázisát";
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void textBox7_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Megmutatja a jelenléti lap mentési helyét";
        }

        private void textBox7_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Böngészőablakban tudja kiválasztani a mentés helyét";
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void textBox8_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Itt adhatja meg a hétvégére eső munkanapok adatbázisát";
        }

        private void textBox8_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void button7_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Kiválaszthatja a fájl megnyitás ablak segítségével az adatbázist";
        }

        private void button7_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        

        

              
    }
}