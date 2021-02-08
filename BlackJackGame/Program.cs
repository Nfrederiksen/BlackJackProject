using System;

namespace BlackJackGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Deck myDeck = new Deck();
            Console.WriteLine("I made a deck! \n First card is {0}", myDeck.GetCards()[0]);

            var myShoe = new Shoe(6);
            Console.WriteLine("I made a shoe of decks! \nFirst card is {0}", myShoe.DealCard());
        }
    }
}