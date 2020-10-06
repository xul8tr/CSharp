using System.Collections.Generic;
using BombermanBasics.Interfaces;

namespace BombermanBasics
{
    public class Explosion
    {
        public Explosion(GameBoard gameBoard, Bomb explodedBomb)
        {
            m_radius = explodedBomb.getExplosiveRadius();
            m_explodedBomb = explodedBomb;
            m_gameBoard = gameBoard;
        }

        public int Radius
        {
            get
            {
                return m_radius;
            }
        }

        public Bomb getExplodedBomb()
        {
            return m_explodedBomb;
        }

        public static List<Field> getExplodedFields(Bomb explodedBomb, GameBoard gameBoard, IGameController gameController)
        {
            Field place = explodedBomb.getOwner().Field;

            List<Field> retVals = new List<Field>();
            for (int iX = place.X; iX >= gameBoard.MinX && place.X - iX <= explodedBomb.getExplosiveRadius(); --iX)
            {
                Field currentField = gameBoard.getField(iX, place.Y);
                if (currentField != null && gameController.isExplodable(currentField))
                    retVals.Add(currentField);
                if (!gameController.isExplodable(currentField))
                    break;
            }
            for (int iX = place.X + 1; iX <= gameBoard.MaxX && iX - place.X <= explodedBomb.getExplosiveRadius(); ++iX)
            {
                Field currentField = gameBoard.getField(iX, place.Y);
                if (currentField != null && gameController.isExplodable(currentField))
                    retVals.Add(currentField);
                if (!gameController.isExplodable(currentField))
                    break;
            }

            for (int iY = place.Y - 1; iY >= gameBoard.MinY && place.Y - iY <= explodedBomb.getExplosiveRadius(); --iY)
            {
                Field currentField = gameBoard.getField(place.X, iY);
                if (currentField != null && gameController.isExplodable(currentField))
                    retVals.Add(currentField);
                if (!gameController.isExplodable(currentField))
                    break;
            }
            for (int iY = place.Y + 1; iY <= gameBoard.MaxY && iY - place.Y <= explodedBomb.getExplosiveRadius(); ++iY)
            {
                Field currentField = gameBoard.getField(place.X, iY);
                if (currentField != null && gameController.isExplodable(currentField))
                    retVals.Add(currentField);
                if (!gameController.isExplodable(currentField))
                    break;
            }

            return retVals;
        }

        public List<Field> getExplodedFields(IGameController gameController)
        {
            return getExplodedFields(m_explodedBomb, m_gameBoard, gameController);
        }

        public int getDistanceFactor(Field other)
        {
            int diffX = other.X - m_explodedBomb.Field.X;
            int diffY = other.Y - m_explodedBomb.Field.Y;
            return (diffX*diffX + diffY*diffY);
        }

        private int m_radius;
        private Bomb m_explodedBomb;
        private GameBoard m_gameBoard;
    }
}
