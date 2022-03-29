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
    public class Display:FrameworkElement
    {
        Size area;
        IGameModel model;
        public void SetupSize(Size area)
        {
            this.area = area;
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
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Backgrounds", "mkmap1.jpg"), 
                UriKind.RelativeOrAbsolute))), null, new Rect(0,0,area.Width, area.Height));

            //bombák kirajzolása
            if (area.Width > 0 && area.Height > 0 && model != null)
            {
                foreach (var bomb in model.Bombs)
                {
                    drawingContext.DrawRectangle(BombBrush, null, new Rect(bomb.Center.X - area.Width / 24, bomb.Center.Y - area.Width / 24, area.Width / 12, area.Height / 12));
                    string bomb_hp = BombHp(bomb);

                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images","BombHp", "Heath", bomb_hp),
                    UriKind.RelativeOrAbsolute))), null, new Rect(bomb.Center.X - area.Width / 24, bomb.Center.Y + area.Height / 35, area.Width / 22, area.Height / 30));
                    ;

                }
            }
        }
        //(ezt a metódust le se nyissátok xd)
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
