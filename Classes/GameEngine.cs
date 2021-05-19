﻿using LudoGame.Classes;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoGame
{
    public class GameEngine
    {
        CanvasBitmap backgroundImage, blackholeImage, menuBackGroundImage;
        private CanvasBitmap[] _sprites;
        //private Sound[] _sounds;
        private GameTile[] _gameTiles;
        private Player _player;
        private AIPlayer _aIPlayer;
        private InputReader _input;
        private string _fileloction;
        private Cards[] _cards;
        private Wormhole _wormhole;

        public static int CurrentGameState = 0;
        public List<GameState> GameStates = new List<GameState>();

        public void GameStateInit()
        {
            GameStates.Add(new GameState() { name = "Menu" });
            GameStates.Add(new GameState() { name = "Playing" });
            GameStates.Add(new GameState() { name = "Paused" });
            GameStates.Add(new GameState() { name = "GameOver" });
        }
    }
}
