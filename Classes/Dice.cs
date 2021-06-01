using System;

namespace LudoGame.Classes
{
    public static class Dice
    {
        public static int MyThrow
        {
            get
            {
                if (_isMyThrow)
                {
                    return _lastThrow;
                }
                return 0;
            }
        }

        private static bool _isMyThrow;
        private static int _lastThrow;
        private static Random _num = new Random();

        public static int ThrowDice()
        {
            _lastThrow = _num.Next(1, 7);
            _isMyThrow = true;
            return _lastThrow;
        }

        public static int UseDice()
        {

            if (_isMyThrow)
            {
                _isMyThrow = false;
                return _lastThrow;
            }
            return 0;
        }
    }
}