using System;
namespace BlackJackGame
{
    public class Player : BasePlayer
    {//# Player: Player class extends from BasePlayer:
        private int _balance;
        private int _bet;
        private int _sideBet;
        
        // P-Less Constructor
        public Player()
        {
            _balance = 0;
            _bet = 0;
            _sideBet = 0;
        }

        public int Bet
        {
            get => _bet;
            set => _bet = value;
        }

        public int SideBet
        {
            get => _sideBet;
            set => _sideBet = value;
        }

        public int Balance
        {
            get => _balance;
            set => _balance = value;
        }

        public void AddToBalance(int payment)
        {
            _balance += payment;
        }

        public override void PrintHands()
        {
            int handNum = 0;
            foreach (var hand in GetHands())
            {
                ++handNum;
                Console.WriteLine("Player Hand #{0}:", handNum);
                /*foreach (var card in hand.GetCards())
                {
                    //Console.WriteLine("A Card Shows: {0} of {1}", card.FaceValue, card.Suit);
                }*/
                Display.PrettyPrintHand(hand);
            }
        }
    }
}