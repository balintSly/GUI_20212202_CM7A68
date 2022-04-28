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
        public bool GameStarted { get; set; }
        void SetupSize(Size area); //átveszi a képernyőméretet
        string SelectedMapPath { get; set; }
        List<Bomb> Bombs { get; set; }
        List<Explosion> Explosions { get; set; }
        void NewRedFallingBomb(Point robotPos);
        void NewGreenFallingBomb(Point robotPos);
        void NewRedThrowingBomb(Point robotPos, int direction);
        void NewGreenThrowingBomb(Point robotPos, int direction);
        public TimeSpan RoundTime { get; set; }
        bool GamePaused { get; set; }
        void InitLogic();
        public bool GameOver { get; set; }
        object LockObject { get; set; }
        Random r { get; set; }
    }
}