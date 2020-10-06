using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanBasics.Interfaces
{
    public class AIStep
    {
        public enum StepEnum
        {
            PUT_BOMB,   //put bomb onto the current field of the player
            EXPLODE,    //explode all of the bombs of the player
            MOVE,   //in this case, 'MoveEnum' describes the step
            STAY
        }
        public enum MoveEnum
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }

        public StepEnum nextStep
        {
            get;
            set;
        }

        public MoveEnum nextMove
        {
            get;
            set;
        }

        public AIStep()
        {
            nextStep = StepEnum.STAY;
            nextMove = MoveEnum.RIGHT;
        }
    }

    public interface IAIPlugin
    {
        void Initialize(IGameController controller);

        void Initialize(ITeam thisTeam);

        AIStep RequireStep(int seed);
        void Finish(bool won);

        void GameStarted();
    }
}
