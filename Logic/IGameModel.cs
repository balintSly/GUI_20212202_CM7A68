using GUI_20212202_CM7A68.Models;
using System.Drawing;

namespace GUI_20212202_CM7A68.Logic
{
    public interface IGameModel
    {
        Robot Robot1 { get; set; }
        Robot Robot2 { get; set; }
        void SetupSize(Size area);
        bool Robot1IsMoving { get; set; }
        bool Robot2IsMoving { get; set; }

    }
}