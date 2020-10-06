
using System.Collections.Generic;


namespace BombermanBasics.Interfaces
{
    public enum FieldStateEnum
    {
        IS_EMPTY,
        IS_BOMBED,
        IS_PLAYER_ON_FIELD,
        IS_WALL
    }

    public interface IField
    {
        bool isEmpty();

        bool isWall();

        /// <summary>
        /// returns the teams if getState() is IS_PLAYER_ON_FIELD,
        /// empty array otherwise
        /// </summary>
        /// <returns></returns>
        ITeam[] getTeamsI();

        bool isAnyTeamOnField();

        /// <summary>
        /// returns the bombs of the field.
        /// </summary>
        /// <returns></returns>
        List<IBomb> getBombsI();

        /// <summary>
        /// returns if the field contains the given team or not
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        bool contains(ITeam team);

        /// <summary>
        /// returns if the field contains the given bomb or not
        /// </summary>
        /// <param name="bomb"></param>
        /// <returns></returns>
        bool contains(IBomb bomb);

        /// <summary>
        /// get the bomb of the given team if the field is bombed
        /// </summary>
        /// <param name="team"></param>
        /// <returns>null if the field is not bombed</returns>
        IBomb getBombI(ITeam team);

        bool isBombed();

        bool isNeighbour(IField other);

        int X { get; }
        int Y { get; }
    }
}
