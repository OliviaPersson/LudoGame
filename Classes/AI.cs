using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoGame.Classes
{
    class AI
    {
        Color[] colors = new Color[] { Color.Blue, Color.Green, Color.Red, Color.Yellow };
        enum Color
        {
            Yellow,
            Red,
            Green,
            Blue
        }

        public void InitAI()
        {
            WhoToMove();
            ThrowDice();
            DecisionMaking();
        }
        private void ThrowDice()
        {
            int steps = Dice.randomNum();
            MoveAI(Color.Blue);
        }
        private void MoveAI(Color color) 
        {

        }
        private Color WhoToMove()
        {


            return Color.Blue;
        }
        private void DecisionMaking()
        {

        }
    }
}
