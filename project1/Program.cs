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
            Player[,] playerProfile = JsonConvert.DeserializeObject<Player[,]>(File.ReadAllText(@"..\..\..\player.JSON"));
            //Creating an array for position data
            string[] position = new string[playerProfile.GetLength(0)];
            for (int i = 0; i < playerProfile.GetLength(0); i++)
            {
                for (int j = 0; j < playerProfile.GetLength(1); j++)
                {
                    position[i] = playerProfile[i, j].Position;
                }
            }//End of array creation
            List<int> rankPick = new List<int>(); //List created for holding the players ranks that were picked
            //Global variables created
            double moneyBank = 95000000;
            int row, column;
            int pickCount = 0;
            bool effectiveDraft = false;
            string lowCost = "You have made some COST EFFECTIVE draft choices.\n";
            //Invoking greeting and key press methods
            Greeting(moneyBank);
            KeyCapture(out ConsoleKey sentinel, ref pickCount);
            //Main while loop invoking methods when user did not press Escape
            while (sentinel != ConsoleKey.Escape)
            {
                OutputTable(playerProfile, position);
                row = GetRow(position, moneyBank);
                CheckRow(ref row);
                OutputPositionTable(playerProfile, row, position);       
                column = GetColumn(ref rankPick, playerProfile, row, position, moneyBank);
                CheckColumn(ref column);
                AccumPrice(playerProfile, ref moneyBank, row, column, ref pickCount);
                CostEffective(ref rankPick, ref pickCount, ref moneyBank, ref effectiveDraft, lowCost);
                KeyCapture(out sentinel, ref pickCount);
            }
            //Invoking the closing message output after user pressed X or all 5 picks are used
            OutputPrice(playerProfile, pickCount, moneyBank, ref effectiveDraft, lowCost, position);
            Console.WriteLine("Please press any key to exit.");
            Console.ReadKey();
        } //End of main
        static void Greeting(double moneyBank)//Greeting message to user
        {
            Console.Write("Welcome to the 2019 NFL Draft!\nYou will begin the draft with ");
            ColoredConsoleWrite(ConsoleColor.DarkGreen, $"{moneyBank.ToString("c")}");
            Console.Write("!\nYou will only have 5 picks.");
            Console.WriteLine("\n\nPlayers can not be selected more than once or if you can't afford them.\n\nAlso, please maximize the screen before you begin.\n");
        }
        //Capture Key Method
        static void KeyCapture(out ConsoleKey key, ref int pickCount)
        {
            Console.WriteLine("If you would like to draft a player, please press any key.\nIf you would like to end the draft, please press Escape.");
            key = Console.ReadKey().Key;
            if (pickCount == 5)//If statement to catch if user is out of picks
            {
                Console.Clear();
                Console.WriteLine("You are at your max amount of picks.\nThe draft will now end. Please press any key to continue.");
                Console.ReadKey();
                key = ConsoleKey.Escape;//Ends the while loop
            }
        }
        //Output table for the user to see all the player data
        static void OutputTable(Player[,] player, string [] pos)
        {
            Console.Clear();
            Console.Write("Position".PadRight(20));//Labeling the far left column and adding the length to equal 20
            for (int i = 0; i < player.GetLength(1); i++)//For loop to get the name of rank data from rank array
            {
                CenterConsoleWrite(ConsoleColor.White, $"{player[i, i].Rank}");//Outputting rank names to top of the table
            }
            Console.WriteLine("\n");
            for (int i = 0; i < player.GetLength(0); i++)//For loop to run for the length of number of names in the array
            {
                Console.Write($"{pos[i]}".PadRight(20));//Writing the position of the player for that row
                for (int b = 0; b < 3; b++)
                {
                    //For loop writing names for each position from array data
                    for (int x = 0; x < player.GetLength(1); x++)
                    {
                        if (player[i, x].pickedPlayer == false)
                        {
                            if (b == 0)
                            {
                                CenterConsoleWrite(ConsoleColor.White, $"{player[i, x].Name}");
                            }
                            else if (b == 1)
                            {
                                CenterConsoleWrite(ConsoleColor.White, $"{player[i, x].School}");
                            }
                            else if (b == 2)
                            {
                                CenterConsoleWrite(ConsoleColor.DarkGreen, $"{player[i, x].Salary.ToString("c")}");
                            }
                        }
                        else
                        {
                            if (b == 0)
                            {
                                CenterConsoleWrite(ConsoleColor.DarkRed, $"{player[i, x].Name}");
                            }
                            else if (b == 1)
                            {
                                CenterConsoleWrite(ConsoleColor.DarkRed, $"{player[i, x].School}");
                            }
                            else if (b == 2)
                            {
                                CenterConsoleWrite(ConsoleColor.DarkRed, $"{player[i, x].Salary.ToString("c")}");
                            }
                        }
                    }
                    Console.WriteLine("");
                    CenterConsoleWrite(ConsoleColor.White, "");
                }
                Console.WriteLine("");
            }
        }
        static void OutputPositionTable(Player[,] player, int row, string[] pos)
        {
            Console.Clear();
            CenterConsoleWrite(ConsoleColor.White, "");
            for (int i = 0; i < player.GetLength(1); i++)
            {
                CenterConsoleWrite(ConsoleColor.White, $"{player[row, i].Rank}");//Outputting rank names to top of the table
            }
            Console.WriteLine("\n");
            Console.Write($"{pos[row]}".PadRight(20));// Outputting the position based on the row the user selected
            for (int b = 0; b < 3; b++)
            {
                //For loop writing names for each position from array data
                for (int x = 0; x < player.GetLength(1); x++)
                {
                    if (player[row, x].pickedPlayer == false)
                    {
                        if (b == 0)
                        {
                            CenterConsoleWrite(ConsoleColor.White, $"{player[row, x].Name}");
                        }
                        else if (b == 1)
                        {
                            CenterConsoleWrite(ConsoleColor.White, $"{player[row, x].School}");
                        }
                        else if (b == 2)
                        {
                            CenterConsoleWrite(ConsoleColor.DarkGreen, $"{player[row, x].Salary.ToString("c")}");
                        }
                    }
                    else
                    {
                        if (b == 0)
                        {
                            CenterConsoleWrite(ConsoleColor.DarkRed, $"{player[row, x].Name}");
                        }
                        else if (b == 1)
                        {
                            CenterConsoleWrite(ConsoleColor.DarkRed, $"{player[row, x].School}");
                        }
                        else if (b == 2)
                        {
                            CenterConsoleWrite(ConsoleColor.DarkRed, $"{player[row, x].Salary.ToString("c")}");
                        }
                    }
                }
                Console.WriteLine("");
                CenterConsoleWrite(ConsoleColor.White, "");
            }
            Console.WriteLine("");
        }
        static int GetRow(string[] pos, double price)//Method to create options for user and capture the position they select
        {
            Console.Write($"You have ");//Writing money remaining as a string with a cost format
            ColoredConsoleWrite(ConsoleColor.DarkGreen, $"{ price.ToString("c")}");
            Console.Write(" remaining.\n\n");
            int row;
            Console.WriteLine("Please select the position of the player you would like to draft.\nThen press enter.\n");
            for (int i = 0; i < pos.Length; i++)
            {
                Console.WriteLine($"  {i + 1}.) {pos[i]}");
            }
            try//Error catching
            {
                row = Convert.ToInt32(Console.ReadLine());
                return row = row - 1;
            }
            catch
            {
                return row = -1;
            } 
        }
        //Method to capture the users rank they select
        static int GetColumn(ref List<int> rankPick, Player[,] player, int row, string[] pos, double price)
        {
            int column;
            Console.Write("You have ");//Writing money remaining as a string with a cost format
            ColoredConsoleWrite(ConsoleColor.DarkGreen, $"{ price.ToString("c")}");
            Console.Write(" remaining.\n\n");
            Console.WriteLine($"You have selected:\n{row + 1}.) {pos[row]}\n");//Reminding the user of their position selection
            Console.WriteLine("Please enter the rank of the player you would like to draft.\nThen press enter.\n");
            for (int i = 0; i < player.GetLength(1); i++)//For loop giving the user options for rank selection
            {
                Console.WriteLine($"  {i + 1}.) {player[i, i].Rank}");
            }
            try
            {
                column = Convert.ToInt32(Console.ReadLine());
                rankPick.Add(column);//Adding the rank pick to a list
                return column = column - 1;//Setting the user selection to equal data array selection
            }
            catch
            {
                return column = -1;
            }
        }
        static void CheckRow(ref int row)//Checking to make sure the user inputs a number within range
        {
            while ((row < 0) || (row > 7))//Number oustide of range will force the user to input correctly
            {
                try
                {
                    Console.WriteLine("Invalid entry, please enter a number between 1 and 8.");
                    row = (Convert.ToInt32(Console.ReadLine()) - 1);
                }
                catch
                {
                    row = -1;
                }
            }
        }
        static void CheckColumn(ref int column)//Checking to make sure the user inputs a number within range
        {
            while ((column < 0) || (column > 4))//Number oustide of range will force the user to input correctly
            {
                try
                {
                    Console.WriteLine("Invalid entry, please enter a number between 1 and 5.");
                    column = (Convert.ToInt32(Console.ReadLine()) - 1);
                }
                catch
                {
                    column = -1;
                }
            }
        }
        //Method to calculate if player has been picked, cost after player has been picked, and if they have enough to select the player
        static void AccumPrice(Player[,] player, ref double accum, int row, int column, ref int pickCount)
        {
            if (player[row, column].pickedPlayer == false)//Checking if the player has not been picked
            {
                if (accum >= player[row, column].Salary)//Checking to see if the user can afford the player
                {
                    Console.Clear();
                    accum -= player[row, column].Salary;//Removing the players price from the users bank
                    Console.Write($"You have selected {player[row, column].Name} from {player[row, column].School} for ");
                    ColoredConsoleWrite(ConsoleColor.DarkGreen, $"{player[row, column].Salary.ToString("c")}");
                    Console.Write("!\nYou have ");
                    ColoredConsoleWrite(ConsoleColor.DarkGreen, $"{ accum.ToString("c")} ");
                    Console.WriteLine("remaining.\n");
                    player[row, column].pickedPlayer = true;//Changing the player to picked
                    pickCount++;//Increasing the pick count
                }
                else//If the user can not afford the player
                {
                    Console.Clear();
                    ColoredConsoleWrite(ConsoleColor.DarkRed, "Invalid option!\nYou only have ");
                    ColoredConsoleWrite(ConsoleColor.DarkGreen, $"{accum.ToString("c")}");
                    ColoredConsoleWrite(ConsoleColor.DarkRed, " remaining. Please select a valid option.\n");
                    return;
                }
            }
            else//If the player has already been picked
            {
                ColoredConsoleWrite(ConsoleColor.DarkRed, "This player has already been picked.\n");
                return;
            }
        }
        //Method to determine if the user has had a cost effective draft
        static void CostEffective(ref List<int> rankPick, ref int pick, ref double accum, ref bool effectiveDraft, string lowCost)
        {
            if (pick == 3)//Checking to see if the user is on the 3rd pick
            {
                //Sorting the rank list and reversing the order of it
                rankPick.Sort();
                rankPick.Reverse();
                for (int i = 0; i < rankPick.Count; i++)//For loop for length of list
                {
                    if (rankPick[i] > 3)//Checking if the value of indexed list 
                    {
                        break;//Breaks out of loop
                    }
                    else
                    {
                        if (accum > 30000000)//Checking to see the if the users money is above the value
                        {
                            ColoredConsoleWrite(ConsoleColor.DarkYellow, lowCost);//Outputs message if the user had an effective draft
                            effectiveDraft = true;//Changes the cost effective value
                            break;//Ends the loop
                        }
                    }
                }
            }
        }
        //Method that outputs a final message to the user
        static void OutputPrice(Player[,] player, int pickCount, double moneyBank, ref bool effectiveDraft, string lowCost, string[] position)
        {
            Console.Clear();
            if (pickCount == 0)//Output message to user if they end the draft before they picke a player
            {
                Console.WriteLine("Please come back when you are ready to draft!");
            }
            else
            {
                if (effectiveDraft == true)//Outputs message to the use if they had an effective draft
                {
                    Console.Write("Congratulations, ");
                    ColoredConsoleWrite(ConsoleColor.DarkYellow, $"{lowCost}");
                }
                else//Ouputs message to user at the end of draft
                {
                    Console.WriteLine("Congratulations on completing your draft!");
                }
                //Output message telling the user their remaining amount
                Console.Write("Based on your selections, you have ");
                ColoredConsoleWrite(ConsoleColor.DarkGreen, $"{moneyBank.ToString("c")} ");
                Console.WriteLine("remaining to pay-out signing bonuses.\nYou have drafted:\n");
                for (int a = 0; a < 5; a++)
                {
                    //Formatting the output to in the final message of who the user drafted
                    for (int i = 0; i < player.GetLength(0); i++)
                    {
                        for (int x = 0; x < player.GetLength(1); x++)
                        {
                            if (player[i, x].pickedPlayer == true)
                            {
                                if (a == 0)
                                {
                                    CenterConsoleWrite(ConsoleColor.White, $"{player[i, x].Rank}");
                                }
                                else if (a == 1)
                                {
                                    CenterConsoleWrite(ConsoleColor.White, $"{position[i]}");
                                }
                                else if (a == 2)
                                {
                                    CenterConsoleWrite(ConsoleColor.White, $"{player[i, x].Name}");
                                }
                                else if (a == 3)
                                {
                                    CenterConsoleWrite(ConsoleColor.White, $"{player[i, x].School}");
                                }
                                else if (a == 4)
                                {
                                    CenterConsoleWrite(ConsoleColor.DarkGreen, $"{player[i, x].Salary.ToString("c")}");
                                }
                            }
                        }
                    }
                    Console.WriteLine("");
                }
            }
            Console.WriteLine("\nThis application will now close.");
        }
        //Method by albucurus on https://www.daniweb.com/programming/software-development/threads/475572/changing-color-of-font-within-a-single-console-writeline
        public static void ColoredConsoleWrite(ConsoleColor color, string text)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = originalColor;
        }
        public static void CenterConsoleWrite(ConsoleColor color, string text)
        {
            int length = 20;
            int stringLength = text.Length;
            int buffer = (length - stringLength) / 2;
            string spacer = "";
            for (int i = 0; i < buffer; i++)
            {
                spacer += " " ;
            }
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            if(stringLength % 2 == 0)
            {
                Console.Write(spacer + text + spacer);
            }
            else
            {
                Console.Write(" " + spacer + text + spacer);
            }
            Console.ForegroundColor = originalColor;
        }
    }
    class Player
    {
        public string Position { get; set; }
        public string Rank { get; set; }
        public string Name { get; set; }
        public string School { get; set; }
        public double Salary { get; set; }
        public bool pickedPlayer = false;
    }
}