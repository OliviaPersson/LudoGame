using System;

namespace LudoGame.Classes
{
    public partial class Dice
    {
        public static int randomNum()
        {
            Random num = new Random();
            int number = num.Next(1, 7);
            return number;
        }
    }
}