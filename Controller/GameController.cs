using GUI_20212202_CM7A68.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GUI_20212202_CM7A68.Controller
{
    public class GameController
    {
        IGameControl logic;
        public GameController(IGameControl gameControl)
        {
            this.logic = gameControl;
        }
        bool robot1IsInAir;
        bool robot2IsInAir;
        public async void KeyPressed(Key key)
        {
            if (logic.GameStarted)
            {
                switch (key)
                {
                    case Key.Up:
                        if (logic.Robots[1].IsJumping == false && robot2IsInAir == false && logic.Robots[1].IsControllable)
                        {
                            robot2IsInAir = true; //letiltja, hogy ne lehessen double jumpolni lényegében
                            logic.Robots[1].IsJumping = true;
                            for (int i = 0; i < 20; i++)
                            {
                                await Task.Delay(1);
                                logic.MoveRobot(Directions.up, logic.Robots[1]);
                            }
                            for (int i = 0; i < 20; i++)
                            {
                                await Task.Delay(1);
                                logic.RobotDescend(logic.Robots[1]);
                            }
                            robot2IsInAir = false;
                        }
                        break;
                    case Key.Down:
                        if (logic.Robots[1].IsControllable)
                        {
                            logic.MoveRobot(Directions.down, logic.Robots[1]);
                        }
                        break;
                    case Key.Left:
                        if (logic.Robots[1].IsMoving == false && logic.Robots[1].IsControllable)
                        {
                            logic.Robots[1].IsMoving = true;
                            while (logic.Robots[1].IsMoving)
                            {
                                await Task.Delay(1);
                                logic.MoveRobot(Directions.left, logic.Robots[1]);
                            }
                        }
                        break;
                    case Key.Right:
                        if (logic.Robots[1].IsMoving == false && logic.Robots[1].IsControllable)
                        {
                            logic.Robots[1].IsMoving = true;
                            while (logic.Robots[1].IsMoving)
                            {
                                await Task.Delay(1);
                                logic.MoveRobot(Directions.right, logic.Robots[1]);
                            }
                        }
                        break;
                    case Key.RightShift:
                        if (logic.Robots[1].IsControllable)
                        {
                            logic.MoveRobot(Directions.bomb, logic.Robots[1]);
                        }

                        break;

                    case Key.W:
                        if (logic.Robots[0].IsJumping == false && robot1IsInAir == false && logic.Robots[0].IsControllable)
                        {
                            robot1IsInAir = true;
                            logic.Robots[0].IsJumping = true;
                            for (int i = 0; i < 20; i++)
                            {
                                await Task.Delay(1);
                                logic.MoveRobot(Directions.up, logic.Robots[0]);
                            }
                            for (int i = 0; i < 20; i++)
                            {
                                await Task.Delay(1);
                                logic.RobotDescend(logic.Robots[0]);
                            }
                            robot1IsInAir = false;
                        }

                        break;
                    case Key.S:
                        if (logic.Robots[0].IsControllable)
                        {
                            logic.MoveRobot(Directions.down, logic.Robots[0]);
                        }
                        break;
                    case Key.A:
                        if (logic.Robots[0].IsMoving == false && logic.Robots[0].IsControllable)
                        {
                            logic.Robots[0].IsMoving = true;
                            while (logic.Robots[0].IsMoving)
                            {
                                await Task.Delay(1);
                                logic.MoveRobot(Directions.left, logic.Robots[0]);
                            }
                        }

                        break;
                    case Key.D:
                        if (logic.Robots[0].IsMoving == false && logic.Robots[0].IsControllable)
                        {
                            logic.Robots[0].IsMoving = true;
                            while (logic.Robots[0].IsMoving)
                            {
                                await Task.Delay(1);
                                logic.MoveRobot(Directions.right, logic.Robots[0]);
                            }
                        }
                        break;
                    case Key.Space:
                        if (logic.Robots[0].IsControllable)
                        {
                            logic.MoveRobot(Directions.bomb, logic.Robots[0]);
                        }
                        break;
                    case Key.Escape:
                        logic.GamePaused = true;
                        break;
                    default:
                        break;
                }
            }
            

        }
        public void KeyReleased(Key key)
        {
            if (logic.GameStarted)
            {
                if ((key == Key.Down || key == Key.Left || key == Key.Right) && logic.Robots[1].IsControllable)
                {
                    logic.Robots[1].IsMoving = false;
                }
                else if (key == Key.S || key == Key.D || key == Key.A && logic.Robots[0].IsControllable)
                {
                    logic.Robots[0].IsMoving = false;
                }
                if (key == Key.W && logic.Robots[0].IsControllable)
                {
                    logic.Robots[0].IsJumping = false;
                }
                if (key == Key.Up && logic.Robots[0].IsControllable)
                {
                    logic.Robots[1].IsJumping = false;
                }
            }
        }
    }
}
