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

        bool direction;

        public Item(Point center, Size area)
        {
            direction = true;
            Speed = new Vector(0, Area.Height / 112.5);
            this.Center = center;
            this.Area = area;
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
        public HealBoost(Point center, Size area) : base(center, area)
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
        public ArmorBoost(Point center, Size area) : base(center, area)
        {
            this.Value = 20;
        }
        public override void Change(ref Robot robot)
        {
            robot.Shield += Value;
        }
    }
}
