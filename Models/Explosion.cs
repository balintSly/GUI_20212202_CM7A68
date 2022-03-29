using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_20212202_CM7A68.Models
{
    public class Explosion      //TODO: different explosion types using polymorphism
    {
        public Point Center { get; set; }
        public int Damage { get; set; }     
        public int ExplosionHeight { get; set; }
        public int ExplosionWidth { get; set; }
        Robot friendly;

        public Explosion(Robot friendly)
        {
            this.friendly = friendly;
        }

        public void Detonate()
        {
            //TODO: check surroundings for non-friendly robot
        }

    }
}
