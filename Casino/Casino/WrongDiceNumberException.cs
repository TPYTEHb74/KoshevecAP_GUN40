using System;

namespace Casino.Dice
{
    public class WrongDiceNumberException : Exception
    {
        public WrongDiceNumberException(int value, int min, int max)
            : base($"User number: {value}. Allowed range: {min} - {max}.")
        {
        }
    }
}
