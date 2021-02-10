using System;
using System.Collections.Generic;
using System.Drawing;

namespace BlackJackGame
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var player = new Player();
            var dealer = new Dealer();
            var game = new Game(player, dealer);
            game.Start();
            
        }
    }
}