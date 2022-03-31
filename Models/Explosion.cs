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
        public int FrameCount { get; set; }
        public bool LastFrameFlag { get; set; }
        public List<string> CenterAnim { get; set; }        // contains the paths for the required animations  
        public List<string> LeftAnim { get; set; }
        public List<string> TopAnim { get; set; }
        public List<string> RightAnim { get; set; }
        public List<string> BottomAnim { get; set; }
        public List<string> SideContAnim { get; set; }
        public List<string> VertContAnim { get; set; }

        bool robot1HitFlag = false;
        bool robot2HitFlag = false;
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

        public Explosion(Size gameArea ,Point Center, int Damage = 10, int Height = 1, int Width = 1)
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


        public void CheckHitBox(Robot Robot1, Robot Robot2)  
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

            Rect robot1Rect = new Rect(Robot1.Center.X - gameArea.Width / 12, Robot1.Center.Y - gameArea.Height / 6, gameArea.Width / 6, gameArea.Height / 3);
            Rect robot2Rect = new Rect(Robot2.Center.X - gameArea.Width / 12, Robot2.Center.Y - gameArea.Height / 6, gameArea.Width / 6, gameArea.Height / 3);
            if (!robot1HitFlag && (verticalHitBox.IntersectsWith(robot1Rect) || HorizontalHitBox.IntersectsWith(robot1Rect)))
            {
                Robot1.Health -= Damage;
                robot1HitFlag = true;
            }
            if (!robot2HitFlag && (verticalHitBox.IntersectsWith(robot2Rect) || HorizontalHitBox.IntersectsWith(robot2Rect)))
            {
                Robot2.Health -= Damage;
                robot2HitFlag = true;
            }


        }

    }
}
