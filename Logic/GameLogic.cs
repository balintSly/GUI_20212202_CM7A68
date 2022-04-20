using GUI_20212202_CM7A68.Models;
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
        public bool Robot1IsMoving { get; set; }
        public bool Robot2IsMoving { get; set; }
        public bool Robot1IsJumping { get; set; }
        public bool Robot2IsJumping { get; set; }
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public string PlayerOneColor { get; set; }
        public string PlayerTwoColor { get; set; }
        public TimeSpan RoundTime { get; set; }
        public string SelectedMapPath { get; set; }
        public bool GamePaused { get; set; } //esc lenyomásra menü fel, visszaszámlálás leáll
        public List<Explosion>  Explosions { get; set; }
        public int PlayerOneWins { get; set; }
        public int PlayerTwoWins { get; set; }
        public bool GameOver { get; set; }

        int robotspeedX; //mozgás sebessége
        int robotspeedY; //ugrás sebessége
        public bool RobotsSpawned { get; set; } //true, ha már létrehoztuk a robotokat
       
        public void InitLogic()
        {
            this.Robots= new List<Robot>();
            this.Robots.Add(new Robot(new Point(area.Width / 10, (int)(area.Height * 0.8))));
            this.Robots.Add(new Robot(new Point((int)(area.Width * 0.9), (int)(area.Height * 0.8))));
            this.RoundTime = TimeSpan.FromMinutes(3);
            this.GamePaused = false;
            Bombs = new List<Bomb>();
            Explosions = new List<Explosion>();
            CreateBots();

        }//visszaszámláló 3:00-ra, robotok újrapéldányosítva, GamePaused=false
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
                PlayerOneWins++;
                InitLogic();
            }
            else if(Robots[0].Health==0)
            {
                PlayerTwoWins++;
                InitLogic();
            }
            if (PlayerOneWins+PlayerTwoWins==3)
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
                        NewGreenThrowingBomb(Robots[1].Center, Robots[0].Center.X > Robots[1].Center.X ? 1 : 1);
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
            bool robot2IsInAir = false;
            while (!GameOver)
            {
                switch (r.Next(0, 6))
                {
                    case 0:
                        if (Robot2IsJumping == false && robot2IsInAir == false)
                        {
                            Robot2IsJumping = true;
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
                            robot2IsInAir = false;
                        }
                        break;
                    case 1:
                        MoveRobot(Directions.down, robot);
                        break;
                    case 2:
                        if (Robot2IsMoving == false)
                        {
                            Robot2IsMoving = true;
                            while (Robot2IsMoving)
                            {
                                await Task.Delay(1);
                                MoveRobot(Directions.left, robot);
                            }
                        }
                        break;
                    case 3:
                        if (Robot2IsMoving == false)
                        {
                            Robot2IsMoving = true;
                            while (Robot2IsMoving)
                            {
                                await Task.Delay(1);
                                MoveRobot(Directions.right, robot);
                            }
                        }
                        break;
                    case 4:
                        MoveRobot(Directions.bomb, robot);
                        break;
                    default:Robot2IsMoving = false;
                        break;
                }
                Thread.Sleep(1000);
            }
           
        }
    }
}
