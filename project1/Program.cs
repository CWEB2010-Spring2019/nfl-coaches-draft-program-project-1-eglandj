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

                row = getRow(position, moneyBank);

                checkRow(ref row);

                outputPositionTable(row, playerName, salary, school, position, rank, pickedPlayer);
        
                column = getColumn(ref rankPick, rank, row, position, moneyBank);

                checkColumn(ref column);

                accumPrice(playerName, school, salary, ref moneyBank, row, column, pickedPlayer, ref pickCount);

                costEffective(ref rankPick, ref pickCount, ref moneyBank, ref effectiveDraft, lowCost);

                keyCapture(out sentinel, ref pickCount);
            }
            //Invoking the closing message output after user pressed X or all 5 picks are used
            outputPrice(moneyBank, ref effectiveDraft, lowCost, pickedPlayer, rank, playerName, school, salary, position);
            Console.WriteLine("Please press any key to exit.");
            Console.ReadKey();

        } //End of main
        static void greeting(double money)//Greeting message to user
        {
            Console.WriteLine($"Welcome to the 2019 NFL Draft!\nYou will begin the draft with {money.ToString("c")}!\nYou will only have 5 picks.");
            Console.WriteLine("You will not be able to select a player if they have been \nselected or if you do not have enough money.\n");
        }

        //Capture Key Mothed
        static void keyCapture(out ConsoleKey key, ref int pickCount)
        {
            Console.WriteLine("If you would like to draft a player, please press any key.\nIf not, please press X to exit.");
            key = Console.ReadKey().Key;

            if (pickCount == 5)//If statement to catch if user is out of picks
            {
                Console.Clear();
                Console.WriteLine("You are at your max amount of picks.\nThe draft will now end. Please press any key to continue.");
                Console.ReadKey();
                key = ConsoleKey.X;//Ends the while loop
            }
        }
        //Output table for the user to see all the player data
        static void outputTable(string[,] name, double[,] sal, string[,] school, string[] pos, string[,] rank, bool[,] picked)
        {
            Console.Clear();

            Console.Write("Position".PadRight(20));//Labeling the far left column and adding the length to equal 20
            
            for (int i = 0; i < rank.GetLength(1); i++)//For loop to get the name of rank data from rank array
            {
                Console.Write($"{rank[i, i]}".PadRight(20));//Outputting rank names to top of the table
            }
            Console.WriteLine("\n");

            for (var i = 0; i < name.GetLength(0); i++)//For loop to run for the length of number of names in the array
            {
                Console.Write($"{pos[i].PadRight(20)}");//Writing the position of the player for that row
                //For loop writing names for each position from array data
                for (var x = 0; x < name.GetLength(1); x++)
                {
                    if (picked[i, x] == false)//Will write the players name with a black background if they have not been picked
                    {
                        Console.Write($"{name[i, x].PadRight(20)}");
                    }
                    else//Writing the players name with a red background if they have been picked
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.Write($"{name[i, x].PadRight(20)}");
                        Console.ResetColor();//Resetting the background to black for the data to follow
                    }

                }
                //Creating two new line and making the length equal to others
                Console.WriteLine("");
                Console.Write("".PadRight(20));

                for (var x = 0; x < school.GetLength(1); x++)
                {
                    if (picked[i, x] == false)//Will write the players school with a black background if they have not been picked
                    {
                        Console.Write($"{school[i, x].PadRight(20)}");
                    }
                    else//Writing the players school with a red background if they have been picked
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.Write($"{school[i, x].PadRight(20)}");
                        Console.ResetColor();//Resetting the background to black for the data to follow
                    }
                }
                //Creating two new line and making the length equal to others
                Console.WriteLine("");
                Console.Write("".PadRight(20));

                for (var x = 0; x < sal.GetLength(1); x++)
                {

                    if (picked[i, x] == false)//Will write the players salary with a black background if they have not been picked
                    {
                        Console.Write($"{sal[i, x].ToString("c").PadRight(20)}");
                    }
                    else//Writing the players salary with a red background if they have been picked
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.Write($"{sal[i, x].ToString("c").PadRight(20)}");//Writing salary as a string with a cost format 
                        Console.ResetColor();//Resetting the background to black for the data to follow
                    }
                }
                Console.WriteLine("\n");
            }

        }
        static void outputPositionTable(int row, string[,] name, double[,] sal, string[,] school, string[] pos, string[,] rank, bool[,] picked)
        {
            Console.Clear();
            Console.Write("".PadRight(20));
            for (int i = 0; i < rank.GetLength(1); i++)
            {
                Console.Write($"{rank[row, i]}".PadRight(20));//Outputting rank names to top of the table
            }
            Console.WriteLine("\n");

            Console.Write($"{pos[row].PadRight(20)}");// Outputting the position based on the row the user selected

            for (var x = 0; x < name.GetLength(1); x++)
            {
                if (picked[row, x] == false)//Will write the players name with a black background if they have not been picked
                {
                    Console.Write($"{name[row, x].PadRight(20)}");
                }
                else//Writing the players name with a red background if they have been picked
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Write($"{name[row, x].PadRight(20)}");
                    Console.ResetColor();//Resetting the background to black for the data to follow
                }
            }
            Console.WriteLine("");
            Console.Write("".PadRight(20));

            for (var x = 0; x < name.GetLength(1); x++)
            {
                if (picked[row, x] == false)//Will write the players name with a black background if they have not been picked
                {
                    Console.Write($"{school[row, x].PadRight(20)}");
                }
                else//Writing the players name with a red background if they have been picked
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Write($"{school[row, x].PadRight(20)}");
                    Console.ResetColor();//Resetting the background to black for the data to follow
                }
            }
            Console.WriteLine("");
            Console.Write("".PadRight(20));

            for (var x = 0; x < name.GetLength(1); x++)
            {
                if (picked[row, x] == false)//Will write the players name with a black background if they have not been picked
                {
                    Console.Write($"{sal[row, x].ToString("c").PadRight(20)}");
                }
                else//Writing the players name with a red background if they have been picked
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Write($"{sal[row, x].ToString("c").PadRight(20)}");
                    Console.ResetColor();//Resetting the background to black for the data to follow
                }
            }
            Console.WriteLine("\n");
        }
        static int getRow(string[] pos, double price)//Method to create options for user and capture the position they select
        {
            Console.WriteLine($"You have {price.ToString("c")} remaining.\n");//Writing money remaining as a string with a cost format 
            int row;
            Console.WriteLine("Please select the position of the player you would like to draft.\nThen press enter.");
            for (int i = 0; i < pos.Length; i++)
            {
                Console.WriteLine($"{i + 1}.) {pos[i]}");
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
        static int getColumn(ref List<int> rankPick, string[,] rank, int row, string[] pos, double price)
        {
            int column;
            Console.WriteLine($"You have {price.ToString("c")} remaining.\n");//Writing money remaining as a string with a cost format
            Console.WriteLine($"You have selected:\n{row + 1}.) {pos[row]}\n");//Reminding the user of their position selection
            Console.WriteLine("Please enter the rank of the player you would like to draft.\n Then press enter");
            for (int i = 0; i < rank.GetLength(1); i++)//For loop giving the user options for rank selection
            {
                Console.WriteLine($"{i + 1}.) {rank[i, i]}");
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
        static void checkRow(ref int row)//Checking to make sure the user inputs a number within range
        {
            while ((row < 0) || (row > 7))//Number oustide of range will force the user to input correctly
            {
                try
                {
                    Console.WriteLine("Invalid entry, please enter a number between 1 and 8.");
                    row = Convert.ToInt32(Console.ReadLine()) - 1;
                }
                catch
                {
                    row = -1;
                }
                
            }
        }
        static void checkColumn(ref int column)//Checking to make sure the user inputs a number within range
        {
            while ((column < 0) || (column > 4))//Number oustide of range will force the user to input correctly
            {
                try
                {
                    Console.WriteLine("Invalid entry, please enter a number between 1 and 5.");
                    column = Convert.ToInt32(Console.ReadLine()) - 1;
                }
                catch
                {
                    column = -1;
                }
            }
        }

        //Method to calculate if player has been picked, cost after player has been picked, and if they have enough to select the player
        static void accumPrice(string[,] name, string[,] school, double[,] price, ref double accum, int row, int column, bool[,] picked, ref int pickCount)
        {
            if (picked[row, column] == false)//Checking if the player has not been picked
            {
                if (accum >= price[row, column])//Checking to see if the user can afford the player
                {
                    Console.Clear();
                    accum -= price[row, column];//Removing the players price from the users bank
                    Console.WriteLine($"You have selected {name[row, column]} from {school[row, column]} for {price[row, column].ToString("c")}");
                    Console.WriteLine($"You have {accum.ToString("c")} remaining.\n");
                    picked[row, column] = true;//Changing the player to picked
                    pickCount++;//Increasing the pick count
                }
                else//If the user can not afford the player
                {
                    Console.Clear();
                    Console.WriteLine("Invalid option");
                    Console.WriteLine($"You only have {accum.ToString("c")} remaining. Please select a valid option.");
                    return;
                }
            }
            else//If the player has already been picked
            {
                Console.WriteLine("This player has already been picked.");
                return;
            }
        }
        //Method to determine if the user has had a cost effective draft
        static void costEffective(ref List<int> rankPick, ref int pick, ref double accum, ref bool effectiveDraft, string lowCost)
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
                            Console.WriteLine(lowCost);//Outputs message if the user had an effective draft
                            effectiveDraft = true;//Changes the cost effective value
                            break;//Ends the loop
                        }
                    }
                }
            }
        }
        //Method that outputs a final message to the user
        static void outputPrice(double totalPrice, ref bool effectiveDraft, string lowCost, bool[,] picked, string[,] rank, string[,] name, string[,] school, double[,] price, string[] position)
        {
            Console.Clear();
            if (effectiveDraft == true)//Outputs message to the use if they had an effective draft
            {
                Console.WriteLine($"Congratulations, {lowCost}");
            }
            else//Ouputs message to user at the end of draft
            {
                Console.WriteLine("Congratulations on completing your draft!");
            }
            //Output message telling the user their remaining amount
            Console.WriteLine("Based on your selections, you have {0} remaining.", totalPrice.ToString("c"));
            Console.WriteLine("You have drafted:\n");

            //Formatting the output to in the final message of who the user drafted
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
            }//End of formatting
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
            pickedPlayer = false;//Setting the player pick to false
        }

    }
}