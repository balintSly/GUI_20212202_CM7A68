using GUI_20212202_CM7A68.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_20212202_CM7A68.Controller
{
    public class GameController
    {
        IGameControl logic;
        public GameController(IGameControl gameControl)
        {
            this.logic = gameControl;
        }
    }
}
