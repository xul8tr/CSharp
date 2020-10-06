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
    /// Interaction logic for TeamIndicator.xaml
    /// </summary>
    public partial class TeamIndicator : UserControl
    {
        Team m_team;
        System.Windows.Media.Brush m_aliveBackground;
        System.Windows.Media.Brush m_aliveForeground;
        System.Windows.Media.Brush m_aliveTeamNameForeground;
        bool m_isDead = false;

        public TeamIndicator(Team team)
        {
            InitializeComponent();

            m_team = team;
            E_ActiveImage.DataContext = team;
            E_TeamNameLabel.DataContext = team;
            E_ColorBorder.DataContext = team;
            E_Points.DataContext = team;

            team.OnDeactivate += Team_OnDeactivate;
            team.OnActive += Team_OnActive;
            team.OnDeath += Team_OnDeath;
            team.OnResetField += Team_OnResetField;
        }

        private void Team_OnDeath()
        {
            Dispatcher.Invoke(OnDeath);
        }

        private void Team_OnResetField()
        {
            Dispatcher.Invoke(OnResetField);
        }

        private void saveAliveBackgrounds()
        {
            if (!m_isDead)
            {
                m_aliveBackground = E_ColorBorder.Background;
                m_aliveForeground = E_Points.Foreground;
                m_aliveTeamNameForeground = E_TeamNameLabel.Foreground;
            }
        }

        private void reloadAliveBackGrounds()
        {
            if (m_isDead)
            {
                E_ColorBorder.Background = m_aliveBackground;
                E_Points.Foreground = m_aliveForeground;
                E_TeamNameLabel.Foreground = m_aliveTeamNameForeground;
            }
        }

        private void OnDeath()
        {
            if (!m_isDead)
            {
                saveAliveBackgrounds();
            }
            E_ColorBorder.Background = new SolidColorBrush(Colors.Black);
            E_Points.Foreground = new SolidColorBrush(Colors.White);
            E_TeamNameLabel.Foreground = new SolidColorBrush(Colors.White);
            m_isDead = true;
        }

        private void OnResetField()
        {
            if (m_isDead)
            {
                reloadAliveBackGrounds();
            }
            m_isDead = false;
        }

        private void Team_OnActive()
        {
            Dispatcher.Invoke(OnActive);
        }

        private void OnActive()
        {
            E_ActiveImage.Visibility = Visibility.Visible;
        }

        private void OnDeactivate()
        {
            E_ActiveImage.Visibility = Visibility.Hidden;
        }

        private void Team_OnDeactivate()
        {
            Dispatcher.Invoke(OnDeactivate);
        }
    }
}
