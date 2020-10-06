using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfDrones
{
    internal class FelderitoDron: Dron, IMegfigyelo
    {
        public FelderitoDron(ushort azonosito, string elnevezes, Position pozicio, bool kepesHokepetKesziteni)
            : base(azonosito, elnevezes, pozicio)
        {
            KepesHokepetKesziteni = kepesHokepetKesziteni;
            _fenytartomanyokbanMennyiFotoKeszult = new Dictionary<Fenytartomany, int>();
        }

        private bool _kepesHokepetKesziteni;

        public bool KepesHokepetKesziteni
        {
            get { return _kepesHokepetKesziteni; }
            private set { _kepesHokepetKesziteni = value; }
        }
        
        void IMegfigyelo.Fotoz(Fenytartomany ft)
        {
            if (EletbenVan)
            {
                if (ft == Fenytartomany.hokep && !KepesHokepetKesziteni)
                {
                    throw new DronException(this, "Hőkép készítése ezen a drónon nem támogatott.");
                }
                else
                {
                    if (FenytartomanyokbanMennyiFotoKeszult.ContainsKey(ft))
                    {
                        FenytartomanyokbanMennyiFotoKeszult[ft] += 1;
                    }
                    else
                    {
                        FenytartomanyokbanMennyiFotoKeszult.Add(ft, 1);
                    }
                    this.Jelentes(string.Format("A fénykép a(z) {0} fénytartományban elkészült.", ft.ToString()));
                }
            }
            else
            {
                throw new DronException(this, "A drón megsemmisült, nem tud már fotózni sehogyan se. [Lelőtték :( ]"); 
            }
        }

        private Dictionary<Fenytartomany,int> _fenytartomanyokbanMennyiFotoKeszult;

        public Dictionary<Fenytartomany,int> FenytartomanyokbanMennyiFotoKeszult
        {
            get { return _fenytartomanyokbanMennyiFotoKeszult; }
            private set { _fenytartomanyokbanMennyiFotoKeszult = value; }
        }
        
    }
}
