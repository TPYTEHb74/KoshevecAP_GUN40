using System;
using System.Collections.Generic;
using Casino.Core;
using Casino.Dice;

namespace Casino.Games.DiceGame
{
    public class DiceGame : CasinoGameBase
    {
        private readonly int _diceCount;
        private readonly int _min;
        private readonly int _max;

        private readonly List<Dice.Dice> _dices = new List<Dice.Dice>();

        public DiceGame(int diceCount, int min, int max)
        {
            if (diceCount <= 0)
            {
                throw new ArgumentException("Dice count must be greater than zero.");
            }

            _diceCount = diceCount;
            _min = min;
            _max = max;

            FactoryMethod();
        }

        protected override void FactoryMethod()
        {
            for (int i = 0; i < _diceCount; i++)
            {
                _dices.Add(new Dice.Dice(_min, _max));
            }
        }

        public override void PlayGame()
        {
            int playerScore = RollDices();
            int computerScore = RollDices();

            Console.WriteLine($"Player score: {playerScore}");
            Console.WriteLine($"Computer score: {computerScore}");

            if (playerScore > computerScore)
            {
                OnWinInvoke();
            }
            else if (playerScore < computerScore)
            {
                OnLooseInvoke();
            }
            else
            {
                OnDrawInvoke();
            }
        }

        private int RollDices()
        {
            int sum = 0;

            foreach (Dice.Dice dice in _dices)
            {
                sum += dice.Number;
            }

            return sum;
        }
    }
}

