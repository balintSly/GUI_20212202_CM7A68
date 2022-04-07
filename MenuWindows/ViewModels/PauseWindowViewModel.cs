using GUI_20212202_CM7A68.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_20212202_CM7A68.MenuWindows.ViewModels
{
    public class PauseWindowViewModel
    {
        IGameModel model;
        public void SetupModel(IGameModel model)
        {
            this.model=model;
        }
    }
}
