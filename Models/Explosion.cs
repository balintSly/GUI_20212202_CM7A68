using System;
using System.Collections.Generic;
using System.Drawing;
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
        public int ExplosionHeight { get; set; }
        public int ExplosionWidth { get; set; }                 
        List<string> CenterAnim { get; set; }        // contains the paths for the required animations
        List<string> LeftAnim { get; set; }        
        List<string> TopAnim { get; set; }        
        List<string> RightAnim { get; set; }        
        List<string> BottomAnim { get; set; }
        List<string> SideContAnim { get; set; }
        List<string> VertContAnim { get; set; }

        Robot friendly;
        Size area;
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
            ;
                          
               
        }

        public Explosion(Robot friendly, Size area ,Point Center)    // maybe expect dmg, width and height as parameters
        {
            InitLists();
            SetupAnims();
            this.friendly = friendly;
            this.Center = Center;
            this.area = area;
            
            //temporary hardcode
            ExplosionHeight = 2;
            ExplosionWidth = 4;
            //

        }

        public void Detonate()
        {
            //TODO: Check surroundings for non-friendly robot
            //TODO: Call hit robot's GetDamaged(int damage) method

        }

    }
}
