﻿using GUI_20212202_CM7A68.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace GUI_20212202_CM7A68.Logic
{
    public interface IGameModel
    {
        List<Robot> Robots { get; }
        void SetupSize(Size area); //átveszi a képernyőméretet
        bool Robot1IsMoving { get; set; } //ezen keresztül tudja a renderer, hogy mozog a robot
        bool Robot2IsMoving { get; set; }
        string Player1Name { get; set; }
        string Player2Name { get; set; }
        string PlayerOneColor { get; set; }
        string PlayerTwoColor { get; set; }
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
        public int PlayerOneWins { get; set; }
        public int PlayerTwoWins { get; set; }
        public bool GameOver { get; set; }
    }
}