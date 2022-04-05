using GUI_20212202_CM7A68.Logic;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GUI_20212202_CM7A68.MenuWindows.ViewModels
{
    public class MainMenuWindowViewModel
    {
        //dp injection: ablak megkapja a logicot, datacontext.setupmodel metúdussal megkapja ez az osztály
        IGameModel logic;
        public void SetupLogic(IGameModel model)
        {
            this.logic = model;
        }
        public List<string> Maps { get; set; }
        public List<string> PlayerOneSkins { get; set; }
        public List<string> PlayerTwoSkins { get; set; }

        public string PlayerOneName { get; set; }
        public string PlayerTwoName { get; set; }
        public string SelectedMapRoute { get; set; }
        public string SelctedPlayerOneSkinRoute { get; set; }
        public string SelectedPlayerTwoSkinRoute { get; set; }
        public ICommand StartGameCommand { get; set; }
        public MainMenuWindowViewModel()
        {
            var asd = Directory.GetCurrentDirectory();
            Maps = Directory.GetFiles(Path.Combine(asd,"Renderer", "Images", "Backgrounds"), "*.jpg").ToList();
            PlayerOneSkins = Directory.GetFiles(Path.Combine(asd,"Renderer", "Images", "Robots"), "*.png").ToList();
            PlayerTwoSkins = Directory.GetFiles(Path.Combine(asd,"Renderer", "Images", "Robots"), "*.png").ToList();
            PlayerOneName = "PlayerOne";
            PlayerTwoName = "PlayerTwo";
            this.StartGameCommand = new RelayCommand(
                () =>
                {
                    logic.Player1Name = PlayerOneName;
                    logic.Player2Name = PlayerTwoName;
                    logic.SelectedMapPath = SelectedMapRoute;
                }
                );
            ;
        }
    }
}
