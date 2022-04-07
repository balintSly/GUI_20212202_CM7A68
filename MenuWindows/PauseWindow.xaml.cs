﻿using GUI_20212202_CM7A68.Logic;
using GUI_20212202_CM7A68.MenuWindows.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
        public int ButtonFontSize { get; set; }
        public int TitleFontSize { get; set; }
        public PauseWindow(IGameModel model)
        {
            ButtonFontSize = 40;
            TitleFontSize = (int)(ButtonFontSize * 1.2);
            InitializeComponent();
            (this.DataContext as PauseWindowViewModel).SetupModel(model);
        }

        private void ResumeGame(object sender, RoutedEventArgs e)
        {
            SoundPlayer s = new SoundPlayer(System.IO.Path.Combine("MenuWindows", "Sounds", "SAmenuClick.wav"));
            s.Play();
            this.DialogResult = true;
        }

        private void ExitGame(object sender, RoutedEventArgs e)
        {
            SoundPlayer s = new SoundPlayer(System.IO.Path.Combine("MenuWindows", "Sounds", "SAmenuClick.wav"));
            s.Play();
            this.DialogResult= false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Escape)
            {
                SoundPlayer s = new SoundPlayer(System.IO.Path.Combine("MenuWindows", "Sounds", "SAmenuClick.wav"));
                s.Play();
                this.DialogResult = true;
            }
        }

        private void MouseOnButton(object sender, MouseEventArgs e)
        {
            SoundPlayer s = new SoundPlayer(System.IO.Path.Combine("MenuWindows", "Sounds", "SAmenuSelect.wav"));
            s.Play();
        }

        private void pausemenu_Loaded(object sender, RoutedEventArgs e)
        {
            maingrid.Background = new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine("MenuWindows", "Images", "mortalbombatmainmenubackg.jpg"), UriKind.RelativeOrAbsolute)));
        }
    }
}
