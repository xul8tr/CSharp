using BombermanBasics.Interfaces;

namespace BombermanBasics
{
    public class Bomb : Interfaces.IBomb
    {
        public Bomb(Team owner, GameBoard gameBoard, Field targetField, Interfaces.IGameController controller, int explosiveRadius, out bool isValid)
        {
            isValid = true;
            if (!controller.isValidToBomb(targetField, gameBoard, owner))
            {
                isValid = false;
                return;
            }
            m_owner = owner;
            m_gameboard = gameBoard;
            m_explosiveRadius = explosiveRadius;
            m_Field = targetField;
            owner.addBomb(this);
            targetField.putBomb(this, owner);
        }

        public int getExplosiveRadius()
        {
            return m_explosiveRadius;
        }

        public ITeam getOwnerI()
        {
            return m_owner;
        }

        public Team getOwner()
        {
            return m_owner;
        }

        public IField FieldI
        {
            get
            {
                return m_Field;
            }
        }

        public Field Field
        {
            get
            {
                return m_Field;
            }
            set
            {
                m_Field = value;
            }
        }

        public bool IsExploded
        {
            get
            {
                return m_isExploded;
            }
            set
            {
                m_isExploded = value;
            }
        }

        private Team m_owner;
        private Field m_Field;
        private GameBoard m_gameboard;
        private int m_explosiveRadius;
        private bool m_isExploded = false;
    }
}
