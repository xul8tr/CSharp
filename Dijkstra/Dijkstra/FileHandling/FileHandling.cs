using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dijkstra.FileHandling
{
    class FileHandling
    {
        public string Open()
        {
            System.Windows.Forms.OpenFileDialog myDialog = new System.Windows.Forms.OpenFileDialog();
            myDialog.CheckFileExists = true;
            myDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            myDialog.Title = "Dijkstra";
            myDialog.InitialDirectory = Environment.CurrentDirectory;
            myDialog.ShowDialog();
            return myDialog.FileName;
        }
    }
}
