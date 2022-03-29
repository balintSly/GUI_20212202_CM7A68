using GUI_20212202_CM7A68.Models;
using System.Collections.Generic;

namespace GUI_20212202_CM7A68.Logic
{
    public interface IGameModel
    {
        List<Bomb> Bombs { get; set; }
        void NewBomb();
    }
}