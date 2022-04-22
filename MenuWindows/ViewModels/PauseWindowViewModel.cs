using GUI_20212202_CM7A68.Logic;
using GUI_20212202_CM7A68.Models;
using GUI_20212202_CM7A68.Services;
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
    public class PauseWindowViewModel
    {
        IGameModel model;
        public void SetupModel(IGameModel model)
        {
            this.model = model;
        }
    }
}
