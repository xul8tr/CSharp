using BombermanBasics.Interfaces;
using System.Collections.Generic;
using System.Drawing;

namespace BombermanBasics
{
    public class GameBoard : IGameBoard
    {
        public int Size
        {
            get
            {
                if (m_fields != null)
                    return m_fields.Length;
                else
                    return 0;
            }
        }

        public int MinX
        {
            get
            {
                return 0;
            }
        }

        public int MaxX
        {
            get
            {
                if (m_fields != null)
                    return m_fields.Length - 1;
                else
                    return 0;
            }
        }

        public int MinY
        {
            get
            {
                return 0;
            }
        }

        public int MaxY
        {
            get
            {
                if (m_fields != null && m_fields[0] != null)
                    return m_fields[0].Length - 1;
                else
                    return 0;
            }
        }
        public int NumberOfMines { get; }

        public Interfaces.IField[][] Fields
        {
            get
            {
                return m_fields;
            }
        }

        public GameBoard(Bitmap mapImg, System.Func<int, int, Field> allocateField)
        {
            int size = mapImg.Height;
            if (mapImg == null)
            {
                throw new System.Exception("GameBoard: mapImg is null");
            }
            if (mapImg.Width != mapImg.Height)
            {
                throw new System.Exception("GameBoard: map image Height != mapWidth");
            }
            //allocate the fields of the board
            m_fields = new Field[size][];
            for (int iX = 0; iX < m_fields.Length; ++iX)
            {
                m_fields[iX] = new Field[size];
                for (int iY = 0; iY < size; ++iY)
                {
                    m_fields[iX][iY] = allocateField(iX, iY);
                    if (mapImg.GetPixel(iX, iY).GetBrightness() < 0.1)
                    {
                        m_fields[iX][iY].setWall(true);
                    }
                }
            }
        }

        public bool areFieldCoordinatesValid(IField place)
        {
            return areFieldCoordinatesValid(place.X, place.Y);
        }

        public bool areFieldCoordinatesValid(int x, int y)
        {
            if (x < MinX || x > MaxX
                || y < MinY || y > MaxY)
            {
                return false;
            }
            return true;
        }

        public Field getField(int x, int y)
        {
            Field tmp = new Field(false, 0, x, y);
            if (!areFieldCoordinatesValid(tmp))
                return null;
            return m_fields[x][y];
        }

        public IField getFieldI(int X, int Y)
        {
            return getField(X, Y);
        }

        /// <summary>
        /// applies 'applyFieldExplosion' in the order of explosions and explodes the bombs in the radius recursively.
        /// </summary>
        /// <param name=""></param>
        public void explodeFields(IGameController gameController, Bomb bomb, ExplosionFieldCollector explodedFields, int recursionCounter = 0)
        {
            if (bomb == null)
                return;
            Explosion explosion = new Explosion(this, bomb);
            List<Field> fields = explosion.getExplodedFields(gameController);
            foreach (Field currentField in fields)
            {
                if (!bomb.IsExploded)
                {
                    //add all of the exploded fields to the collection
                    explodedFields.addExplodedField(recursionCounter, explosion.getDistanceFactor(currentField), currentField);
                    currentField.IsExploding = true;
                }
            }
            Field fieldOfBomb = bomb.Field;
            fieldOfBomb.removeBomb(bomb);
            bomb.getOwner().removeBomb(bomb);
            bomb.IsExploded = true;

            //explode the recursively activated bombs
            foreach (Field currentField in fields)
            {
                if (currentField.isBombed())    //if there is a bomb on a field inside the radius of the original bomb
                {
                    Bomb[] currentFieldBombs = currentField.getBombs();
                    for (int i = 0; i < currentFieldBombs.Length; ++i)
                    {
                        if (currentFieldBombs[i] != null)
                        {
                            explodeFields(gameController, currentFieldBombs[i], explodedFields, recursionCounter + 1);
                        }
                    }
                }
            }
        }

        private Field[][] m_fields;
    }
}
