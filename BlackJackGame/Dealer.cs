using System;
using System.Collections.Generic;

namespace BlackJackGame
{//# Dealer: Dealer class extends from BasePlayer:
    public class Dealer : BasePlayer
    {
        private string _dealersName;
        // P-Less Constructor
        public Dealer()
        {
            _dealersName = "Chuck Jones";
        }

        public void PrintFirstCard()
        {
            var dealersHands = GetHands();
            var firstCard = dealersHands[0].GetCards()[0];
            Console.WriteLine("A Card Shows: {0} of {1}", firstCard.FaceValue, firstCard.Suit);
        }
        
        public override void PrintHands()
        {
            foreach (var hand in GetHands())
            {
                foreach (var card in hand.GetCards())
                {
                    Console.WriteLine("A Card Shows: {0} of {1}", card.FaceValue, card.Suit);
                }
            }
        }
    }
}