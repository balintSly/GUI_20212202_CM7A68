using GUI_20212202_CM7A68.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_20212202_CM7A68.Logic
{
    public class GameLogic:IGameModel, IGameControl
    {
        Size area;
        public List<Bomb> Bombs { get; set; }
        public void SetupSize(Size area)
        {
            Bombs = new List<Bomb>();
            this.area = area;
        }
        public void NewBomb()
        {
            Bombs.Add(new FallingBomb(new System.Drawing.Point(200, 550)));
            Bombs.Add(new ThrowingBomb(new System.Drawing.Point(1000, 550), -1));
            Bombs.Add(new ThrowingBomb(new System.Drawing.Point(400, 550), 1));

        }
        public void TimeStep()
        {
            //TODO: minden mozgatást, állapotváltozást, ütközést itt állítani, ez 20ms-ként le fog futni
            for (int i = 0; i < Bombs.Count; i++)
            {
                Bombs[i].Move(600, new System.Drawing.Size((int)area.Width, (int)area.Height));
                Bombs[i].Heal -= 1;
                if (Bombs[i].Heal <= 0)
                {
                    Bombs.RemoveAt(i);
                    //robbanás meghívása itt lehetséges lenne
                }
            }
        }

    }
}
