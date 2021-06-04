using Microsoft.Graphics.Canvas;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
                   ChangeTile(gamePiece, diceResult);// Changes tile the amount of times the loop goes.
                }
                CheckIfHitGamePiece(gamePiece);
                gamePiece.isAvailableMove = false;
                return true;
            } 
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Initial method to check if player can move or not
        /// </summary>
        public static void CheckAvailableMoves(int diceResult, GamePiece gamePiece, Player player)
        {
            gamePiece.tempTile = gamePiece.tile;
            if (gamePiece.atHomePosition)
            {
                if (diceResult == 1 || diceResult == 6) 
                {
                    CheckPossibleTile(diceResult, gamePiece, player);
                }
            }
            else
            {
                CheckPossibleTile(diceResult, gamePiece, player);
            }
        }

        /// <summary>
        /// Checks if player can move or not
        /// </summary>
        private static void CheckPossibleTile(int diceResult, GamePiece gamePiece, Player player)
        {

            //Drawable blackHole = GameEngine.drawables[1];
            //blackHole.isHover = true;
            for (int i = 0; i < diceResult; i++)
            {
                for (int j = 0; j < player.GamePieces.Length; j++) // Loop trough players gamepieces
                {
                    GamePiece tempGamePiece = player.GamePieces[j]; //Store current gamepiece 
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
                        if (gamePiece.tempTile.GetNextTile(gamePiece.race) != null)
                        {
                            gamePiece.tempTile = gamePiece.tempTile.GetNextTile(gamePiece.race);
                        }
                        else
                        {
                            if (player.isHumanPlayer)
                            {
                                GameEngine.drawables[1].isHover = true; // Blackhole hover if player can finish a gamepiece
                            }
                        }
                    }
                }
            }

            if (gamePiece.isAvailableMove && GameEngine.drawables[1].isHover == false)
            {
                if (player.isHumanPlayer)
                {
                    gamePiece.tempTile.drawable.isHover = true; // If the piece is able to move highlight the tile the gamepiece can move to
                }
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
                    //Find the player that i human controlled
                    foreach(Player player in GameEngine.players)
                    {
                        if (player.isHumanPlayer)
                        {
                            CheckAvailableMoves(diceSave, gamePiece, player); // Start check
                        }
                    }
                }
                else
                {
                    gamePiece.tempTile.drawable.isHover = false; // Reset highlight effect
                    GameEngine.drawables[1].isHover = false; //Reset highlight effect for blackhole
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

        /// <summary>
        /// Checks if gamePiece lands on another gamepiece ands sends it to homebase
        /// </summary>
        public static void CheckIfHitGamePiece(GamePiece gamePiece)
        {
            foreach (Player player in GameEngine.players)
            {
                foreach (GamePiece playerGamepiece in player.GamePieces)
                {
                    if (playerGamepiece.tile == gamePiece.tile && playerGamepiece.race != gamePiece.race)
                    {
                        playerGamepiece.drawable.Position = playerGamepiece.homePosition;
                        playerGamepiece.atHomePosition = true;
                    }
                }
            }
        }
        private static void ChangeTile(GamePiece gamePiece, int diceResult)
        {
            if (gamePiece.atHomePosition == true)//checks if it tries to leave its home
            {
                if (diceResult == 1 || diceResult == 6)// can only leave home at 1 or 6
                {
                    gamePiece.tile = gamePiece.tile.GetNextTile(gamePiece.race); //puts its next tile as tile    
                    //System.Threading.Thread.Sleep(400);// waits 1 second 
                    gamePiece.drawable.Position = gamePiece.tile.Position; // changes its possition
                    gamePiece.atHomePosition = false;//It leaves its home
                }
            }
            else
            {
                if (gamePiece.tile.GetNextTile(gamePiece.race) == null) // If the next gametile is null it is the black hole
                {
                    foreach (Player player in GameEngine.players) // Find the player that controlls the gamepiece
                    {
                        if (player.race == gamePiece.race)
                        {
                            player.finishedPieces++; // Count gamepieces that is finished
                            for (int i = 0; i < player.GamePieces.Length; i++) //Find the gamepiece that is finished
                            {
                                if (player.GamePieces[i] == gamePiece)
                                {
                                    gamePiece.drawable.isHidden = true; // To hide gamepiece in drawable when finished
                                    player.GamePieces = player.GamePieces.Where(w => w != player.GamePieces[i]).ToArray(); // Delete gamepiece when finished
                                }
                            }
                        }
                    }
                }
                else
                {
                    //System.Threading.Thread.Sleep(400);// waits 1 second 
                    gamePiece.tile = gamePiece.tile.GetNextTile(gamePiece.race);//puts its next tile as tile
                    gamePiece.drawable.Position = gamePiece.tile.Position; // changes its possition
                   
                }
            }
        }
    }
}