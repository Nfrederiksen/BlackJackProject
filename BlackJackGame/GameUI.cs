using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BlackJackGame
{
    public class GameUI
    {//# GameUI: This class contains functions for user-prompts and messages:
        private Player _player;
        private Dealer _dealer;

        // Parameterised Constructor
        public GameUI(Player player, Dealer dealer)
        {
            _player = player;
            _dealer = dealer;
        }

        public int GetUserAction(Hand hand)
        {
            Console.Write(@"__________________________
 T I M E   T O   P L A Y! ");
            // Displayed options depend on the hand.
            List<int> validActions;
            var cards = hand.GetCards();
            if (cards.Count == 2 && cards[0].FaceValue == cards[1].FaceValue)
            {
                Console.Write(@"
.-----.---------------------.
|Press|       Action        |
|-----|---------------------|
|  1  |    Hit              |
|  2  |    Stand            |
|  3  |    Double Down      |
|  4  |    Split            |
|  0  |    Quit             |
'-----'---------------------'
"
                );
                validActions = new List<int>{0, 1, 2, 3, 4};
            }
            else
            {
                Console.Write(@"
.-----.---------------------.
|Press|       Action        |
|-----|---------------------|
|  1  |    Hit              |
|  2  |    Stand            |
|  3  |    Double Down      |
|  0  |    Quit             |
'-----'---------------------'
"
                );
                validActions = new List<int>{0, 1, 2, 3};
            }
            // User makes an input
            string userInput = Console.ReadLine();
            /* Converts to integer type */
            int value;
            if (int.TryParse(userInput, out value) && value > -1 && validActions.Contains(value))
                return value;
            else
                return -1;
        }

        public int GetBetFromUser()
        {
            int bet = 0;
            while (true)
            {
                Console.WriteLine("Place your bet!:");
                // User makes an input.
                string userInput = Console.ReadLine();
                // Converts to integer type.
                int value;
                //--1. Check for valid value/data type.
                if (int.TryParse(userInput, out value) && value > 0)
                    bet = value;
                else
                {
                    Console.WriteLine("Sorry, I didn't understand that. Try Again.");
                    continue;
                }
                //--2. Check for valid bet.
                if (bet <= _player.Balance)
                {
                    Console.WriteLine("\nOk, You are betting {0} CREDITS", bet);
                    break;
                }
                else
                {
                    Console.WriteLine("Sorry, but that is more than your current balance of: {0}", _player.Balance);
                    Console.WriteLine("Try entering a lower amount!");
                }
            }

            return bet;

        }

        public bool UserRestartsGame()
        {
            string reply;
            while (true)
            {
                Console.WriteLine("Do you want to restart? [Y/N]");
                
                try
                {
                    reply = Console.ReadLine().ToLower();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                if (reply == "y" || reply == "n")
                    break;
                
                Console.Write(@"
x x x x x x x x x x x x x x
x Error! Invalid Action!  x
x x x x x x x x x x x x x x

");
            }
            // Return True if user entered Y, false otherwise.
            return reply == "y";
        }
        
        public bool UserAcceptsInsuranceBet()
        {
            string reply;
            while (true)
            {
                Console.WriteLine("Accept Insurance Bet? [Y/N]");
                
                try
                {
                    reply = Console.ReadLine().ToLower();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                if (reply == "y" || reply == "n")
                    break;
                
                Console.Write(@"
x x x x x x x x x x x x x x
x Error! Invalid Action!  x
x x x x x x x x x x x x x x

");
            }
            // Return True if user entered Y, false otherwise.
            return reply == "y";
        }

        public void ShowStartingHands()
        {
            Console.WriteLine("=============== [?][ ] ================\n" +
                              "Dealer's Hand:");
            _dealer.PrintFirstCard();
            _player.PrintHands();
            Console.WriteLine("=============== [ ][ ] ================\n");
        }

        public void ShowAllHands()
        {
            Console.WriteLine("=======================================\n" +
                              "Dealer's Hand:");
            _dealer.PrintHands();
            _player.PrintHands();
            Console.WriteLine("=======================================\n");
        }

        public void ShowScores(int handIdx)
        {
            string startStr = "";
            string endStr;
            var hands = _player.GetHands();
            if (hands[handIdx].ResolveScore() == 0)
                endStr = "\t[BUST]\n";
            else if (hands[handIdx].StandState)
                endStr = "\t[STAND]\n";
            else
                endStr = "";

            if (hands.Count > 1)
            {
                var scores = hands[handIdx].GetScores();
                foreach (var score in scores)
                {
                    startStr += score + "/";
                }
                Console.WriteLine("___________________________\n" + 
                                  "Your scores for Hand #{0} are:", handIdx + 1);
                Console.WriteLine(startStr + endStr);
            }
            else
            {
                var scores = hands[0].GetScores();
                foreach (var score in scores)
                {
                    startStr = startStr + score + "/";
                }
                Console.WriteLine("___________________________\n" + 
                                  "Your scores are:\n" + startStr + endStr);
            }
        }

        public void StartScreen(int roundNum)
        {
            Console.Write(@"
=======#┌──────────────────────┐#======┌──────┐┌──────┐  ===
=======#| Let's Play Blackjack |#===== |A.--. ||K.--. | ===
=======#└──────────────────────┘#====  | (\/) || :/\: |===
                 /777                  | :\/: || :\/: |
                (o o)                  | '--'A|| '--'K|
                 (_)--b                └──────┘└──────┘
");
            Console.WriteLine("Round {0} \t\t\t Balance: {1} CREDITS", roundNum, _player.Balance);
        }

        public void GameOverScreen()
        {
            Console.Write(@"
========================================
========== G A M E    O V E R ==========
========================================
");
        }

        public void EPIC_WIN_SCREEN()
        {
            Console.WriteLine("\nMan! You. Are. An. Absolute. Legend!" + 
                              "\nYou've reached over 20.000 CREDITS! Congrats!" + 
                              "\nYou are 'The Black Jack'!");
            Console.Write(@"
 __       __  ______  __    __  __    __  ________  _______  
/  |  _  /  |/      |/  \  /  |/  \  /  |/        |/       \ 
$$ | / \ $$ |$$$$$$/ $$  \ $$ |$$  \ $$ |$$$$$$$$/ $$$$$$$  |
$$ |/$  \$$ |  $$ |  $$$  \$$ |$$$  \$$ |$$ |__    $$ |__$$ |
$$ /$$$  $$ |  $$ |  $$$$  $$ |$$$$  $$ |$$    |   $$    $$< 
$$ $$/$$ $$ |  $$ |  $$ $$ $$ |$$ $$ $$ |$$$$$/    $$$$$$$  |
$$$$/  $$$$ | _$$ |_ $$ |$$$$ |$$ |$$$$ |$$ |_____ $$ |  $$ |
$$$/    $$$ |/ $$   |$$ | $$$ |$$ | $$$ |$$       |$$ |  $$ |
$$/      $$/ $$$$$$/ $$/   $$/ $$/   $$/ $$$$$$$$/ $$/   $$/
");
        }

        public void WinByBlackjackMsg()
        {
            Console.Write(@"
##########################################################
#  We Got BlackJack baby! Pay 3:2 of the bet! [X_____x]  #
##########################################################

");
        }
        public void WinMsg()
        {
            Console.Write(@"
################################################
#  Player Wins! Pay 1:1 of the bet! [>_____<]  #
################################################

");
        }

        public void LoseByBlackjackMsg()
        {
            Console.Write(@"
############################################################################
#  Player Loses, Dealer has Blackjack! Collect bet from player! [^_____^]  #
############################################################################

");
        }
        public void LoseMsg()
        {
            Console.Write(@"
###################################################################
#  Player Loses, Dealer wins! Collect bet from player! [^_____^]  #
###################################################################

");
        }
        public void TieMsg()
        {
            Console.Write(@"
##############################################################
#  We tied! Pay Nothing. Bet goes back to player. [-_____-]  #
##############################################################

");
        }
        public void BustMsg()
        {
            Console.Write(@"
/|/|/|/|/|/|/|/|/|/|/|/|/
|  Oh no! It's a BUST!  |
|/|/|/|/|/|/|/|/|/|/|/|/|

");
        }
        
        public void OfferInsuranceMsg()
        {
            Console.Write(@"
.-----------------------------------------------------.
| Yikes! Dealer might have a blackjack on their hand. |
| Would you like some Insurance?      ['  ..  ']      |
'-----------------------------------------------------'

");
        }
        public void InsuranceWinMsg()
        {
            Console.Write(@"
.------------------------------------------------------------.
|  ...Hey, but look at that!     [O . O]                     |
|  Player Wins Insurance Bet! Pay 2:1 of the bet! [>___<]    |
'------------------------------------------------------------'

");
        }
        public void InsuranceLoseMsg()
        {
            Console.Write(@"
.-----------------------------------------------------------------------.
|  Player Loses Insurance Bet! Collect side-bet from player! [^_____^]  |
");
            Console.WriteLine("|                    [ Player Lost {0} CREDITS ]                        |", _player.SideBet);
            Console.WriteLine("'-----------------------------------------------------------------------'");
        }
    }
}