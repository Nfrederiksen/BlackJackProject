using System.Collections.Generic;

namespace BlackJackGame
{
    public abstract class BasePlayer
    {
        private List<Hand> _Hands;
        protected FancyDisplay Display;
        
        // Parameter-less Constructor
        public BasePlayer()
        {
            _Hands = new List<Hand>();
            Display = new FancyDisplay();
        }

        public List<Hand> GetHands()
        {
            return _Hands;
        }

        public void AddHand(Hand hand)
        {
            _Hands.Add(hand);
        }

        public void RemoveHand(Hand hand)
        {
            _Hands.Remove(hand);
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

        public bool HasBlackJack(Hand hand)
        {//  Condition for blackjack is, that player has 1 hand and only 2 cards that make 21.
            // Returns True or False.
            return (hand.ResolveScore() == 21 && GetHands().Count == 1 && GetHands()[0].GetCards().Count == 2);
        }

        public abstract void PrintHands();
    }
}