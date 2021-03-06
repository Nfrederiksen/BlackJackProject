﻿using System;
using System.Collections.Generic;

namespace BlackJackGame
{
    
    public class Deck
    {
        private List<BlackJackCard> _CardList = new List<BlackJackCard>();
        public enum Suits
        {
            HEARTS,
            DIAMONDS,
            SPADES,
            CLUBS
        }
    
        public Deck()
        {
            // Fill the card list with blackjackcard-objects. 
            for (var i = 1; i < 14; i++)
            {
                foreach (var suit in Enum.GetNames(typeof(Suits)))
                {
                    var bjCard = new BlackJackCard(suit, i);
                    _CardList.Add(bjCard);
                }         
            }
        }
        // Getter for the card list.
        public List<BlackJackCard> GetCards()
        {
            return _CardList;
        }
    }
}