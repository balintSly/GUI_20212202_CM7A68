using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI_20212202_CM7A68.Models
{
    public abstract class Bomb
    {
        public int BombCount { get; set; }
        public Size Area { get; set; }
        public Vector Speed { get; set; }
        public Point Center { get; set; }
        public abstract void Move(int Floor);
        public double Heal { get; set; }
        public int Power { get; set; }
        public ConsoleColor Color { get; set; }
    }

    public class FallingBomb : Bomb
    {
        public FallingBomb(Point center, Size area, ConsoleColor color)
        {
            this.Center = center;
            this.Area = area;
            Speed = new Vector(0, area.Height/112.5);
            Heal = 100;
            Power = 10;
            this.BombCount = 0;
            this.Color = color;
        }
        public override void Move(int Floor)
        {
            Point newCenter =
                new Point(Center.X + (int)Speed.X,
                Center.Y + (int)Speed.Y);
            if (newCenter.Y <= Floor)
            {
                Center = newCenter;

            }
        }
    }
    internal class ThrowingBomb : Bomb
    {
        public ThrowingBomb(Point center, Size area, int directionX, ConsoleColor color)
        {
            Center = center;
            this.direction = directionX;
            this.Area = area;
            x = 0;
            Heal = 100;
            Power = 20;
            Speed = new Vector(area.Width/120.0, area.Height/65.0);
            this.BombCount = 0;
            this.Color = color;
        }
        double x;
        //direction: melyik irányba kell dobni, 1-jobbra, (-1)-balra
        int direction;

        private void RightMove(int Floor)
        {
            if (Center.Y - Speed.Y <= Floor)
            {
                //ennyit megy felfelé
                if (x < 15)
                {
                    x++;
                    Point newCenter_up =
                            new Point(Center.X + (int)Speed.X,
                            Center.Y - (int)Speed.Y);
                    Center = newCenter_up;
                }
                //vízszintes irány
                else if (x >= 15 && x < 20)
                {
                    x++;
                    Point newCenter_cons =
                            new Point(Center.X + (int)Speed.X,
                            Center.Y);
                    Center = newCenter_cons;
                }
                //zuhanás
                else
                {
                    Point newCenter_down = new Point(Center.X + (int)Speed.X,
                                Center.Y + (int)Speed.Y);
                    Center = newCenter_down;
                }
            }
            else if (x >= 20 && x < 27)
            {
                x++;
                Point newCenter_cons =
                            new Point(Center.X + (int)Speed.X,
                            Center.Y);
                Center = newCenter_cons;
            }
        }
        private void LeftMove(int Floor)
        {
            if (Center.Y - Speed.Y <= Floor)
            {
                //felfelé
                if (x < 15)
                {
                    x++;
                    Point newCenter_up =
                            new Point(Center.X - (int)Speed.X,
                            Center.Y - (int)Speed.Y);
                    Center = newCenter_up;
                }
                //vízszintes
                else if (x >= 15 && x < 20)
                {
                    x++;
                    Point newCenter_cons =
                            new Point(Center.X - (int)Speed.X,
                            Center.Y);
                    Center = newCenter_cons;
                }
                //lefelé
                else
                {
                    Point newCenter_down = new Point(Center.X - (int)Speed.X,
                                Center.Y + (int)Speed.Y);
                    Center = newCenter_down;
                }
            }
            else if(x>=20 && x < 27)
            {
                x++;
                Point newCenter_cons =
                            new Point(Center.X - (int)Speed.X,
                            Center.Y);
                Center = newCenter_cons;
            }
        }

        //TODO: pattogás a falról
        public override void Move(int Floor)
        {
            //jobbra
            if (direction == 1)
            {
                if (Center.X < Area.Width-Area.Width/24)
                {
                    RightMove(Floor);
                }
                else
                {
                    x = 20;
                    direction = -1;
                }

                
            }
            //balra
            else if (direction == -1)
            {
                if (Center.X > Area.Width/24)
                {
                    LeftMove(Floor);
                }
                else
                {
                    x = 20;
                    direction = 1;
                }
                
            }
        }
    }
}
