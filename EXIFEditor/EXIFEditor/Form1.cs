using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ElementIT.ImageProperties;

namespace EXIFEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string m_DirectoryPath = @"c:\Users\Mau\Downloads\Hongkong\toedit";
            ImageProperties m_ImageProperties;
            ImagePropertyItem m_ImagePropertyItem;
            System.IO.DirectoryInfo m_DirectoryInfo = new System.IO.DirectoryInfo(m_DirectoryPath);
            System.IO.FileInfo[] m_FileInfo = m_DirectoryInfo.GetFiles();
            string m_Length = m_FileInfo.Length.ToString();
            int i = 0;
            listBox1.Items.Clear();
            foreach (System.IO.FileInfo File in m_FileInfo)
            {
                i++;                               
                m_ImageProperties = new ImageProperties(File.FullName);
                m_ImagePropertyItem = m_ImageProperties[m_ImageProperties.IndexOf(306)];
                DateTime m_DateTime;
                string retval = System.Text.Encoding.ASCII.GetString((byte[])m_ImagePropertyItem.Value, 0, m_ImagePropertyItem.arrayLength - 1);
                char[] splitter = new char[] { ' ' };
                string[] retval1 = retval.Split(splitter);
                retval1[0] = retval1[0].Replace(":", ".");
                retval = retval1[0] + " " + retval1[1];
                DateTime.TryParse(retval, out m_DateTime);
                m_DateTime = m_DateTime.AddMinutes(60d);
                //m_ImagePropertyItem = m_ImageProperties.Add(306);
                //m_ImagePropertyItem.Type = PropertyType.ASCIIArray;
                m_ImagePropertyItem.Value = System.Text.Encoding.ASCII.GetBytes(m_DateTime.ToString() + '\0');
                m_ImagePropertyItem.SetPropertyToImage();
                string m_filename = File.Name.Remove(0, File.Name.LastIndexOf("_"));
                retval = m_DateTime.ToString().Replace(":", "_");
                retval = retval.Replace(".", "_");
                retval = retval.Replace(" ", "");
                m_filename = retval + m_filename;
                m_ImageProperties.Image.Save(m_filename);
                listBox1.Items.Add(File.Name + "->" + m_filename + ". Done");
                label1.Text = m_Length + "/" + i;
                Application.DoEvents();
                m_ImagePropertyItem.Dispose();
                m_ImageProperties.Image.Dispose();
                m_ImageProperties.Dispose();
            }
        }
    }
}
