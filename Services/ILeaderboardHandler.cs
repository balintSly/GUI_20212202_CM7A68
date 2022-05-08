using GUI_20212202_CM7A68.Models;
using System.Collections.Generic;

namespace GUI_20212202_CM7A68.Services
{
    public interface ILeaderboardHandler
    {
        IList<Player> GetLeaderboard();
        void SaveGame(Player playerOne, Player playerTwo);
    }
}