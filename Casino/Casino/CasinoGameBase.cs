using Casino.Games.BlackJack;
using Casino.Games.DiceGame;
using System;

namespace Casino.Core
{
    public abstract class CasinoGameBase
    {
        public event Action OnWin;
        public event Action OnLoose;
        public event Action OnDraw;

        protected CasinoGameBase()
        {
            
        }
        


        public abstract void PlayGame();

        protected abstract void FactoryMethod();

        protected void OnWinInvoke()
        {
            Console.WriteLine("You win!");
            OnWin?.Invoke();
        }

        protected void OnLooseInvoke()
        {
            Console.WriteLine("You loose!");
            OnLoose?.Invoke();
        }

        protected void OnDrawInvoke()
        {
            Console.WriteLine("Draw!");
            OnDraw?.Invoke();
        }
    }
}
