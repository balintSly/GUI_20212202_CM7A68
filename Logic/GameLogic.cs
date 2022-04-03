using GUI_20212202_CM7A68.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_20212202_CM7A68.Logic
{
    public class GameLogic : IGameModel, IGameControl
    {
        Size area;
        public Robot Robot1 { get; set; }
        public Robot Robot2 { get; set; }
        public bool Robot1IsMoving { get; set; }
        public bool Robot2IsMoving { get; set; }
        public bool Robot1IsJumping { get; set; }
        public bool Robot2IsJumping { get; set; }

        public List<Explosion>  Explosions { get; set; }

        int robotspeedX; //mozgás sebessége
        int robotspeedY; //ugrás sebessége
        bool spawned; //true, ha már létrehoztuk a robotokat
        public enum Directions
        {
            up, down, left, right, bomb
        }
        public List<Bomb> Bombs { get; set; }
        public void SetupSize(Size area)
        {
            Bombs = new List<Bomb>();
            Explosions = new List<Explosion>();
            this.area = area;
            if (!spawned)
            {
                this.Robot1 = new Robot(new Point(area.Width / 10, (int)(area.Height * 0.8)));
                this.Robot2 = new Robot(new Point((int)(area.Width * 0.9), (int)(area.Height * 0.8)));
                spawned = true;
            }
            Robot1.Center = new Point(area.Width / 10, (int)(area.Height * 0.8));//robotok spawnpointja
            Robot2.Center = new Point((int)(area.Width * 0.9), (int)(area.Height * 0.8));
            robotspeedX = (int)area.Width / 50;
            robotspeedY = (int)area.Height / 50;
        }
        public void TimeStep()
        {
            //TODO: minden mozgatást, állapotváltozást, ütközést itt állítani, ez 20ms-ként le fog futni
            for (int i = 0; i < Bombs.Count; i++)
            {
                Bombs[i].Move((int)(area.Height*0.85));
                Bombs[i].Heal -= 1;
                if (Bombs[i].Heal <= 0)
                {
                    Explosions.Add(new Explosion(       
                            area,
                            Bombs[i].Center,
                            10,
                            1,
                            3
                        ));
                    Bombs.RemoveAt(i);                
                }
            }
            for (int i = 0; i < Explosions.Count; i++)
            {
                if (Explosions[i].LastFrameFlag)
                {
                    Explosions.RemoveAt(i);
                }
            }
        }

        public void MoveRobot1(Directions direction)
        {
            var oldpos = Robot1.Center;
            switch (direction)
            {
                case Directions.up:
                    if (oldpos.Y - 2 * robotspeedY > 0) 
                    {
                        Robot1.Center = new Point(oldpos.X, oldpos.Y - robotspeedY);
                    }
                    break;
                case Directions.down:
                    //todo guggolás
                    break;
                case Directions.left:
                    if (oldpos.X - 2 * robotspeedX >= 0)
                    {
                        Robot1.Center = new Point(oldpos.X - robotspeedX, oldpos.Y);
                    }
                    break;
                case Directions.right:
                    if (oldpos.X + 2 * robotspeedX <= area.Width)
                    {
                        Robot1.Center = new Point(oldpos.X + robotspeedX, oldpos.Y);
                    }
                    break;
                case Directions.bomb:
                    NewGreenThrowingBomb(Robot1.Center, Robot1.Center.X > Robot2.Center.X ? -1 : 1);
                    NewGreenFallingBomb(Robot1.Center);
                    break;
                default:
                    break;
            }
        }
        public void Robot1Descend() //segédmetódus az ugráshoz
        {
            var oldpos = Robot1.Center;
            if (oldpos.Y + robotspeedY <=area.Height*0.8)
            {
                Robot1.Center = new Point(oldpos.X, oldpos.Y + robotspeedY);
            }
        }
        //piros && zöld zuhanó bomba létrehozása
        public void NewRedFallingBomb(System.Windows.Point robotPos)
        {
            Bombs.Add(new FallingBomb(new Point((int)robotPos.X, (int)robotPos.Y - area.Height * 0.05), area, ConsoleColor.Red));
            
        }
        public void NewGreenFallingBomb(System.Windows.Point robotPos)
        {
            Bombs.Add(new FallingBomb(new Point((int)robotPos.X, (int)robotPos.Y - area.Height * 0.05), area, ConsoleColor.Green));

        }

        public void MoveRobot2(Directions direction)
        {
            var oldpos = Robot2.Center;
            switch (direction)
            {
                case Directions.up:
                    if (oldpos.Y - 2 * robotspeedY > 0)
                    {
                        Robot2.Center = new Point(oldpos.X, oldpos.Y - robotspeedY);
                    }
                    break;
                case Directions.down:
                    //todo uggolás
                    break;
                case Directions.left:
                    if (oldpos.X - 2 * robotspeedX >= 0)
                    {
                        Robot2.Center = new Point(oldpos.X - robotspeedX, oldpos.Y);
                    }
                    break;
                case Directions.right:
                    if (oldpos.X + 2 * robotspeedX <= area.Width)
                    {
                        Robot2.Center = new Point(oldpos.X + robotspeedX, oldpos.Y);
                    }
                    break;
                case Directions.bomb:
                    NewRedThrowingBomb(Robot2.Center, Robot1.Center.X > Robot2.Center.X ? 1 : -1);
                    break;
                default:
                    break;
            }
        }
        public void Robot2Descend()
        {
            var oldpos = Robot2.Center;
            if (oldpos.Y + robotspeedY <= area.Height * 0.8)
            {
                Robot2.Center = new Point(oldpos.X, oldpos.Y + robotspeedY);
            }
        }
        //piros && zöld dobálós bomba létrehozása
        public void NewRedThrowingBomb(System.Windows.Point robotPos, int direction)
        {
            Bombs.Add(new ThrowingBomb(new Point((int)robotPos.X, (int)robotPos.Y),area, direction, ConsoleColor.Red));
        }
        public void NewGreenThrowingBomb(System.Windows.Point robotPos, int direction)
        {
            Bombs.Add(new ThrowingBomb(new Point((int)robotPos.X, (int)robotPos.Y), area, direction, ConsoleColor.Green));
        }
    }
}
