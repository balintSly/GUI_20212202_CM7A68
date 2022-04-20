using GUI_20212202_CM7A68.Models;
using System.Collections.Generic;
using static GUI_20212202_CM7A68.Logic.GameLogic;

namespace GUI_20212202_CM7A68.Logic
{
    public interface IGameControl
    {
        List<Robot> Robots { get; set; }
        void MoveRobot(Directions direction, Robot robot);
        void RobotDescend(Robot robot);
        bool Robot1IsMoving { get; set; } //a skinváltások miatt kellenek ezek a boolok
        bool Robot2IsMoving { get; set; }
        bool Robot1IsJumping { get; set; }
        bool Robot2IsJumping { get; set; }
        bool GamePaused { get; set; }

    }
}