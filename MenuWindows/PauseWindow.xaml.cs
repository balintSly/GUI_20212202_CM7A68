using GUI_20212202_CM7A68.Logic;
using GUI_20212202_CM7A68.MenuWindows.ViewModels;
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
    /// Interaction logic for PauseWindow.xaml
    /// </summary>
    public partial class PauseWindow : Window
    {
        public PauseWindow(IGameModel model)
        {
            InitializeComponent();
            (this.DataContext as PauseWindowViewModel).SetupModel(model);
        }

        private void ResumeGame(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void ExitGame(object sender, RoutedEventArgs e)
        {
            this.DialogResult= false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Escape)
            {
                this.DialogResult = true;
            }
        }
    }
}
