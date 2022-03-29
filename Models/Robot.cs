using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GUI_20212202_CM7A68.Logic.GameLogic;

namespace GUI_20212202_CM7A68.Models
{
    public class Robot      //TEMPORARY: Remove before merging
    {
        public bool IsControllable { get; set; }
        public int Health { get; set; }
        public int Shield { get; set; }
        public Point Center { get; set; }
        public Robot(Point spawnpoint)
        {
            this.Health = 100;
            this.Shield = 100;
            this.Center = spawnpoint;
        }
    }
}