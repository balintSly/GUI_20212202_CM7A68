using GUI_20212202_CM7A68.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static GUI_20212202_CM7A68.Logic.GameLogic;

namespace GUI_20212202_CM7A68.Controller
{
    public class GameController
    {
        IGameControl logic;
        public GameController(IGameControl gameControl)
        {
            this.logic = gameControl;
        }
        public void KeyPressed(Key key)
        {
            switch (key)
            {
                case Key.Up:
                    logic.MoveRobot1(Directions.up);
                    break;
                case Key.Down:
                    logic.MoveRobot1(Directions.down);
                    break;
                case Key.Left:
                    logic.MoveRobot1(Directions.left);
                    break;
                case Key.Right:
                    logic.MoveRobot1(Directions.right);
                    break;


                case Key.W:
                    logic.MoveRobot2(Directions.up);
                    break;
                case Key.S:
                    logic.MoveRobot2(Directions.down);
                    break;
                case Key.A:
                    logic.MoveRobot2(Directions.left);
                    break;
                case Key.D:
                    logic.MoveRobot2(Directions.right);
                    break;
                default:
                    break;
            }
        }
    }
}
