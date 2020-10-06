using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bomberman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ConfigHandler m_configHandler;
        GameController m_gameController;
        BombermanBasics.GameBoard m_gameBoard;
        bool m_isPlaying;
        bool m_isGameStarted;

        public MainWindow()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            m_configHandler = ConfigHandler.GetConfig();

            m_gameController = new GameController(m_configHandler);
            m_gameController.Initialize();
            m_gameBoard = m_gameController.gameBoard;

            E_StepsElapsedLabel.DataContext = m_gameController;
            E_MinesLeftLabel.DataContext = m_gameController;

            CreateGameBoard(m_gameController);

            m_isPlaying = false;
            m_isGameStarted = false;
            E_PlayImage.Visibility = Visibility.Visible;
            E_PauseImage.Visibility = Visibility.Hidden;

            var teams = m_gameController.Teams;
            foreach (var team in teams)
            {
                UIElements.TeamIndicator indicator = new UIElements.TeamIndicator(team);
                E_TeamsContainer.Children.Add(indicator);
                E_TeamsContainer.RowDefinitions.Add(new RowDefinition());
                Grid.SetRow(indicator, E_TeamsContainer.RowDefinitions.Count - 1);
            }
        }

        private void CreateGameBoard(GameController gameController)
        {
            for (int i = 0; i < m_gameBoard.Size; ++i)
            {
                E_GameBoard.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < m_gameBoard.Size; ++i)
            {
                E_GameBoard.RowDefinitions.Add(new RowDefinition());
            }

            for (int x = 0; x < m_gameBoard.Size; ++x)
            {
                for (int y = 0; y < m_gameBoard.Size; ++y)
                {
                    Bomberman.Field field = m_gameBoard.Fields[x][y] as Bomberman.Field;
                    if (field != null)
                    {
                        UIElements.BombermanField fieldToAdd = new UIElements.BombermanField(field, gameController);
                        E_GameBoard.Children.Add(fieldToAdd);
                        Grid.SetColumn(fieldToAdd, x);
                        Grid.SetRow(fieldToAdd, y);
                    }
                }
            }
        }

        private void PausePlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (!m_isGameStarted)
            {
                m_gameController.StartGame();
                m_isGameStarted = true;
                m_isPlaying = true;
                E_PlayImage.Visibility = Visibility.Hidden;
                E_PauseImage.Visibility = Visibility.Visible;
            }
            else
            {
                if (m_isPlaying)
                {
                    m_gameController.Pause();
                    E_PlayImage.Visibility = Visibility.Visible;
                    E_PauseImage.Visibility = Visibility.Hidden;
                }
                else
                {
                    m_gameController.Resume();
                    E_PlayImage.Visibility = Visibility.Hidden;
                    E_PauseImage.Visibility = Visibility.Visible;
                }

                m_isPlaying = !m_isPlaying;
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            m_gameController.Finish();
            m_gameController.Initialize();
            for (int i = 0; i < E_GameBoard.Children.Count; ++i)
            {
                UIElements.BombermanField uiField = E_GameBoard.Children[i] as UIElements.BombermanField;
                if (uiField != null)
                {
                    Field oldField = uiField.FieldP;
                    Field newField = m_gameController.gameBoard.getField(oldField.X, oldField.Y) as Field;
                    if (newField != null)
                        uiField.ReInit(newField, m_gameController);
                }
            }

            E_GameBoard.ColumnDefinitions.Clear();
            E_GameBoard.RowDefinitions.Clear();
            E_GameBoard.Children.Clear();

            E_TeamsContainer.RowDefinitions.Clear();
            E_TeamsContainer.Children.Clear();

            GC.Collect();

            Initialize();
        }
    }
}
