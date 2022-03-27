using GUI_20212202_CM7A68.Logic;
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
        public void SetupSize(Size area)
        {
            this.area = area;
            model.SetupSize(new System.Drawing.Size((int)area.Width, (int)area.Height));
            InvalidateVisual();
        }
        public void SetupModel(IGameModel model)
        {
            this.model = model;
        }
        int piccount = 0;
        string robot1skin = "robotpic_stand2.png";
        string robot2skin = "robotpic_stand2.png";
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (area.Width > 0 && area.Height > 0)
            {
                base.OnRender(drawingContext);
                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Backgrounds", "mkmap1.jpg"),
                    UriKind.RelativeOrAbsolute))), null, new Rect(0, 0, area.Width, area.Height));

                #region Képvátogatás
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
                //robot1
                string hp1 = $"Helath_{model.Robot1.Health}_pecent.png";
                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "HUD", "Health", hp1),
                   UriKind.RelativeOrAbsolute))), null, new Rect(area.Width * 0.02, area.Height * 0.02, area.Width * 0.4, area.Height * 0.05));
                //robot2
                string hp2 = $"Helath_{model.Robot2.Health}_pecent.png";
                drawingContext.PushTransform(new ScaleTransform(-1, 1, area.Width * 0.78, area.Height * 0.02));
                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "HUD", "Health", hp2),
                   UriKind.RelativeOrAbsolute))), null, new Rect(area.Width * 0.58, area.Height * 0.02, area.Width * 0.4, area.Height * 0.05));
                drawingContext.Pop();
                #endregion
            }

        }
    }
}
