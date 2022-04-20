﻿using GUI_20212202_CM7A68.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace GUI_20212202_CM7A68.Logic
{
    public enum Directions
    {
        up, down, left, right, bomb
    }
    public class GameLogic : IGameModel, IGameControl
    {
        Size area;
        public List<Robot> Robots { get; set; }
        public List<Player> Players { get; set; }
        public TimeSpan RoundTime { get; set; }
        public string SelectedMapPath { get; set; }
        public bool GamePaused { get; set; } //esc lenyomásra menü fel, visszaszámlálás leáll
        public List<Explosion>  Explosions { get; set; }
        public bool GameOver { get; set; }
        public bool GameStarted { get; set; }

        int robotspeedX; //mozgás sebessége
        int robotspeedY; //ugrás sebessége
        public bool RobotsSpawned { get; set; } //true, ha már létrehoztuk a robotokat
       
        public void InitLogic()
        {
            this.Robots= new List<Robot>();
            this.Robots.Add(new Robot(new Point(area.Width / 10, (int)(area.Height * 0.8))));
            this.Robots.Add(new Robot(new Point((int)(area.Width * 0.9), (int)(area.Height * 0.8))));
            if (Players==null)
            {
                Players = new List<Player>();
            }
            if (Players.Count>0)
            {
                Robots[0].IsControllable = Players[0].IsPlayer;
                Robots[1].IsControllable = Players[1].IsPlayer;
                CreateBots();
            }
            this.RoundTime = TimeSpan.FromMinutes(1.5);
            this.GamePaused = false;
            Bombs = new List<Bomb>();
            Explosions = new List<Explosion>();
        }//visszaszámláló beállít, robotok újrapéldányosítva, GamePaused=false
        public List<Bomb> Bombs { get; set; }
        public void SetupSize(Size area)
        {
           
            this.area = area;
            if (!RobotsSpawned)
            {
                InitLogic();
                RobotsSpawned = true;
            }
            Robots[0].Center = new Point(area.Width / 10, (int)(area.Height * 0.8));//robotok spawnpointja
            Robots[1].Center = new Point((int)(area.Width * 0.9), (int)(area.Height * 0.8));
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
            if (Robots[1].Health==0)
            {
                Players[0].WonRounds++;
                InitLogic();
            }
            else if(Robots[0].Health==0)
            {
                Players[1].WonRounds++;
                InitLogic();
            }
            if (Players.Sum(x=>x.WonRounds)==3)
            {
                GameOver = true;
            }
        }

        public void MoveRobot(Directions direction, Robot robot)
        {
            var oldpos = robot.Center;
            switch (direction)
            {
                case Directions.up:
                    if (oldpos.Y - 2 * robotspeedY > 0) 
                    {
                        robot.Center = new Point(oldpos.X, oldpos.Y - robotspeedY);
                    }
                    break;
                case Directions.down:
                    //todo guggolás
                    break;
                case Directions.left:
                    if (oldpos.X - 2 * robotspeedX >= 0)
                    {
                        robot.Center = new Point(oldpos.X - robotspeedX, oldpos.Y);
                    }
                    break;
                case Directions.right:
                    if (oldpos.X + 2 * robotspeedX <= area.Width)
                    {
                        robot.Center = new Point(oldpos.X + robotspeedX, oldpos.Y);
                    }
                    break;
                case Directions.bomb:
                    if (robot == Robots[0])
                    {
                        NewGreenThrowingBomb(Robots[0].Center, Robots[0].Center.X > Robots[1].Center.X ? -1 : 1);
                    }
                    else
                    {
                        NewRedThrowingBomb(Robots[1].Center, Robots[0].Center.X > Robots[1].Center.X ? 1 : -1);
                    }
                   
                    break;
                default:
                    break;
            }
        }
        public void RobotDescend(Robot robot) //segédmetódus az ugráshoz
        {
            var oldpos = robot.Center;
            if (oldpos.Y + robotspeedY <=area.Height*0.8)
            {
                robot.Center = new Point(oldpos.X, oldpos.Y + robotspeedY);
            }
        }
        //piros && zöld zuhanó bomba létrehozása
        public void NewRedFallingBomb(Point robotPos)
        {
            Bombs.Add(new FallingBomb(new Point((int)robotPos.X, (int)robotPos.Y - area.Height * 0.05), area, ConsoleColor.Red));
            
        }
        public void NewGreenFallingBomb(Point robotPos)
        {
            Bombs.Add(new FallingBomb(new Point((int)robotPos.X, (int)robotPos.Y - area.Height * 0.05), area, ConsoleColor.Green));

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
        private void CreateBots()
        {
            var ts = Robots.Where(x=>!x.IsControllable).Select(x=> new Task(() => AIBehavior(x), TaskCreationOptions.LongRunning)).ToList();
            ts.ForEach(x => x.Start());
        }
        Random r = new Random();
        private async void AIBehavior(Robot robot)
        {
            bool robotIsInAir = false;
            while (!GameOver)
            {
                if (!GamePaused && GameStarted)
                {
                    switch (r.Next(0, 8))
                    {
                        case 0:
                            if (robot.IsJumping == false && robotIsInAir == false)
                            {
                                robot.IsJumping = true;
                                for (int i = 0; i < 20; i++)
                                {
                                    await Task.Delay(1);
                                    MoveRobot(Directions.up, robot);
                                }
                                for (int i = 0; i < 20; i++)
                                {
                                    await Task.Delay(1);
                                    RobotDescend(robot);
                                }
                                robotIsInAir = false;
                            }
                            break;
                        case 1:
                            MoveRobot(Directions.down, robot);
                            break;
                        case 2:
                            if (robot.IsMoving == false && robot.Center.X>area.Width*0.2)
                            {
                                robot.IsMoving = true;
                                while (robot.IsMoving)
                                {
                                    await Task.Delay(1);
                                    MoveRobot(Directions.left, robot);
                                    if (robot.Center.X < area.Width * 0.05)
                                        robot.IsMoving = false;
                                }
                            }
                            break;
                        case 3:
                            if (robot.IsMoving == false && robot.Center.X<area.Width*0.8)
                            {
                                robot.IsMoving = true;
                                while (robot.IsMoving)
                                {
                                    await Task.Delay(1);
                                    MoveRobot(Directions.right, robot);
                                    if (robot.Center.X>area.Width*0.95)
                                        robot.IsMoving = false;
                                }
                            }
                            break;
                        case 4:
                            MoveRobot(Directions.bomb, robot);
                            break;
                        default:
                            robot.IsMoving = false;
                            break;
                    }
                }
                Thread.Sleep(r.Next(500,1000));
            }
           
        }
    }
}
