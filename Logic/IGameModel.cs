using GUI_20212202_CM7A68.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace GUI_20212202_CM7A68.Logic
{
    public interface IGameModel
    {
        List<Robot> Robots { get; }
        List<Player> Players { get; set; }
        List<Bomb> Bombs { get; set; }
        List<Explosion> Explosions { get; set; }
        bool GameStarted { get; set; }
        bool GameOver { get; set; }
        bool GamePaused { get; set; }
        TimeSpan RoundTime { get; set; }
        void InitLogic();
        void SetupSize(Size area); //átveszi a képernyőméretet
        string SelectedMapPath { get; set; }
        List<Item> Items { get; set; }
        void ItemTimeStep();
        void NewRedFallingBomb(Point robotPos);
        void NewGreenFallingBomb(Point robotPos);
        void NewRedThrowingBomb(Point robotPos, int direction);
        void NewGreenThrowingBomb(Point robotPos, int direction);
        object LockObject { get; set; }
        Random r { get; set; }
    }
}