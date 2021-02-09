using System.Collections.Generic;

namespace BlackJackGame
{
    public class Hand
    {
        private List<BlackJackCard> _CardList;
        private bool _standState;

        // Parameterised Constructor
        public Hand(BlackJackCard card1, BlackJackCard card2)
        {
            // A hand always starts off w/ 2 cards.
            _standState = false;
            _CardList = new List<BlackJackCard> {card1, card2};
        }

        public void AddCard(BlackJackCard card)
        {
            _CardList.Add(card);
        }

        // Getter for the card list.
        public List<BlackJackCard> GetCards()
        {
            return _CardList;
        }

        public BlackJackCard GetLastCard()
        {
            return _CardList[_CardList.Count - 1];
        }
        public int ReadFirstCardValue()
        {
            return _CardList[0].GameValue;
        }

        public bool StandState
        {
            get { return _standState; }
            set { _standState = value; }
        }

        public List<int> GetScores()
        { /* Summarize the face value of each card, but
            if an Ace is involved then there are 2 versions of the score */
            var totals = new List<int>{0};
            foreach (var card in _CardList)
            {
                var newTotal = new List<int>();
                foreach (var score in totals)
                {
                    if (card.GameValue == 1)
                    {
                        newTotal.Add(card.GameValue + score);
                        newTotal.Add(11 + score);
                    }
                    else
                    {
                        newTotal.Add(card.GameValue + score);
                    }
                }

                totals = newTotal;
            }
            
            return totals;
        }

        public int ResolveScore()
        {//  Get highest score which is less than or equal to 21
            int bestScore = 0;
            var scores = GetScores();
            foreach (var score in scores)
            {
                if (score <= 21 && score > bestScore)
                {
                    bestScore = score;
                } 
            }
            return bestScore;
        }

        public int GetFinalScore()
        {// In case hand gets bust, let's find the latest score.
            int finalScore = ResolveScore();
            return (finalScore == 0) ? GetScores()[0] : finalScore;
        }
    }
}