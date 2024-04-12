using System;
/*
     Final Project: Connect 4
    
    Institution: Bow Valley College
    Program: Software Development
    Course: SODV1202: Introduction to Object Oriented Programming
    Instructor: Sohaib Bajwa
    Student Name: Sasha Greene
    Student ID: ************************
     */

namespace Connect4
{
    class Program
    {
        static void Main(string[] args)
        {
            GameManager gameManager = new GameManager();
            gameManager.InitialiseGame();
            Console.ReadLine(); //just stops the game before it quits out when ai wins
        }
    }
    class Grid
    {
        //We use integers to determine ownership of each "space"
        //0 = Neutral
        //1 = Player
         //2 = AI

        public int columns;
        public int rows;
        int[,] grid;
        GameManager gameManager;

        public Grid(int rows, int columns, GameManager gMan)
        {
            this.gameManager = gMan;
            this.rows = rows;
            this.columns = columns;
            grid = new int[this.rows, this.columns];
            DisplayGrid();
        }
        public void DropPiece(int column, bool player)
        {
            column--; //subtract one from the column to make it work with our array: 1 becomes 0, 7 becomes 6 etc...
            //Check if there is space for a piece to drop
            for (int i = (this.rows - 1); i >= 0; i--)
            {
                if (grid[i, column] == 0) //if spot is neutral we can place a piece there
                {
                    if (player)
                    {
                        grid[i, column] = 1;
                        Console.WriteLine();
                        Console.WriteLine("Player 1 dropped a piece in row " + (i + 1) + " column " + (column + 1));    //Drop piece
                        Console.WriteLine();
                    }
                    else
                    {
                        grid[i, column] = 2;
                        Console.WriteLine();
                        Console.WriteLine("Player 2 dropped a piece in row " + (i + 1) + " column " + (column + 1));    //Drop piece
                        Console.WriteLine();
                    }
                    break;
                }
            }

            DisplayGrid();

            if (CheckForWin(player))
            {
                gameManager.Victory(player);
            }
        }
        private bool CheckForWin(bool playerturn)
            {
                //check through grid to see if there is any piece belonging to the player that just dropped
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        if (playerturn == true)//check for player win
                        {
                            if (grid[i, j] == 1)
                            {
                                if (CheckForLine(i, j, playerturn))
                                {
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            if (grid[i, j] == 2)//check for player 2 win
                            {
                                if (CheckForLine(i, j, playerturn))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }

                return false; //return false if a player doesnt win

            }
        private bool CheckForLine(int row, int column, bool playerTurn) //check for line around a specified place on the board
        {
            int checkingNumber = 0; //this sets the number we are looking for based on which player just dropped a piece
            if (playerTurn)
            {
                checkingNumber = 1;
            }
            else
            {
                checkingNumber = 2;
            }

            int piececount = 0; //counter to keep track of how many pieces we have in a row

            //check 3 spaces to the left
            for (int i = 0; i <= 3; i++)
            {
                if (column - i >= 0)//column cant go below 0
                {
                    if (grid[row, column - i] == checkingNumber)
                    {
                        piececount++;

                        if (piececount >= 4)//once we have 4 pieces in a row, return true to trigger victory.
                        {
                            return true;
                        }
                    }
                }
            }

            //check 3 spaces up
            piececount = 0; //reset counter to 0 for the next check
            for (int i = 0; i <= 3; i++)
            {
                if (row + i < rows)//row cant go above the top row
                {
                    if (grid[row + i, column] == checkingNumber)
                    {
                        piececount++;

                        if (piececount >= 4)//once we have 4 pieces in a row, return true to trigger victory.
                        {
                            return true;
                        }
                    }
                }
            }

            //check 3 spaces diagonal right up
            piececount = 0;//reset counter to 0 for the next check
            for (int i = 0; i <= 3; i++)
            {
                if (row + i < rows)//row cant go above the top row
                {
                    if (column - i >= 0) //column cant go below 0
                    {
                        if (grid[row + i, column - i] == checkingNumber)
                        {
                            piececount++;

                            if (piececount >= 4)//once we have 4 pieces in a row, return true to trigger victory.
                            {
                                return true;
                            }
                        }
                    }
                }

            }
            //check 3 spaces diagonal left up
            piececount = 0;//reset counter to 0 for the next check
            for (int i = 0; i <= 3; i++)
            {

                if (row + i < rows)//row cant go above the top row
                {
                    if (column + i < columns) //column cant go below 0
                    {
                        if (grid[row + i, column + i] == checkingNumber)
                        {
                            piececount++;

                            if (piececount >= 4)
                            {
                                return true;
                            }

                        }
                    }
                }

            }

            //if we didnt find 4 in a row, return false so we dont trigger victory.
            return false;
        }
        public void DisplayGrid()
        {

            Console.WriteLine("  1  2  3  4  5  6  7 ");
            Console.WriteLine("-----------------------");
            for (int i = 0; i < rows; i++)
            {
                if (i > 0)
                {
                    Console.WriteLine();

                }
                Console.Write("|");


                for (int j = 0; j < columns; j++)
                {
                    if(grid[i, j] == 1)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                    }
                    else if(grid[i, j] == 2)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    Console.Write(" " + grid[i, j] + " ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.Write("|");
            }
            Console.WriteLine();
            Console.WriteLine("-----------------------");
            Console.WriteLine();

        }

        public bool CheckIfSpace(int column)
        {
            return true;
        }
    }

    class GameManager
    {
        public bool playAgainstAI;
        
        public bool isGameInitialised = false;
        public bool isVictoryAchieved = false;
        bool isPlayer1Turn = true;

        Player player1;
        Player player2;

        Grid grid;

        private void PlayGame()
        {
            string command = "";  //Command format: Drop 4 (Drop Piece in column 4)

            do
            {
                if (isPlayer1Turn)
                {  //handle player turn
                    
                    HandlePlayerTurn(isPlayer1Turn, command, grid);
                }
                else // second players turn
                {
                    
                    
                    if (playAgainstAI) //handle ai turn
                    {
                        HandleAITurn(isPlayer1Turn, grid);
                    }
                    else //handle player 2 turn
                    {
                        HandlePlayerTurn(isPlayer1Turn, command, grid);
                    }

                }

                if (isVictoryAchieved)
                {
                    break;
                }
            } while (command != "exit");
        }
        public void InitialiseGame()
        {
            int difficultyLevel = 0;
            Console.WriteLine("Welcome to Connect 4! The goal is to get 4 pieces to line up straight or diagonally!");//Welcome Message
            do
            {
                Console.WriteLine("Would you like to play against ai or another human?");
                string cmd = Console.ReadLine();
                cmd = cmd.Trim();
                cmd = cmd.ToLower();
                string[] split = cmd.Split();
                if (split[0] == "ai")
                {
                    Console.WriteLine("Would you like the ai to be easy or normal?");
                    cmd = Console.ReadLine();
                    cmd = cmd.Trim();
                    cmd = cmd.ToLower();
                    split = cmd.Split();

                    if (split[0] == "easy")
                    {
                        Console.WriteLine("set AI to easy");
                        difficultyLevel = 0;
                    }else if (split[0] == "normal")
                    {
                        Console.WriteLine("set AI to normal");
                        difficultyLevel = 1;
                    }
                    else
                    {
                        Console.WriteLine("#ERROR Incorrect difficulty input, defaulting to normal");
                    }
                    playAgainstAI = true;
                    isGameInitialised = true;
                }
                else if (split[0] == "human")
                {

                    playAgainstAI = false;
                    isGameInitialised = true;
                }
                else
                {
                    Console.WriteLine("#ERROR: Please Enter whether you want to play against an ai or human");
                }
            } while (!isGameInitialised);

            grid = new Grid(6, 7, this);//create the grid 
            player1 = new Player(); //initialise player 1
            if (playAgainstAI)
            {
                player2 = new AI(difficultyLevel); //initialise player 2(or ai)
            }
            else
            {
                player2 = new Player(); //initialise player 2(or ai)
            }

            PlayGame(); //Start the actual game after initialisation
        }
        private void HandlePlayerTurn(bool isPlayer1Turn, string command, Grid grid)
        {
            Console.WriteLine();

            if (isPlayer1Turn)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Player 1 Turn, Enter a Command:");
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Player 2 Turn, Enter a Command:");
            }

            Console.BackgroundColor = ConsoleColor.Black;

            command = Console.ReadLine();
            command = command.Trim();
            command = command.ToLower();
            string[] split = command.Split();

            if (split[0] == "drop")
            {
                if (split.Length > 1)
                {
                    //#TODO fix bug where if you only type drop with no parameters
                    int column = int.Parse(split[1]);
                    if (column >= 1 && column <= grid.columns) //make sure column exists, should be between 1-7
                    {
                        grid.DropPiece(column, isPlayer1Turn);
                        EndTurn(isPlayer1Turn);
                    }
                    else
                    {
                        Console.WriteLine("#ERROR: That Column does not exist! try to drop a piece between columns 1-7!");
                    }
                }
                else
                {
                    Console.WriteLine("#ERROR: Enter a command as well as the perameters! ex) 'drop 3'");
                }
            }
            else if (split[0] == "?")
            {
                Console.WriteLine("Help: Command Examples: '?', 'drop 3', 'exit'");
            }
        }
        private void HandleAITurn(bool playerTurn, Grid grid)
        {

            player2.TakeTurn(grid);
            EndTurn(playerTurn);

        }
        private void EndTurn(bool playerTurn)
        {

            if (playerTurn)
            {

                isPlayer1Turn = false;
                

            }
            else
            {
                isPlayer1Turn = true;
                
            }
        }

        public void Victory(bool playerTurn) //public so we can trigger it when victory conditions are met
        {
            if (playerTurn)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Congratulations, Player 1! you connected 4!");
                isVictoryAchieved = true;
                Console.Read();
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Congratulations, Player 2! you connected 4!");
                isVictoryAchieved = true;
                Console.Read();
            }
        }
    }

    class Player
    {
        public bool isAIControlled;
        public virtual void TakeTurn(Grid grid)
        {

        }

    }

    class Human : Player
    {
        public Human()
        {
            this.isAIControlled = false;
        }

        public override void TakeTurn(Grid grid)
        {

        }
    }

    class AI : Player
    {
        int difficulty = 0; //0: easy, 1: Normal, 2: Hard //#TODO implement difficulty settings
        int lastDropLocation=-1;
        public AI(int difficulty)
        {
            this.isAIControlled = true;
            this.difficulty = difficulty;
        }

        public override void TakeTurn(Grid grid)
        {
            if (difficulty == 0) //this difficulty is totally random and makes the ai pretty dumb
            {
                Random rand = new Random();
                int randomColumn = rand.Next(1, 8);
                if (grid.CheckIfSpace(randomColumn))
                {
                    grid.DropPiece(randomColumn, false);
                }
                else
                {
                    TakeTurn(grid);
                }
            }
            if (difficulty == 1) //this difficulty, the ai will always place pieces next to a place they have already tried to put a piece.
            {
                if (lastDropLocation == -1) //if first drop
                {
                    Random rand = new Random();
                    int randomColumn = rand.Next(1, 8);
                    lastDropLocation = randomColumn;
                    if (grid.CheckIfSpace(randomColumn))
                    {
                        grid.DropPiece(randomColumn, false);
                    }
                    else
                    {
                        TakeTurn(grid);
                    }
                }
                else
                {
                    Random rand = new Random();

                    if (lastDropLocation == 1) //we are on the left side so we can only choose the same column or the one to the right
                    {
                        int randomColumn = rand.Next(1, 2);
                        lastDropLocation = randomColumn;
                      
                        if (grid.CheckIfSpace(randomColumn))
                        {
                            grid.DropPiece(randomColumn, false);
                        }
                        else
                        {
                            TakeTurn(grid);
                        }
                    }
                    else if (lastDropLocation == 8) //we are on the rightside so we can only choose the same column or the one to the left
                    {
                        int randomColumn = rand.Next(7, 8);
                        lastDropLocation = randomColumn;
                       
                        if (grid.CheckIfSpace(randomColumn))
                        {
                            grid.DropPiece(randomColumn, false);
                        }
                        else
                        {
                            TakeTurn(grid);
                        }
                    }
                    else //we are somewhere in the middle so we can drop either on the same column or to either side
                    {
                        int randomColumn = rand.Next(lastDropLocation-1, lastDropLocation+1);
                        lastDropLocation = randomColumn;
                     
                        if (grid.CheckIfSpace(randomColumn))
                        {
                            grid.DropPiece(randomColumn, false);
                        }
                        else
                        {
                            TakeTurn(grid);
                        }
                    }
                }
            }
            if (difficulty == 2)//this difficulty is where they will actively try to stop the player from winning
            {

            }
        }
    }
}

