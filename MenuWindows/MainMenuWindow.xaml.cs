using GUI_20212202_CM7A68.Logic;
using GUI_20212202_CM7A68.MenuWindows.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Media;

namespace GUI_20212202_CM7A68.MenuWindows
{
    /// <summary>
    /// Interaction logic for MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : Window
    {
        public MainMenuWindow(IGameModel model)
        {
            ButtonFontSize = 64;
            TitleFontSize = (int)(ButtonFontSize * 1.2);
            ContentFontSize = (int)(ButtonFontSize * 0.8);
            InitializeComponent();
            (this.DataContext as MainMenuWindowViewModel).SetupLogic(model);

        }
        public double MapSize { get; set; }
        public int ButtonFontSize { get; set; }
        public int TitleFontSize { get; set; }
        public int ContentFontSize { get; set; }
        public double CharachterSize { get; set; }
        private void BackToMainMenu(object sender, RoutedEventArgs e)
        {
            SoundPlayer s = new SoundPlayer(System.IO.Path.Combine("MenuWindows", "Sounds", "SAmenuClick.wav"));
            s.Play();
            var asd = borderGrid.Children.OfType<Grid>();
            foreach (Grid g in asd)
                g.Visibility = Visibility.Collapsed;
            mainGrid.Visibility = Visibility.Visible;
            ;
        }

        private void NewGame(object sender, RoutedEventArgs e)
        {
            SoundPlayer s = new SoundPlayer(System.IO.Path.Combine("MenuWindows", "Sounds", "SAmenuClick.wav"));
            s.Play();
            mainGrid.Visibility = Visibility.Collapsed;
            newGameGrid.Visibility = Visibility.Visible;
        }

        private void Leaderboard(object sender, RoutedEventArgs e)
        {
            SoundPlayer s = new SoundPlayer(System.IO.Path.Combine("MenuWindows", "Sounds", "SAmenuClick.wav"));
            s.Play();
            mainGrid.Visibility = Visibility.Collapsed;
            leaderboardGrid.Visibility = Visibility.Visible;
            
        }

        private void Description(object sender, RoutedEventArgs e)
        {
            SoundPlayer s = new SoundPlayer(System.IO.Path.Combine("MenuWindows", "Sounds", "SAmenuClick.wav"));
            s.Play();
            mainGrid.Visibility = Visibility.Collapsed;
            descriptionGrid.Visibility = Visibility.Visible;
            
        }

        private void Options(object sender, RoutedEventArgs e)
        {
            SoundPlayer s = new SoundPlayer(System.IO.Path.Combine("MenuWindows", "Sounds", "SAmenuClick.wav"));
            s.Play();
            mainGrid.Visibility = Visibility.Collapsed;
            optionsGrid.Visibility = Visibility.Visible;
            
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            SoundPlayer s = new SoundPlayer(System.IO.Path.Combine("MenuWindows", "Sounds", "SAmenuClick.wav"));
            s.Play();
            DialogResult = false;
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            SoundPlayer s = new SoundPlayer(System.IO.Path.Combine("MenuWindows", "Sounds", "SAmenuClick.wav"));
            s.Play();
            this.DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MapSize = (e.Source as Window).ActualWidth / 8.6;
            this.ButtonFontSize = (int)((e.Source as Window).ActualWidth / 30);
            this.CharachterSize = (e.Source as Window).ActualWidth / 14.19;
            borderGrid.Background = new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine("MenuWindows", "Images", "mortalbombatmainmenubackg.jpg"), UriKind.RelativeOrAbsolute)));

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if ((e.Source as Window).ActualWidth > 0)
            {
                this.MapSize = (e.Source as Window).ActualWidth / 8.6;
                this.CharachterSize = (e.Source as Window).ActualWidth / 14.19;
            }
        }
        private void MouseOnButton(object sender, MouseEventArgs e)
        {
            SoundPlayer s = new SoundPlayer(System.IO.Path.Combine("MenuWindows", "Sounds", "SAmenuSelect.wav"));
            s.Play();
        }
        private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SoundPlayer s = new SoundPlayer(System.IO.Path.Combine("MenuWindows", "Sounds", "SAselectionChanged.wav"));
            s.Play();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SoundPlayer s = new SoundPlayer(System.IO.Path.Combine("MenuWindows", "Sounds", "typewritersound.wav"));
            s.Play();
        }
    }
}
