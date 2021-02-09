using System;
using System.Collections.Generic;

namespace BlackJackGame
{
    public class Shoe
    {
        private List<BlackJackCard> _CardList = new List<BlackJackCard>();
        private int _numOfDecks;
        
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

        private void Shuffle()
        {
            // Here we swap cards in the deck randomly.
            int totalNumCards = _CardList.Count;
            for (int i = 0; i < totalNumCards; i++)
            {
                int j = new Random().Next(i - 1, totalNumCards);
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
    }
}
