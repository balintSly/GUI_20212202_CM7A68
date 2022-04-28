using GUI_20212202_CM7A68.Logic;
using GUI_20212202_CM7A68.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI_20212202_CM7A68.Renderer
{
    public class Display : FrameworkElement
    {
        Size area;
        IGameModel model;
        public TimeSpan TimeFromGameStart { get; set; }

        public bool Quit { get; set; }
        public bool MenuLoaded { get; set; }
        public bool FirstRender { get; set; }
        public void SetupSize(Size area)
        {
            this.area = area;
            model.SetupSize(new Size((int)area.Width, (int)area.Height));
            InvalidateVisual();
        }

        public void SetupModel(IGameModel model)
        {
            this.model = model;
        }
        int piccount = 0;
        string robot1skin = "robotpic_stand2_";
        string robot2skin = "robotpic_stand2_";
        bool showround = false;
        bool showfight = false;
        bool roundDisplayStarted = false;
        public MediaPlayer MusicPlayer { get; set; }
        Queue<string> musicPaths = new Queue<string>();
        private void StartMusic()
        {
            new Task(() =>
            {
                MusicPlayer = new MediaPlayer();
                var musicpath = musicPaths.Dequeue();
                MusicPlayer.Open(new Uri(musicpath, UriKind.RelativeOrAbsolute));
                MusicPlayer.Play();
                while (model.GameStarted)
                {
                    lock (model.LockObject)
                    {
                        Monitor.Wait(model.LockObject);
                    }
                    MusicPlayer.Pause();
                    if (model.GameStarted)
                    {
                        lock (model.LockObject)
                        {
                            Monitor.Wait(model.LockObject);
                        }
                        MusicPlayer.Play();
                    }
                    else
                    {
                        MusicPlayer.Stop();
                    }
                }
                MusicPlayer.Stop();

            }, TaskCreationOptions.LongRunning).Start();

        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (area.Width > 0 && area.Height > 0)
            {
                base.OnRender(drawingContext);
                #region startup

                if (!MenuLoaded)
                {
                    if (FirstRender)
                    {
                        new Task(() =>
                        {
                            Thread.Sleep(4000);
                            MenuLoaded = true;
                        }, TaskCreationOptions.LongRunning).Start();
                        FirstRender = false;
                        Directory.GetFiles(Path.Combine("Renderer", "Music"), "*.wav").OrderBy(x => model.r.Next(0, 100)).ToList().ForEach(x => musicPaths.Enqueue(x));
                        ;
                    }
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Backgrounds", "Controls", "mkbombatkontrols.jpg"), UriKind.RelativeOrAbsolute))), null, new Rect(0, 0, area.Width, area.Height)); //loading screen
                }
                #endregion
                else
                {
                    #region StartRound
                    if (!model.GameStarted && !roundDisplayStarted && !model.GameOver)
                    {
                        roundDisplayStarted = true;
                        new Task(() =>
                        {
                            showround = true;
                            if (model.Players.Sum(x => x.WonRounds) == 0)//round 1
                            {
                                SoundPlayer s = new SoundPlayer(System.IO.Path.Combine("Renderer", "Sounds", "mk_round1.wav"));
                                s.Play();
                            }
                            else if (model.Players.Sum(x => x.WonRounds) == 1)//round2
                            {
                                SoundPlayer s = new SoundPlayer(System.IO.Path.Combine("Renderer", "Sounds", "mk_round2.wav"));
                                s.Play();
                            }
                            else if (model.Players.Sum(x => x.WonRounds) == 2)//final
                            {
                                SoundPlayer s = new SoundPlayer(System.IO.Path.Combine("Renderer", "Sounds", "mk_finalround.wav"));
                                s.Play();
                            }
                            Thread.Sleep(1250);
                            showround = false;
                            showfight = true;
                            Thread.Sleep(800);
                            showfight = false;
                            model.GameStarted = true;
                            roundDisplayStarted = false;
                            StartMusic();

                        }, TaskCreationOptions.LongRunning).Start();
                    }
                    #endregion
                    //map kirajzolás
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(model.SelectedMapPath, UriKind.RelativeOrAbsolute))),
                        null, new Rect(0, 0, area.Width, area.Height));

                    #region Skinváltogatás
                    //állaptok szerint kép kiválsztása: áll, mozog, ugrik, gugol, dob
                    //ugrás: robot.center.y < (int)(area.Height * 0.8) 
                    if (model.Robots[0].Center.Y < (int)(area.Height * 0.8))
                    {
                        robot1skin = "robotpic_jump_";
                    }
                    else if (!model.Robots[0].IsMoving)
                    {
                        robot1skin = "robotpic_stand_";
                    }
                    else
                    {
                        piccount++;
                        if (piccount % 9 == 0 || piccount % 9 == 1 || piccount % 9 == 2)
                        {
                            robot1skin = "robotpic_stand_";
                        }
                        else if (piccount % 9 == 3 || piccount % 9 == 4 || piccount % 9 == 5)
                        {
                            robot1skin = "robotpic_step_";
                        }
                        else if (piccount % 9 == 6 || piccount % 9 == 7 || piccount % 9 == 8)
                        {
                            robot1skin = "robotpic_step2_";
                            piccount %= 9;
                        }
                    }
                    if (model.Robots[1].Center.Y < (int)(area.Height * 0.8))
                    {
                        robot2skin = "robotpic_jump_";
                    }
                    else if (!model.Robots[1].IsMoving)
                    {
                        robot2skin = "robotpic_stand_";
                    }
                    else
                    {
                        piccount++;
                        if (piccount % 9 == 0 || piccount % 9 == 1 || piccount % 9 == 2)
                        {
                            robot2skin = "robotpic_stand_";
                        }
                        else if (piccount % 9 == 3 || piccount % 9 == 4 || piccount % 9 == 5)
                        {
                            robot2skin = "robotpic_step_";
                        }
                        else if (piccount % 9 == 6 || piccount % 9 == 7 || piccount % 9 == 8)
                        {
                            robot2skin = "robotpic_step2_";
                            piccount %= 9;
                        }
                    }
                    robot1skin += model.Players[0].SelectedColor;
                    robot2skin += model.Players[1].SelectedColor;
                    #endregion
                    #region RobotKirajzolás
                    if (model.Robots[0].Center.X < model.Robots[1].Center.X)//merre nézzenek a robotok, kirajzolásuk
                    {
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Robots", robot1skin),
                     UriKind.RelativeOrAbsolute))), null, new Rect(model.Robots[0].Center.X - area.Width / 12, model.Robots[0].Center.Y - area.Height / 6, area.Width / 6, area.Height / 3));

                        drawingContext.PushTransform(new ScaleTransform(-1, 1, model.Robots[1].Center.X, model.Robots[1].Center.Y));
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Robots", robot2skin),
                      UriKind.RelativeOrAbsolute))), null, new Rect(model.Robots[1].Center.X - area.Width / 12, model.Robots[1].Center.Y - area.Height / 6, area.Width / 6, area.Height / 3));
                        drawingContext.Pop();
                    }
                    else
                    {
                        drawingContext.PushTransform(new ScaleTransform(-1, 1, model.Robots[0].Center.X, model.Robots[0].Center.Y));
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Robots", robot1skin),
                   UriKind.RelativeOrAbsolute))), null, new Rect(model.Robots[0].Center.X - area.Width / 12, model.Robots[0].Center.Y - area.Height / 6, area.Width / 6, area.Height / 3));
                        drawingContext.Pop();

                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Robots", robot2skin),
                      UriKind.RelativeOrAbsolute))), null, new Rect(model.Robots[1].Center.X - area.Width / 12, model.Robots[1].Center.Y - area.Height / 6, area.Width / 6, area.Height / 3));
                    }
                    #endregion
                    #region HUD kirajzolás
                    //áttetsző fekete háttér

                    //drawingContext.DrawRectangle(new SolidColorBrush(Color.FromArgb(200, 0, 0, 0)), null, new Rect(0,0, area.Width, area.Height * 0.122));
                    drawingContext.DrawEllipse(new SolidColorBrush(Color.FromArgb(200, 0, 0, 0)), null, new Point(area.Width * 0.02, area.Height * 0.121), area.Width * 0.2, area.Height * 0.1);
                    drawingContext.DrawEllipse(new SolidColorBrush(Color.FromArgb(200, 0, 0, 0)), null, new Point(area.Width * 0.98, area.Height * 0.121), area.Width * 0.2, area.Height * 0.1);

                    drawingContext.DrawEllipse(new SolidColorBrush(Color.FromArgb(200, 0, 0, 0)), null, new Point(area.Width * 0.5, area.Height * 0.1), area.Width * 0.12, area.Height * 0.09);

                    //robot1
                    string hp1 = $"Helath_{model.Robots[0].Health}_pecent.png";
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "HUD", "Health", hp1),
                       UriKind.RelativeOrAbsolute))), null, new Rect(area.Width * 0.048, area.Height * 0.02, area.Width * 0.4, area.Height * 0.05));

                    string armor1 = $"Armor_{model.Robots[0].Shield}_pecent.png";
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "HUD", "Armor", armor1),
                      UriKind.RelativeOrAbsolute))), null, new Rect(area.Width * 0.048, area.Height * 0.07, area.Width * 0.4, area.Height * 0.05));
                    //robot1bombái
                    string robot1bombbar = $"Helath_{model.Robots[0].BombLoading}_pecent.png";
                    drawingContext.PushTransform(new RotateTransform(-90, area.Width * 0.001, area.Height * 0.5));
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "HUD", "Health", robot1bombbar),
                       UriKind.RelativeOrAbsolute))), null, new Rect(area.Width * 0.002, area.Height * 0.5, area.Width * 0.1, area.Height * 0.01));
                    drawingContext.Pop();
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Bomb", "bomb1.png"),
                       UriKind.RelativeOrAbsolute))), null, new Rect(area.Width * 0.023 - area.Width * 0.05 / 2, area.Height * 0.275 - area.Width * 0.05 / 2, area.Width * 0.05, area.Width * 0.05));
                    drawingContext.DrawText(new FormattedText($"{model.Robots[0].BombNumber}", System.Globalization.CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight, new Typeface(new FontFamily("Consolas"), FontStyles.Normal, FontWeights.Bold,
                        FontStretches.Normal), area.Height * 0.05, Brushes.Red), new Point(area.Width * 0.029 - area.Width * 0.05 / 2, area.Height * 0.3 - area.Width * 0.05 / 2));


                    //robot2
                    string hp2 = $"Helath_{model.Robots[1].Health}_pecent.png";
                    drawingContext.PushTransform(new ScaleTransform(-1, 1, area.Width * 0.78, area.Height * 0.02));
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "HUD", "Health", hp2),
                       UriKind.RelativeOrAbsolute))), null, new Rect(area.Width * 0.608, area.Height * 0.02, area.Width * 0.4, area.Height * 0.05));
                    drawingContext.Pop();

                    string armor2 = $"Armor_{model.Robots[1].Shield}_pecent.png";
                    drawingContext.PushTransform(new ScaleTransform(-1, 1, area.Width * 0.78, area.Height * 0.07));
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "HUD", "Armor", armor2),
                      UriKind.RelativeOrAbsolute))), null, new Rect(area.Width * 0.608, area.Height * 0.07, area.Width * 0.4, area.Height * 0.05));
                    drawingContext.Pop();
                    //robot2bombái
                    string robot2bombbar = $"Helath_{model.Robots[1].BombLoading}_pecent.png";
                    drawingContext.PushTransform(new RotateTransform(-90, area.Width * 0.993, area.Height * 0.5));
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "HUD", "Health", robot2bombbar),
                       UriKind.RelativeOrAbsolute))), null, new Rect(area.Width * 0.993, area.Height * 0.5, area.Width * 0.1, area.Height * 0.01));
                    drawingContext.Pop();
                    drawingContext.PushTransform(new ScaleTransform(-1, 1, area.Width * 0.9999, area.Height * 0.275 - area.Width * 0.05 / 2));
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Bomb", "bomb1.png"),
                       UriKind.RelativeOrAbsolute))), null, new Rect(area.Width * 0.9999, area.Height * 0.275 - area.Width * 0.05 / 2, area.Width * 0.05, area.Width * 0.05));
                    drawingContext.Pop();
                    drawingContext.DrawText(new FormattedText($"{model.Robots[1].BombNumber}", System.Globalization.CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight, new Typeface(new FontFamily("Consolas"), FontStyles.Normal, FontWeights.Bold,
                        FontStretches.Normal), area.Height * 0.05, Brushes.Red), new Point(area.Width * 0.999 - area.Width * 0.05 / 2, area.Height * 0.3 - area.Width * 0.05 / 2));

                    //ikonok
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "HUD", "Icons", "health_icon.png"),
                        UriKind.RelativeOrAbsolute))), null, new Rect(area.Width * 0.002, area.Height * 0.015, area.Width * 0.05, area.Height * 0.065));
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "HUD", "Icons", "armor_icon.png"),
                        UriKind.RelativeOrAbsolute))), null, new Rect(area.Width * 0.002, area.Height * 0.065, area.Width * 0.05, area.Height * 0.065));

                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "HUD", "Icons", "health_icon.png"),
                       UriKind.RelativeOrAbsolute))), null, new Rect(area.Width * 0.95, area.Height * 0.015, area.Width * 0.05, area.Height * 0.065));
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "HUD", "Icons", "armor_icon.png"),
                        UriKind.RelativeOrAbsolute))), null, new Rect(area.Width * 0.95, area.Height * 0.065, area.Width * 0.05, area.Height * 0.065));

                    //óra
                    if (model.RoundTime.TotalSeconds <= 10)
                    {
                        if (TimeFromGameStart.TotalSeconds % 2 == 0)
                        {
                            drawingContext.DrawText(new FormattedText(model.RoundTime.ToString(@"mm\:ss"), System.Globalization.CultureInfo.CurrentCulture,
                            FlowDirection.LeftToRight, new Typeface(new FontFamily("Consolas"), FontStyles.Normal, FontWeights.Bold,
                            FontStretches.Normal), area.Height * 0.05, Brushes.Red), new Point(area.Width * 0.465, area.Height * 0.05));
                        }
                    }
                    else
                    {
                        drawingContext.DrawText(new FormattedText(model.RoundTime.ToString(@"mm\:ss"), System.Globalization.CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight, new Typeface(new FontFamily("Consolas"), FontStyles.Normal, FontWeights.Bold,
                        FontStretches.Normal), area.Height * 0.05, Brushes.Red), new Point(area.Width * 0.465, area.Height * 0.05));
                    }


                    //állás
                    drawingContext.DrawText(new FormattedText($"{model.Players[0].WonRounds}:{model.Players[1].WonRounds}", System.Globalization.CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight, new Typeface(new FontFamily("Consolas"), FontStyles.Normal, FontWeights.Bold,
                        FontStretches.Normal), area.Height * 0.05, Brushes.Red), new Point(area.Width * 0.48, area.Height * 0.12));

                    //nevek
                    drawingContext.DrawText(new FormattedText(model.Players[0].Name, System.Globalization.CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight, new Typeface(new FontFamily("Consolas"), FontStyles.Normal, FontWeights.Bold,
                        FontStretches.Normal), area.Height * 0.05, Brushes.Red), new Point(area.Width * 0.02, area.Height * 0.13));

                    drawingContext.DrawText(new FormattedText(model.Players[1].Name, System.Globalization.CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight, new Typeface(new FontFamily("Consolas"), FontStyles.Normal, FontWeights.Bold,
                        FontStretches.Normal), area.Height * 0.05, Brushes.Red), new Point(area.Width * 0.85, area.Height * 0.13));


                    #endregion
                    #region BombaKirajzolás
                    foreach (var bomb in model.Bombs)
                    {
                        string BombBrush = "";
                        if (bomb.BombCount % 4 == 0)
                        {
                            BombBrush = "LargeBombStaticFrame1.png";
                            bomb.BombCount++;
                        }
                        else if (bomb.BombCount % 4 == 1)
                        {
                            BombBrush = "LargeBombStaticFrame2.png";
                            bomb.BombCount++;
                        }
                        else if (bomb.BombCount % 4 == 2)
                        {
                            BombBrush = "LargeBombStaticFrame3.png";
                            bomb.BombCount++;
                        }
                        else if (bomb.BombCount % 4 == 3)
                        {
                            BombBrush = "LargeBombStaticFrame4.png";
                            bomb.BombCount++;
                        }
                        if (bomb.Color == ConsoleColor.Red)
                        {
                            drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "RedBombs", BombBrush),
                            UriKind.RelativeOrAbsolute))), null, new Rect(bomb.Center.X - area.Width / 10, bomb.Center.Y - area.Height / 10, area.Width / 5, area.Height / 5));
                        }
                        else if (bomb.Color == ConsoleColor.Green)
                        {
                            drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "GreenBombs", BombBrush),
                            UriKind.RelativeOrAbsolute))), null, new Rect(bomb.Center.X - area.Width / 10, bomb.Center.Y - area.Height / 10, area.Width / 5, area.Height / 5));
                        }

                        string bomb_hp = BombHp(bomb);


                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "BombHp", "Heath", bomb_hp),
                        UriKind.RelativeOrAbsolute))), null, new Rect(bomb.Center.X - area.Width / 32, bomb.Center.Y + area.Height / 16, area.Width / 18, area.Height / 35));


                    }
                    #endregion
                    #region items
                    foreach (var item in model.Items)
                    {
                        //több item esetén bővíteni kell
                        if (item is HealBoost)
                        {
                            drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Items", "HealBooster", "health_icon.png"),
                            UriKind.RelativeOrAbsolute))), null, item.Hitbox);
                        }
                        else if (item is ArmorBoost)
                        {
                            drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Items", "ArmorBooster", "armor_icon.png"),
                            UriKind.RelativeOrAbsolute))), null, item.Hitbox);
                        }
                    }
                    #endregion
                    #region RobbanásKirajzolás
                    foreach (var explosion in model.Explosions)
                    {
                        DrawExplosion(drawingContext, explosion);
                        explosion.CheckHitBox(model.Robots[0], model.Robots[1], model.Bombs);
                    }
                    if (model.GamePaused)
                    {
                        drawingContext.DrawRectangle(new SolidColorBrush(Color.FromArgb(200, 0, 0, 0)),
                       null, new Rect(0, 0, area.Width, area.Height));
                    }
                    else if (model.GameOver)
                    {
                        drawingContext.DrawRectangle(new SolidColorBrush(Color.FromArgb(200, 0, 0, 0)),
                       null, new Rect(0, 0, area.Width, area.Height));
                    }
                    #endregion
                    #region Round Kirajzolás
                    if (showround)
                    {
                        var path = "";
                        if (model.Players.Sum(x => x.WonRounds) == 0)//round 1
                        {
                            path = Path.Combine("Renderer", "Images", "Rounds", "round1piccropped.png");
                        }
                        else if (model.Players.Sum(x => x.WonRounds) == 1)//round2
                        {
                            path = Path.Combine("Renderer", "Images", "Rounds", "round2piccropped.png");
                        }
                        else if (model.Players.Sum(x => x.WonRounds) == 2)//final
                        {
                            path = Path.Combine("Renderer", "Images", "Rounds", "round3piccropped.png");
                        }
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute))),
                      null, new Rect(area.Width / 2 - area.Width / 6, area.Height / 2 - area.Height / 6, area.Width / 3, area.Height / 3));
                    }
                    else if (showfight)
                    {
                        var path = Path.Combine("Renderer", "Images", "Rounds", "fightpiccropped.png");
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute))),
                     null, new Rect(area.Width / 2 - area.Width / 6, area.Height / 2 - area.Height / 6, area.Width / 3, area.Height / 3));
                    }
                    #endregion
                }

            }


        }
        private string BombHp(Bomb b)
        {
            if (b.Heal <= 100 && b.Heal > 95)
            {
                return $"Helath_100_pecent.png";
            }
            else if (b.Heal <= 95 && b.Heal > 90)
            {
                return $"Helath_95_pecent.png";
            }
            else if (b.Heal <= 90 && b.Heal > 85)
            {
                return $"Helath_90_pecent.png";
            }
            else if (b.Heal <= 85 && b.Heal > 80)
            {
                return $"Helath_85_pecent.png";
            }
            else if (b.Heal <= 80 && b.Heal > 75)
            {
                return $"Helath_80_pecent.png";
            }
            else if (b.Heal <= 75 && b.Heal > 70)
            {
                return $"Helath_75_pecent.png";
            }
            else if (b.Heal <= 70 && b.Heal > 65)
            {
                return $"Helath_70_pecent.png";
            }
            else if (b.Heal <= 65 && b.Heal > 60)
            {
                return $"Helath_65_pecent.png";
            }
            else if (b.Heal <= 60 && b.Heal > 55)
            {
                return $"Helath_60_pecent.png";
            }
            else if (b.Heal <= 55 && b.Heal > 50)
            {
                return $"Helath_55_pecent.png";
            }
            else if (b.Heal <= 50 && b.Heal > 45)
            {
                return $"Helath_50_pecent.png";
            }
            else if (b.Heal <= 45 && b.Heal > 40)
            {
                return $"Helath_45_pecent.png";
            }
            else if (b.Heal <= 40 && b.Heal > 35)
            {
                return $"Helath_40_pecent.png";
            }
            else if (b.Heal <= 35 && b.Heal > 30)
            {
                return $"Helath_35_pecent.png";
            }
            else if (b.Heal <= 30 && b.Heal > 25)
            {
                return $"Helath_30_pecent.png";
            }
            else if (b.Heal <= 25 && b.Heal > 20)
            {
                return $"Helath_25_pecent.png";
            }
            else if (b.Heal <= 20 && b.Heal > 15)
            {
                return $"Helath_20_pecent.png";
            }
            else if (b.Heal <= 15 && b.Heal > 10)
            {
                return $"Helath_15_pecent.png";
            }
            else if (b.Heal <= 10 && b.Heal > 5)
            {
                return $"Helath_10_pecent.png";
            }
            else if (b.Heal <= 5 && b.Heal > 0)
            {
                return $"Helath_5_pecent.png";
            }
            else
            {
                return $"Helath_0_pecent.png";
            }

        }//(ezt a metódust le se nyissátok xd)
        public void DrawExplosion(DrawingContext drawingContext, Explosion explosion)
        {
            double cursorX;
            double cursorY;

            #region vertical_drawing
            if (explosion.Height > 0)
            {
                cursorX = explosion.Center.X - explosion.PartSize.Width / 2;    //set cursor to the bottom left corner of the center part
                cursorY = explosion.Center.Y - explosion.PartSize.Height / 2;   //
                for (int i = 0; i < (explosion.Height * 2) + 2; i++)            //+2 to help place top and bottom parts
                {
                    if (i < explosion.Height)                   //Upwards
                    {
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(explosion.VertContAnim[explosion.FrameCount], UriKind.RelativeOrAbsolute))),
                            null,
                            new Rect(
                            cursorX,
                            cursorY - explosion.PartSize.Height * i,
                            explosion.PartSize.Width,
                            explosion.PartSize.Height)
                            );
                    }
                    else if (i == explosion.Height)             //Top part
                    {
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(explosion.TopAnim[explosion.FrameCount], UriKind.RelativeOrAbsolute))),
                            null,
                            new Rect(
                            cursorX,
                            cursorY - explosion.PartSize.Height * i,
                            explosion.PartSize.Width,
                            explosion.PartSize.Height)
                            );
                        cursorX = explosion.Center.X - explosion.PartSize.Width / 2;    //set cursor to the bottom left corner of the center part
                        cursorY = explosion.Center.Y - explosion.PartSize.Height / 2;
                    }
                    else if (i < (explosion.Height * 2) + 1)                                        //Downwards
                    {

                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(explosion.VertContAnim[explosion.FrameCount], UriKind.RelativeOrAbsolute))),
                           null,
                           new Rect(
                           cursorX,
                           cursorY + explosion.PartSize.Height * ((i - 1) % explosion.Height),
                           explosion.PartSize.Width,
                           explosion.PartSize.Height)
                           );
                    }
                    else
                    {
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(explosion.BottomAnim[explosion.FrameCount], UriKind.RelativeOrAbsolute))),
                            null,
                            new Rect(
                            cursorX,
                            cursorY + explosion.PartSize.Height * (explosion.Height),
                            explosion.PartSize.Width,
                            explosion.PartSize.Height)
                            );
                    }
                }
            }
            #endregion

            #region horizontal_drawing
            if (explosion.Width > 0)
            {
                cursorX = explosion.Center.X - explosion.PartSize.Width / 2;
                cursorY = explosion.Center.Y - explosion.PartSize.Height / 2;
                for (int i = 0; i < (explosion.Width * 2) + 2; i++)            //+2 to help place top and bottom parts
                {
                    if (i < explosion.Width)                   //LeftContinuation
                    {
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(explosion.SideContAnim[explosion.FrameCount], UriKind.RelativeOrAbsolute))),
                            null,
                            new Rect(
                            cursorX - explosion.PartSize.Width * i,
                            cursorY,
                            explosion.PartSize.Width,
                            explosion.PartSize.Height)
                            );
                    }
                    else if (i == explosion.Width)             //Leftmost
                    {
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(explosion.LeftAnim[explosion.FrameCount], UriKind.RelativeOrAbsolute))),
                            null,
                            new Rect(
                            cursorX - explosion.PartSize.Width * i,
                            cursorY,
                            explosion.PartSize.Width,
                            explosion.PartSize.Height)
                            );
                        cursorX = explosion.Center.X - explosion.PartSize.Width / 2;
                        cursorY = explosion.Center.Y - explosion.PartSize.Height / 2;
                    }
                    else if (i < (explosion.Width * 2) + 1)                                        //RightContinuation
                    {

                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(explosion.SideContAnim[explosion.FrameCount], UriKind.RelativeOrAbsolute))),
                           null,
                           new Rect(
                           cursorX + explosion.PartSize.Width * ((i - 1) % explosion.Width),
                           cursorY,
                           explosion.PartSize.Width,
                           explosion.PartSize.Height)
                           );
                    }
                    else                                                                           //RightMost
                    {
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(explosion.RightAnim[explosion.FrameCount], UriKind.RelativeOrAbsolute))),
                            null,
                            new Rect(
                            cursorX + explosion.PartSize.Width * (explosion.Width),
                            cursorY,
                            explosion.PartSize.Width,
                            explosion.PartSize.Height)
                            );
                    }
                }
            }
            #endregion

            //Center piece
            drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(explosion.CenterAnim[explosion.FrameCount], UriKind.RelativeOrAbsolute))),
                null,
                new Rect(
                    explosion.Center.X - explosion.PartSize.Width / 2,
                    explosion.Center.Y - explosion.PartSize.Height / 2,
                    explosion.PartSize.Width,
                    explosion.PartSize.Height));


            ++explosion.FrameCount;
            if (explosion.FrameCount == explosion.CenterAnim.Count)
            {
                explosion.LastFrameFlag = true;
                explosion.FrameCount = 0;                                   //fixes ArgumentOutOfRangeException
            }

        }
    }
}
