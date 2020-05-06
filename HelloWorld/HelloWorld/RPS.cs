using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    class RPS
    {
        readonly Random ThisIsAI = new Random();
        bool validInput = false;
        struct Score
        {
            public int Player {get; set;}
            public int AI { get; set; }
        }
        Score score;
        public enum Choices
        {
            Rock,
            Paper,
            Scissors
        }
        public enum States
        {
            Tie,
            Win,
            Loss
        }
        int currentRound;
        States ResolveRound(Choices player_a, Choices player_b)
        {
            int intcase =  (((player_a - player_b)% Enum.GetNames(typeof(Choices)).Length)+ Enum.GetNames(typeof(Choices)).Length)% Enum.GetNames(typeof(Choices)).Length;

            //This works, because it wraps around the numbers, and if the distance between player and ai is 1, that is a win, if it is 2, then it is a loss.

            //(n % M) + M) % M
            // Rock Papers Scissors 0 1 2
            // 0 2 = -2
            // 0 1 = -1


            switch (intcase)
            {
                case 2:
                    return States.Loss;
                case 1:
                    return States.Win;
                case 0:
                    return States.Tie;
                default:
                    return States.Tie;
            }

        }

        Choices Pick()
        {
            return (Choices)ThisIsAI.Next(0, 2);
        }

        public void Play(int round = 0)
        {
            if (round == 0)
            {
                Console.WriteLine("Welcome to the Rock, Paper, Scissors Bot!");

            }
            else
            {
                Console.WriteLine("Next Round! You are in round " + round +".\nThe current score is: " + score.Player + " to " + score.AI);

            }
            Choices player = GetPlayerInput("Make your choice:");
            Choices AI = Pick();
            switch (ResolveRound(player, AI))
            {
                case States.Tie:
                    Console.WriteLine("The AI also picked " + AI + "! It was a tie!");
                    break;
                case States.Win:
                    Console.WriteLine("The AI picked " + AI + ". You won.");
                    score.Player++;
                    break;
                case States.Loss:
                    Console.WriteLine("Oh no! The AI picked " + AI + ", and you lost.");
                    score.AI++;
                    break;
            }
            if (score.AI >= 3 || score.Player >= 3)
            {
                if (score.AI >= 3)
                {
                    Win(1);
                    score.AI = 0;
                    score.Player = 0;
                }
                else
                {
                    Win(0);
                    score.AI = 0;
                    score.Player = 0;
                }
            }
            else
            {
                currentRound++;
                Play(currentRound);
            }
        }

        void Win(int player)
        {
            if (player == 0)
            {
                Console.WriteLine("Congrats! You Won!");
            }
            else
            {
                Console.WriteLine("Aw... The AI won.");
            }
        }
        Choices GetPlayerInput(string ConsoleText)
        {
            validInput = false;
            Choices choice = 0;
            do
            {
                Console.Write(ConsoleText+"\n");
                if (true)
                {
                    ConsoleKeyInfo temp = Console.ReadKey();
                    switch (temp.Key)
                    {
                        case ConsoleKey.UpArrow:
                            choice = Choices.Scissors;
                            validInput = true;
                            break;
                        case ConsoleKey.LeftArrow:
                            choice = Choices.Rock;
                            validInput = true; 
                            break;
                        case ConsoleKey.RightArrow:
                            choice = Choices.Paper;
                            validInput = true; 
                            break;
                        default:
                            validInput = false;
                            break;
                    }
                }
                else
                {
#pragma warning disable CS0162
                    string temp = Console.ReadLine();
                    if (temp == "q" || temp == "quit")
                    {
                        Program.MainMenu();
                        return 0;
                    }
                    switch (temp)
                    {
                        case "rock":
                        case "r":
                        case "1":
                            choice = Choices.Rock;
                            validInput = true;
                            break;
                        case "paper":
                        case "p":
                        case "2":
                            choice = Choices.Paper;
                            validInput = true;
                            break;
                        case "scissors":
                        case "s":
                        case "3":
                            choice = Choices.Scissors;
                            validInput = true;
                            break;
                        default:
                            validInput = false;
                            break;
#pragma warning restore CS0162
                    }


                }
                Console.Write("You picked " + choice + "!\n");


            } while (!validInput);
            return choice;
        }
        
    }
}
