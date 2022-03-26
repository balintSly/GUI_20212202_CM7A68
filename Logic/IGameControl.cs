using static GUI_20212202_CM7A68.Logic.GameLogic;

namespace GUI_20212202_CM7A68.Logic
{
    public interface IGameControl
    {
        void MoveRobot1(Directions direction);
        void MoveRobot2(Directions direction);
    }
}