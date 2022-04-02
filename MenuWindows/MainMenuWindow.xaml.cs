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
using System.Windows.Shapes;

namespace GUI_20212202_CM7A68.MenuWindows
{
    /// <summary>
    /// Interaction logic for MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : Window
    {
        public MainMenuWindow()
        {
            InitializeComponent();
        }

        private void BackToMainMenu(object sender, RoutedEventArgs e)
        {
            var asd = borderGrid.Children.OfType<Grid>();
            foreach (Grid g in asd)
                g.Visibility = Visibility.Collapsed;
            mainGrid.Visibility = Visibility.Visible;
            ;
        }

        private void NewGame(object sender, RoutedEventArgs e)
        {
            mainGrid.Visibility=Visibility.Collapsed;
            newGameGrid.Visibility=Visibility.Visible;
        }

        private void Leaderboard(object sender, RoutedEventArgs e)
        {
            mainGrid.Visibility = Visibility.Collapsed;
            leaderboardGrid.Visibility = Visibility.Visible;
        }

        private void Description(object sender, RoutedEventArgs e)
        {
            mainGrid.Visibility = Visibility.Collapsed;
            descriptionGrid.Visibility = Visibility.Visible;
        }

        private void Options(object sender, RoutedEventArgs e)
        {
            mainGrid.Visibility = Visibility.Collapsed;
            optionsGrid.Visibility = Visibility.Visible;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            DialogResult=false;
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
