using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfDrones
{
    class DronGyar
    {
        private ushort _dronAzonosito;
        private FileKezelo _fileKezel;

        public FileKezelo Filekezel
        {
            get { return _fileKezel; }
            set 
            {
                if (value != null)
                {
                    _fileKezel = value;
                }
                else
                {
                    throw new Exception("Filekezelo nem lehet null!");
                }
            }
        }

        private List<Dron> _dronok;

        public List<Dron> Dronok
        {
            get { return _dronok; }
            private set { _dronok = value; }
        }
        

        public DronGyar(FileKezelo fileKezelo)
        {
            Filekezel = fileKezelo;
            Gyartas();
        }

        private void Gyartas()
        {
            Dronok = new List<Dron>();
            _dronAzonosito = 0;

            foreach (string sor in Filekezel.FajlOsszesSor)
            {
                if (!string.IsNullOrEmpty(sor))
                {
                    if (sor.StartsWith("//") || sor.StartsWith("Drónok_Kezd"))
                    {
                        continue;
                    }

                    if (sor.StartsWith("Drónok_Vége"))
                    {
                        break;
                    }

                    if (sor.StartsWith("Felderito") || sor.StartsWith("Harci"))
                    {
                        DronGyartas(sor);
                    }
                }
            }
        }

        private void DronGyartas(string sor)
        {
            string[] parameterek = Darabolas(sor);
            bool felderito =sor.StartsWith("Felderito");
            if ((felderito && (parameterek.Length != 5)) || (!felderito && (parameterek.Length != 6)))
            {
                throw new Exception("Nem megfelelő mennyiségű paraméter a drón legyártásához");
            }

            ushort azonosito = _dronAzonosito++;
            string elnevezes = parameterek[1];
            Position pozicio = PozicioAParameterbol(parameterek[2], parameterek[3]);

            if (felderito)
            {
                bool kepesHokepetKesziteni = KepesEHokepetKesziteni(parameterek[4]);
                FelderitoDron felderitoDron = new FelderitoDron(azonosito, elnevezes, pozicio, kepesHokepetKesziteni);
                Dronok.Add(felderitoDron);
            }
            else
            {
                uint bombakSzama = UintParser(parameterek[4]);
                float lotav = FloatParser(parameterek[5]);
                HarciDron harciDron = new HarciDron(azonosito, elnevezes, pozicio, bombakSzama, lotav);
                Dronok.Add(harciDron);
            }            
        }

        private string[] Darabolas(string sor)
        {
            char[] separator = { ';' };
            return sor.Split(separator);
        }

        private Position PozicioAParameterbol(string xPos, string yPos)
        {
            float x, y;
            string tizedesvesszo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            xPos= xPos.Replace(".", tizedesvesszo);
            xPos = xPos.Replace(",", tizedesvesszo);
            yPos = yPos.Replace(".", tizedesvesszo);
            yPos = yPos.Replace(",", tizedesvesszo);

            if (!float.TryParse(xPos, out x) || !float.TryParse(yPos, out y))
            {
                throw new Exception(string.Format("A drón pozíciója nem határozható meg ezekből a paraméterekből:{0},{1}", xPos, yPos));
            }

            return new Position(x, y);
        }

        private bool KepesEHokepetKesziteni(string parameter)
        {
            return parameter.ToUpper() == "IGEN";
        }

        private float FloatParser(string parameter)
        {
            float f;
            if (!float.TryParse(parameter, out f))
            {
                throw new Exception(string.Format("Nem tudom floatként értelmezni:{0}", parameter));
            }

            return f;
        }

        private uint UintParser(string parameter)
        {
            uint u;
            if (!uint.TryParse(parameter, out u))
            {
                throw new Exception(string.Format("Nem tudom uintként értelmezni:{0}", parameter));
            }

            return u;
        }
    }
}
