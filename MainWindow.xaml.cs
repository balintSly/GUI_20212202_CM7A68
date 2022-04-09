using GUI_20212202_CM7A68.Controller;
using GUI_20212202_CM7A68.Logic;
using GUI_20212202_CM7A68.MenuWindows;
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
using System.Windows.Threading;

namespace GUI_20212202_CM7A68
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        GameLogic logic;
        GameController controller;
        public MainWindow()
        {
            InitializeComponent();
            InitGame();
            var menu=new MainMenuWindow(this.logic);
            if (menu.ShowDialog()==false)
            {
                Application.Current.Shutdown();
            }
          
        }
        public void InitGame()
        {
            logic = new GameLogic();
            logic.InitLogic();
            display.SetupModel(logic);
            controller = new GameController(logic);
            DispatcherTimer gametimer = new DispatcherTimer();
            gametimer.Interval = TimeSpan.FromMilliseconds(17);
            gametimer.Tick += Gametimer_Tick;
            gametimer.Start();

            logic.SetupSize(new Size((int)mainGrid.ActualWidth, (int)mainGrid.ActualHeight));
        }

        private void Gametimer_Tick(object? sender, EventArgs e)
        {
            if (logic.GamePaused)
            {
                var pausemenu = new PauseWindow(this.logic);
                if (pausemenu.ShowDialog() == false)
                {
                    var mainmenu = new MainMenuWindow(this.logic);
                    if (mainmenu.ShowDialog() == true)
                    {
                        logic.InitLogic();
                    }
                    else
                    {
                        Application.Current.Shutdown();
                    }
                }
                else
                {
                    logic.GamePaused = false;
                }
            }
            else 
            {
                logic.TimeStep();
                display.TimeFromGameStart += TimeSpan.FromSeconds(17);// egy időztő van, kb. összeadjuk a delayeket, és durván másodpercenként kivonunk 1 secet az alap 3 percből
                if (display.TimeFromGameStart.TotalSeconds % 680 == 0 && display.GameStarted && !logic.GamePaused)
                {
                    logic.RoundTime -= TimeSpan.FromSeconds(1); //csökkentjük a köridőt 1 seccel
                }
            }
            display.InvalidateVisual();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            display.SetupSize(new Size(mainGrid.ActualWidth, mainGrid.ActualHeight));
            logic.SetupSize(new Size((int)mainGrid.ActualWidth, (int)mainGrid.ActualHeight));
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            display.SetupSize(new Size(mainGrid.ActualWidth, mainGrid.ActualHeight));
            logic.SetupSize(new Size((int)mainGrid.ActualWidth, (int)mainGrid.ActualHeight));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            controller.KeyPressed(e.Key);
            display.InvalidateVisual();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            controller.KeyReleased(e.Key);
            display.InvalidateVisual();
        }
    }
}
