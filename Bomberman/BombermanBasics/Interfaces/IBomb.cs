namespace BombermanBasics.Interfaces
{
    public interface IBomb
    {
        int getExplosiveRadius();

        ITeam getOwnerI();

        IField FieldI
        {
            get;
        }
    }
}
