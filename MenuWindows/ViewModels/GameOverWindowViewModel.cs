using GUI_20212202_CM7A68.Logic;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_20212202_CM7A68.MenuWindows.ViewModels
{
    public class GameOverWindowViewModel:ObservableRecipient
    {
        IGameModel model;
        public void SetupModel(IGameModel model)
        {
            this.model = model;
            this.WinnerName=model.PlayerOneWins > model.PlayerTwoWins ? model.Player1Name : model.Player2Name;

        }
        private string winnerName;

        public string WinnerName
        {
            get { return winnerName; }
            set { SetProperty(ref winnerName, value); }
        }
    }
}
