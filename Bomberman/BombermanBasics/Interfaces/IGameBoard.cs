namespace BombermanBasics.Interfaces
{
    public interface IGameBoard
    {
        int Size
        {
            get;
        }

        int MinX
        {
            get;
        }

        int MaxX
        {
            get;
        }

        int MinY
        {
            get;
        }

        int MaxY
        {
            get;
        }

        bool areFieldCoordinatesValid(IField place);

        bool areFieldCoordinatesValid(int x, int y);

        IField getFieldI(int X, int Y);
    }
}
