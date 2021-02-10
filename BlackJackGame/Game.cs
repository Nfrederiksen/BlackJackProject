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
                        Console.WriteLine("-> [] You were dealt a: {0} of {1}",
                            hand.GetLastCard().FaceValue, hand.GetLastCard().Suit);
                        BustCheck(hand);
                        break;

                    // Stand: 
                    case 2:
                        hand.StandState = true;
                        Stand(hand);
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
                        Console.WriteLine("Thank You for Playing My Blackjack Game!\n");

                        Thread.Sleep(3000);
                        Console.Clear(); 
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("\nPlease Make a Valid Selection!\n");
                        Thread.Sleep(1000);
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
                Thread.Sleep(1000);
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
                Console.WriteLine("\n_____________________________\n" + 
                                  "      S E T T L E M E N T!    ");
                Console.WriteLine("Number of Hands:{0}", hands.Count);
                Console.WriteLine("Total bet is at:{0}", _player.Bet);
                Thread.Sleep(1000);
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
            Console.WriteLine("-> [] You were dealt a: {0} of {1}\n",
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
                    Console.Write("-> [] Dealer was dealt a: {0} of {1}\n", 
                        dealersHand.GetLastCard().FaceValue,
                        dealersHand.GetLastCard().Suit);
                    Thread.Sleep(1000);
                }
                else
                {
                    break;
                }
            }
            
            return bestScore;
        }

        public void Start()
        {
            while (true)
            {    // Enter the player's starting balance.
                _player.Balance = 1000;
                int roundNum = 1;
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
                            //  #Resolve to Side bet/Insurance bet.
                            if (_dealer.HasBlackJack(dealerHand))
                            {
                                playerHand.StandState = true;
                                Stand(playerHand);
                                _gameUI.InsuranceWinMsg();
                                _player.AddToBalance(2 * _player.SideBet);
                                // #Clear hands and move on to next round.
                                _player.ClearHands();
                                _dealer.ClearHands();
                            }
                            else
                            {
                                _gameUI.InsuranceLoseMsg();
                                _player.AddToBalance(-1 * _player.SideBet);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No...? Well fine then, it's your credits after all!");
                        }
                    }

                    // ---- GAME: Main Event ----
                    // #Break out of this loop for a new round.
                    while (true)
                    {
                        var handIndex = 0;
                        var hands = _player.GetHands();
                        foreach (var hand in hands)
                        {
                            if (!hand.StandState)
                            {
                                if (hand.ResolveScore() == 21)
                                {
                                    // Hand is 21, then auto-stand.
                                    Console.WriteLine("Good hand! You have 21.");
                                    hand.StandState = true;
                                    Stand(hand);
                                }
                                else
                                {
                                    // User gets to choose an action.
                                    _gameUI.ShowScores(handIndex);
                                    handIndex++;
                                    int inputAct = _gameUI.GetUserAction(hand);
                                    PlayAction(inputAct, hand);
                                    // If the user wants to split (4), we basically soft-restart.
                                    if (inputAct == 4)
                                    {
                                        break;
                                    }
                                    // If the user wants to quit mid-game.
                                    if (inputAct == 0)
                                    {
                                        System.Environment.Exit(1);
                                    }
                                }
                            }
                        }
                        // #Display active cards on the table, unless all hands are standing, then we move on.
                        if (_player.AllHandsStand())
                        {
                            Console.WriteLine("Moving on to the next round...");
                            break;
                        }
                        _gameUI.ShowStartingHands();
                    }
                    // #Ending the round.
                    _player.ClearHands();
                    _dealer.ClearHands();
                    // #'Next Round' or 'Game Over' or 'EPIC Win' Screen?
                    if (_player.Balance <= 0)
                    {
                        _gameUI.GameOverScreen();
                        break;
                    }
                    if (_player.Balance >= 20000)
                    {
                        _gameUI.EPIC_WIN_SCREEN();
                        break;
                    }
                    roundNum++;
                }
                // #Offer the User to restart the Blackjack game.
                if (!_gameUI.UserRestartsGame())
                {
                    break;
                }
                else
                {
                    Console.Clear();
                }
            }
            Console.WriteLine("Thank you for playing... Good bye ;)");
        }
    }
}