using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace LudoGame.Classes
{
    public class GamePiece
    {
        public GameTile tile;
        public GameTile tempTile;
        public GameRace race;
        public bool shield;
        public bool atHomePosition = true;
        public bool isAvailableMove = false;
        public bool IsOccupied = false;
        public Vector2 homePosition;
        public Drawable drawable;

        public GamePiece(GameRace gameRace, Vector2 offset, GameTile tile, Drawable drawable)
        {
            this.race = gameRace;
            this.tile = tile;
            this.drawable = drawable;
            this.homePosition = tile.Position + offset;
            this.tempTile = tile;
        }

        public static bool MoveToGameTile(int diceResult, GamePiece gamePiece)
        {
            if (gamePiece.isAvailableMove)
            {
                for (int i = 0; i < diceResult; i++) // Calls the change tile funk or each number on the dice
                {
                    gamePiece.ChangeTile(gamePiece, diceResult);// Changes tile the amount of times the loop goes.
                }
                gamePiece.isAvailableMove = false;
                return true;
            } 
            else
            {
                return false;
            }
        }

        /*
        public static void MovePiece(GamePiece gamePiece, GameTile gameTile)
        {
            if (gameTile.previousTile == null && gamePiece.atHomePosition == false && gamePiece.race == gameTile.raceHome)
            {
                gamePiece.drawable.Position = gamePiece.homePosition;
            }
            else if(gamePiece.race == gameTile.raceHome || gameTile.raceHome == GameRace.None)
            {
                gamePiece.drawable.Position = gameTile.Position;
                gamePiece.atHomePosition = false;
            }
        }
        */

        /// <summary>
        /// Initial method to check if player can move or not
        /// </summary>
        public static void CheckAvailableMoves(int diceResult, GamePiece gamePiece, GamePiece[] gamePieces)
        {
            gamePiece.tempTile = gamePiece.tile;

            if (gamePiece.atHomePosition)
            {
                if (diceResult == 1 || diceResult == 6) 
                {
                    CheckPossibleTile(diceResult, gamePiece, gamePieces);
                }
            }
            else
            {
                CheckPossibleTile(diceResult, gamePiece, gamePieces);
            }
        }

        /// <summary>
        /// Checks if player can move or not
        /// </summary>
        private static void CheckPossibleTile(int diceResult, GamePiece gamePiece, GamePiece[] gamePieces)
        {
            for (int i = 0; i < diceResult; i++)
            {
                for (int j = 0; j < gamePieces.Length; j++) // Loop trough players gamepieces
                {
                    GamePiece tempGamePiece = gamePieces[j]; //Store current gamepiece 

                    if (gamePiece.tempTile.nextTile != tempGamePiece.tile) //Check that the next tile is not occupide by red gamepiece
                    {
                        gamePiece.isAvailableMove = true;
                    } 
                    else
                    {
                        gamePiece.isAvailableMove = false;
                        break;
                    }
                }
                if (gamePiece.isAvailableMove)
                {
                    if (gamePiece.atHomePosition)
                    {
                        gamePiece.tempTile = gamePiece.tempTile.nextTile; // If true check next tile until all steps are checked or a red piece is in the way
                    }
                    else
                    {
                        gamePiece.tempTile = gamePiece.tempTile.GetNextTile(gamePiece.race);
                    }

                }
            }

            if (gamePiece.isAvailableMove)
            {
                gamePiece.tempTile.drawable.isHover = true; // If the piece is able to move highlight the tile the gamepiece can move to
            }
        }

        /// <summary>
        /// Checks if gamePiece is red and change isHoover in drawable class
        /// </summary>
        public static void Hover(GamePiece gamePiece, bool isHover, int diceSave)
        {
            if (gamePiece.race == GameRace.Red) //Only want hover on red player pieces, not future AI
            {
                gamePiece.drawable.isHover = isHover; // Set and reset highlight effect on gamepieces

                if (isHover && diceSave > 0) //Check only where a gamepiece can move when hoverd and dice is more than zero
                {
                    CheckAvailableMoves(diceSave, gamePiece, GameEngine._player.GamePieces); // Start check
                }
                else
                {
                    gamePiece.tempTile.drawable.isHover = false; // Reset highlight effect
                }
            }
        }
     
        public static GamePiece[] CreateGamePieces(GameRace race, CanvasBitmap sprite, float offset, GameTile gameTile, CanvasBitmap highlightSprite)
        {
            GamePiece[] gamePieces = {
            CreateGamePiece(race, sprite, new Vector2(offset,offset), gameTile, highlightSprite),
            CreateGamePiece(race, sprite, new Vector2(-offset, -offset), gameTile, highlightSprite),
            CreateGamePiece(race, sprite, new Vector2(offset, -offset), gameTile, highlightSprite),
            CreateGamePiece(race, sprite, new Vector2(-offset, offset), gameTile, highlightSprite) };
            return gamePieces;
        }

        private static GamePiece CreateGamePiece(GameRace race, CanvasBitmap sprite, Vector2 offsetPosition, GameTile gameTile, CanvasBitmap highlightSprite)
        {
            Drawable draw = new Drawable(sprite, gameTile.Position + offsetPosition, 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale), highlightSprite);
            GameEngine.drawables.Add(draw);
            return new GamePiece(race, offsetPosition, gameTile, draw);
        }

        private void ChangeTile(GamePiece gamePiece, int diceResult)
        {
            if (atHomePosition == true)//checks if it tries to leave its home
            {
                if (diceResult == 1 || diceResult == 6)// can only leave home at 1 or 6
                {
                    tile = tile.nextTile; //puts its next tile as tile    
                    System.Threading.Thread.Sleep(500);// waits 1 second 
                    gamePiece.drawable.Position = tile.Position; // changes its possition
                    atHomePosition = false;//It leaves its home
                }
            }
            else
            {
                System.Threading.Thread.Sleep(500);// waits 1 second 
                tile = tile.GetNextTile(race);//puts its next tile as tile    
                gamePiece.drawable.Position = tile.Position; // changes its possition
            }
        }
    }
}