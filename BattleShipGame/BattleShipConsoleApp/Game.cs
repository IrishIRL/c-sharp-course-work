using System;
using System.IO;
using System.Text.Json;
using BattleShipBrain;
using BattleShipConsoleUI;

namespace BattleShipConsoleApp
{
    public class Game
    {
        private GameConfig _conf = default!;
        private BsBrain _brain = default!;

        public string PlayGame()
        {
            _conf = new GameConfig();
            //Stack GameSaveState = new Stack();
            
            int closeCondition;
            int loadSavedGame = LoadSavedGame();

            if (loadSavedGame != 0)
            {
                LoadGameConfig();
                _brain = new BsBrain(_conf);
                if (RandomGame() == 1)
                {
                    _brain = _brain.RandomShipPlacement(_conf);
                }
                else
                {
                    _brain = MainUI.PlaceShipUi(_conf); 
                }
            }
            else
            {
                // In case we start game with the ready board, then we do not use config at the build stage.
                // That means all the data there is still old. Since I use _conf.BoardSizes in other places,
                // I decided to fix boardSizes here.
                _conf.BoardSizeX = _brain.GameBoards[_brain.CurrentPlayerNo].Board.GetLength(0);
                _conf.BoardSizeY = _brain.GameBoards[_brain.CurrentPlayerNo].Board.GetLength(1);
            }
            
            Console.Clear();
            do
            {
                closeCondition = ChoosePlaceToPlantBomb();
                
                // This part will ask if the user wants to redo their turn,
                // but from my point of view, there is no point to undo the turn in the competitive game,
                // so I will leave this code commented.
                 
                /*
                _brain.GameSaveState(GameSaveState);
                
                bool popState = GameSaveStateBack();
                if (popState)
                {
                    GameSaveState.Pop();
                    var dto = JsonSerializer.Deserialize<SaveGameDto>(GameSaveState.Peek()!.ToString());
                    var gameBoardArray = dto!.GetGameBoard();
                    _brain._gameBoards[0] = gameBoardArray[0];
                    _brain._gameBoards[1] = gameBoardArray[1];
                    _brain._currentPlayerNo = dto.CurrentPlayerNo;
                }
                */

                _brain.CurrentPlayerNo = _brain.OneToZero(_brain.CurrentPlayerNo);
            } while (closeCondition == 0);

            _brain.CurrentPlayerNo = _brain.OneToZero(_brain.CurrentPlayerNo);
            
            string returnString = MainUI.GameClosure(_brain, closeCondition);
            return returnString;
        }
        
        // Load Game Config (We check that the config exists and if exists, we update current config to the loaded config).
        // We always grab config from local .json file, which could be set up from the Config Options.
        public void LoadGameConfig()
        {
            string confFile = MainUI.LoadLocalConfigUi();
            var confText = File.ReadAllText(confFile);
            _conf = JsonSerializer.Deserialize<GameConfig>(confText)!;
        }

        public int RandomGame()
        {
            Console.WriteLine("Do you want to make a random game? (y/N)");
            var yesNo = Console.ReadKey(true);
            if (yesNo.Key.ToString().ToLower() == "y")
            {
                return 1;
            }
            return 0;
        }
        public bool GameSaveStateBack()
        {
            Console.WriteLine("Do you want to revert a turn? (y/N)");
            var yesNo = Console.ReadKey(true);
            if (yesNo.Key.ToString().ToLower() == "y")
            {
                return true;
            }

            return false;
        }
        // Load Saved Game
        public int LoadSavedGame()
        {
            // returnResult will decide if we need to make a new board with new ships.
            // If it is not eq to 0, then we should.
            // If it is eq to 0, no new board should be made.
            int returnResult = 1;
            Console.WriteLine("Do you want to load saved game? (y/N)");
            var yesNo = Console.ReadKey(true);
            if (yesNo.Key.ToString().ToLower() == "y")
            {
                _brain = new BsBrain(_conf);
                returnResult = MainUI.LoadSavedGameUi(_brain);
            }

            return returnResult;
        }
        
        // Function that asks player where to place the bomb.
        public int ChoosePlaceToPlantBomb()
        {
            bool hitCondition;
            do
            {
                // Check for win conditions start
                var winCondition = _brain.GameWinCondition();
                if (winCondition) return 1;
                // Check for win conditions end
                
                // DECLARATIONS START
                // correctLocation is used to check if the place is not already bombed. If it is, then ask again.
                bool incorrectLocation = false;
                // xyLocationConverted is used to save X and Y coordinates for the bomb in the array list.
                int[] xyLocationConverted = new int[2];
                // DECLARATIONS END
                
                Console.Clear();
                
                /*if (hitCondition)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("You shot the ship! Choose the next place to shoot.");
                    Console.ForegroundColor = ConsoleColor.White;
                }*/
                
                // VISUALIZER START //
                Console.WriteLine("Current Player: {0}", _brain.CurrentPlayerNo + 1);
                Console.WriteLine("\n");
                for (int i = 0; i < _conf.BoardSizeX*5+10; i++)
                {
                    Console.Write("="); 
                }
                Console.WriteLine("\nYour board");
                for (int i = 0; i < _conf.BoardSizeX*5+10; i++)
                {
                    Console.Write("="); 
                }
                Console.WriteLine("\n");
                BSConsoleUI.DrawBoard(false, _brain.GetUserBoard());
                Console.WriteLine("\n");
                for (int i = 0; i < _conf.BoardSizeX*5+10; i++)
                {
                    Console.Write("="); 
                }
                
                Console.WriteLine("\nYour enemies board");
                for (int i = 0; i < _conf.BoardSizeX*5+10; i++)
                {
                    Console.Write("="); 
                }
                Console.WriteLine("\n");
                BSConsoleUI.DrawBoard(true, _brain.GetEnemyBoard());
                Console.WriteLine("\n");
                for (int i = 0; i < _conf.BoardSizeX*5+10; i++)
                {
                    Console.Write("="); 
                }
                Console.WriteLine("\nWrite ee to exit the game. Write ss to exit and save the game."); 
                // VISUALIZER END //
                
                do
                {
                    // incorrectLocation returns true if there has been bomb shot already.
                    if(incorrectLocation) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Please input place that you did not bomb yet!"); Console.ForegroundColor = ConsoleColor.White; }
                    xyLocationConverted = BombLocationXy(0);
                    incorrectLocation = _brain.CheckForBomb(xyLocationConverted[0], xyLocationConverted[1]);
                } while (incorrectLocation);

                for (int i = 0; i < 2; i++)
                {
                    if (xyLocationConverted[i] == -1) return -1;
                    if (xyLocationConverted[i] == -2) return -2;
                }

                hitCondition = _brain.PlaceBomb(xyLocationConverted);
                if (hitCondition)
                {
                    bool isShipSunk = _brain.IsShipSunk(xyLocationConverted);

                    if (isShipSunk)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You fully shot the ship! It is now sunk!\nClick enter to continue.");
                        Console.ReadLine();
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("You hit the ship! Search for nearby coordinates! Left ship size is: {0}.\nClick enter to continue.", _brain.LeftShipSize(xyLocationConverted));
                        Console.ReadLine();
                        Console.ForegroundColor = ConsoleColor.White;   
                    }
                }
            } while (hitCondition);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You missed! Please give the second user control. Click enter after that.");
            Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            
            return 0;
        }
        public int[] BombLocationXy(int i)
        {
            char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            int[] xy = new int[2];
            string[] location = {"x", "y"};
            int[] maxXy = {_conf.BoardSizeX, _conf.BoardSizeY};

            for (; i < 2; i++)
            {
                Console.WriteLine("Input bomb location on {0} coordinate:", location[i]);
                var x = Console.ReadLine()?.Trim();

                // Since I am using int[], I have decided to transform exit and save to -1 and -2.
                // From my point of view, there is no problem with that. Even though player may manually exit the game inputting -1 and -2, not letters 'ee' or 'ss'.  
                if (x?.ToLower() == "ee")
                {
                    xy[i] = -1;
                    break;
                }

                if (x?.ToLower() == "ss")
                {
                    xy[i] = -2;
                    break;
                }

                var success = int.TryParse(x, out var element);

                if (i == 0)
                {
                    for (int loop = 0; loop < maxXy[i]; loop++)
                    {
                        if (x?.ToUpper() == letters[loop].ToString().ToUpper())
                        {
                            element = loop;
                            success = true;
                            break;
                        }
                    }   
                }

                if (!success)
                {
                    Console.WriteLine("Please input correct number!");
                    i--;
                }
                else
                {
                    xy[i] = element;
                    
                    if (xy[i] > maxXy[i] - 1)
                    {
                        Console.WriteLine("Please input number less than {0}", maxXy[i] - 1);
                        i--;
                    }
                    else if (xy[i] < 0)
                    {
                        Console.WriteLine("Please input number bigger than 0");
                        i--;
                    }
                }
            }
            return xy;
        }
    }
}