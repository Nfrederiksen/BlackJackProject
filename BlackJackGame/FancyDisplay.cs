using System;
using System.Collections.Generic;

namespace BlackJackGame
{
    /*
     * I want to print out the cards in this pretty format
     *
     *  ╔══════╗╔══════╗╔══════╗╔══════╗
        ║5.--. ║║7.--. ║║4.--. ║║J.--. ║
        ║ :(): ║║ (\/) ║║ :/\: ║║ :/\: ║
        ║ ()() ║║ :\/: ║║ :\/: ║║ (__) ║
        ║ '--'5║║ '--'7║║ '--'4║║ '--'J║
        ╚══════╝╚══════╝╚══════╝╚══════╝
     * 
     */
    public class FancyDisplay
    {
        private IDictionary<int, string> _numberNames;
        private IDictionary<string, string> _topSuits;
        private IDictionary<string, string> _bottomSuits;
        
        // Parameter-less Constructor
        public FancyDisplay()
        {
            _numberNames = new Dictionary<int, string>();
            _numberNames.Add(11,"J"); //adding a key/value using the Add() method
            _numberNames.Add(12,"Q");
            _numberNames.Add(13,"K");
            _numberNames.Add(1, "A");
            
            _topSuits = new Dictionary<string, string>();
            _topSuits.Add("HEARTS","║ (\\/) ║");
            _topSuits.Add("DIAMONDS","║ :/\\: ║");
            _topSuits.Add("SPADES","║ :/\\: ║");
            _topSuits.Add("CLUBS","║ :(): ║");
            
            _bottomSuits = new Dictionary<string, string>();
            _bottomSuits.Add("HEARTS","║ :\\/: ║");
            _bottomSuits.Add("DIAMONDS","║ :\\/: ║");
            _bottomSuits.Add("SPADES","║ (__) ║");
            _bottomSuits.Add("CLUBS","║ ()() ║");
        }
        
        public void PrettyPrintHand(Hand hand)
        {
            string row1 = "";
            string row2 = "";
            string row3 = "";
            string row4 = "";
            string row5 = "";
            string row6 = "";

            foreach (var card in hand.GetCards())
            {
                string m_val;
                var m_suit = _topSuits[card.Suit];
                var m_suit2 = _bottomSuits[card.Suit];
                if (_numberNames.ContainsKey(card.FaceValue))
                {
                    m_val = _numberNames[card.FaceValue];
                }
                else
                {
                    m_val = card.FaceValue.ToString();
                }
                row1 += "╒══════╕";
                if (card.FaceValue == 10)
                {
                    row2 += "║"+m_val+"--. ║";
                    row5 += "║ '--"+m_val+"║";
                }
                else
                {
                    row2 += "║"+m_val+".--. ║";
                    row5 += "║ '--'"+m_val+"║";
                }

                row3 += m_suit;
                row4 += m_suit2;
                row6 += "╘══════╛";
            }
            Console.WriteLine(row1);
            Console.WriteLine(row2);
            Console.WriteLine(row3);
            Console.WriteLine(row4);
            Console.WriteLine(row5);
            Console.WriteLine(row6);
        } 
 
    }
}