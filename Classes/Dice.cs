using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LudoGame.Classes;
using Windows.UI.Xaml;

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
