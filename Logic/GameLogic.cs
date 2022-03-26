using GUI_20212202_CM7A68.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_20212202_CM7A68.Logic
{
    public class GameLogic:IGameModel, IGameControl
    {
        Size area;
        public Robot Robot1 { get; set; }
        public Robot Robot2 { get; set; }
        public bool Robot1IsMoving { get; set; }
        public bool Robot2IsMoving { get; set; }
        int robotspeed;
        bool spawned;
        public enum Directions
        {
            up, down, left, right
        }
        public void SetupSize(Size area)
        {
            this.area = area;
            if (!spawned)
            {
                this.Robot1 = new Robot(new Point(area.Width / 10, (int)(area.Height*0.8)));
                this.Robot2 = new Robot(new Point((int)(area.Width*0.9), (int)(area.Height*0.8)));
                spawned = true;
            }
            Robot1.Center = new Point(area.Width / 10, (int)(area.Height * 0.8));
            Robot2.Center = new Point((int)(area.Width*0.9), (int)(area.Height * 0.8));  
            robotspeed = area.Width / 50;
        }
        public void TimeStep()
        { 
        //TODO: minden mozgatást, állapotváltozást, ütközést itt állítani, ez 20ms-ként le fog futni
        }

        public void MoveRobot1(Directions direction)
        {
            var oldpos = Robot1.Center;
            switch (direction)
            {
                case Directions.up:
                    //todo ugrás
                    break;
                case Directions.down:
                    //todo uggolás
                    break;
                case Directions.left:
                    if (oldpos.X - 2*robotspeed >= 0)
                    {
                        Robot1.Center = new Point(oldpos.X-robotspeed, oldpos.Y);
                    }
                    break;
                case Directions.right:
                    if (oldpos.X + 2*robotspeed <=area.Width)
                    {
                        Robot1.Center = new Point(oldpos.X+robotspeed, oldpos.Y);
                    }
                    break;
                default:
                    break;
            }
        }

        public void MoveRobot2(Directions direction)
        {
            var oldpos = Robot2.Center;
            switch (direction)
            {
                case Directions.up:
                    //todo ugrás
                    break;
                case Directions.down:
                    //todo uggolás
                    break;
                case Directions.left:
                    if (oldpos.X - 2*robotspeed >= 0)
                    {
                       Robot2.Center = new Point(oldpos.X - robotspeed, oldpos.Y);
                    }
                    break;
                case Directions.right:
                    if (oldpos.X + 2*robotspeed <= area.Width)
                    {
                        Robot2.Center = new Point(oldpos.X + robotspeed, oldpos.Y);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
