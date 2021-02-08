using System;
using System.Collections.Generic;

namespace BlackJackGame
{
    public class Shoe
    {
        private List<Card> CardList = new List<Card>();
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
                CardList.AddRange(myDeck.GetCards());
            }
        }

        private void Shuffle()
        {
            // Here we swap cards in the deck randomly.
            int totalNumCards = CardList.Count;
            for (int i = 0; i < totalNumCards; i++)
            {
                int j = new Random().Next(1, totalNumCards);
                var tempCard = CardList[i];
                CardList[i] = CardList[j];
                CardList[j] = tempCard;
            }
        }

        public Card DealCard()
        {    // In case we ran out of cards, auto-refill the shoe.
            if (CardList.Count == 0)
            {
                CreateShoe();
            }
            
            Card dealtCard = CardList[0];
            CardList.RemoveAt(0);
            return dealtCard;
        }
    }
}
