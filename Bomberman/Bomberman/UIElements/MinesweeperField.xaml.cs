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

namespace Bomberman.UIElements
{
    /// <summary>
    /// Interaction logic for BombermanField.xaml
    /// </summary>
    public partial class BombermanField : UserControl
    {
        static Color[] s_bgColors = { Colors. Black, Colors.Blue, Colors.Green, Colors.Maroon, Colors.Olive, Colors.Crimson, Colors.Crimson, Colors.Crimson, Colors.Crimson };
        delegate void HighlightDelegate(Color color);

        Field m_field;
        GameController m_gameController;

        public Field FieldP
        {
            get
            {
                return m_field;
            }
        }

        public BombermanField(Field field, GameController gameController)
        {
            InitializeComponent();

            m_field = field;
            m_gameController = gameController;

            field.OnDraw += OnDraw;
            field.OnHighlighted += OnHighlighted;
            field.OnUnHighlighted += OnUnHighlighted;
        }

        public void ReInit(Field field, GameController gameController)
        {
            m_field = field;
            m_gameController = gameController;
        }

        private void OnDraw()
        {
            try
            {
                Dispatcher.Invoke(Draw);
            }
            catch (System.Threading.Tasks.TaskCanceledException)
            {
                if (m_gameController != null)
                    m_gameController.Finish();
            }
        }

        private void Draw()
        {
            //show bombs
            if (m_field.isBombed())
            {
                E_BombImage.Visibility = Visibility.Visible;
            }
            else
            {
                E_BombImage.Visibility = Visibility.Hidden;
            }
            //show players
            const int imageCount = 4;
            Image[] imageReferences = new Image[imageCount]; //There is 4 png player image
            imageReferences[0] = E_RedPlayerImage;
            imageReferences[1] = E_BluePlayerImage;
            imageReferences[2] = E_GreenPlayerImage;
            imageReferences[3] = E_YellowPlayerImage;
            for (int i = 0; i < m_field.NumberOfTeams; ++i)
            {
                int imageIdx = i % imageCount;
                if (m_field.getTeamsI()[i] != null)
                {
                    if (m_field.getTeamsI()[i].IsAlive)
                    {
                        imageReferences[imageIdx].Visibility = Visibility.Visible;
                    }
                    else
                    {
                        imageReferences[imageIdx].Visibility = Visibility.Hidden;
                    }
                }
                else
                {
                    imageReferences[imageIdx].Visibility = Visibility.Hidden;
                }
            }

            //show explosions
            if (m_field.IsExploding)
            {
                E_ExplosionImage.Visibility = Visibility.Visible;
            }
            else
            {
                E_ExplosionImage.Visibility = Visibility.Hidden;
            }

            //show walls
            if (m_field.isWall())
            {
                E_WallImage.Visibility = Visibility.Visible;
            }
            else
            {
                E_WallImage.Visibility = Visibility.Hidden;
            }
        }

        private void OnUnHighlighted()
        {
            Dispatcher.Invoke(UnHighlight);
        }

        private void OnHighlighted(Color color)
        {
            Dispatcher.Invoke(new HighlightDelegate(Highlight), color);
        }

        public void Highlight(Color color)
        {
            E_BaseBorder.BorderBrush = new SolidColorBrush(color);
            E_BaseBorder.BorderThickness = new Thickness(3);
            E_MainContainer.Margin = new Thickness(0);
        }

        public void UnHighlight()
        {
            E_BaseBorder.BorderBrush = new SolidColorBrush(Colors.Black);
            E_BaseBorder.BorderThickness = new Thickness(2);
            E_MainContainer.Margin = new Thickness(1);
        }
    }
}
