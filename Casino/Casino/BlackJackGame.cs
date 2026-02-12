using System;
using System.Collections.Generic;
using System.Linq;
using Casino.Cards;
using Casino.Core;

namespace Casino.Games.BlackJack
{
    public class BlackJackGame : CasinoGameBase
    {
        private readonly int _cardCount;
        private readonly List<Card> _cards = new List<Card>();
        private Queue<Card> Deck;

        public BlackJackGame(int cardCount)
        {
            if (cardCount <= 0)
            {
                throw new ArgumentException("Card count must be greater than zero.");
            }

            _cardCount = cardCount;

            FactoryMethod();
        }

        protected override void FactoryMethod()
        {
            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            {
                foreach (CardRank rank in Enum.GetValues(typeof(CardRank)))
                {
                    _cards.Add(new Card(suit, rank));
                }
            }

            Shuffle();
        }

        public override void PlayGame()
        {
            List<Card> player = new List<Card>();
            List<Card> computer = new List<Card>();

            DealInitialCards(player, computer);

            while (true)
            {
                int playerScore = CalculateScore(player);
                int computerScore = CalculateScore(computer);

                PrintHands(player, computer, playerScore, computerScore);

                if (playerScore > 21 && computerScore > 21)
                {
                    OnDrawInvoke();
                    return;
                }

                if (playerScore <= 21 &&
                    (computerScore > 21 || playerScore > computerScore))
                {
                    OnWinInvoke();
                    return;
                }

                if (computerScore <= 21 &&
                    (playerScore > 21 || computerScore > playerScore))
                {
                    OnLooseInvoke();
                    return;
                }

                player.Add(Deck.Dequeue());
                computer.Add(Deck.Dequeue());
            }
        }

        private void DealInitialCards(List<Card> player, List<Card> computer)
        {
            player.Add(Deck.Dequeue());
            player.Add(Deck.Dequeue());

            computer.Add(Deck.Dequeue());
            computer.Add(Deck.Dequeue());
        }

        private int CalculateScore(List<Card> hand)
        {
            int score = 0;

            foreach (Card card in hand)
            {
                if ((int)card.Rank <= 10)
                {
                    score += (int)card.Rank;
                }
                else if (card.Rank == CardRank.Ace)
                {
                    score += 11;
                }
                else
                {
                    score += 10;
                }
            }

            return score;
        }

        private void PrintHands(List<Card> player, List<Card> computer, int playerScore, int computerScore)
        {
            Console.WriteLine("Player cards:");
            player.ForEach(c => Console.WriteLine(c));

            Console.WriteLine("Computer cards:");
            computer.ForEach(c => Console.WriteLine(c));

            Console.WriteLine($"Player score: {playerScore}");
            Console.WriteLine($"Computer score: {computerScore}");
        }

        private void Shuffle()
        {
            Random random = new Random();
            Deck = new Queue<Card>(_cards.OrderBy(x => random.Next()));
        }
    }
}
