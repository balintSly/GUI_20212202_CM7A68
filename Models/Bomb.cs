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
        public Size Area { get; set; }
        public Vector Speed { get; set; }
        public Point Center { get; set; }
        public abstract void Move(int Floor);
        public double Heal { get; set; }
        public int Power { get; set; }
    }

    public class FallingBomb : Bomb
    {
        public FallingBomb(Point center, Size area)
        {
            this.Center = center;
            this.Area = area;
            Speed = new Vector(0, area.Height/112.5);
            Heal = 100;
            Power = 10;
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
        public ThrowingBomb(Point center, Size area, int directionX)
        {
            Center = center;
            this.direction = directionX;
            this.Area = area;
            x = 0;
            Heal = 100;
            Power = 20;
            Speed = new Vector(area.Width/120.0, area.Height/65.0);
        }
        double x;
        //direction: melyik irányba kell dobni, 1-jobbra, (-1)-balra
        int direction;

        private void RightMove(int Floor)
        {
            if (Center.Y - Speed.Y <= Floor)
            {
                //ennyit megy felfelé
                if (x <= Speed.X * 4)
                {
                    x++;
                    Point newCenter_up =
                            new Point(Center.X + (int)Speed.X,
                            Center.Y - (int)Speed.Y);
                    Center = newCenter_up;
                }
                //vízszintes irány
                else if (x >= Speed.X * 4 && x < Speed.X * 5)
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
            else if (x >= Speed.X * 5 && x < Speed.X * 6)
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
                if (x < Speed.X * 4)
                {
                    x++;
                    Point newCenter_up =
                            new Point(Center.X - (int)Speed.X,
                            Center.Y - (int)Speed.Y);
                    Center = newCenter_up;
                }
                //vízszintes
                else if (x >= Speed.X * 4 && x < Speed.X * 5)
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
            else if(x>=Speed.X*5 && x < Speed.X*6)
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
                if (Center.X <=Area.Width)
                {
                    RightMove(Floor);
                }
                else
                {
                    x = Speed.X * 5;
                    direction = -1;
                }

                
            }
            //balra
            else if (direction == -1)
            {
                if (Center.X >= 0)
                {
                    LeftMove(Floor);
                }
                else
                {
                    x = Speed.X * 5;
                    direction = 1;
                }
                
            }
        }
    }
}
