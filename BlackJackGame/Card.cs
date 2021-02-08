namespace BlackJackGame
{    //The following class encapsulates a playing card
    public class Card
    {
        private string _suit;
        private int _faceValue;
        
        // Parameter-less Constructor.
        public Card()
        {
            _suit = "";
            _faceValue = -1;
        }
        // Parameterised Constructor.
        public Card(string suit, int faceValue)
        {
            _suit = suit;
            _faceValue = faceValue;
        }
        // Property for card Suit.
        public string Suit
        {
            get
            {
                return _suit;
            }
        }
        // Property for card Face Value. (This is just syntax-sugar, works just like above)
        public int FaceValue => _faceValue;
        
        // What happens if we print this obj.
        public override string ToString()
        {
            return "Card is a: " + _faceValue + " of " + _suit;
        }
    }
}