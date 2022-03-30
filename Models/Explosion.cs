using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_20212202_CM7A68.Models
{
    //TODO: different explosion types using polymorphism
    //TODO: process explosion asset cleaner
    //TODO: setup animations and explosion parts
    //TODO: refactor bomb animations to separate class
    public class Explosion      
    {
        public Point Center { get; set; }
        public int Damage { get; set; }     
        public int Height { get; set; }
        public int Width { get; set; }
        public Size PartSize { get; set; }                  // required for rendering
        public List<string> CenterAnim { get; set; }        // contains the paths for the required animations   //MAYBE: change lists to queue to make animating easier
        public List<string> LeftAnim { get; set; }
        public List<string> TopAnim { get; set; }
        public List<string> RightAnim { get; set; }
        public List<string> BottomAnim { get; set; }
        public List<string> SideContAnim { get; set; }
        public List<string> VertContAnim { get; set; }
        public int FrameCount { get; set; }

        Size gameArea;

        void InitLists()
        {
            CenterAnim = new List<string>();
            LeftAnim = new List<string>();
            TopAnim = new List<string>();
            RightAnim = new List<string>();
            BottomAnim = new List<string>();
            SideContAnim = new List<string>();
            VertContAnim = new List<string>();
        }
        void SetupAnims()   
        {
            string[] fileNames = new string[] { "center", "left", "top", "right", "bottom", "sidecont", "vertcont" };
            List<string>[] partLists = new List<string>[] { CenterAnim, LeftAnim, TopAnim, RightAnim, BottomAnim, SideContAnim, VertContAnim };

            for (int i = 0; i < partLists.Length; ++i)
            {
                for (int j = 0; j < 7; ++j)
                {
                    partLists[i].Add(Path.Combine("Renderer", "Images", "Explosions", "Bomberman", $"{fileNames[i]}{j}.png"));
                }
            }              
        }

        public Explosion(Size gameArea ,Point Center, int Height = 1, int Width = 1, int Damage = 10)
        {
            InitLists();
            SetupAnims();
            PartSize = new Size(gameArea.Width / 18, gameArea.Height / 8);
            this.Damage = Damage;
            this.Center = Center;
            this.gameArea = gameArea;
            FrameCount = 0;

            this.Height = Height;
            this.Width = Width;


        }


        public void Detonate(Rect robot1, Rect robot2)  //hitbox.IntersectsWith(robotRect)
        {
            Rect verticalHitBox = new Rect(
                Center.X - (PartSize.Width / 2) - (Width * PartSize.Width),
                Center.Y - (PartSize.Height / 2),
                Width * PartSize.Width * 2 + PartSize.Width,
                PartSize.Height
                );

            Rect HorizontalHitBox = new Rect(
                Center.X - (PartSize.Width / 2),
                Center.Y - (PartSize.Height / 2) - (Height * PartSize.Height),
                PartSize.Width,
                Height * 2 * PartSize.Height + PartSize.Height
                );


            //TODO: Check surroundings for non-friendly robot
            //TODO: Call hit robot's GetDamaged(int damage) method


        }

    }
}
