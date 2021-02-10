using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace BlackJackGame
{
    public class Shoe
    {
        private List<BlackJackCard> _CardList = new List<BlackJackCard>();
        private int _numOfDecks;
        // For Advanced Random-function.
        private static readonly RNGCryptoServiceProvider Generator = new RNGCryptoServiceProvider();
        
        // Parameterised Constructor
        public Shoe(int numOfDecks)
        {
            _numOfDecks = numOfDecks;
            // Run Create Shoe.
            CreateShoe();
            // Run Shuffle all cards.
            Shuffle();
        }

        private void CreateShoe()
        {
            for (var deck = 0; deck < _numOfDecks; deck++)
            {
                Deck myDeck = new Deck();
                _CardList.AddRange(myDeck.GetCards());
            }
        }

        public void Shuffle()
        {
            // Here we swap cards in the deck randomly.
            int totalNumCards = _CardList.Count - 1;

            for (int i = 0; i < totalNumCards; i++)
            {
                int j = GoodRandomNumber(i, totalNumCards - 1);
                //Console.WriteLine("j:" + j);
                var tempCard = _CardList[i];
                _CardList[i] = _CardList[j];
                _CardList[j] = tempCard;
            }
        }

        public BlackJackCard DealCard()
        {    // In case we ran out of cards, auto-refill the shoe.
            if (_CardList.Count == 0)
            {
                CreateShoe();
            }
            BlackJackCard dealtCard = _CardList[0];
            _CardList.RemoveAt(0);
            return dealtCard;
        }
        
        // [*] Utility Function, source->https://scottlilly.com/create-better-random-numbers-in-c/
        private int GoodRandomNumber(int min, int max)
        {
            byte[] randomNumber = new byte[1];
            Generator.GetBytes(randomNumber);
            double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);

            /* We are using Math.Max, and substracting 0.00000000001, 
               to ensure "multiplier" will always be between 0.0 and .99999999999
               Otherwise, it's possible for it to be "1", which causes problems in our rounding.*/
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

            // We need to add one to the range, to allow for the rounding done with Math.Floor
            int range = max - min + 1;

            double randomValueInRange = Math.Floor(multiplier * range);

            return (int)(min + randomValueInRange);
        }
    }
}
