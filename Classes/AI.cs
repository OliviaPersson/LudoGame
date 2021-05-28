namespace LudoGame.Classes
{
    internal class AI
    {
        private Color[] colors = new Color[] { Color.Blue, Color.Green, Color.Red, Color.Yellow };

        private enum Color
        {
            Yellow,
            Red,
            Green,
            Blue
        }

        public void InitAI()
        {
            bool GetSix = false;
            do
            {
                WhoToMove();
                GetSix = ThrowDice();
                DecisionMaking();
            } while (GetSix == true);
        }

        private bool ThrowDice()
        {
            int steps = Dice.randomNum();
            MoveAI(Color.Blue);
            if (steps == 6)
            {
                return true;
            }
            else
            {
                return false;
            }
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