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
            player[,] playerProfile = JsonConvert.DeserializeObject<player[,]>(File.ReadAllText(@"C:\Users\eglandj\Documents\Visual Studio 2017\Projects\Project 1\project1\player.JSON"));

            string[,] playerName = new string[playerProfile.GetLength(0), playerProfile.GetLength(1)];
            for(int i = 0; i < playerProfile.GetLength(0); i++)
            {
                for (int j = 0; j < playerProfile.GetLength(1); j++)
                {
                    playerName[i,j] = playerProfile[i, j].Name;
                    
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
            /*string[,] playerName = 
            {
                {"Andrew", "Anders", "Abe", "David"},
                {"Donald", "Jared", "Kai", "Sean"},
                {"Steve", "Kyle", "Jacob", "Joe"}
            };
            double[,] salary =
            {
                {25.43, 25.10, 25.01, 24.48},
                {25.34, 24.99, 24.58, 23.89},
                {24.74, 24.54, 24.21, 23.98}
            };
            string[] position = { "1. QB", "2. RB", "3. WR" };*/

            List<int> rankPick = new List<int>();
            ConsoleKey sentinel;
            double moneyBank = 95000000;
            int row, column;
            int pickCount = 1;

            greeting();
            keyCapture(out sentinel);

            while (sentinel != ConsoleKey.X)
            {
                Console.Clear();

                outputTable(playerName, salary, school, position);

                row = getRow();

                checkRow(ref row);

                column = getColumn(rankPick);

                checkColumn(ref column);

                accumPrice(playerName, salary, ref moneyBank, row, column);

                costEffective(rankPick, ref pickCount, ref moneyBank);

                keyCapture(out sentinel);
            }

            outputPrice(moneyBank);

        }

        static void greeting()
        {
            Console.WriteLine("Welcome to the 2019 NFL Draft!");
        }

        static void keyCapture(out ConsoleKey key)
        {
            Console.WriteLine("If you would like to draft a player, please press any key.\nIf not, please press X to exit.");
            key = Console.ReadKey(true).Key;
        }
        static void outputTable(string[,] name, double[,] sal, string[,] school, string[] pos)
        {
            Console.WriteLine($"  Ranks: 1 \t 2  \t 3 \t 4\n\n");
            for (var i = 0; i < name.GetLength(0); i++)
            {

                Console.Write($"{pos[i].PadRight(20)}");
                for (var x = 0; x < name.GetLength(1); x++)
                {
                    Console.Write($"{name[i, x].PadRight(20)}");
                }
                Console.WriteLine("");
                Console.Write("".PadRight(20));
                for (var x = 0; x < school.GetLength(1); x++)
                {
                    Console.Write($"{school[i, x].PadRight(20)}");
                }
                Console.WriteLine("");
                Console.Write("".PadRight(20));
                for (var x = 0; x < sal.GetLength(1); x++)
                {

                    Console.Write($"{sal[i, x].ToString("c").PadRight(20)}");
                }

                Console.WriteLine("");
                Console.WriteLine("\n");
            }

        }
        static int getRow()
        {
            int row; //Local
            Console.WriteLine("Please select the position of the player you would like to draft");
            row = Convert.ToInt32(Console.ReadLine());
            return row = row - 1;
        }
        static int getColumn(List<int> rankPick)
        {
            int column; //Local
            Console.WriteLine("Please enter the rank of the player you would like to draft");
            column = (Convert.ToInt32(Console.ReadLine()) - 1);
            if (column < 3)
            {
                rankPick.Add(column);

            }
            return column;
        }
        static void checkRow(ref int num)
        {
            while ((num < 0) || (num > 2))
            {
                Console.WriteLine("Invalid entry, please enter a number between 0 and 2");
                num = Convert.ToInt32(Console.ReadLine());
            }
        }
        static void checkColumn(ref int num)
        {
            while ((num < 0) || (num > 3))
            {
                Console.WriteLine("Invalid entry, please enter a number between 0 and 3");
                num = Convert.ToInt32(Console.ReadLine());
            }
        }
        static void accumPrice(string[,] prod, double[,] price, ref double accum, int row, int column)
        {
            if (accum >= price[row, column])
            {
                accum -= price[row, column];
                Console.WriteLine($"\nYou have selected {prod[row, column]} for {price[row, column].ToString("c")}");
                Console.WriteLine($"You have ${Math.Round(accum, 2)} remaining.\n");
            }
            else
            {
                Console.WriteLine("Invalid option");
                Console.WriteLine(accum);
                return;
            }
        }
        static void outputPrice(double totalPrice)
        {
            Console.Clear();
            Console.WriteLine("Based on your selections, you have {0} remaining", totalPrice.ToString("c"));
        }
        static void costEffective(List<int> rankPick, ref int pick, ref double accum)
        {
            for (int i = 0; i < rankPick.Count; i++)
            {
                if (rankPick[i] < 3)
                {
                    if (pick > 2 && accum > 25.00)
                    {
                        Console.WriteLine("You have made some COST EFFECTIVE draft choices\n");
                        break;
                    }
                }
            }
            pick++;
        }
        
    }
    class player
    {
        public string Position { get; set; }
        public string Rank { get; set; }
        public string Name { get; set; }
        public string School { get; set; }
        public double Salary { get; set; }
        
    }
}