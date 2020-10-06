using System;

namespace CopyHostName
{
    class Program
    {
        [STAThread]
        static void Main()
        {            
            System.Windows.Forms.Clipboard.SetText(Environment.MachineName);
        }
    }
}