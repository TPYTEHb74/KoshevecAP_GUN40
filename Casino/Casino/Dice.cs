using System;

namespace Casino.Dice
{
    public struct Dice
    {
        private static readonly Random Random = new Random();

        private readonly int _min;
        private readonly int _max;

        public int Number => Random.Next(_min, _max + 1);

        public Dice(int min, int max)
        {
            if (min < 1 || max > int.MaxValue || min > max)
            {
                throw new WrongDiceNumberException(min, 1, int.MaxValue);
            }

            _min = min;
            _max = max;
        }
    }
}
