using GUI_20212202_CM7A68.Controller;
using GUI_20212202_CM7A68.Logic;
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
            logic = new GameLogic();
            display.SetupModel(logic);
            controller = new GameController(logic);
            DispatcherTimer gametimer=new DispatcherTimer();
            gametimer.Interval = TimeSpan.FromMilliseconds(17);
            gametimer.Tick += Gametimer_Tick;
            gametimer.Start();
        }

        private void Gametimer_Tick(object? sender, EventArgs e)
        {
            logic.TimeStep();
            display.InvalidateVisual();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            display.SetupSize(new Size(mainGrid.ActualWidth, mainGrid.ActualHeight));
            logic.SetupSize(new System.Drawing.Size((int)mainGrid.ActualWidth, (int)mainGrid.ActualHeight));
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            display.SetupSize(new Size(mainGrid.ActualWidth, mainGrid.ActualHeight));
            logic.SetupSize(new System.Drawing.Size((int)mainGrid.ActualWidth, (int)mainGrid.ActualHeight));
        }
    }
}
