using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BombermanBasics.Interfaces;
using System.Drawing;

namespace TestAIPlugin
{
    public class TestAIPlugin : IAIPlugin
    {
        private IGameController m_gameController;
        private IGameBoard m_gameBoard;
        private ITeam m_thisTeam;

        public void Initialize(IGameController controller)
        {
            m_gameController = controller;
        }

        public void Initialize(ITeam thisTeam)
        {
            m_thisTeam = thisTeam;
        }
        public void GameStarted()
        {
            m_gameBoard = m_gameController.IgameBoard;
        }

        public AIStep RequireStep(int seed)
        {
            //How to get the place of your player
            //m_thisTeam.FieldI.X;
            //m_thisTeam.FieldI.Y;

            //m_thisTeam.getBombsI();   //get all of your bombs
            //m_gameController.isExplodable(m_gameBoard.getFieldI(5, 6));   //is (5, 6) field explodable (walls are not explodable)
            //m_gameController.isValidToBomb(m_gameBoard.getFieldI(2, 4), m_gameBoard, m_thisTeam);    //can you put bomb onto field (2, 4) or not

            //How to get all of the players of the game (including your team)
            //m_gameController.getTeamsI();
            //m_gameController.getTeamsI().Length;  //count of the teams in the game
            //m_gameController.getTeamsI()[2].IsAlive;    //is the given team still alive or not

            //m_gameController.getTeamsI()[2].getBombsI();  //returns all of the bombs of team 2

            //How to get the sizes of gameboard
            //m_gameBoard.Size;   //== MaxX - MinX + 1
            //m_gameBoard.MinX;   //the smallest valid x coordinate on the board
            //m_gameBoard.MaxX;   //the largest valid x coordinate on the board
            //m_gameBoard.MinY;
            //m_gameBoard.MaxY;
            //How to get a given field by coordinates, returns null if the coordinates are not valid
            //m_gameBoard.getFieldI(X, Y);

            //field examples
            //m_gameBoard.getFieldI(5, 8).getBombsI();  //return all of the bombs of field (5, 8)

            //bomb examples
            //List<IBomb> allBombs = m_gameController.getBombsI();    //get all of the bombs on the board
            //IBomb bomb = m_gameBoard.getFieldI(5, 8).getBombsI().ToArray[1];  //range check required before ToArray()
            //bomb.getOwnerI();   //get the owner team of a given bomb
            //bomb.FieldI.X;        //the place of the bomb
            //bomb.FieldI.Y;
            //bomb.getExplosiveRadius();    //the explosive radius of the given bomb

            Random random = new Random(seed);

            AIStep response = new AIStep();
            int nextStepi = random.Next(typeof(AIStep.StepEnum).GetEnumValues().Length);
            int nextMovei = random.Next(typeof(AIStep.MoveEnum).GetEnumValues().Length);

            response.nextStep = (AIStep.StepEnum)nextStepi;
            //response.nextStep = AIStep.StepEnum.MOVE;
            response.nextMove = (AIStep.MoveEnum)nextMovei;

            return response;
        }

        public void Finish(bool won)
        {

        }
    }
}
