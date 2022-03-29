using GUI_20212202_CM7A68.Logic;
using GUI_20212202_CM7A68.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        public string Time { get; set; }
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
        public Brush BombBrush
        {
            get
            {
                return new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images","Bomb", "bomb1.png"), UriKind.RelativeOrAbsolute)));
            }
        }
        int piccount = 0;
        string robot1skin = "robotpic_stand2.png";
        string robot2skin = "robotpic_stand2.png";
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (area.Width > 0 && area.Height > 0)
            {
                base.OnRender(drawingContext);
                //map kirajzolás
                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Backgrounds", "mkmap1.jpg"),
                    UriKind.RelativeOrAbsolute))), null, new Rect(0, 0, area.Width, area.Height));

                #region Skinváltogatás
                //állaptok szerint kép kiválsztása: áll, mozog, ugrik, gugol, dob
                //ugrás: robot.center.y < (int)(area.Height * 0.8) 
                if (model.Robot1.Center.Y < (int)(area.Height * 0.8))
                {
                    robot1skin = "robotpic_jump.png";
                }
                else if (!model.Robot1IsMoving)
                {
                    robot1skin = "robotpic_stand2.png";
                }
                else
                {
                    piccount++;
                    if (piccount % 9 == 0 || piccount % 9 == 1 || piccount % 9 == 2)
                    {
                        robot1skin = "robotpic_stand.png";
                    }
                    else if (piccount % 9 == 3 || piccount % 9 == 4 || piccount % 9 == 5)
                    {
                        robot1skin = "robotpic_step.png";
                    }
                    else if (piccount % 9 == 6 || piccount % 9 == 7 || piccount % 9 == 8)
                    {
                        robot1skin = "robotpic_step2.png";
                        piccount %= 9;
                    }
                }
                if (model.Robot2.Center.Y < (int)(area.Height * 0.8))
                {
                    robot2skin = "robotpic_jump.png";
                }
                else if (!model.Robot2IsMoving)
                {
                    robot2skin = "robotpic_stand2.png";
                }
                else
                {
                    piccount++;
                    if (piccount % 9 == 0 || piccount % 9 == 1 || piccount % 9 == 2)
                    {
                        robot2skin = "robotpic_stand.png";
                    }
                    else if (piccount % 9 == 3 || piccount % 9 == 4 || piccount % 9 == 5)
                    {
                        robot2skin = "robotpic_step.png";
                    }
                    else if (piccount % 9 == 6 || piccount % 9 == 7 || piccount % 9 == 8)
                    {
                        robot2skin = "robotpic_step2.png";
                        piccount %= 9;
                    }
                }
                #endregion
                #region RobotKirajzolás
                if (model.Robot1.Center.X < model.Robot2.Center.X)//merre nézzenek a robotok, kirajzolásuk
                {
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Robots", robot1skin),
                 UriKind.RelativeOrAbsolute))), null, new Rect(model.Robot1.Center.X - area.Width / 12, model.Robot1.Center.Y - area.Height / 6, area.Width / 6, area.Height / 3));

                    drawingContext.PushTransform(new ScaleTransform(-1, 1, model.Robot2.Center.X, model.Robot2.Center.Y));
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Robots", robot2skin),
                  UriKind.RelativeOrAbsolute))), null, new Rect(model.Robot2.Center.X - area.Width / 12, model.Robot2.Center.Y - area.Height / 6, area.Width / 6, area.Height / 3));
                    drawingContext.Pop();
                }
                else
                {
                    drawingContext.PushTransform(new ScaleTransform(-1, 1, model.Robot1.Center.X, model.Robot1.Center.Y));
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Robots", robot1skin),
               UriKind.RelativeOrAbsolute))), null, new Rect(model.Robot1.Center.X - area.Width / 12, model.Robot1.Center.Y - area.Height / 6, area.Width / 6, area.Height / 3));
                    drawingContext.Pop();

                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Robots", robot2skin),
                  UriKind.RelativeOrAbsolute))), null, new Rect(model.Robot2.Center.X - area.Width / 12, model.Robot2.Center.Y - area.Height / 6, area.Width / 6, area.Height / 3));
                }
                #endregion
                #region HUD kirajzolás
                //áttetsző fekete háttér
                drawingContext.DrawRectangle(new SolidColorBrush(Color.FromArgb(150, 0, 0, 0))
                    , null, new Rect(0, area.Height * 0.02, area.Width, area.Height * 0.1));
                //robot1
                string hp1 = $"Helath_{model.Robot1.Health}_pecent.png";
                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "HUD", "Health", hp1),
                   UriKind.RelativeOrAbsolute))), null, new Rect(area.Width * 0.048, area.Height * 0.02, area.Width * 0.4, area.Height * 0.05));

                string armor1 = $"Armor_{model.Robot1.Shield}_pecent.png";
                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "HUD", "Armor", armor1),
                  UriKind.RelativeOrAbsolute))), null, new Rect(area.Width * 0.048, area.Height * 0.07, area.Width * 0.4, area.Height * 0.05));

                //robot2
                string hp2 = $"Helath_{model.Robot2.Health}_pecent.png";
                drawingContext.PushTransform(new ScaleTransform(-1, 1, area.Width * 0.78, area.Height * 0.02));
                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "HUD", "Health", hp2),
                   UriKind.RelativeOrAbsolute))), null, new Rect(area.Width * 0.608, area.Height * 0.02, area.Width * 0.4, area.Height * 0.05));
                drawingContext.Pop();

                string armor2 = $"Armor_{model.Robot2.Shield}_pecent.png";
                drawingContext.PushTransform(new ScaleTransform(-1, 1, area.Width * 0.78, area.Height * 0.07));
                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "HUD", "Armor", armor1),
                  UriKind.RelativeOrAbsolute))), null, new Rect(area.Width * 0.608, area.Height * 0.07, area.Width * 0.4, area.Height * 0.05));
                drawingContext.Pop();

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
                drawingContext.DrawText(new FormattedText(Time, System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight, new Typeface(new FontFamily("Consolas"), FontStyles.Normal, FontWeights.Bold,
                    FontStretches.Normal), area.Height * 0.05, Brushes.Red), new Point(area.Width * 0.465, area.Height * 0.05));


                #endregion
                foreach (var bomb in model.Bombs)
                {
                    drawingContext.DrawRectangle(BombBrush, null, new Rect(bomb.Center.X - area.Width / 24, bomb.Center.Y - area.Width / 24, area.Width / 12, area.Height / 12));
                    string bomb_hp = BombHp(bomb);

                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "BombHp", "Heath", bomb_hp),
                    UriKind.RelativeOrAbsolute))), null, new Rect(bomb.Center.X - area.Width / 24, bomb.Center.Y + area.Height / 35, area.Width / 22, area.Height / 30));
                    ;

                }
            }
            //(ezt a metódust le se nyissátok xd)
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

        }
    }
}
