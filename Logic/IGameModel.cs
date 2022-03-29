using GUI_20212202_CM7A68.Models;
using System.Collections.Generic;
using System.Windows;

namespace GUI_20212202_CM7A68.Logic
{
    public interface IGameModel
    {
        List<Bomb> Bombs { get; set; }
        void NewFallingBomb(Point robotPos);
        void NewThrowingBomb(Point robotPos, int direction);
    }
}