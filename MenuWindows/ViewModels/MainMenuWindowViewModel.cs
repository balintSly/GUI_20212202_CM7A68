using GUI_20212202_CM7A68.Logic;
using GUI_20212202_CM7A68.Models;
using GUI_20212202_CM7A68.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GUI_20212202_CM7A68.MenuWindows.ViewModels
{
    public class MainMenuWindowViewModel : ObservableRecipient
    {
        IGameModel logic;

        public void SetupLogic(IGameModel model)
        {
            this.logic = model;
        }
        public List<string> Maps { get; set; }
        public ObservableCollection<string> PlayerOneSkins { get; set; }
        public ObservableCollection<string> PlayerTwoSkins { get; set; }
        public List<string> AllSkin { get; set; }
        public string PlayerOneName { get => playerOneName; set => SetProperty(ref playerOneName, value); }
        public string PlayerTwoName { get => playerTwoName; set => SetProperty(ref playerTwoName, value); }
        public string SelectedMapRoute { get; set; }

        public bool PlayerOneIsHuman { get => playerOneIsHuman; set => SetProperty(ref playerOneIsHuman, value); }
        public bool PlayerTwoIsHuman { get => playerTwoIsHuman; set => SetProperty(ref playerTwoIsHuman, value); }
        public bool PlayerOneIsAI
        {
            get => playerOneIsAI;
            set
            {
                playerOneIsAI = value;
                if (value)
                {
                    PlayerOneName = "BOT_LEFT";
                    PlayerOneIsHuman = false;
                }
                else
                {
                    PlayerOneName = "PlayerOne";
                    PlayerOneIsHuman = true;
                }
            }
        }
        public bool PlayerTwoIsAI
        {
            get => playerTwoIsAI;
            set
            {
                playerTwoIsAI = value;
                if (value)
                {
                    PlayerTwoName = "BOT_RIGHT";
                    PlayerTwoIsHuman = false;
                }
                else
                {
                    PlayerTwoName = "PlayerTwo";
                    PlayerTwoIsHuman = true;
                }
            }
        }

        private string selectedPlayerOneSkinRoute;
        public string SelectedPlayerOneSkinRoute
        {
            get => selectedPlayerOneSkinRoute;
            set
            {
                SetProperty(ref selectedPlayerOneSkinRoute, value);
                if (selectedPlayerOneSkinRoute != null)
                {
                    PlayerTwoSkins.Remove(selectedPlayerOneSkinRoute);
                    AllSkin.Where(x => x != selectedPlayerOneSkinRoute).ToList().ForEach(x =>
                    {
                        if (!PlayerTwoSkins.Contains(x))
                            PlayerTwoSkins.Add(x);
                    });
                }

            }
        }

        private string selectedPlayerTwoSkinRoute;
        private string playerOneName;
        private string playerTwoName;
        private bool playerOneIsAI;
        private bool playerTwoIsAI;
        private bool playerOneIsHuman;
        private bool playerTwoIsHuman;

        public string SelectedPlayerTwoSkinRoute
        {
            get => selectedPlayerTwoSkinRoute;
            set
            {
                SetProperty(ref selectedPlayerTwoSkinRoute, value);
                if (selectedPlayerTwoSkinRoute != null)
                {
                    PlayerOneSkins.Remove(selectedPlayerTwoSkinRoute);
                    AllSkin.Where(x => x != selectedPlayerTwoSkinRoute).ToList().ForEach(x =>
                    {
                        if (!PlayerOneSkins.Contains(x))
                            PlayerOneSkins.Add(x);
                    });
                }

            }
        }

        public ICommand StartGameCommand { get; set; }
        public ILeaderboardHandler LeaderboardHandler { get; set; }
        public ObservableCollection<Player> Players { get; set; }

        public MainMenuWindowViewModel(ILeaderboardHandler leaderboardHandler)
        {
            this.LeaderboardHandler = leaderboardHandler;
            this.Players = new ObservableCollection<Player>(this.LeaderboardHandler.GetLeaderboard());
            var asd = Directory.GetCurrentDirectory();
            Maps = Directory.GetFiles(Path.Combine(asd, "Renderer", "Images", "Backgrounds"), "*.jpg").ToList();

            AllSkin = Directory.GetFiles(Path.Combine(asd, "Renderer", "Images", "Robots"), "*stand_*").ToList();


            PlayerOneSkins = new ObservableCollection<string>(AllSkin.GetRange(0, 7));
            PlayerOneSkins.RemoveAt(1);
            PlayerTwoSkins = new ObservableCollection<string>(AllSkin.GetRange(1, 6));
            ;

            PlayerOneName = "PlayerOne";
            PlayerTwoName = "PlayerTwo";
            PlayerOneIsHuman = true;
            PlayerTwoIsHuman = true;

            this.StartGameCommand = new RelayCommand(
                () =>
                {
                    logic.SelectedMapPath = SelectedMapRoute;
                    logic.Players = new List<Player>();
                    logic.Players.Add(new Player(PlayerOneName, 0, 0) {SelectedColor= SelectedPlayerOneSkinRoute.Split('_')[4], IsPlayer=!PlayerOneIsAI });
                    logic.Players.Add(new Player(PlayerTwoName, 0, 0) {SelectedColor= SelectedPlayerTwoSkinRoute.Split('_')[4], IsPlayer=!PlayerTwoIsAI });
                    logic.InitLogic();
                }
                );
            ;
        }
        public MainMenuWindowViewModel() : this(Ioc.Default.GetService<ILeaderboardHandler>())
        {

        }
    }
}
