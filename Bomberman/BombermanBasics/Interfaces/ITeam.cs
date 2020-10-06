namespace BombermanBasics.Interfaces
{
    public interface ITeam
    {
        string Name
        {
            get;
        }

        int Points
        {
            get;
        }

        bool IsAlive
        {
            get;
        }

        //returns the place of the player
        IField FieldI
        {
            get;
        }

        bool ownsBomb(IBomb bomb);

        IBomb[] getBombsI();

        int getNumberOfBombs();
    }
}
