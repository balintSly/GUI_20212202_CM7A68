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
        public Vector Speed { get; set; }
        public System.Drawing.Point Center { get; set; }
        public abstract void Move(int Floor, System.Drawing.Size area);
        public double Heal { get; set; }
        public int Power { get; set; }
    }

    public class FallingBomb : Bomb
    {
        public FallingBomb(System.Drawing.Point center)
        {
            this.Center = center;
            Speed = new Vector(0, 2);
            Heal = 100;
            Power = 10;
        }
        public override void Move(int Floor, System.Drawing.Size are)
        {
            System.Drawing.Point newCenter =
                new System.Drawing.Point(Center.X + (int)Speed.X,
                Center.Y + (int)Speed.Y);
            if (newCenter.Y <= Floor)
            {
                Center = newCenter;

            }
        }
    }
}
