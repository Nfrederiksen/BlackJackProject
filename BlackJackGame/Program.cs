using System;

namespace BlackJackGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var player = new Player();
            var dealer = new Dealer();
            var shoe = new Shoe(3);
            player.Balance = 1000;
            
            Hand hand1 = new Hand(shoe.DealCard(),shoe.DealCard());
            player.AddHand(hand1);
            var gameUI = new GameUI(player, dealer);

            gameUI.GetUserAction(hand1);

        }
    }
}