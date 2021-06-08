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
        public Vector2 Position { get { return drawable.Position; } set { drawable.Position = value; } }

        public GameTile tile;
        public GameTile currentTile;
        public GameTile tempTile;
        public GameTile moveToTile;
        public GameTile baseTile;

        public GameRace race;
        public bool shield;
        public bool moving;
        public bool atHomePosition = true;
        public bool isAvailableMove = false;
        public bool IsOccupied = false;
        public bool isHover = false;
        public Vector2 homePosition;
        public Drawable drawable;

        public GamePiece(GameRace gameRace, Vector2 offset, GameTile tile, Drawable drawable)
        {
            this.race = gameRace;
            this.tile = tile;
            baseTile = tile;
            this.drawable = drawable;
            this.homePosition = tile.Position + offset;
        }

        public static bool MoveToGameTile(int diceResult, GamePiece gamePiece)
        {
            if (gamePiece.isAvailableMove)
            {
                for (int i = 0; i < diceResult; i++) // Calls the change tile funk or each number on the dice
                {
                    //await Task.Delay(400);
                    ChangeTile(gamePiece, diceResult);// Changes tile the amount of times the loop goes.

                    if (diceResult - 1 == i)
                    {
                        CheckIfHitGamePiece(gamePiece);

                        gamePiece.isAvailableMove = false;
                        return true;
                    }
                }
                return true;

            }
            else
            {
                return false;
            }
        }

        public static bool TryMovingPiece(float tileSpeed)
        {
            bool aPieceIsMoving = false;
            foreach (GamePiece piece in GameEngine.gamePieces)
            {
                if (piece.tile != piece.moveToTile && piece.moveToTile != null)
                {
                    aPieceIsMoving = true;
                    GameTile nextTile = piece.tile.GetNextTile(piece.race);
                    // if the piece is closer than the threshold snapp to the tile
                    if (Vector2Math.Magnitude(nextTile.Position - piece.Position) < 5)
                    {
                        piece.Position = nextTile.Position;
                        piece.tile = nextTile;
                        
                        if (piece.tile == piece.moveToTile)
                        {
                            CheckIfHitGamePiece(piece);
                        }
                        
                    }
                    else
                    {
                        piece.Position += Vector2Math.Normalized(nextTile.Position - piece.Position) * tileSpeed * (float)GameEngine.GameCanvas.TargetElapsedTime.TotalSeconds;
                    }
                    break;
                }
            }

            return aPieceIsMoving;
        }

        /// <summary>
        /// Initial method to check if player can move or not
        /// </summary>
        public bool CheckAvailableMoves(int diceResult, Player player)
        {
            bool canMove = false;
            if (tile == baseTile)
            {
                if (diceResult == 1 || diceResult == 6)
                {
                    canMove = FindAvailableMove(diceResult, player);
                }
            }
            else
            {
                canMove = FindAvailableMove(diceResult, player);
            }

            return canMove;
        }

        /// <summary>
        /// Checks if player can move or not
        /// </summary>
        private bool FindAvailableMove(int diceResult, Player player)
        {
            tempTile = tile;
            for (int i = 0; i < diceResult; i++)
            {
                foreach (GamePiece otherPiece in player.GamePieces) // Loop trough players gamepieces
                {
                    if (otherPiece == this)
                    {
                        continue;
                    }

                    if (tempTile.GetNextTile(race) == otherPiece.tile) //Check that the next tile is not occupide by  gamepiece
                    {
                        return false;
                    }
                    else
                    {
                        isAvailableMove = true;
                    }
                }

                if (isAvailableMove)
                {
                    if (tempTile.GetNextTile(race) != null)
                    {
                        tempTile = tempTile.GetNextTile(race);
                    }
                    else
                    {
                        if (player == GameEngine.player)
                        {
                            GameEngine.drawables[1].isHover = true; // Blackhole hover if player can finish a gamepiece
                        }
                    }

                }
            }

            if (isAvailableMove && GameEngine.drawables[1].isHover == false)
            {
                if (player == GameEngine.player && Dice.DiceSave > 0)
                {
                    tempTile.drawable.isHover = true; // If the piece is able to move highlight the tile the gamepiece can move to
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if gamePiece is player piece and change isHoover in drawable class
        /// </summary>
        public void Hover(bool isHover, int diceSave)
        {
            if (race == GameEngine.player.race) //Only want hover on player pieces, AI
            {
                drawable.isHover = isHover; // Set and reset highlight effect on gamepieces

                if (isHover && diceSave > 0) //Check only where a gamepiece can move when hoverd and dice is more than zero
                {
                    CheckAvailableMoves(diceSave, GameEngine.player); // Start chec
                }
                else if (tempTile != null)
                {
                    tempTile.drawable.isHover = false; // Reset highlight effect
                    GameEngine.drawables[1].isHover = false; //Reset highlight effect for blackhole
                }
            }
        }

        public static GamePiece[] CreateGamePieces(GameRace race, CanvasBitmap sprite, float offset, GameTile gameTile)
        {
            GamePiece[] gamePieces = {
            CreateGamePiece(race, sprite, new Vector2(offset,offset), gameTile),
            CreateGamePiece(race, sprite, new Vector2(-offset, -offset), gameTile),
            CreateGamePiece(race, sprite, new Vector2(offset, -offset), gameTile),
            CreateGamePiece(race, sprite, new Vector2(-offset, offset), gameTile) };
            return gamePieces;
        }

        private static GamePiece CreateGamePiece(GameRace race, CanvasBitmap sprite, Vector2 offsetPosition, GameTile gameTile)
        {
            Drawable draw = new Drawable(sprite, gameTile.Position + offsetPosition, 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale));
            GameEngine.drawables.Add(draw);
            return new GamePiece(race, offsetPosition, gameTile, draw);
        }

        /// <summary>
        /// Checks if gamePiece lands on another gamepiece ands sends it to homebase
        /// </summary>
        public static void CheckIfHitGamePiece(GamePiece gamePiece)
        {
            foreach (GamePiece otherGamePiece in GameEngine.gamePieces)
            {
                if (otherGamePiece.tile == gamePiece.tile && otherGamePiece.race != gamePiece.race)
                {
                    otherGamePiece.tile = otherGamePiece.baseTile;
                    otherGamePiece.moveToTile = otherGamePiece.tile;
                    otherGamePiece.Position = otherGamePiece.homePosition;
                   // otherGamePiece.Position = otherGamePiece.;//Prob wont work
                    otherGamePiece.atHomePosition = true;
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