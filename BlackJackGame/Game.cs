using System;
using System.Threading;
using System.Xml.Schema;

namespace BlackJackGame
{   //# Game: This class encapsulates a blackjack game:
    public class Game
    {
        private Player _player;
        private Dealer _dealer;
        private const int MaxNumOfDecks = 3;
        private Shoe _shoe;
        private GameUI _gameUI;
        
        // Parameterised Constructor
        public Game(Player player, Dealer dealer)
        {
            _player = player;
            _dealer = dealer;
            _shoe = new Shoe(MaxNumOfDecks);
            _gameUI = new GameUI(player,dealer); 
        }

        public void PlayAction(int action, Hand hand)
        {
            switch (action)
                {
                    // Hit: A card is dealt and added to hand.
                    case 1:
                        Hit(hand);
                        Console.WriteLine("You were dealt a: {0} of {1}",
                            hand.GetLastCard().FaceValue, hand.GetLastCard().Suit);
                        BustCheck(hand);
                        break;

                    // Stand: 
                    case 2:
                        hand.StandState = true;
                        break;
                    // Double Down: Doubles the original bet and proceeds to Hit then Stand.
                    case 3:
                        DoubleDown(hand);
                        break;
                    // Split: Another hand is created and one card is dealt to each hand.
                    case 4:
                        Split(hand);
                        break;
                    //  Quit
                    case 0:
                        Console.WriteLine("Thank You for Using the Vendy!\n");

                        Thread.Sleep(4000);
                        Console.Clear(); 
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Please Make a Valid Selection\n");
                        Thread.Sleep(100);
                        break;
                }
        }

        private void Hit(Hand hand)
        {
            BlackJackCard newCard = _shoe.DealCard();
            hand.AddCard(newCard);
        }

        private void BustCheck(Hand hand)
        {
            if (hand.ResolveScore() == 0)
            {
                // Hand is bust, then auto-stand.
                _gameUI.BustMsg();
                hand.StandState = true;
                Stand(hand);
            }
            else
            {
                hand.StandState = false;
            }
        }

        private void Stand(Hand h)
        {
            // Only Do if all hand are in standing state.
            if (_player.AllHandsStand())
            {
                var dealersHand = _dealer.GetHands()[0];
                var hands = _player.GetHands();
                Console.WriteLine("\n\n_____________________________" + 
                                  "      S E T T L E M E N T!    ");
                Console.WriteLine("Number of Hands:{0}", hands.Count);
                Console.WriteLine("Total bet is at:{0}", _player.Bet);
                _gameUI.ShowAllHands();
                
                foreach (var hand in hands)
                {// If player's hand is busted or dealer has under 16, no need for the dealer to play any further.
                    int dealersScore;
                    if (hand.ResolveScore() != 0 && dealersHand.ResolveScore() < 17)
                    {
                        dealersScore = DealerPlays();
                        _gameUI.ShowAllHands();
                    }
                    else
                    {
                        dealersScore = dealersHand.ResolveScore();
                    }
                    
                    Console.WriteLine("Dealer has: {0}", dealersHand.GetFinalScore());
                    Console.WriteLine(" and Player has: {0}", hand.GetFinalScore());
                    
                    var playersScore = hand.ResolveScore();
                    // If player has blackjack and dealer doesn't.
                    if (_player.HasBlackJack(hand) && !_dealer.HasBlackJack(dealersHand))
                    {
                        _gameUI.WinByBlackjackMsg();
                        _player.AddToBalance((int)Math.Round(_player.Bet * 1.5));
                    }
                    // If dealer instead has blackjack and player doesn't.
                    else if (_dealer.HasBlackJack(dealersHand) && !_player.HasBlackJack(hand))
                    {
                        _gameUI.LoseByBlackjackMsg();
                        _player.AddToBalance(-1 * _player.Bet);
                    }
                    // If player scores better than dealer.
                    else if (playersScore > dealersScore)
                    {
                        _gameUI.WinMsg();
                        _player.AddToBalance(_player.Bet);
                    }
                    // If dealer scores better than player.
                    else if (playersScore < dealersScore)
                    {
                        _gameUI.LoseMsg();
                        _player.AddToBalance(-1 * _player.Bet);
                    }
                    // If both have equal score.
                    else
                    {
                        _gameUI.TieMsg();
                    }
                }
            }
        }

        private void DoubleDown(Hand hand)
        {
            _player.Bet *= 2;
            Hit(hand);
            Console.WriteLine("-> You were dealt a: {0} of {1}",
                hand.GetLastCard().FaceValue, hand.GetLastCard().Suit);
            BustCheck(hand);
            if (!hand.StandState)
            {
                hand.StandState = true;
                Stand(hand);
            }
        }

        private void Split(Hand hand)
        {
            // PLayer gets 2 hands. How fun :)
            var cards = hand.GetCards();
            _player.AddHand(new Hand(cards[0], _shoe.DealCard()));
            _player.AddHand(new Hand(cards[1], _shoe.DealCard()));
            _player.RemoveHand(hand);
        }
        
        private int DealerPlays()
        {
            int bestScore;
            var dealersHand = _dealer.GetHands()[0];
            while (true)
            {
                bestScore = dealersHand.ResolveScore();
                if (bestScore == 0)
                {
                    Console.WriteLine("Yep, dealer went BUST!\n");
                    break;
                }
                if (bestScore < 17)
                {
                    Hit(dealersHand);
                    // Display latest added card to hand.
                    Console.Write("-> [] Dealer was dealt a: {0} of {1}", 
                        dealersHand.GetLastCard().FaceValue,
                        dealersHand.GetLastCard().Suit);
                }
                else
                {
                    break;
                }
            }
            
            return bestScore;
        }

        private void Start()
        {
            while (true)
            {    // Enter the player's starting balance.
                _player.Balance = 1000;
                int roundNum = 0;
                while (true)
                {// ---- GAME: Start Screen ----
                    _gameUI.StartScreen(roundNum);
                    int inputBet = _gameUI.GetBetFromUser();
                    _player.Bet = inputBet;
                    // #Deal cards to player.
                    var playerHand = new Hand(_shoe.DealCard(), _shoe.DealCard());
                    _player.AddHand(playerHand);
                    // #Deal cards to dealer.
                    var dealerHand = new Hand(_shoe.DealCard(), _shoe.DealCard());
                    _dealer.AddHand(dealerHand);
                    // #Display cards to the user.
                    _gameUI.ShowStartingHands();
                
                // ---- GAME: Insurance Scenario ----
                if (dealerHand.ReadFirstCardValue() == 10 || dealerHand.ReadFirstCardValue() == 1)
                {
                    _gameUI.OfferInsuranceMsg();
                    if (_gameUI.UserAcceptsInsuranceBet())
                    {
                        _player.SideBet = (int) Math.Round(0.5 * inputBet);
                        // TODO
                    }
                }
                }
            }
        }
    }
}