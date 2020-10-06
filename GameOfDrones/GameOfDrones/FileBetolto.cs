using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfDrones
{
    internal class FileKezelo
    {
        private string _fajlNev;

        public string Fajlnev
        {
            get { return _fajlNev; }
            private set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _fajlNev = value;
                }
                else
                {
                    throw new Exception ("A fájlnév nem lehet üres!");
                }
            }
        }

        public FileKezelo(string fajlNev)
        {
            Fajlnev = fajlNev;
            FajlOsszesSor = System.IO.File.ReadAllLines(fajlNev);
        }

        private string[] _fajlOsszesSor;

        public string[] FajlOsszesSor
        {
            get { return _fajlOsszesSor; }
            private set { _fajlOsszesSor = value; }
        }
    }
}
