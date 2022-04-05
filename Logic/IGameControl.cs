using static GUI_20212202_CM7A68.Logic.GameLogic;

namespace GUI_20212202_CM7A68.Logic
{
    public interface IGameControl
    {
        void MoveRobot1(Directions direction);
        void Robot1Descend();
        void MoveRobot2(Directions direction);
        void Robot2Descend();
        bool Robot1IsMoving { get; set; } //a skinváltások miatt kellenek ezek a boolok
        bool Robot2IsMoving { get; set; }
        bool Robot1IsJumping { get; set; }
        bool Robot2IsJumping { get; set; }
        bool GamePaused { get; set; }

    }
}