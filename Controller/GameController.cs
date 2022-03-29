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
        bool robot1IsInAir;
        bool robot2IsInAir;
        public async void KeyPressed(Key key)
        {
            switch (key)
            {
                case Key.Up:
                    if (logic.Robot2IsJumping == false && robot2IsInAir==false)
                    {
                        robot2IsInAir = true; //letiltja, hogy ne lehessen double jumpolni lényegében
                        logic.Robot2IsJumping = true;
                        for (int i = 0; i < 20; i++)
                        {
                            await Task.Delay(1);
                            logic.MoveRobot2(Directions.up);
                        }
                        for (int i = 0; i < 20; i++)
                        {
                            await Task.Delay(1);
                            logic.Robot2Descend();
                        }
                        robot2IsInAir=false;
                    }
                    break;
                case Key.Down:
                    logic.MoveRobot2(Directions.down);
                    break;
                case Key.Left:
                    if (logic.Robot2IsMoving == false)
                    {
                        logic.Robot2IsMoving = true;
                        while (logic.Robot2IsMoving)
                        {
                            await Task.Delay(1);
                            logic.MoveRobot2(Directions.left);
                        }
                    }
                    break;
                case Key.Right:
                    if (logic.Robot2IsMoving == false)
                    {
                        logic.Robot2IsMoving = true;
                        while (logic.Robot2IsMoving)
                        {
                            await Task.Delay(1);
                            logic.MoveRobot2(Directions.right);
                        }
                    }
                    break;


                case Key.W:
                    if (logic.Robot1IsJumping==false && robot1IsInAir==false)
                    {
                        robot1IsInAir=true;
                        logic.Robot1IsJumping = true;
                        for (int i = 0; i < 30; i++)
                        {
                            await Task.Delay(1);
                            logic.MoveRobot1(Directions.up);
                        }
                        for (int i = 0; i < 30; i++)
                        {
                            await Task.Delay(1);
                            logic.Robot1Descend();
                        }
                        robot1IsInAir = false;
                    }
                    
                    break;
                case Key.S:
                    logic.MoveRobot1(Directions.down);
                    break;
                case Key.A:
                    if (logic.Robot1IsMoving == false)
                    {
                        logic.Robot1IsMoving = true;
                        while (logic.Robot1IsMoving)
                        {
                            await Task.Delay(1);
                            logic.MoveRobot1(Directions.left);
                        }
                    }
                    break;
                case Key.D:
                    if (logic.Robot1IsMoving == false)
                    {
                        logic.Robot1IsMoving = true;
                        while (logic.Robot1IsMoving)
                        {
                            await Task.Delay(1);
                            logic.MoveRobot1(Directions.right);
                        }
                    }
                    break;
                default:
                    break;
            }

        }
        public void KeyReleased(Key key)
        {
            if (key == Key.Down || key == Key.Left || key == Key.Right)
            {
                logic.Robot2IsMoving = false;
            }
            else if (key == Key.S || key == Key.D || key == Key.W)
            {
                logic.Robot1IsMoving = false;
            }
            if (key == Key.A)
            {
                logic.Robot1IsJumping = false;
            }
            if (key == Key.Up)
            {
                logic.Robot2IsJumping = false;
            }
        }
    }
}
