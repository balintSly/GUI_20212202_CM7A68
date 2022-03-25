using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_20212202_CM7A68.Logic
{
    public class GameLogic:IGameModel, IGameControl
    {
        Size area;
        public void SetupSize(Size area)
        {
            this.area = area;
        }

    }
}
