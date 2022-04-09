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
        public abstract void Change(Robot robot);
        public Vector Speed { get; set; }
        static Random r = new Random();
        public Rect Hitbox { get; set; }

        bool direction;

        public Item(Size area)
        {
            direction = true;
            Speed = new Vector(0, Area.Height / 250);
            this.Center = new Point(Randomizer(0,(int)area.Width), (int)(area.Height * 0.85));
            this.Area = area;
            Hitbox = new Rect(Center.X - area.Width / 32, Center.Y - area.Height / 32, area.Width / 16, area.Height / 16);
        }

        public bool CheckHitbox(Robot Robot1, Robot Robot2)
        {
            Rect robot1Rect = new Rect(Robot1.Center.X - Area.Width / 12, Robot1.Center.Y - Area.Height / 6, Area.Width / 6, Area.Height / 3);
            Rect robot2Rect = new Rect(Robot2.Center.X - Area.Width / 12, Robot2.Center.Y - Area.Height / 6, Area.Width / 6, Area.Height / 3);

            if (Hitbox.IntersectsWith(robot1Rect))
            {
                Change(Robot1);
                return true;
            }
            else if (Hitbox.IntersectsWith(robot2Rect))
            {
                Change(Robot2);
                return true;
            }
            return false;
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
                Hitbox = new Rect(Center.X - Area.Width / 32, Center.Y - Area.Height / 32, Area.Width / 16, Area.Height / 16);
                direction = true;
            }
            else
            {
                direction = false;
                Speed = new Vector(0, -(Area.Height / 250));
            }
            if (newCenter.Y >= Area.Height*0.8 && !direction)
            {
                Center = newCenter;
                Hitbox = new Rect(Center.X - Area.Width / 32, Center.Y - Area.Height / 32, Area.Width / 16, Area.Height / 16);
                direction = false;
            }
            else
            {
                direction = true;
                Speed = new Vector(0, Area.Height / 250);
            }
        }
    }

    public class HealBoost : Item
    {
        public HealBoost(Size area) : base(area)
        {
            this.Value = 10;
        }
        public override void Change(Robot robot)
        {
            if (robot.Health + Value <= 100)
            {
                robot.Health += Value;
            }
            else
            {
                robot.Health = 100;
            }
            
        }
    }

    public class ArmorBoost : Item
    {
        public ArmorBoost(Size area) : base(area)
        {
            this.Value = 10;
        }
        public override void Change(Robot robot)
        {
            if (robot.Shield + Value <= 100)
            {
                robot.Shield += Value;
            }
            else
            {
                robot.Shield = 100;
            }
            
        }
    }
}
