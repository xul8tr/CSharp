using System;
using System.Collections.Generic;
using BombermanBasics.Interfaces;
using System.Reflection;
using System.Drawing;
using BombermanBasics;
using System.Threading;
using System.ComponentModel;
using System.Windows.Media;

namespace Bomberman
{
    public class Team: BombermanBasics.Team, INotifyPropertyChanged
    {
        public static System.Windows.Media.Color[] s_colors = { Colors.Red, Colors.CornflowerBlue, Colors.LimeGreen, Colors.Yellow };

        public delegate void SimpleEvent();
        public event SimpleEvent OnActive;
        public event SimpleEvent OnDeactivate;
        public event SimpleEvent OnDeath;
        public event SimpleEvent OnResetField;
        public event PropertyChangedEventHandler PropertyChanged;

        private bool m_isDead;
        private int m_points;

        public System.Windows.Media.Color TeamColor { get; set; }
        public System.Windows.Media.Brush TeamColorBrush { get; set; }
        public IAIPlugin AIPlugin { get; set; }
        public override bool IsDead
        {
            get
            {
                return m_isDead;
            }
            set
            {
                if (value == true && !m_isDead)
                {
                    m_isDead = true;
                    if (OnDeath != null)
                    {
                        OnDeath();
                    }
                }
                else if (value == false && m_isDead)    //reset to alive
                {
                    m_isDead = false;
                    if (OnResetField != null)
                    {
                        OnResetField();
                    }
                }
            }
        }
        public override int Points   //overrides the BombermanBasics.Team.Points property
        {
            get
            {
                return m_points;
            }
            set
            {
                m_points = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Points"));  //Todo: check the initialization of this delegate
            }
        }
        public bool Active
        {
            set
            {
                if (value)
                {
                    if (OnActive != null)
                    {
                        OnActive();
                    }
                }
                else
                {
                    if (OnDeactivate != null)
                    {
                        OnDeactivate();
                    }
                }
            }
        }

        public Team(string name,
                    System.Windows.Media.Color teamColor,
                    IAIPlugin aiPlugin) : base(name)
        {
            TeamColor = teamColor;
            TeamColorBrush = new SolidColorBrush(TeamColor);
            AIPlugin = aiPlugin;
        }
    }

    public class GameController : IGameController
    {
        public bool isValidToBomb(IField field, IGameBoard gameBoard, ITeam bombingTeam)
        {
            if (!gameBoard.areFieldCoordinatesValid(field))
            {
                return false;
            }
            if (bombingTeam.FieldI != field) //only the field of the bombing team can be bombed
            {
                return false;
            }
            if (field.getBombI(bombingTeam) != null)
                return false;
            if (field.isWall())
                return false;
            return true;
        }

        public bool isValidToMove(IField target, IGameBoard gameBoard, ITeam movingTeam)
        {
            if (!gameBoard.areFieldCoordinatesValid(target))
                return false;
            if (target == null)
                return false;
            if (movingTeam == null)
                return false;
            if (movingTeam.FieldI == null)
                return false;

            if (!movingTeam.FieldI.isNeighbour(target))
                return false;

            if (target.isWall()) //if he target field is wall
                return false;
            return true;
        }

        public ITeam[] getTeamsI()
        {
            return m_teams.ToArray();
        }

        public List<IBomb> getBombsI()
        {
            List<IBomb> retVal = new List<IBomb>();
            Team[] teams = m_teams.ToArray();
            for (int i = 0; i < m_teams.Count; ++i)
            {
                IBomb[] bombsOfTeam = teams[i].getBombsI();
                for (int j = 0; j < bombsOfTeam.Length; ++j)
                    retVal.Add(bombsOfTeam[j]);
            }
            return retVal;
        }

        public int getExplosiveRadius()
        {
            if (m_configHandler != null)
                return m_configHandler.getExplosiveRadius();
            else
                return 0;
        }

        public bool isExplodable(IField field)
        {
            if (!field.isWall())
                return true;
            return false;
        }

        public List<IField> getDangerousFields()
        {
            List<IField> responseList = new List<IField>();
            foreach (Team team in m_teams)
            {
                Bomb[] currentBombs = team.getBombs();
                foreach (Bomb currentBomb in currentBombs)
                {
                    List<BombermanBasics.Field> currentList = Explosion.getExplodedFields(currentBomb, m_gameBoard, this);
                    responseList.AddRange(currentList);
                }
            }
            return responseList;
        }

        int m_waitTimeBetweenStepsMs;

        List<Team> m_teams;

        ConfigHandler m_configHandler;
        BombermanBasics.GameBoard m_gameBoard;

        Thread m_gameControllerThread;
        ManualResetEvent m_controllerThreadEvent;

        bool m_gameFinished = false;

        public int StepsElapsed { get; set; }

        public int MaxCountOfPlayerBombs
        {
            get
            {
                return m_configHandler.getMaxCountOfPlayerBombs();
            }
        }

        public IGameBoard IgameBoard
        {
            get
            {
                return m_gameBoard;
            }
        }

        public GameBoard gameBoard
        {
            get
            {
                return m_gameBoard;
            }
        }

        internal List<Team> Teams
        {
            get
            {
                return m_teams;
            }
        }

        public GameController(ConfigHandler configHandler)
        {
            m_configHandler = configHandler;
        }

        public bool Initialize()
        {
            bool result = LoadTeamsAndPlugins();

            if (result)
            {
                m_waitTimeBetweenStepsMs = m_configHandler.WaitTimeBetweenStepsMS;
                result = CreateBoard();
            }

            return result;
        }

        private bool LoadTeamsAndPlugins()
        {
            m_teams = new List<Team>();
            foreach (var team in m_configHandler.Teams)
            {
                IAIPlugin aiPlugin = InitializeAIPlugin(team.DLLPath);

                if (aiPlugin != null)
                {
                    Team teamToAdd = new Team(team.Name,
                                              Team.s_colors[m_teams.Count],
                                              aiPlugin);
                    m_teams.Add(teamToAdd);
                    aiPlugin.Initialize(teamToAdd);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private IAIPlugin InitializeAIPlugin(string path)
        {
            Assembly pluginDLL = Assembly.LoadFrom(path);
            if (pluginDLL != null)
            {
                Type[] types = pluginDLL.GetExportedTypes();

                for (int i = 0; i < types.Length; i++)
                {
                    Type type = types[i];
                    if (type.GetInterface("BombermanBasics.Interfaces.IAIPlugin") != null && type != null)
                    {
                        IAIPlugin plugin = pluginDLL.CreateInstance(type.FullName) as IAIPlugin;
                        if (plugin != null)
                        {
                            plugin.Initialize(this);
                            return plugin;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        internal void Finish()
        {
            m_gameFinished = true;

            foreach (var team in m_teams)
            {
                team.AIPlugin.Finish(false);
            }
        }

        internal void Resume()
        {
            m_controllerThreadEvent.Set();
        }

        internal void Pause()
        {
            m_controllerThreadEvent.Reset();
        }

        internal void StartGame()
        {
            m_controllerThreadEvent = new ManualResetEvent(true);

            m_gameControllerThread = new Thread(new ThreadStart(ControllerThread_Work));
            m_gameControllerThread.Start();
        }

        private int GetNumberOfTeamsDead()
        {
            int counter = 0;
            foreach (var team in m_teams)
            {
                if (team.IsDead) ++counter;
            }

            return counter;
        }

        private void ControllerThread_Work()
        {
            foreach (var team in m_teams)
            {
                team.AIPlugin.GameStarted();
            }
            drawMap();
            AIStep[] nextSteps = new AIStep[m_teams.Count];
            int seed = 0;
            while (!m_gameFinished)
            {
                Thread.Sleep(m_waitTimeBetweenStepsMs);
                m_controllerThreadEvent.WaitOne();

                for (int i = 0; i < nextSteps.Length; ++i)
                {
                    if (m_teams[i].IsAlive)
                    {
                        nextSteps[i] = m_teams[i].AIPlugin.RequireStep(seed);
                        ++seed;
                    }
                }
                ProcessSteps(nextSteps);
                drawMap();
                postProcessMap();   //remove explosion fires, etc...
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nextSteps">it contains the chosen steps of the given teams</param>
        private void ProcessSteps(AIStep[] nextSteps)
        {
            //process the explosions first
            for (int i = 0; i < nextSteps.Length; ++i)
            {
                Team currentPlayer = m_teams.ToArray()[i];
                if (currentPlayer == null)
                    continue;
                if (currentPlayer.Field == null)
                    continue;
                if (currentPlayer.IsDead)   //dead players doesn't do anything
                    continue;
                if (nextSteps[i].nextStep == AIStep.StepEnum.EXPLODE)
                {
                    Bomb[] currentPlayerBombs = currentPlayer.getBombs();
                    ExplosionFieldCollector explodedFields = new ExplosionFieldCollector();
                    for (int j = 0; j < currentPlayerBombs.Length; ++j)
                    {
                        m_gameBoard.explodeFields(this, currentPlayerBombs[j], explodedFields);
                    }
                    processExplodedFields(i, currentPlayer, explodedFields);
                }
            }
            //process the moves after explosions
            for (int i = 0; i < nextSteps.Length; ++i)
            {
                Team currentPlayer = m_teams.ToArray()[i];
                if (currentPlayer == null)
                    continue;
                if (currentPlayer.Field == null)
                    continue;
                if (currentPlayer.IsDead)   //players dead from the explosions doesn't do anything
                    continue;
                if (nextSteps[i].nextStep == AIStep.StepEnum.PUT_BOMB)
                {
                    bool isValid = false;
                    if (MaxCountOfPlayerBombs <= 0 || (MaxCountOfPlayerBombs > 0 && currentPlayer.getNumberOfBombs() < MaxCountOfPlayerBombs))
                    {
                        Bomb newBomb = new Bomb(currentPlayer, m_gameBoard, currentPlayer.Field, this, getExplosiveRadius(), out isValid);
                    }
                }
                else if (nextSteps[i].nextStep == AIStep.StepEnum.MOVE)
                {
                    Field target;
                    if (getTargetField(currentPlayer.Field.X, currentPlayer.Field.Y, nextSteps[i].nextMove, out target))
                    {
                        if (isValidToMove(target, m_gameBoard, currentPlayer))
                        {
                            currentPlayer.Field.removeTeam(currentPlayer);
                            target.addTeam(currentPlayer, i);
                        }
                    }
                }
            }
        }

        void processExplodedFields(int teamIdx, Team team, ExplosionFieldCollector explodedFields)
        {
            int exceptionCntr = 0;
            while (!explodedFields.isEmpty())
            {
                ++exceptionCntr;
                if (exceptionCntr > 1000)
                {
                    throw new Exception("GameController.processExplodedFields(): counter error");
                }
                List<ExplosionFieldCollector.OrderedFields> fieldsOrderedByDistance = explodedFields.popTheLeastTimeFactor();
                ExplosionFieldCollector.OrderedFields[] fieldsOrderedByDist = fieldsOrderedByDistance.ToArray();
                int distance = (fieldsOrderedByDistance.Count > 0) ? (fieldsOrderedByDist[0].DistanceFactor) : 0;
                int distancePointCntr = 0;
                for (int i = 0; i < fieldsOrderedByDist.Length; ++i)
                {
                    BombermanBasics.Field currentField = fieldsOrderedByDist[i].FieldP;
                    if (currentField != null)
                    {
                        List<BombermanBasics.Team> killedTeams;
                        currentField.killTeamsOnField(out killedTeams);
                        currentField.clearBombs();  //these bombs are now exploded

                        foreach (Team killedTeam in killedTeams)
                        {
                            int oldPoints = killedTeam.Points;
                            killedTeam.Points = oldPoints + distancePointCntr;    //the survived teams got their points
                        }
                        foreach (Team survivedTeam in m_teams)
                        {
                            int oldPoints = survivedTeam.Points;
                            survivedTeam.Points = oldPoints + GetNumberOfTeamsDead();    //the survived teams got their points
                        }
                    }
                    if (fieldsOrderedByDist[i].DistanceFactor > distance)
                    {
                        distance = fieldsOrderedByDist[i].DistanceFactor;
                        distancePointCntr++;
                    }
                }
            }
        }

        private bool getTargetField(int inX, int inY, AIStep.MoveEnum step, out Field target)
        {
            int tx = 0, ty = 0;
            bool ret = true;
            target = null;

            ret = moveCoordinates(inX, inY, step, out tx, out ty);
            if (ret == false)
                return false;

            target = m_gameBoard.getField(tx, ty) as Field;
            if (target == null)
                ret = false;
            return ret;
        }

        public bool moveCoordinates(int inX, int inY, AIStep.MoveEnum step, out int x, out int y)
        {
            if (step == AIStep.MoveEnum.UP)
            {
                x = inX;
                y = inY;
                if (gameBoard.areFieldCoordinatesValid(x, y + 1))
                {
                    y++;
                    return true;
                }
            }
            else if (step == AIStep.MoveEnum.DOWN)
            {
                x = inX;
                y = inY;
                if (gameBoard.areFieldCoordinatesValid(x, y - 1))
                {
                    y--;
                    return true;
                }
            }
            else if (step == AIStep.MoveEnum.LEFT)
            {
                x = inX;
                y = inY;
                if (gameBoard.areFieldCoordinatesValid(x - 1, y))
                {
                    x--;
                    return true;
                }
            }
            else if (step == AIStep.MoveEnum.RIGHT)
            {
                x = inX;
                y = inY;
                if (gameBoard.areFieldCoordinatesValid(x + 1, y))
                {
                    x++;
                    return true;
                }
            }
            x = 0;
            y = 0;
            return false;
        }

        private bool CreateBoard()
        {
            Bitmap mapImg = null;

            if (m_configHandler.MapOrigin == "generate")    //don't use generated filed in bomberman game
            {
                //mapSize = m_configHandler.MapSize;
            }
            else if (m_configHandler.MapOrigin == "file")
            {
                string path = "../../Resources/";
                path += m_configHandler.MapPath;
                mapImg = new Bitmap(path);
                //MapLoader loader = new MapLoader(m_configHandler.MapPath);
                //mapSize = loader.MapSize;
            }
            else
            {
                throw new Exception("Invalid value provided for 'mapOrigin' config.");
            }

            Func<int, int, BombermanBasics.Field> allocateField = (int x, int y) =>
            {
                return new Bomberman.Field(m_configHandler.Teams.Count, x, y);  //TODO: complete constructor like BombermanBasics.Field
            };
            m_gameBoard = new GameBoard(mapImg, allocateField);
            putTeamsToBoardOnGameStart();

            return true;
        }

        private void putTeamsToBoardOnGameStart()
        {
            Team[] teams = m_teams.ToArray();
            //put the teams into a random empty place on the board
            for (int i = 0; i < teams.Length; ++i)
            {
                Random random = new Random();
                int x, y;
                //find a random empty field
                do
                {
                    x = random.Next((int)m_gameBoard.MaxX - 1);
                    y = random.Next((int)m_gameBoard.MaxY - 1);
                } while (!m_gameBoard.getField(x, y).isEmpty());
                m_gameBoard.getField(x, y).addTeam(teams[i], i);
            }
        }

        private void postProcessMap()
        {
            for (int iX = m_gameBoard.MinX; iX < m_gameBoard.MaxX; ++iX)
            {
                for (int iY = m_gameBoard.MinY; iY < m_gameBoard.MaxY; ++iY)
                {
                    BombermanBasics.Field field = m_gameBoard.getField(iX, iY);
                    if (field != null)
                    {
                        field.IsExploding = false;
                    }
                }
            }
        }

        public void drawMap()
        {
            for (int iX = m_gameBoard.MinX; iX <= m_gameBoard.MaxX; ++iX)
            {
                for (int iY = m_gameBoard.MinY; iY <= m_gameBoard.MaxY; ++iY)
                {
                    Field field = m_gameBoard.getField(iX, iY) as Field;
                    if (field != null)
                    {
                        field.Draw();
                    }
                }
            }
        }
    }
}
