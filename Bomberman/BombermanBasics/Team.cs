using BombermanBasics.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BombermanBasics
{
    public class Team : ITeam
    {
        public virtual string Name { get; set; }

        public virtual bool IsDead
        {
            get;
            set;
        }

        public virtual int Points
        {
            get;
            set;
        }

        public virtual bool IsAlive //depends on IsDead property
        {
            get
            {
                return !IsDead;
            }
            set
            {
                IsDead = !value;
            }
        }

        public Team(string name)
        {
            IsDead = false; //IsAlive = true is also set by this
            Points = 0;
            Name = name;
            m_bombs = new List<Bomb>();
        }

        //returns the place of the 
        public IField FieldI
        {
            get
            {
                return m_field;
            }
        }

        public Field Field
        {
            get
            {
                return m_field;
            }
            set
            {
                m_field = value;
            }
        }

        public bool addBomb(Bomb newBomb)
        {
            if (!m_bombs.Contains(newBomb))
            {
                m_bombs.Add(newBomb);
                return true;
            }
            return false;
        }

        public bool removeBomb(Bomb bomb)
        {
            if (m_bombs.Contains(bomb))
            {
                m_bombs.Remove(bomb);
                return true;
            }
            return false;
        }

        public bool ownsBomb(IBomb bomb)
        {
            return m_bombs.Contains(bomb);
        }

        public IBomb[] getBombsI()
        {
            if (m_bombs != null)
                return m_bombs.ToArray();
            else
                return new Bomb[0];
        }

        public int getNumberOfBombs()
        {
            return m_bombs.Count;
        }

        public Bomb[] getBombs()
        {
            if (m_bombs != null)
                return m_bombs.ToArray();
            else
                return new Bomb[0];
        }

        public void clearBombs()
        {
            m_bombs.Clear();
        }

        private Field m_field;
        private List<Bomb> m_bombs;
    }
}
