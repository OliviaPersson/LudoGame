using System;

namespace LudoGame.Classes
{
    public partial class Dice
    {
        public static int DiceSave { get; set; }

        private static Random num = new Random();

        /// <summary>
        /// Randomizes a new dice roll,
        /// this is saved in DiceSave
        /// </summary>
        /// <returns>A number between 1-6</returns>
        public static int RollDice()
        {
            if (DiceSave == 0)
            {
                DiceSave = num.Next(1, 7);
            }
            return DiceSave;
        }

        /// <summary>
        /// Call this when the dice is supposed to be used for a move.
        /// Only call when a valid move is performed,
        /// or if no valid moves exist since it disables further readings of the roll
        /// </summary>
        /// <returns>the last dice roll</returns>
        public static int UseDice()
        {
            int dice = DiceSave;
            DiceSave = 0;
            return dice;
        }
    }
}