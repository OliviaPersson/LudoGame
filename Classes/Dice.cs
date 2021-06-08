using System;

namespace LudoGame.Classes
{
    public partial class Dice
    {
        public static int DiceSave { get; set; }

        private static Random num = new Random();
        public static int RollDice()
        {
            if (DiceSave == 0)
            {
                DiceSave = num.Next(1, 7);
            }

            return DiceSave;
        }
    }
}