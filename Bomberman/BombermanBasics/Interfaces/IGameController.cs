using System.Collections.Generic;

namespace BombermanBasics.Interfaces
{
    public interface IGameController
    {
        IGameBoard IgameBoard { get; }

        bool isValidToBomb(IField field, IGameBoard gameBoard, ITeam bombingTeam);

        bool isValidToMove(IField field, IGameBoard gameBoard, ITeam movingTeam);

        /// <summary>
        /// get every team of the gameplay
        /// </summary>
        /// <returns></returns>
        ITeam[] getTeamsI();

        /// <summary>
        /// get all of the bombs of the gameBoard
        /// </summary>
        /// <returns></returns>
        List<IBomb> getBombsI();

        int MaxCountOfPlayerBombs
        {
            get;
        }

        int getExplosiveRadius();

        bool isExplodable(IField field);

        List<IField> getDangerousFields();

        /// <summary>
        /// Moves the given coordinates ('inX' and 'inY') to the given direction.
        /// returns true if the step was inside the boundaries of gameboard.
        /// otherwise returns false and x=0, y=0.
        /// It does not care the content of the target field (wall, etc).
        /// </summary>
        /// <param name="inX"></param>
        /// <param name="inY"></param>
        /// <param name="step"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        bool moveCoordinates(int inX, int inY, AIStep.MoveEnum step, out int x, out int y);
    }
}
