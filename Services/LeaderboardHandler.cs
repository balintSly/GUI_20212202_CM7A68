using GUI_20212202_CM7A68.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_20212202_CM7A68.Services
{
    public class LeaderboardHandler : ILeaderboardHandler
    {
        public void SaveGame(Player playerOne, Player playerTwo)
        {
            if (!File.Exists("leaderboard.json"))
            {
                File.WriteAllText("leaderboard.json", JsonConvert.SerializeObject(new List<Player>() { playerOne, playerTwo }));
            }
            else
            {
                var players = JsonConvert.DeserializeObject<List<Player>>(File.ReadAllText("leaderboard.json"));
                if (!players.Contains(playerOne))
                {
                    players.Add(playerOne);
                }
                else
                {
                    var old = players.Where(x => x.Name == playerOne.Name).FirstOrDefault();
                    old.WonMatches += playerOne.WonMatches;
                    old.WonRounds += playerOne.WonRounds;
                }
                if (!players.Contains(playerTwo))
                {
                    players.Add(playerTwo);
                }
                else
                {
                    var old = players.Where(x => x.Name == playerTwo.Name).FirstOrDefault();
                    old.WonMatches += playerTwo.WonMatches;
                    old.WonRounds += playerTwo.WonRounds;
                }
                File.WriteAllText("leaderboard.json", JsonConvert.SerializeObject(players));
            }
        }
        public IList<Player> GetLeaderboard()
        {
            if (!File.Exists("leaderboard.json") || File.ReadAllText("leaderboard.json").Length == 0)
            {
                return new List<Player>();
            }
            else
            {
                return JsonConvert.DeserializeObject<List<Player>>(File.ReadAllText("leaderboard.json"));
            }
        }
    }
}
