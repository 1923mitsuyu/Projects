using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Game;
class IFN563_Assessment2
{
    public static GamesModel GamesData { get; set; } = new();

    static void Main()
    {
        Mode mode = new Mode();
        Assistance manual = new Assistance();
        Console.WriteLine("Welcome to our games. I guarantee you will have a fantastic time");
        GamesData.Games = Array.Empty<GamesModel.GameModel>();
        // Choose the game type
        if (mode.ChooseGame() == 1) // Choose SOS (1)
        {
            // Create two objects 
            SOS sos = new SOS();
            Board board = new Board(3, 3, 1);

            // Choose game mode (human[1] vs computer[2])
            if (mode.ChooseMode() == 1)  // Human mode [1]
            {
                // The game sarts and ends 
                sos.PlayGame1(mode, board, manual);
            }
            else // Computer mode [2]
            {
                // The game sarts and ends 
                sos.PlayGame2(mode, board, manual);
            }
        }
        else // Choose ConnectFour
        {
            ConnectFour connectFour = new ConnectFour();
            Board boardConnectFour = new(6, 7, 1);

            if (mode.ChooseMode() == 1) // Human mode [1]
            {
                // The game sarts and ends 
                connectFour.PlayGame3(mode, boardConnectFour, manual);
            }
            // Computer mode [2]
            else
            {   // The game sarts and ends 
                connectFour.PlayGame4(mode, boardConnectFour, manual);
            }
        }
    }

    abstract class Game
    {
        private string gameName;

        // Initialise the game
        public abstract void InitiateGame(Mode mode, Board board, Assistance manual);

        // SOS Play the game (Human)
        protected abstract void MakePlay1(Human whichone, Board board);

        // SOS Play the game (Computer)
        protected abstract void MakePlay2(Computer whichone, Board board);

        // Display the human winner 
        protected abstract void DisplayHumanWinner(Human whichone);

        // Display the computer winner 
        protected abstract void DisplayComputerWinner(Computer whichone);

        // Get the saved data
        public static void GetSave()
        {
            const string FILENAME = "save1.json";
            FileStream inFile = new FileStream(FILENAME, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(inFile);
            string tempData = reader.ReadLine();
            Board data = JsonSerializer.Deserialize<Board>(tempData);
            //Board board = new Board()
        }

        // Template Method for SOS (Human vs Human)
        public void PlayGame1(Mode mode, Board board, Assistance manual)
        {
            // Create two objects 
            Human human1 = new Human("Player1", 1, 1);
            Human human2 = new Human("Player2", 2, 1);

            // Shows the rule of the game and the board 
            InitiateGame(mode, board, manual);

            List<GamesModel.GameModel> gameList = GamesData.Games.ToList();
            gameList.Add(new GamesModel.GameModel
            {
                GameId = board.GameId,
                GameName = gameName,
                GameBoard = new GamesModel.BoardModel
                {
                    Rows = board.Rows,
                    Columns = board.Columns,
                    Field = board.field
                },
                GameMode = new GamesModel.ModeModel
                {
                    GameMode = mode.GameMode,
                    GameType = mode.GameType
                }
            });
            GamesData.Games = gameList.ToArray();
            List<GamesModel.PlayerModel> playerList = new(){
                new GamesModel.PlayerModel(){
                    GameId = human1.GameId,
                    IsHuman = true,
                    PlayerId = human1.PlayerId,
                    PlayerName = human1.PlayerName,
                    PlayerMoves = Array.Empty<GamesModel.PlayerMovesModel>()
                },
                new GamesModel.PlayerModel(){
                    GameId = human2.GameId,
                    IsHuman = true,
                    PlayerId = human2.PlayerId,
                    PlayerName = human2.PlayerName,
                    PlayerMoves = Array.Empty<GamesModel.PlayerMovesModel>()
                }
            };
            GamesData.Games.Last().Players = playerList.ToArray();

            // The game never ends unless all squares are used
            while (!board.CheckAllSquareUsed())
            {
                // Player1：Make a move & Validate a move & Display a move
                MakePlay1(human1, board);

                // Check if SOS is found
                while (board.FindPoints(human1.Row, human1.Column, ref human1.playerPoints))
                {
                    // Display the number of points the player gets
                    Console.WriteLine("{0} got {1} points so far", human1.PlayerName, human1.playerPoints);

                    // Check if the board is filled up
                    if (board.CheckAllSquareUsed()) { break; }

                    // Make a move & Validate a move & Display a move
                    MakePlay1(human1, board);
                }
                // Check if the board is filled up
                if (board.CheckAllSquareUsed()) { break; }

                // Player2：Make a move & Validate a move & Display a move
                MakePlay1(human2, board);

                // Check if SOS is found
                while (board.FindPoints(human2.Row, human2.Column, ref human2.playerPoints))
                {
                    // Display the number of points the player gets
                    Console.WriteLine("{0} got {1} points so far", human2.PlayerName, human2.playerPoints);

                    // Check if the board is filled up
                    if (board.CheckAllSquareUsed()) { break; }

                    // Make a move & Validate a move & Display a move
                    MakePlay1(human2, board);
                }
                // Check if the board is filled up
                if (board.CheckAllSquareUsed()) { break; }
            }
            // Get out of the loop and check the winner
            if (human1.playerPoints > human2.playerPoints)
            {
                DisplayHumanWinner(human1);
            }
            else if (human2.playerPoints > human1.playerPoints)
            {
                DisplayHumanWinner(human2);
            }
            else
            {
                Console.WriteLine("There is no winner. DRAW.");
            }
            Utils.SaveDataToJson(GamesData);
            // The end of the game
            Console.WriteLine("Thank you very much for playing!");
        }

        // Template Method for SOS (Human vs Computer)
        public void PlayGame2(Mode mode, Board board, Assistance manual)
        {
            // Create two objects 
            Computer computer = new Computer("Computer", 3, 1);
            Human human3 = new Human("You", 4, 1);

            // Shows the rule of the game and the board 
            InitiateGame(mode, board, manual);

            List<GamesModel.GameModel> gameList = GamesData.Games.ToList();
            gameList.Add(new GamesModel.GameModel
            {
                GameId = board.GameId,
                GameName = gameName,
                GameBoard = new GamesModel.BoardModel
                {
                    Rows = board.Rows,
                    Columns = board.Columns,
                    Field = board.field
                },
                GameMode = new GamesModel.ModeModel
                {
                    GameMode = mode.GameMode,
                    GameType = mode.GameType
                }
            });
            GamesData.Games = gameList.ToArray();
            List<GamesModel.PlayerModel> playerList = new(){
                new GamesModel.PlayerModel(){
                    GameId = computer.GameId,
                    IsHuman = false,
                    PlayerId = computer.PlayerId,
                    PlayerName = computer.PlayerName,
                    PlayerMoves = Array.Empty<GamesModel.PlayerMovesModel>()
                },
                new GamesModel.PlayerModel(){
                    GameId = human3.GameId,
                    IsHuman = true,
                    PlayerId = human3.PlayerId,
                    PlayerName = human3.PlayerName,
                    PlayerMoves = Array.Empty<GamesModel.PlayerMovesModel>()
                }
            };
            GamesData.Games.Last().Players = playerList.ToArray();

            // The game never ends unless all squares are used
            while (!board.CheckAllSquareUsed())
            {
                // Computer：Make a move & Validate a move & Display a move
                MakePlay2(computer, board);

                // Check if SOS is found
                while (board.FindPoints(computer.Row, computer.Column, ref computer.playerPoints))
                {
                    // Display the number of points the player gets
                    Console.WriteLine("{0} got {1} points so far", computer.PlayerName, computer.playerPoints);

                    // Check if the board is filled up
                    if (board.CheckAllSquareUsed()) { break; }

                    // Make a move & Validate a move & Display a move
                    MakePlay2(computer, board);
                }
                // Check if the board is filled up
                if (board.CheckAllSquareUsed()) { break; }

                // Human Player : Make a move & Validate a move & Display a move
                MakePlay1(human3, board);

                // Check if SOS is found
                while (board.FindPoints(human3.Row, human3.Column, ref human3.playerPoints))
                {
                    // Display the number of points the player gets
                    Console.WriteLine("{0} got {1} points so far", human3.PlayerName, human3.playerPoints);

                    // Check if the board is filled up
                    if (board.CheckAllSquareUsed()) { break; }

                    // Make a move & Validate a move & Display a move
                    MakePlay1(human3, board);
                }
                // Check if the board is filled up
                if (board.CheckAllSquareUsed()) { break; }
            }
            // Get out of the loop and check the winner
            if (computer.playerPoints > human3.playerPoints)
            {
                DisplayComputerWinner(computer);
            }
            else if (human3.playerPoints > computer.playerPoints)
            {
                DisplayHumanWinner(human3);
            }
            else
            {
                Console.WriteLine("There is no winner. DRAW.");
            }
            Utils.SaveDataToJson(GamesData);
            // The end of the game
            Console.WriteLine("Thank you very much for playing!");
        }

        // Template Method for Connect Four (Human vs Human)
        public void PlayGame3(Mode mode, Board boardConnectFour, Assistance manual)
        {
            // Create 2 objects for the two players
            Human human1 = new Human("Player1", 1, 2, 'X'); //🔴
            Human human2 = new Human("Player2", 2, 2, 'O'); //🟡

            // Create a object to define a current player
            Human currentPlayer = human1;

            char[][] charBoard;

            // Get the current row for the chosen column
            int[] rowCounter = new int[7] { 6, 6, 6, 6, 6, 6, 6 };
            int row;

            // Shows the rule of the game and the board 
            InitiateGame(mode, boardConnectFour, manual);

            List<GamesModel.GameModel> gameList = GamesData.Games.ToList();
            gameList.Add(new GamesModel.GameModel
            {
                GameId = boardConnectFour.GameId,
                GameName = gameName,
                GameBoard = new GamesModel.BoardModel
                {
                    Rows = boardConnectFour.Rows,
                    Columns = boardConnectFour.Columns,
                    Field = boardConnectFour.field
                },
                GameMode = new GamesModel.ModeModel
                {
                    GameMode = mode.GameMode,
                    GameType = mode.GameType
                }
            });
            GamesData.Games = gameList.ToArray();
            List<GamesModel.PlayerModel> playerList = new(){
                new GamesModel.PlayerModel(){
                    GameId = human1.GameId,
                    IsHuman = true,
                    PlayerId = human1.PlayerId,
                    PlayerName = human1.PlayerName,
                    PlayerMoves = Array.Empty<GamesModel.PlayerMovesModel>()
                },
                new GamesModel.PlayerModel(){
                    GameId = human2.GameId,
                    IsHuman = true,
                    PlayerId = human2.PlayerId,
                    PlayerName = human2.PlayerName,
                    PlayerMoves = Array.Empty<GamesModel.PlayerMovesModel>()
                }
            };
            GamesData.Games.Last().Players = playerList.ToArray();

            // The game never ends unless all squares are used
            while (!boardConnectFour.CheckAllSquareUsed())
            {
                // Player1 
                if (currentPlayer == human1)
                {
                    // Player1's turn
                    Console.WriteLine("Player1's turn");

                    // Make a move & Validate a move & Display a move
                    MakePlay1(currentPlayer, boardConnectFour);

                    // Get the current row for the chosen column
                    row = rowCounter[currentPlayer.Column - 1];

                    // Decrement the row by 1 if it's not already at the minimum (1)
                    if (row > 1)
                    {
                        rowCounter[currentPlayer.Column - 1]--;
                    }
                    // Display the move if it is found to be valid
                    boardConnectFour.DisplayMove(row, currentPlayer.Column, currentPlayer.ConnectFourMoveType);
                    charBoard = boardConnectFour.field;

                    // Check if four consecutive tokens are created 
                    if (boardConnectFour.CheckForWin(charBoard, boardConnectFour.Rows, boardConnectFour.Columns, human1.ConnectFourMoveType))
                    {
                        // Display the winner
                        DisplayHumanWinner(human1);
                        break;
                    }

                    // Flip the turn 
                    currentPlayer = human2;
                }
                // Player 2
                else
                {
                    // Player2's turn
                    Console.WriteLine("Player2's turn");

                    // Make a move & Validate a move & Display a move
                    MakePlay1(currentPlayer, boardConnectFour);

                    // Get the current row for the chosen column
                    row = rowCounter[currentPlayer.Column - 1];

                    // Decrement the row by 1 if it's not already at the minimum (1)
                    if (row > 1)
                    {
                        rowCounter[currentPlayer.Column - 1]--;
                    }

                    // Display the move if it is found to be valid
                    boardConnectFour.DisplayMove(row, currentPlayer.Column, currentPlayer.ConnectFourMoveType);
                    charBoard = boardConnectFour.field;

                    // Check if four consecutive tokens are created 
                    if (boardConnectFour.CheckForWin(charBoard, boardConnectFour.Rows, boardConnectFour.Columns, human1.ConnectFourMoveType))
                    {
                        // Display the winner
                        DisplayHumanWinner(human1);
                        break;
                    }
                    // Flip the turn 
                    currentPlayer = human1;
                }
            }
            // Dispay the winner to end the game
            if (boardConnectFour.CheckAllSquareUsed())
            {
                Console.WriteLine("There is no winner. DRAW.");
            }
            Utils.SaveDataToJson(GamesData);
            // The end of the game
            Console.WriteLine("Thank you very much for playing!");
        }

        public void PlayGame4(Mode mode, Board boardConnectFour, Assistance manual)
        {
            // Create 2 objects for the two players
            Computer computer = new Computer("Computer", 3, 2, 'X');//🔴
            Human human3 = new Human("You", 4, 2, 'O');//🟡

            // Create a object to define a current player
            Player currentPlayer = computer;

            // Get the current row for the chosen column
            int[] rowCounter = new int[7] { 6, 6, 6, 6, 6, 6, 6 };

            int row;
            char[][] charBoard;

            // Shows the rule of the game and the board 
            InitiateGame(mode, boardConnectFour, manual);

            List<GamesModel.GameModel> gameList = GamesData.Games.ToList();
            gameList.Add(new GamesModel.GameModel
            {
                GameId = boardConnectFour.GameId,
                GameName = gameName,
                GameBoard = new GamesModel.BoardModel
                {
                    Rows = boardConnectFour.Rows,
                    Columns = boardConnectFour.Columns,
                    Field = boardConnectFour.field
                },
                GameMode = new GamesModel.ModeModel
                {
                    GameMode = mode.GameMode,
                    GameType = mode.GameType
                }
            });
            GamesData.Games = gameList.ToArray();
            List<GamesModel.PlayerModel> playerList = new(){
                new GamesModel.PlayerModel(){
                    GameId = computer.GameId,
                    IsHuman = false,
                    PlayerId = computer.PlayerId,
                    PlayerName = computer.PlayerName,
                    PlayerMoves = Array.Empty<GamesModel.PlayerMovesModel>()
                },
                new GamesModel.PlayerModel(){
                    GameId = human3.GameId,
                    IsHuman = true,
                    PlayerId = human3.PlayerId,
                    PlayerName = human3.PlayerName,
                    PlayerMoves = Array.Empty<GamesModel.PlayerMovesModel>()
                }
            };
            GamesData.Games.Last().Players = playerList.ToArray();

            // The game never ends unless all squares are used
            while (!boardConnectFour.CheckAllSquareUsed())
            {
                // Computer
                if (currentPlayer == computer)
                {
                    // Computer's turn
                    Console.WriteLine("Computer's turn");

                    // Make a move & Validate a move & Display a move
                    MakePlay2(computer, boardConnectFour);

                    int column = computer.Column;
                    // Check if the column is full
                    if (!boardConnectFour.ValidateMoveConnectFour(currentPlayer.Column))
                    {
                        Console.WriteLine("Computer chose a full column, please choose another column!");
                        computer.MakeMoveByComputerConnectFour();
                    }

                    // Get the current row for the chosen column
                    row = rowCounter[column - 1];

                    // Decrement the row by 1 if it's not already at the minimum (1)
                    if (row > 1)
                    {
                        rowCounter[column - 1]--; // Decrement the value in columnRows
                    }

                    // Display the move if it is found to be valid
                    boardConnectFour.DisplayMove(row, computer.Column, computer.ConnectFourMoveType);
                    charBoard = boardConnectFour.field;

                    // Check if four consecutive tokens are created 
                    if (boardConnectFour.CheckForWin(charBoard, boardConnectFour.Rows, boardConnectFour.Columns, currentPlayer.ConnectFourMoveType))
                    {
                        // Display the winner
                        DisplayComputerWinner(computer);
                        break;
                    }
                    // Flip the turn 
                    currentPlayer = human3;
                }
                // Player1
                else
                {
                    // Player1's turn
                    Console.WriteLine("Your turn");

                    // Make a move & Validate a move & Display a move
                    MakePlay1(human3, boardConnectFour);

                    // Get the current row for the chosen column
                    row = rowCounter[human3.Column - 1];

                    // Decrement the row by 1 if it's not already at the minimum (1)
                    if (row > 1)
                    {
                        rowCounter[human3.Column - 1]--;
                    }

                    // Display the move if it is found to be valid
                    boardConnectFour.DisplayMove(row, human3.Column, human3.ConnectFourMoveType);
                    charBoard = boardConnectFour.field;

                    // Check if four consecutive tokens are created 
                    if (boardConnectFour.CheckForWin(charBoard, boardConnectFour.Rows, boardConnectFour.Columns, human3.ConnectFourMoveType))
                    {
                        // Display the winner
                        DisplayHumanWinner(human3);
                        break;
                    }
                    // Flip the turn 
                    currentPlayer = computer;
                }
            }
            // Dispay the winner to end the game
            if (boardConnectFour.CheckAllSquareUsed())
            {
                Console.WriteLine("There is no winner. DRAW.");
            }
            Utils.SaveDataToJson(GamesData);
            // The end of the game
            Console.WriteLine("Thank you very much for playing!");
        }
    }

    class SOS : Game  // Inherit from Game Class
    {
        private string gameName = "SOS";

        public override void InitiateGame(Mode mode, Board board, Assistance manual)
        {
            // Shows the rule of the game
            Console.WriteLine("You chose {0}. The player who gets more SOS is the winner!", gameName);

            // Explain the meaning of row and column
            manual.ShowManual();

            // Display the board 
            board.DisplayBoard();
        }

        // Make a move & Validate a move & Display a move (Human)
        protected override void MakePlay1(Human whichone, Board board)
        {
            // Make a move 
            whichone.MakeMoveSOS();

            // Validate a move
            while (!board.ValidateMove(whichone.Row, whichone.Column))
            {
                Console.WriteLine("Your move is invalid, please make another move!");
                // Make a move if it is not valid
                whichone.MakeMoveSOS();
            }
            // Display a move
            board.DisplayMove(whichone.Row, whichone.Column, whichone.MoveType);
        }

        // make a move & Validate a move & Display a move (Computer)
        protected override void MakePlay2(Computer whichone, Board board)
        {
            // Make a move 
            whichone.MakeMoveByComputerSOS();

            // Validate a move
            while (!board.ValidateMove(whichone.Row, whichone.Column))
            {
                Console.WriteLine("Your move is invalid, please make another move!");
                // Make a move if it is not valid
                whichone.MakeMoveByComputerSOS();
            }
            // Display a move
            board.DisplayMove(whichone.Row, whichone.Column, whichone.MoveType);
        }

        // Display the winner (Human)
        protected override void DisplayHumanWinner(Human whichone)
        {
            Console.WriteLine("{0} is over! {1} win! Congratulations!", gameName, whichone.PlayerName);
        }
        // Display the winner (Computer)
        protected override void DisplayComputerWinner(Computer whichone)
        {
            Console.WriteLine("{0} is over! {1} win! Congratulations!", gameName, whichone.PlayerName);
        }
    }

    class ConnectFour : Game // Inherit from Game Class
    {
        private string gameName = "Connect Four";

        public override void InitiateGame(Mode mode, Board board, Assistance manual)
        {
            // Shows the rule of the game
            Console.WriteLine("You chose {0}. The player who gets four consecutive O or X is the winner!", gameName);

            // Validate a move
            manual.ShowManual();

            // Display the board
            board.DisplayBoard();
        }

        // Make a move & Validate a move & Display a move (Human)
        protected override void MakePlay1(Human currentPlayer, Board boardConnectFour)
        {
            // Make a move 
            currentPlayer.ChooseColumnConnectFour();

            // Validate a move
            if (!boardConnectFour.ValidateMoveConnectFour(currentPlayer.Column))
            {
                Console.WriteLine("Invalid column. Please choose a valid column.");

                // Make a move if it is not valid
                currentPlayer.ChooseColumnConnectFour();
            }
        }

        // Make a move & Validate a move & Display a move (Human)
        protected override void MakePlay2(Computer computerPlayer, Board boardConnectFour)
        {
            // Make a move 
            computerPlayer.MakeMoveByComputerConnectFour();

            // Validate a move
            if (!boardConnectFour.ValidateMoveConnectFour(computerPlayer.Column))
            {
                Console.WriteLine("Computer chose a full column, please choose another column!");

                // Make a move if it is not valid
                computerPlayer.MakeMoveByComputerConnectFour();
            }

        }

        // Display the winner (Human)
        protected override void DisplayHumanWinner(Human whichOne)
        {
            Console.WriteLine("{0} is over! {1} win! Congratulations!", gameName, whichOne.PlayerName);
        }

        // Display the winner (Computer)
        protected override void DisplayComputerWinner(Computer whichone)
        {
            Console.WriteLine("{0} is over! {1} win! Congratulations!", gameName, whichone.PlayerName);
        }
    }

    abstract class Player
    {
        // Fields
        public string PlayerName { get; set; }
        public int PlayerId { get; set; }
        public int GameId { get; set; }
        public char MoveType { get; set; }
        public int TentativeMoveType { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int playerPoints = 0;
        public char ConnectFourMoveType { get; set; }

        // Choose a move type 
        protected abstract char ChooseMove();
        public abstract int ChooseColumnConnectFour();
    }

    class Human : Player // Inherit from Player class 
    {
        // Overloading Constructor (for SOS)
        public Human(string PlayerName, int PlayerId, int GameId)
        {
            this.PlayerName = PlayerName;
            this.PlayerId = PlayerId;
            this.GameId = GameId;
        }

        // Overloading Constructor (for Connect Four)
        public Human(string PlayerName, int PlayerId, int GameId, char ConnectFourMoveType)
        {
            this.PlayerName = PlayerName;
            this.PlayerId = PlayerId;
            this.GameId = GameId;
            this.ConnectFourMoveType = ConnectFourMoveType;
        }

        // Choose S or O 
        protected override char ChooseMove()
        {
            Console.WriteLine("------------------------------------------------------------------------------------------------");
            Console.WriteLine("{0}", PlayerName);
            Console.Write("Please choose S or O >>");

            // Prompt a userinput and turn it into an uppercase if necessary
            MoveType = Convert.ToChar(Console.ReadLine().ToUpper());

            // Continue to re-prompt the player if the number is invalid
            while (MoveType != 'S' && MoveType != 'O')
            {
                Console.Write("That is invalid. Please choose S or O >>");
                MoveType = Convert.ToChar(Console.ReadLine());
            }
            return MoveType;
        }

        // Choose row and column
        public void MakeMoveSOS()
        {
            {
                GamesModel.PlayerModel currentPlayer = Utils.GetPlayerByPlayerIdOfCurrentGame(PlayerId);
                GamesModel.PlayerMovesModel move = new();

                // Display the move type that the player chose
                Console.WriteLine("You chose {0}", ChooseMove());

                // Ask row 
                Console.Write("Please enter a row number 1, 2, or 3 >>");
                Row = Convert.ToInt32(Console.ReadLine());

                // Continue to re-prompt the player if the number is invalid
                while (Row < 1 || Row > 3)
                {
                    Console.Write("The number is invalid. Please enter a row number 1, 2, or 3 >>");
                    Row = Convert.ToInt32(Console.ReadLine());
                }

                //move.row = Row;

                // Ask column 
                Console.Write("Please enter a column number 1, 2, or 3 >>");
                Column = Convert.ToInt32(Console.ReadLine());

                // Continue to re-prompt the player if the number is invalid
                while (Column < 1 || Column > 3)
                {
                    Console.Write("The number is invalid. Please enter a column number 1, 2, or 3 >>");
                    Column = Convert.ToInt32(Console.ReadLine());
                }
                move.Row = Row;
                move.Column = Column;
                int currentPlayerArrayIndex = GamesData.Games.Last().Players.Select((playerObj, idx) => new { playerObj, idx }).First(playerObj => playerObj.playerObj.PlayerId == currentPlayer.PlayerId).idx;
                GamesData.Games.Last().Players[currentPlayerArrayIndex] = currentPlayer;
                List<GamesModel.PlayerMovesModel> playerMoves = GamesData.Games.Last().Players[currentPlayerArrayIndex].PlayerMoves.ToList();
                playerMoves.Add(move);
                GamesData.Games.Last().Players[currentPlayerArrayIndex].PlayerMoves = playerMoves.ToArray();
            }
        }

        // Initialize all columns with 6
        private int[] columnRows = new int[7] { 6, 6, 6, 6, 6, 6, 6 };

        public override int ChooseColumnConnectFour()
        {
            GamesModel.PlayerModel currentPlayer = Utils.GetPlayerByPlayerIdOfCurrentGame(PlayerId);
            GamesModel.PlayerMovesModel move = new();

            // Ask column 
            Console.Write("Please choose a column between 1 and 7 >>");
            Column = Convert.ToInt32(Console.ReadLine());

            // Re-prompt the player if the number is invalid
            while (Column < 1 || Column > 7)
            {
                Console.Write("The number is invalid. Please choose a column between 1 and 7 >>");
                Column = Convert.ToInt32(Console.ReadLine());
            }

            // Get the row for the chosen column
            int row = columnRows[Column - 1];

            // Check if the column is already full
            if (row < 1)
            {
                Console.WriteLine("Column is already full. Please choose another column.");
                ChooseColumnConnectFour(); // Reprompt for input
            }

            // Decrement the row by 1
            columnRows[Column - 1]--;

            move.Row = Row;
            move.Column = Column;
            int currentPlayerArrayIndex = GamesData.Games.Last().Players.Select((playerObj, idx) => new { playerObj, idx }).First(playerObj => playerObj.playerObj.PlayerId == currentPlayer.PlayerId).idx;
            GamesData.Games.Last().Players[currentPlayerArrayIndex] = currentPlayer;
            List<GamesModel.PlayerMovesModel> playerMoves = GamesData.Games.Last().Players[currentPlayerArrayIndex].PlayerMoves.ToList();
            playerMoves.Add(move);
            GamesData.Games.Last().Players[currentPlayerArrayIndex].PlayerMoves = playerMoves.ToArray();

            return Column;
        }
    }

    class Computer : Player // Inherit from Player class 
    {
        // Overloading Constructor 
        public Computer(string PlayerName, int PlayerId, int GameId)
        {
            this.PlayerName = PlayerName;
            this.PlayerId = PlayerId;
            this.GameId = GameId;
        }

        public Computer(string PlayerName, int PlayerId, int GameId, char ConnectFourMoveType)
        {
            this.PlayerName = PlayerName;
            this.PlayerId = PlayerId;
            this.GameId = GameId;
            this.ConnectFourMoveType = ConnectFourMoveType;
        }

        // Choose S or O 
        protected override char ChooseMove()
        {
            int min = 1;
            int max = 2;
            // Create an object to generate some random numbers
            Random random = new Random();
            Console.WriteLine("------------------------------------------------------------------------------------------------");
            Console.WriteLine("{0}", PlayerName);
            TentativeMoveType = random.Next(min, max + 1);
            if (TentativeMoveType == 1)
            {
                MoveType = 'S';
                return MoveType;
            }
            else
            {
                MoveType = 'O';
                return MoveType;
            }
        }

        // Choose row 
        public int ChooseRowByComputerSOS()
        {
            int min = 1;
            int max = 3;
            // Create an object to generate some random numbers
            Random random = new Random();
            return Row = random.Next(min, max + 1);
        }

        // Choose column
        public int ChooseColumnByComputerSOS()
        {
            int min = 1;
            int max = 3;
            // Create an object to generate some random numbers
            Random random = new Random();
            return Column = random.Next(min, max + 1);
        }

        // Display move type, row, and column
        public void MakeMoveByComputerSOS()
        {
            GamesModel.PlayerModel currentPlayer = Utils.GetPlayerByPlayerIdOfCurrentGame(PlayerId);
            GamesModel.PlayerMovesModel move = new();
            move.MoveType = ChooseMove();
            move.Row = ChooseRowByComputerSOS();
            move.Column = ChooseColumnByComputerSOS();

            Console.WriteLine("The computer chose {0}", Convert.ToChar(move.MoveType));
            Console.WriteLine("The computer chose {0} for a row", move.Row);
            Console.WriteLine("The computer chose {0} for a column", move.Column);

            int currentPlayerArrayIndex = GamesData.Games.Last().Players.Select((playerObj, idx) => new { playerObj, idx }).First(playerObj => playerObj.playerObj.PlayerId == currentPlayer.PlayerId).idx;
            GamesData.Games.Last().Players[currentPlayerArrayIndex] = currentPlayer;
            List<GamesModel.PlayerMovesModel> playerMoves = GamesData.Games.Last().Players[currentPlayerArrayIndex].PlayerMoves.ToList();
            playerMoves.Add(move);
            GamesData.Games.Last().Players[currentPlayerArrayIndex].PlayerMoves = playerMoves.ToArray();
        }

        // Choose column
        public override int ChooseColumnConnectFour()
        {
            int min = 1;
            int max = 7;
            // Create an object to generate some random numbers
            Random random = new Random();
            Column = random.Next(min, max + 1);
            return Column;
        }

        // Display column
        public void MakeMoveByComputerConnectFour()
        {
            GamesModel.PlayerModel currentPlayer = Utils.GetPlayerByPlayerIdOfCurrentGame(PlayerId);
            GamesModel.PlayerMovesModel move = new();
            move.Column = ChooseColumnConnectFour();

            Console.WriteLine("The computer chose {0} for a column", move.Column);

            int currentPlayerArrayIndex = GamesData.Games.Last().Players.Select((playerObj, idx) => new { playerObj, idx }).First(playerObj => playerObj.playerObj.PlayerId == currentPlayer.PlayerId).idx;
            GamesData.Games.Last().Players[currentPlayerArrayIndex] = currentPlayer;
            List<GamesModel.PlayerMovesModel> playerMoves = GamesData.Games.Last().Players[currentPlayerArrayIndex].PlayerMoves.ToList();
            playerMoves.Add(move);
            GamesData.Games.Last().Players[currentPlayerArrayIndex].PlayerMoves = playerMoves.ToArray();
        }
    }

    class Board
    {
        // Fields 
        private int rows;
        private int columns;
        private int gameId;
        public char[][] field;

        // Properties 
        public int Rows
        {
            get { return rows; }
            set { rows = value; }
        } 
        public int Columns
        {
            get { return columns; }
            set { columns = value; }
        }
        public int GameId
        {
            get { return gameId; }
            set { gameId = value; }
        }

        // Constructor 
        public Board(int Rows, int Columns, int GameId)
        {
            this.Rows = Rows;
            this.Columns = Columns;
            this.GameId = GameId;
            this.field = new char[Rows][];  // Initialize field array
            for (int i = 0; i < Rows; i++)
            {
                 // Initialize inner arrays
                field[i] = new char[Columns]; 
                for (int j = 0; j < Columns; j++)
                {
                    // Set initial value for each cell
                    field[i][j] = ' ';  
                }
            }
        }

        // Display the board 
        public void DisplayBoard()
        {
            for (int i = 0; i < Rows; ++i)
            {
                for (int j = 0; j < Columns; j++)
                {
                    // Draw top part of cell
                    Console.Write("┌───┐");  
                }
                Console.WriteLine();

                for (int j = 0; j < Columns; j++)
                {
                    // Draw content of cell
                    Console.Write("│ " + field[i][j] + " │");  
                }
                Console.WriteLine();

                for (int j = 0; j < Columns; j++)
                {
                    // Draw bottom part of cell
                    Console.Write("└───┘");  
                }
                Console.WriteLine();
            }
        }

        // Validate moves for SOS
        public bool ValidateMove(int row, int col)
        {
            bool valid = false;
            if (field[row - 1][col - 1] == ' ')
            {
                valid = true;
            }
            return valid;
        }

        // Validate moves for ConnectFour
        public bool ValidateMoveConnectFour(int col)
        {
            if (col < 1 || col > 7)
            {
                // Invalid column
                return false; 
            }
            // Valid move
            return true; 
        }

        // Check of all squares are used
        public bool CheckAllSquareUsed()
        {
            bool flag = false;
            int count = 0;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (field[i][j] != ' ')
                    {
                        count++;
                    }

                    if (count == Rows * Columns)
                    {
                        flag = true;
                    }
                }
            }
            return flag;
        }

        // Display moves
        public void DisplayMove(int row, int col, char moveType)
        {
            field[row - 1][col - 1] = moveType;

            //Display board renewed
            for (int i = 0; i < Rows; ++i)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Console.Write("┌───┐");
                }
                Console.WriteLine();

                for (int j = 0; j < Columns; j++)
                {
                    Console.Write("│ " + field[i][j] + " │");
                }
                Console.WriteLine();

                for (int j = 0; j < Columns; j++)
                {
                    Console.Write("└───┘");
                }
                Console.WriteLine();
            }
        }

        // Find if SOS is newly created 
        public bool FindPoints(int row, int col, ref int playerPoints)
        {
            bool found = false;
            // Check each Rows
            if (col == 1)
            {
                if (field[row - 1][col - 1] == 'S' && field[row - 1][col] == 'O' && field[row - 1][col + 1] == 'S')
                {
                    playerPoints++;
                    found = true;
                }
            }
            else if (col == 2)
            {
                if (field[row - 1][col - 2] == 'S' && field[row - 1][col - 1] == 'O' && field[row - 1][col] == 'S')
                {
                    playerPoints++;
                    found = true;

                }
            }
            else if (col == 3)
            {
                if (field[row - 1][col - 3] == 'S' && field[row - 1][col - 2] == 'O' && field[row - 1][col - 1] == 'S')
                {
                    playerPoints++;
                    found = true;

                }
            }
            // Check each Columns
            if (row == 1)
            {
                if (field[row - 1][col - 1] == 'S' && field[row][col - 1] == 'O' && field[row + 1][col - 1] == 'S')
                {
                    playerPoints++;
                    found = true;
                }
            }

            else if (row == 2)
            {
                if (field[row - 2][col - 1] == 'S' && field[row - 1][col - 1] == 'O' && field[row][col - 1] == 'S')
                {
                    playerPoints++;
                    found = true;
                }
            }
            else if (row == 3)
            {
                if (field[row - 3][col - 1] == 'S' && field[row - 2][col - 1] == 'O' && field[row - 1][col - 1] == 'S')
                {
                    playerPoints++;
                    found = true;
                }

            }
            // Check diagonals (both directions)
            if (row == 1 && col == 1)
            {
                if (field[row - 1][col - 1] == 'S' && field[row][col] == 'O' && field[row + 1][col + 1] == 'S')
                {
                    playerPoints++;
                    found = true;
                }
            }
            else if (row == 2 && col == 2)
            {
                if (field[row - 2][col - 2] == 'S' && field[row - 1][col - 1] == 'O' && field[row][col] == 'S')
                {
                    playerPoints++;
                    found = true;
                }
            }
            else if (row == 3 && col == 3)
            {
                if (field[row - 3][col - 3] == 'S' && field[row - 2][col - 2] == 'O' && field[row - 1][col - 1] == 'S')
                {
                    playerPoints++;
                    found = true;
                }
            }
            else if (row == 1 && col == 3)
            {
                if (field[row - 1][col - 1] == 'S' && field[row][col - 2] == 'O' && field[row + 1][col - 3] == 'S')
                {
                    playerPoints++;
                    found = true;
                }
            }
            else if (row == 2 && col == 2)
            {
                if (field[row - 2][col] == 'S' && field[row - 1][col - 1] == 'O' && field[row][col - 2] == 'S')
                {
                    playerPoints++;
                    found = true;
                }
            }
            else if (row == 3 && col == 1)
            {
                if (field[row - 3][col + 1] == 'S' && field[row - 2][col] == 'O' && field[row - 1][col - 1] == 'S')
                {
                    playerPoints++;
                    found = true;
                }
            }
            return found;
        }

        // Check if four O or X is created 
        public bool CheckForWin(char[][] field, int rows, int cols, char playerPiece)
        {
            // Check for horizontal wins
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col <= cols - 4; col++)
                {
                    if (field[row][col] == playerPiece &&
                        field[row][col + 1] == playerPiece &&
                        field[row][col + 2] == playerPiece &&
                        field[row][col + 3] == playerPiece)
                    {
                        return true;
                    }
                }
            }

            // Check for vertical wins
            for (int row = 0; row <= rows - 4; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (field[row][col] == playerPiece &&
                        field[row + 1][col] == playerPiece &&
                        field[row + 2][col] == playerPiece &&
                        field[row + 3][col] == playerPiece)
                    {
                        return true;
                    }
                }
            }

            // Check for diagonal wins (positive slope)
            for (int row = 0; row <= rows - 4; row++)
            {
                for (int col = 0; col <= cols - 4; col++)
                {
                    if (field[row][col] == playerPiece &&
                        field[row + 1][col + 1] == playerPiece &&
                        field[row + 2][col + 2] == playerPiece &&
                        field[row + 3][col + 3] == playerPiece)
                    {
                        return true;
                    }
                }
            }

            // Check for diagonal wins (negative slope)
            for (int row = 0; row <= rows - 4; row++)
            {
                for (int col = 3; col < cols; col++)
                {
                    if (field[row][col] == playerPiece &&
                        field[row + 1][col - 1] == playerPiece &&
                        field[row + 2][col - 2] == playerPiece &&
                        field[row + 3][col - 3] == playerPiece)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    class Mode
    {
        public int GameType { get; set; }
        public int GameMode { get; set; }

        // Choose a game between SOS and Connect four
        public int ChooseGame()
        {
            Console.Write("You can play two games SOS or Connect four. Please enter 1 for SOS game and 2 for connect four >>");
            GameType = Convert.ToInt32(Console.ReadLine());

            // Re-prompt the player if the number is invalid 
            if (GameType < 1 || GameType > 2)
            {
                Console.Write("You can play two games SOS or Connect four. Please enter 1 for SOS game and 2 for connect four >>");
                GameType = Convert.ToInt32(Console.ReadLine());
            }
            return GameType;
        }

        // Choose a mode between human and computer
        public int ChooseMode()
        {
            Console.Write("You can choose who you will play with. Please enter 1 for your friend and 2 for computer>>");
            GameMode = Convert.ToInt32(Console.ReadLine());

            // Re-prompt the player if the number is invalid 
            if (GameMode < 1 || GameMode > 2)
            {
                Console.Write("You can choose who you will play with. Please enter 1 for your friend and 2 for computer >>");
                GameMode = Convert.ToInt32(Console.ReadLine());
            }
            return GameMode;
        }
    }

    class Assistance
    {
        // Show the rule of the game
        public void ShowManual()
        {
            Console.WriteLine("");
            Console.WriteLine("Row means a horizontal line (--) while column means a vertical line(|)");
            Console.WriteLine("");
        }
    }

    static class Utils
    {
        public static GamesModel GetHistoryDataFromJsonFile(string jsonFile = @"gamesData.json")
        {
            string jsonString = File.ReadAllText(jsonFile);
            GamesModel gamesModel = JsonSerializer.Deserialize<GamesModel>(jsonString);
            return gamesModel;
        }
        public static void SaveDataToJson(GamesModel gamesModel, string jsonFile = @"gamesData.json")
        {
            string gameData = JsonSerializer.Serialize<GamesModel>(gamesModel);

            File.WriteAllText(jsonFile, gameData); //',' + playerData);
        }
        public static GamesModel.GameModel GetGameByGameId(int gameId)
        {
            return GamesData.Games.First(game => game.GameId == gameId);
        }
        public static GamesModel.PlayerModel GetPlayerByPlayerIdOfCurrentGame(int playerId)
        {
            GamesModel.GameModel currentGame = GamesData.Games.Last();
            return currentGame.Players.First(player => player.PlayerId == playerId);
        }
        public static GamesModel.PlayerModel[] GetAllPlayersOfCurrentGameToDertermineWinner()
        {
            GamesModel.GameModel currentGame = GamesData.Games.Last();
            return currentGame.Players;
        }
    }
}