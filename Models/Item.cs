using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI_20212202_CM7A68.Models
{
    public abstract class Item
    {
        public Point Center { get; set; }
        public int Value { get; set; }
        public Size Area { get; set; }
        public abstract void Change(ref Robot robot);
        public Vector Speed { get; set; }
        static Random r = new Random();

        bool direction;

        public Item(Size area)
        {
            direction = true;
            Speed = new Vector(0, Area.Height / 112.5);
            this.Center = new Point(Randomizer(0,(int)area.Width), (int)(area.Height * 0.85));
            this.Area = area;
        }

        private int Randomizer(int min, int max)
        {
            int rnd = 0;
            do
            {
                rnd = r.Next(min, max + 1);
            } while (rnd == 0);
            return rnd;
        }

        public void Move(int Floor)
        {
            
            Point newCenter =
                new Point(Center.X + (int)Speed.X,
                Center.Y + (int)Speed.Y);
            if (newCenter.Y <= Floor && direction)
            {
                Center = newCenter;
                direction = true;
            }
            else
            {
                direction = false;
                Speed = new Vector(0, -Area.Height / 112.5);
            }
            if (newCenter.Y >= Floor - Area.Height*0.88 && !direction)
            {
                Center = newCenter;
                direction = false;
            }
            else
            {
                direction = true;
                Speed = new Vector(0, Area.Height / 112.5);
            }
        }
    }

    public class HealBoost : Item
    {
        public HealBoost(Size area) : base(area)
        {
            this.Value = 20;
        }
        public override void Change(ref Robot robot)
        {
            robot.Health += Value;
        }
    }

    public class ArmorBoost : Item
    {
        public ArmorBoost(Size area) : base(area)
        {
            this.Value = 20;
        }
        public override void Change(ref Robot robot)
        {
            robot.Shield += Value;
        }
    }
}
