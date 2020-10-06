using BombermanBasics.Interfaces;
using System.Collections.Generic;

namespace BombermanBasics
{
    public class Field : IField
    {
        public Field(bool isWall, int numberOfTeams, int x, int y)
        {
            m_isWall = isWall;
            X = x;
            Y = y;
            m_bombs = new Bomb[numberOfTeams];  //null references
            m_teams = new Team[numberOfTeams];  //null references
            m_numberOfTeams = numberOfTeams;
            IsExploding = false;
        }

        public int NumberOfTeams
        {
            get
            {
                return m_numberOfTeams;
            }
        }

        public void setWall(bool isWall)
        {
            m_isWall = isWall;
            if (isWall)
            {
                clearBombs();
                clearTeams();
            }
        }

        public void setWallDuringGameplay(bool isWall, out List<Team> killedTeams, out List<Bomb> removedBombs)
        {
            m_isWall = isWall;
            killedTeams = new List<Team>();
            removedBombs = new List<Bomb>();
            if (isWall)
            {
                killTeamsOnField(out killedTeams);
                for (int i = 0; i < m_bombs.Length; ++i)
                {
                    if (m_bombs[i] != null)
                    {
                        removedBombs.Add(m_bombs[i]);
                    }
                }
                clearBombs();
            }
        }

        //it does not perform any point counting
        public void killTeamsOnField(out List<Team> killedTeams)
        {
            killedTeams = new List<Team>();
            for (int i = 0; i < m_teams.Length; ++i)
            {
                if (m_teams[i] != null)
                {
                    killedTeams.Add(m_teams[i]);
                    m_teams[i].IsDead = true;
                }
            }
            clearTeams();
        }

        public bool isEmpty()
        {
            if (isBombed() || isAnyTeamOnField())
                return false;
            if (m_isWall)
                return false;
            return true;
        }

        public bool isWall()
        {
            return m_isWall;
        }

        public bool addTeam(Team team, int teamIdx)
        {
            if (teamIdx < 0 || teamIdx >= m_teams.Length)
            {
                string msg = "addTeam: wrong team index:";
                msg += teamIdx;
                throw new System.Exception(msg);
            }
            if (team == null)
            {
                throw new System.Exception("addTeam: team is null");
            }

            if (m_isWall)
                return false;
            if (m_teams[teamIdx] != null)
                return false;

            m_teams[teamIdx] = team;
            team.Field = this;
            return true;
        }

        public bool removeTeam(Team team)
        {
            for (int i = 0; i < m_teams.Length; ++i)
            {
                if (m_teams[i] == team)
                {
                    m_teams[i] = null;
                    return true;
                }
            }
            return false;
        }

        public bool putBomb(Bomb bomb, Team bomberTeam)
        {
            int teamIdx = bomberTeam.Field.getTeamIdx(bomberTeam);
            if (teamIdx < 0 || teamIdx >= m_bombs.Length)
            {
                string msg = "putBomb: wrong bomb index:";
                msg += teamIdx;
                throw new System.Exception(msg);
            }
            if (bomb == null)
            {
                throw new System.Exception("addTeam: team is null");
            }
            if (m_bombs[teamIdx] != null)
                return false;
            if (m_isWall)
                return false;

            m_bombs[teamIdx] = bomb;
            return true;
        }

        public bool removeBomb(Bomb bomb)
        {
            for (int i = 0; i < m_bombs.Length; ++i)
            {
                if (m_bombs[i] == bomb)
                {
                    m_bombs[i] = null;
                    return true;
                }
            }
            return false;
        }

        public int getTeamIdx(Team team)
        {
            for (int i = 0; i < m_teams.Length; ++i)
            {
                if (m_teams[i] == team)
                    return i;
            }
            return -1;
        }

        public bool contains(ITeam team)
        {
            for (int i = 0; i < m_teams.Length; ++i)
            {
                if (m_teams[i] == team)
                    return true;
            }
            return false;
        }

        public bool contains(IBomb bomb)
        {
            for (int i = 0; i < m_bombs.Length; ++i)
            {
                if (m_bombs[i] == bomb)
                    return true;
            }
            return false;
        }

        public IBomb getBombI(ITeam team)
        {
            for (int i = 0; i < m_bombs.Length; ++i)
            {
                if (m_bombs[i] != null)
                {
                    if (m_bombs[i].getOwner() == team)
                    {
                        return m_bombs[i];
                    }
                }
            }
            return null;
        }

        public bool isBombed()
        {
            if (isAnyBombOnField())
                return true;
            return false;
        }

        public bool isNeighbour(IField other)
        {
            int diffX = X - other.X;
            int diffY = Y - other.Y;
            if ((diffX == 1 || diffX == -1) && diffY == 0)
                return true;
            if ((diffY == 1 || diffY == -1) && diffX == 0)
                return true;
            return false;
        }

        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }

        public bool IsExploding
        {
            get;
            set;
        }

        public ITeam[] getTeamsI()
        {
            return m_teams;
        }

        public Team[] getTeams()
        {
            return m_teams;
        }

        public List<IBomb> getBombsI()
        {
            List<IBomb> retVal = new List<IBomb>();
            for (int i = 0; i < m_bombs.Length; ++i)
            {
                if (m_bombs[i] != null)
                {
                    retVal.Add(m_bombs[i]);
                }
            }
            return retVal;
        }

        public Bomb[] getBombs()
        {
            return m_bombs;
        }

        public bool isAnyTeamOnField()
        {
            for (int i = 0; i < m_teams.Length; ++i)
            {
                if (m_teams[i] != null)
                    return true;
            }
            return false;
        }

        public int getIndexOfGivenTeam(Team givenTeam)
        {
            for (int i = 0; i < m_teams.Length; ++i)
            {
                if (m_teams[i] == givenTeam)
                {
                    return i;
                }
            }
            return -1;
        }

        private void clearTeams()
        {
            for (int i = 0; i < m_teams.Length; ++i)
            {
                m_teams[i] = null;
            }
        }

        private bool isAnyBombOnField()
        {
            for (int i = 0; i < m_bombs.Length; ++i)
            {
                if (m_bombs[i] != null)
                    return true;
            }
            return false;
        }

        public void clearBombs()
        {
            for (int i = 0; i < m_bombs.Length; ++i)
            {
                m_bombs[i] = null;
            }
        }

        bool m_isWall;
        private int m_numberOfTeams;
        private Bomb[] m_bombs = null;
        private Team[] m_teams = null;
    }
}
