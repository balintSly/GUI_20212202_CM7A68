using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GUI_20212202_CM7A68.Logic.GameLogic;

namespace GUI_20212202_CM7A68.Models
{
    public class Robot
    {
        public bool IsControllable { get; set; }
        private int health;
        private int shield;
        static Random r = new Random();
        private int bombNumber;

        public bool IsMoving { get; set; }
        public bool IsJumping { get; set; }
        public int BombNumber
        {
            get => bombNumber;
            set
            {
                if (value > 5)
                {
                    bombNumber = 5;
                }
            }
        }
        public int BombLoading { get; set; }
        public bool IsReloading { get; set; }
        public int Health
        {
            get { return health; }
            set
            {
                if (this.Shield > 0 && r.Next(0, 10) < 3) //armort sebezzük
                {
                    this.Shield -= (health - value);
                }
                else
                {
                    if (value > 0 && value <= 100)
                    {
                        health = value;
                    }
                    else if (value > 100)
                    {
                        health = 100;
                    }
                    else
                    {
                        health = 0;
                    }
                }
            }
        }

        public int Shield
        {
            get => shield;
            set
            {
                if (value > 0 && value <= 100)
                {
                    shield = value;
                }
                else if (value > 100)
                {
                    shield = 100;
                }
                else
                {
                    shield = 0;
                }
            }
        }
        public Point Center { get; set; }
        public Robot(Point spawnpoint)
        {
            this.Health = 100;
            this.Shield = 100;
            this.Center = spawnpoint;
            this.IsMoving = false;
            this.IsJumping = false;
            this.IsControllable = false;
            this.BombNumber = 2;
            this.BombLoading = 0;
            this.IsReloading = false;
        }
    }
}