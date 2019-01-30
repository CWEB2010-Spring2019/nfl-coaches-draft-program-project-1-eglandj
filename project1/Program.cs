using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace project1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Extracting data from a JSON File
            player[,] playerProfile = JsonConvert.DeserializeObject<player[,]>(File.ReadAllText(@"C:\Users\eglandj\Documents\Visual Studio 2017\Projects\Project 1\project1\player.JSON"));

            //Creating arrays for the extracted data
            bool[,] pickedPlayer = new bool[playerProfile.GetLength(0), playerProfile.GetLength(1)];
            for (int i = 0; i < playerProfile.GetLength(0); i++)
            {
                for (int j = 0; j < playerProfile.GetLength(1); j++)
                {
                    pickedPlayer[i, j] = playerProfile[i, j].pickedPlayer;

                }
            }
            string[,] playerName = new string[playerProfile.GetLength(0), playerProfile.GetLength(1)];
            for (int i = 0; i < playerProfile.GetLength(0); i++)
            {
                for (int j = 0; j < playerProfile.GetLength(1); j++)
                {
                    playerName[i, j] = playerProfile[i, j].Name;

                }
            }
            double[,] salary = new double[playerProfile.GetLength(0), playerProfile.GetLength(1)];
            for (int i = 0; i < playerProfile.GetLength(0); i++)
            {
                for (int j = 0; j < playerProfile.GetLength(1); j++)
                {
                    salary[i, j] = playerProfile[i, j].Salary;

                }
            }
            string[,] school = new string[playerProfile.GetLength(0), playerProfile.GetLength(1)];
            for (int i = 0; i < playerProfile.GetLength(0); i++)
            {
                for (int j = 0; j < playerProfile.GetLength(1); j++)
                {
                    school[i, j] = playerProfile[i, j].School;

                }
            }
            string[] position = new string[playerProfile.GetLength(0)];
            for (int i = 0; i < playerProfile.GetLength(0); i++)
            {
                for (int j = 0; j < playerProfile.GetLength(1); j++)
                {
                    position[i] = playerProfile[i, j].Position;

                }
            }
            string[,] rank = new string[playerProfile.GetLength(0), playerProfile.GetLength(1)];
            for (int i = 0; i < playerProfile.GetLength(0); i++)
            {
                for (int j = 0; j < playerProfile.GetLength(1); j++)
                {
                    rank[i, j] = playerProfile[i, j].Rank;

                }
            }//End of array creation

            List<int> rankPick = new List<int>(); //List created for holding the players rank that were picked
            //Global variables created
            ConsoleKey sentinel;
            double moneyBank = 95000000;
            int row, column;
            int pickCount = 0;
            bool effectiveDraft = false;
            string lowCost = "You have made some COST EFFECTIVE draft choices.\n";

            //Invoking greeting and key press methods
            greeting(moneyBank);
            keyCapture(out sentinel, ref pickCount);

            //Main while loop invoking methods when user did not press X
            while (sentinel != ConsoleKey.X)
            {
                outputTable(playerName, salary, school, position, rank, pickedPlayer);

                row = getRow(position);

                checkRow(ref row);

                column = getColumn(rankPick, rank);

                checkColumn(ref column);

                accumPrice(playerName, school, salary, ref moneyBank, row, column, pickedPlayer, ref pickCount);

                costEffective(rankPick, ref pickCount, ref moneyBank, ref effectiveDraft, lowCost);

                keyCapture(out sentinel, ref pickCount);
            }
            //Invoking the closing message output after user pressed X or all 5 picks are used
            outputPrice(moneyBank, ref effectiveDraft, lowCost, pickedPlayer, rank, playerName, school, salary, position);

        } //End of main
        static void greeting(double money)// Greeting message to user
        {
            Console.WriteLine($"Welcome to the 2019 NFL Draft!\nYou will begin the draft with {money.ToString("c")}!\nYou will only have 5 picks.\n");
        }

        static void keyCapture(out ConsoleKey key, ref int pickCount)
        {
            Console.WriteLine("If you would like to draft a player, please press any key.\nIf not, please press X to exit.");
            key = Console.ReadKey(true).Key;
            if (pickCount == 5)
            {
                Console.Clear();
                Console.WriteLine("You are at your max amount of picks.\nThe draft will now end. Please press any key to continue.");
                Console.ReadKey();
                key = ConsoleKey.X;

            }
        }
        static void outputTable(string[,] name, double[,] sal, string[,] school, string[] pos, string[,] rank, bool[,] picked)
        {
            Console.Clear();

            Console.Write("Position".PadRight(20));
            for (int i = 0; i < rank.GetLength(1); i++)
            {
                Console.Write($"{rank[i, i]}".PadRight(20));
            }
            Console.WriteLine("\n");

            for (var i = 0; i < name.GetLength(0); i++)
            {

                Console.Write($"{pos[i].PadRight(20)}");
                for (var x = 0; x < name.GetLength(1); x++)
                {
                    if (picked[i, x] == false)
                    {
                        Console.Write($"{name[i, x].PadRight(20)}");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.Write($"{name[i, x].PadRight(20)}");
                        Console.ResetColor();
                    }

                }
                Console.WriteLine("");
                Console.Write("".PadRight(20));
                for (var x = 0; x < school.GetLength(1); x++)
                {
                    if (picked[i, x] == false)
                    {
                        Console.Write($"{school[i, x].PadRight(20)}");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.Write($"{school[i, x].PadRight(20)}");
                        Console.ResetColor();
                    }
                }
                Console.WriteLine("");
                Console.Write("".PadRight(20));
                for (var x = 0; x < sal.GetLength(1); x++)
                {

                    if (picked[i, x] == false)
                    {
                        Console.Write($"{sal[i, x].ToString("c").PadRight(20)}");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.Write($"{sal[i, x].ToString("c").PadRight(20)}");
                        Console.ResetColor();
                    }
                }

                Console.WriteLine("");
                Console.WriteLine("");
            }

        }
        static int getRow(string[] pos)
        {
            int row;
            Console.WriteLine("Please select the position of the player you would like to draft");
            for (int i = 0; i < pos.Length; i++)
            {
                Console.WriteLine($"{i + 1}.) {pos[i]}");
            }

            row = Convert.ToInt32(Console.ReadLine());
            return row = row - 1;
        }
        static int getColumn(List<int> rankPick, string[,] rank)
        {
            int column;
            Console.WriteLine("Please enter the rank of the player you would like to draft");
            for (int i = 0; i < rank.GetLength(1); i++)
            {
                Console.WriteLine($"{i + 1}.) {rank[i, i]}");
            }
            column = Convert.ToInt32(Console.ReadLine());
            rankPick.Add(column);
            return column = column - 1;
        }
        static void checkRow(ref int num)
        {
            while ((num < 0) || (num > 7))
            {
                Console.WriteLine("Invalid entry, please enter a number between 1 and 8.");
                num = Convert.ToInt32(Console.ReadLine()) - 1;
            }
        }
        static void checkColumn(ref int num)
        {
            while ((num < 0) || (num > 4))
            {
                Console.WriteLine("Invalid entry, please enter a number between 1 and 5.");
                num = Convert.ToInt32(Console.ReadLine()) - 1;
            }
        }
        static void accumPrice(string[,] name, string[,] school, double[,] price, ref double accum, int row, int column, bool[,] picked,  ref int pickCount)
        {
            if (picked[row, column] == false)
            {
                if (accum >= price[row, column])
                {
                    Console.Clear();
                    accum -= price[row, column];
                    Console.WriteLine($"You have selected {name[row, column]} from {school[row, column]} for {price[row, column].ToString("c")}");
                    Console.WriteLine($"You have {accum.ToString("c")} remaining.\n");
                    picked[row, column] = true;
                    pickCount++;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid option");
                    Console.WriteLine($"You only have {accum.ToString("c")} remaining. Please select a valid option.");
                    return;
                }
            }
            else
            {
                Console.WriteLine("This player has already been picked.");
                return;
            }
        }

        static void costEffective(List<int> rankPick, ref int pick, ref double accum, ref bool effectiveDraft, string lowCost)
        {
            if (pick == 3)
            {
                rankPick.Sort();
                rankPick.Reverse();

                for (int i = 0; i < rankPick.Count; i++)
                {
                    if (rankPick[i] < 4)
                    {
                        if (accum > 30000000)
                        {
                            Console.WriteLine(lowCost);
                            effectiveDraft = true;
                            break;
                        }
                    }
                }
            }
        }
        static void outputPrice(double totalPrice, ref bool effectiveDraft, string lowCost, bool[,] picked, string[,] rank, string[,] name, string[,] school, double[,] price, string[] position)
        {
            Console.Clear();
            Console.WriteLine(effectiveDraft);
            if (effectiveDraft == true)
            {
                Console.WriteLine($"Congratulations, {lowCost}");
            }
            else
            {
                Console.WriteLine("Congratulations on completing your draft!");
            }
            Console.WriteLine("Based on your selections, you have {0} remaining.", totalPrice.ToString("c"));
            Console.WriteLine("You drafted in this order:\n");

            for (var i = 0; i < name.GetLength(0); i++)
            {
                for (var x = 0; x < name.GetLength(1); x++)
                {
                    if (picked[i, x] == true)
                    {
                        Console.Write($"{rank[i, x].PadRight(20)}");
                    }
                }
            }
            Console.WriteLine("");
            for (var i = 0; i < name.GetLength(0); i++)
            {
                for (var x = 0; x < name.GetLength(1); x++)
                {
                    if (picked[i, x] == true)
                    {
                        Console.Write($"{position[i].PadRight(20)}");
                    }
                }
            }
            Console.WriteLine("");
            for (var i = 0; i < name.GetLength(0); i++)
            {
                for (var x = 0; x < name.GetLength(1); x++)
                {
                    if (picked[i, x] == true)
                    {
                        Console.Write($"{name[i, x].PadRight(20)}");
                    }
                }
            }
            Console.WriteLine("");
            for (var i = 0; i < school.GetLength(0); i++)
            {
                for (var x = 0; x < school.GetLength(1); x++)
                {
                    if (picked[i, x] == true)
                    {
                        Console.Write($"{school[i, x].PadRight(20)}");
                    }
                }
            }
            Console.WriteLine("");
            for (var i = 0; i < price.GetLength(0); i++)
            {
                for (var x = 0; x < price.GetLength(1); x++)
                {
                    if (picked[i, x] == true)
                    {
                        Console.Write($"{price[i, x].ToString("c").PadRight(20)}");
                    }
                }
            }
            Console.WriteLine("\n");
            Console.WriteLine("This application will now close.");
        }
    }
    class player
    {
        public string Position { get; set; }
        public string Rank { get; set; }
        public string Name { get; set; }
        public string School { get; set; }
        public double Salary { get; set; }
        public bool pickedPlayer { get; set; }

        public player()
        {
            pickedPlayer = false;
        }

    }
}