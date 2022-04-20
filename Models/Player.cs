using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GUI_20212202_CM7A68.Models
{
    public class Player
    {
        public Player(string name, int wonMatches, int wonRounds)
        {
            Name = name;
            WonMatches = wonMatches;
            WonRounds = wonRounds;
        }
        public string Name { get; set; }
        public int WonMatches { get; set; }
        public int WonRounds { get; set; }
        [JsonIgnore]
        public bool IsPlayer { get; set; }
        [JsonIgnore]
        public string SelectedColor { get; set; }
        public override bool Equals(object? obj)
        {
            return this.Name == (obj as Player).Name;
        }
    }
}
