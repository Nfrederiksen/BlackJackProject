namespace BlackJackGame
{
    // Extends Card class, to follow blackjack value rules.
    public class BlackJackCard : Card
    {
        private int _gameValue;
        
        // Parameter-less Constructor
        public BlackJackCard()
            : base("", -1)
        {
            _gameValue = -1;
        }
        // Parameterised Constructor
        public BlackJackCard(string suit, int faceValue)
        : base(suit, faceValue)
        {
            if (faceValue > 10)
                _gameValue = 10;
            else
            {
                _gameValue = faceValue;
            }
        }
        // Property for card game value.
        public int GameValue
        {
            get { return _gameValue; }
        }
    }
}