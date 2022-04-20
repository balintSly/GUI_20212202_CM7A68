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
        bool GamePaused { get; set; }

    }
}