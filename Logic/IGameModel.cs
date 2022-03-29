using GUI_20212202_CM7A68.Models;
using System.Drawing;

namespace GUI_20212202_CM7A68.Logic
{
    public interface IGameModel
    {
        Robot Robot1 { get; set; }
        Robot Robot2 { get; set; }
        void SetupSize(Size area); //átveszi a képernyőméretet
        bool Robot1IsMoving { get; set; } //ezen keresztül tudja a renderer, hogy mozog a robot
        bool Robot2IsMoving { get; set; }

    }
}