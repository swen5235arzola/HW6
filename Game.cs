using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trivia;

/// <summary>
/// Modified By: Adriana C. Arzola
/// HW 6 - Refactoring
/// </summary>
namespace UglyTrivia
{
    public class Game
    {
        #region Properties

        bool[] inPenaltyBox = new bool[6];

        public bool InPenaltyBox
        {
            get { return inPenaltyBox[CurrentPlayer]; }
            set { inPenaltyBox[CurrentPlayer] = value; }
        }
        public bool IsGettingOutOfPenaltyBox { get; set; }

        public bool IsPlayable()
        {
            return (PlayerCount >= 2);
        }

        public int PlayerCount
        {
            get { return players.Count; }
        }

        public int CurrentPlayer { get; set; }

        #endregion

        List<string> players = new List<string>();

        int[] places = new int[6];
        int[] purses = new int[6];

        LinkedList<string> popQuestions = new LinkedList<string>();
        LinkedList<string> scienceQuestions = new LinkedList<string>();
        LinkedList<string> sportsQuestions = new LinkedList<string>();
        LinkedList<string> rockQuestions = new LinkedList<string>();

        public Game()
        {
            for (int i = 0; i < 50; i++)
            {
                popQuestions.AddLast(CreateQuestion(Category.POP, i));
                scienceQuestions.AddLast(CreateQuestion(Category.SCIENCE, i));
                sportsQuestions.AddLast(CreateQuestion(Category.SPORTS, i));
                rockQuestions.AddLast(CreateQuestion(Category.ROCK, i));
            }
        }

        /// <summary>
        /// Generate Question String
        /// </summary>
        /// <param name="category"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public string CreateQuestion(Category category, int index)
        {
            string question;
            switch (category)
            {
                case Category.POP:
                    question = "Pop";
                    break;
                case Category.ROCK:
                    question = "Rock";
                    break;
                case Category.SCIENCE:
                    question = "Science";
                    break;
                case Category.SPORTS:
                    question = "Sports";
                    break;
                default:
                    question = "";
                    break;
            }

            if (question != "")
            {
                question += " Question " + index;
            }

            return question;
        }

        /// <summary>
        /// Add Player
        /// </summary>
        /// <param name="playerName"></param>
        /// <returns>boolean indicating successful addition</returns>
        public bool Add(String playerName)
        {
            players.Add(playerName);
            places[PlayerCount] = 0;
            purses[PlayerCount] = 0;
            inPenaltyBox[PlayerCount] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + players.Count);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roll"></param>
        public void Roll(int roll)
        {
            Console.WriteLine(players[CurrentPlayer] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (InPenaltyBox)
            {
                if (roll % 2 != 0)
                {
                    RemoveFromPenaltyBox();
                    MovePlayer(roll);
                    AskQuestion();
                }
                else
                {
                    Console.WriteLine(players[CurrentPlayer] + " is not getting out of the penalty box");
                    IsGettingOutOfPenaltyBox = false;
                }

            }
            else
            {
                MovePlayer(roll);
                AskQuestion();
            }

        }

        #region Game Actions

        private void MovePlayer(int roll)
        {
            places[CurrentPlayer] = places[CurrentPlayer] + roll;
            if (places[CurrentPlayer] > 11) places[CurrentPlayer] = places[CurrentPlayer] - 12;

            Console.WriteLine(players[CurrentPlayer]
                    + "'s new location is "
                    + places[CurrentPlayer]);
            Console.WriteLine("The category is " + currentCategory());
        }

        private void AskQuestion()
        {
            if (currentCategory() == "Pop")
            {
                Console.WriteLine(popQuestions.First());
                popQuestions.RemoveFirst();
            }
            if (currentCategory() == "Science")
            {
                Console.WriteLine(scienceQuestions.First());
                scienceQuestions.RemoveFirst();
            }
            if (currentCategory() == "Sports")
            {
                Console.WriteLine(sportsQuestions.First());
                sportsQuestions.RemoveFirst();
            }
            if (currentCategory() == "Rock")
            {
                Console.WriteLine(rockQuestions.First());
                rockQuestions.RemoveFirst();
            }
        }

        private String currentCategory()
        {
            if (places[CurrentPlayer] == 0) return "Pop";
            if (places[CurrentPlayer] == 4) return "Pop";
            if (places[CurrentPlayer] == 8) return "Pop";
            if (places[CurrentPlayer] == 1) return "Science";
            if (places[CurrentPlayer] == 5) return "Science";
            if (places[CurrentPlayer] == 9) return "Science";
            if (places[CurrentPlayer] == 2) return "Sports";
            if (places[CurrentPlayer] == 6) return "Sports";
            if (places[CurrentPlayer] == 10) return "Sports";
            return "Rock";
        }

        public bool WasCorrectlyAnswered()
        {
            if (InPenaltyBox)
            {
                if (IsGettingOutOfPenaltyBox)
                {
                    return CheckIfWinner();
                }
                else
                {
                    NextPlayer(); ;
                    return true;
                }
            }
            else
            {
                return CheckIfWinner();
            }
        }

        private bool CheckIfWinner()
        {
            Console.WriteLine("Answer was corrent!!!!");
            purses[CurrentPlayer]++;
            Console.WriteLine(players[CurrentPlayer]
                    + " now has "
                    + purses[CurrentPlayer]
                    + " Gold Coins.");

            bool winner = DidPlayerWin();
            NextPlayer();

            return winner;
        }

        public bool WrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            SendToPenaltyBox();
            NextPlayer();
            return true;
        }

        private void NextPlayer()
        {
            CurrentPlayer++;
            if (CurrentPlayer == players.Count) CurrentPlayer = 0;
        }

        private void SendToPenaltyBox()
        {
            Console.WriteLine(players[CurrentPlayer] + " was sent to the penalty box");
            inPenaltyBox[CurrentPlayer] = true;
        }

        private void RemoveFromPenaltyBox()
        {
            Console.WriteLine(players[CurrentPlayer] + " is getting out of the penalty box");
            IsGettingOutOfPenaltyBox = true;
        }

        private bool DidPlayerWin()
        {
            return !(purses[CurrentPlayer] == 6);
        }
        #endregion
    }
}
