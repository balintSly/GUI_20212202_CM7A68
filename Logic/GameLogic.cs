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
        public List<Player> Players { get; set; }
        public List<Item> Items { get; set; }
        public TimeSpan RoundTime { get; set; }
        public string SelectedMapPath { get; set; }
        private int TickCounter { get; set; }
        public bool GamePaused { get; set; } //esc lenyomásra menü fel, visszaszámlálás leáll
        public List<Explosion> Explosions { get; set; }
        public bool GameOver { get; set; }
        public bool GameStarted { get; set; }
        object bombLock = new object();
        public Random r { get; set; }

        int robotspeedX; //mozgás sebessége
        int robotspeedY; //ugrás sebessége
        public bool RobotsSpawned { get; set; } //true, ha már létrehoztuk a robotokat

        public void InitLogic()
        {
            this.Robots = new List<Robot>();
            this.Robots.Add(new Robot(new Point(area.Width / 10, (int)(area.Height * 0.8))));
            this.Robots.Add(new Robot(new Point((int)(area.Width * 0.9), (int)(area.Height * 0.8))));
            if (Players == null)
            {
                Players = new List<Player>();
            }
            if (Players.Count > 0)
            {
                Robots[0].IsControllable = Players[0].IsPlayer;
                Robots[1].IsControllable = Players[1].IsPlayer;
                CreateBots();
                Robots.ForEach(x => new Task(() => ReloadBombs(x), TaskCreationOptions.LongRunning).Start());
            }
            this.RoundTime = TimeSpan.FromMinutes(1.5);
            this.GamePaused = false;
            this.GameStarted = false;
            Bombs = new List<Bomb>();
            Explosions = new List<Explosion>();
            Items = new List<Item>();
            TickCounter = 0;
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
        public void ItemTimeStep()
        {
            //több item esetén frissíteni kell
            int rnd = r.Next(0, 2);
            switch (rnd)
            {
                case 0:
                    Items.Add(new HealBoost(area));
                    break;
                case 1:
                    Items.Add(new ArmorBoost(area));
                    break;
                default:
                    break;
            }
        }
        public void TimeStep()
        {
            //TODO: minden mozgatást, állapotváltozást, ütközést itt állítani, ez 20ms-ként le fog futni
            if (GameStarted && !GamePaused)
            {
                TickCounter++;
            }
            if (TickCounter==250)
            {
                ItemTimeStep();
                TickCounter = 0;
            }
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Move((int)(area.Height * 0.85));
                if(Items[i].CheckHitbox(Robots[0], Robots[1]))
                {
                    Items.RemoveAt(i);
                }
            }
            
            for (int i = 0; i < Bombs.Count; i++)
            {
                Bombs[i].Move((int)(area.Height * 0.85));
                Bombs[i].Heal -= 1;
                if (Bombs[i].Heal <= 0)
                {
                    
                    Explosions.Add(new Explosion(
                            area,
                            Bombs[i].Center,
                            10,
                            1,
                            5
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
            if (Robots[1].Health == 0)
            {
                Players[0].WonRounds++;
                lock (this.LockObject)
                {
                    GameStarted = false;
                    Monitor.Pulse(this.LockObject);
                }
                InitLogic();
            }
            else if (Robots[0].Health == 0)
            {
                Players[1].WonRounds++;
                lock (this.LockObject)
                {
                    GameStarted = false;
                    Monitor.Pulse(this.LockObject);
                }
                InitLogic();
            }
            if (Players.Sum(x => x.WonRounds) == 3)
            {
                lock (this.LockObject)
                {
                    GameOver = true;
                    GameStarted = false;
                    Monitor.Pulse(this.LockObject);
                }               
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
                    if (robot.BombNumber > 0)
                    {
                        if (robot == Robots[0])
                        {
                            NewGreenThrowingBomb(Robots[0].Center, Robots[0].Center.X > Robots[1].Center.X ? -1 : 1);
                            robot.BombNumber--;
                        }
                        else
                        {
                            NewRedThrowingBomb(Robots[1].Center, Robots[0].Center.X > Robots[1].Center.X ? 1 : -1);
                            robot.BombNumber--;
                        }
                        new Task(() => ReloadBombs(robot), TaskCreationOptions.LongRunning).Start();
                    }
                    break;
                default:
                    break;
            }
        }
        public void RobotDescend(Robot robot) //segédmetódus az ugráshoz
        {
            var oldpos = robot.Center;
            if (oldpos.Y + robotspeedY <= area.Height * 0.8)
            {
                robot.Center = new Point(oldpos.X, oldpos.Y + robotspeedY);
            }
        }
        //piros && zöld zuhanó bomba létrehozása
        public void NewRedFallingBomb(Point robotPos)
        {
            lock (bombLock)
                Bombs.Add(new FallingBomb(new Point((int)robotPos.X, (int)robotPos.Y - area.Height * 0.05), area, ConsoleColor.Red));

        }
        public void NewGreenFallingBomb(Point robotPos)
        {
            lock (bombLock)
                Bombs.Add(new FallingBomb(new Point((int)robotPos.X, (int)robotPos.Y - area.Height * 0.05), area, ConsoleColor.Green));

        }
        //piros && zöld dobálós bomba létrehozása
        public void NewRedThrowingBomb(System.Windows.Point robotPos, int direction)
        {
            lock (bombLock)
                Bombs.Add(new ThrowingBomb(new Point((int)robotPos.X, (int)robotPos.Y), area, direction, ConsoleColor.Red));
        }
        public void NewGreenThrowingBomb(System.Windows.Point robotPos, int direction)
        {
            lock (bombLock)
                Bombs.Add(new ThrowingBomb(new Point((int)robotPos.X, (int)robotPos.Y), area, direction, ConsoleColor.Green));
        }
        private void CreateBots()
        {
            var ts = Robots.Where(x => !x.IsControllable).Select(x => new Task(() => AIBehavior(x), TaskCreationOptions.LongRunning)).ToList();
            ts.ForEach(x => x.Start());
        }
        private async void AIBehavior(Robot robot)
        {
            bool robotIsInAir = false;
            while (!GameOver)
            {
                if (!GamePaused && GameStarted)
                {
                    switch (r.Next(0, 6))
                    {
                        case 0:
                            if (robot.IsJumping == false && robotIsInAir == false)
                            {
                                new Task(() =>
                                {
                                    robot.IsJumping = true;
                                    for (int i = 0; i < 20; i++)
                                    {
                                        Thread.Sleep(10);
                                        MoveRobot(Directions.up, robot);
                                    }
                                    for (int i = 0; i < 20; i++)
                                    {
                                        Thread.Sleep(10);
                                        RobotDescend(robot);
                                    }
                                    robotIsInAir = false;
                                    robot.IsJumping = false;
                                }, TaskCreationOptions.LongRunning).Start();
                                if (robot.IsMoving == false && robot.Center.X < area.Width * 0.3)
                                {
                                    robot.IsMoving = true;
                                    while (robot.IsMoving && GameStarted)
                                    {
                                        await Task.Delay(1);
                                        MoveRobot(Directions.right, robot);
                                        if (robot.Center.X > area.Width * 0.95)
                                            robot.IsMoving = false;
                                        robot.IsMoving = r.Next(0, 100) < 90;
                                    }
                                }
                                else if (robot.IsMoving == false && robot.Center.X > area.Width * 0.7)
                                {
                                    robot.IsMoving = true;
                                    while (robot.IsMoving && GameStarted)
                                    {
                                        await Task.Delay(1);
                                        MoveRobot(Directions.left, robot);
                                        if (robot.Center.X < area.Width * 0.05)
                                            robot.IsMoving = false;
                                        robot.IsMoving = r.Next(0, 100) < 90;
                                    }
                                }

                            }
                            break;
                        case 1:
                            MoveRobot(Directions.down, robot);
                            break;
                        case 2:
                            if (robot.IsMoving == false && robot.Center.X > area.Width * 0.2)
                            {
                                robot.IsMoving = true;
                                while (robot.IsMoving && GameStarted)
                                {
                                    await Task.Delay(1);
                                    MoveRobot(Directions.left, robot);
                                    if (robot.Center.X < area.Width * 0.05)
                                        robot.IsMoving = false;
                                    robot.IsMoving = r.Next(0, 100) < 90;
                                }
                            }
                            break;
                        case 3:
                            if (robot.IsMoving == false && robot.Center.X < area.Width * 0.8)
                            {
                                robot.IsMoving = true;
                                while (robot.IsMoving && GameStarted)
                                {
                                    await Task.Delay(1);
                                    MoveRobot(Directions.right, robot);
                                    if (robot.Center.X > area.Width * 0.95)
                                        robot.IsMoving = false;
                                    robot.IsMoving = r.Next(0, 100) < 90;
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
                Thread.Sleep(r.Next(500, 1000));
            }

        }
        private void ReloadBombs(Robot robot)
        {
            if (!robot.IsReloading)
            {
                robot.IsReloading = true;
                while (robot.BombNumber < 5)
                {
                    while (robot.BombLoading < 100)
                    {
                        Thread.Sleep(100);
                        if (!GamePaused && GameStarted)
                        {
                            robot.BombLoading += 5;
                        }
                    }
                    robot.BombLoading = 0;
                    robot.BombNumber++;
                }
                robot.IsReloading = false;
            }

        }
        public void StartHurt()
        {
            new Task(() =>
            {
                while (Robots.Where(x => x.Health > 0).Count() > 1 && RoundTime.TotalSeconds<=10)
                {
                    foreach (var item in Robots)
                    {
                        item.Health -= 5;
                    }
                    Thread.Sleep(1000);
                }                
            }, TaskCreationOptions.LongRunning).Start();
        }
        public object LockObject { get; set; }
        public GameLogic()
        {
            this.LockObject = new object();
            this.r = new Random();
        }
    }
}
