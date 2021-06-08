using System;

namespace LudoGame.Classes
{
    public partial class Dice
    {
        public static int DiceSave { get; set; }

        private static Random num = new Random();

        /// <summary>
        /// Randomizes a new dice roll
        /// </summary>
        /// <returns>a number between 1-6</returns>
        public static int RollDice()
        {
            if (DiceSave == 0)
            {
                DiceSave = num.Next(1, 7);
            }
            return DiceSave;
        }

        /// <summary>
        /// Uses the dice, marks it as used and sets it up to be able to roll it again
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