using GUI_20212202_CM7A68.Logic;
using GUI_20212202_CM7A68.Models;
using GUI_20212202_CM7A68.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GUI_20212202_CM7A68.MenuWindows.ViewModels
{
    public class GameOverWindowViewModel : ObservableRecipient
    {
        IGameModel model;
        public ICommand SaveGame { get; set; }
        public ILeaderboardHandler LeaderboardHandler { get; set; }
        public void SetupModel(IGameModel model)
        {
            this.model = model;
            this.WinnerName = model.Players[0].WonRounds > model.Players[1].WonRounds ? model.Players[0].Name : model.Players[1].Name;
        }
        private string winnerName;

        public string WinnerName
        {
            get { return winnerName; }
            set { SetProperty(ref winnerName, value); }
        }

        public GameOverWindowViewModel(ILeaderboardHandler handler)
        {
            this.LeaderboardHandler = handler;
            this.SaveGame = new RelayCommand(() =>
            {

                if (model.Players[0].WonRounds > model.Players[1].WonRounds)
                {
                    model.Players[0].WonMatches++;
                }
                else
                {
                    model.Players[1].WonMatches++;
                }
                this.LeaderboardHandler.SaveGame(model.Players[0], model.Players[1]);
            });
        }
        public GameOverWindowViewModel() : this(Ioc.Default.GetService<ILeaderboardHandler>())
        {

        }
    }
}
