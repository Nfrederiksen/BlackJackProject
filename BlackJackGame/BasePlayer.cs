using System.Collections.Generic;

namespace BlackJackGame
{
    public class BasePlayer
    {
        private int _balance;
        protected List<Hand> _Hands;
        
        // Parameter-less Constructor
        public BasePlayer()
        {
            _balance = 0;
            _Hands = new List<Hand>();
        }

        public List<Hand> GetHands()
        {
            return _Hands;
        }

        public void AddHand(Hand h)
        {
            _Hands.Add(h);
        }

        public void RemoveHand(Hand h)
        {
            _Hands.Remove(h);
        }

        public void ClearHands()
        {
            _Hands.Clear();
        }

        public bool AllHandsStand()
        {// If a hand is not in stand-state, then false.
            foreach (var hand in _Hands)
            {
                if (!hand.StandState)
                {
                    return false;
                }
            }

            return true;
        }

        public bool HasBlackJack(Hand h)
        {//  Condition for blackjack is, that player has 1 hand and only 2 cards that make 21.
            // Returns True or False.
            return (h.ResolveScore() == 21 && GetHands().Count == 1 && GetHands()[0].GetCards().Count == 2);
        }
        
        public virtual void PrintHands(){ }
    }
}