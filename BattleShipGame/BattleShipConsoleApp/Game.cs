using System;
using System.Collections;
using System.IO;
using System.Text.Json;
using BattleShipBrain;
using BattleShipConsoleUI;

namespace BattleShipConsoleApp
{
    //TODO: Main button does not work
    //TODO: Random ship placement
    //TODO: Json in web
    //TODO: Web build config
    //TODO: Fix load saved game
    public class Game
    {
        private GameConfig _conf = default!;
        private BsBrain _brain = default!;

        public string PlayGame()
        {
            _conf = new GameConfig();
            
            int closeCondition = 0;
            int loadSavedGame = LoadSavedGame();

            if (loadSavedGame != 0)
            {
                LoadGameConfig();
                _brain = new BsBrain(_conf);
                _brain = MainUI.PlaceShipUi(_conf);
            }
            
            Console.Clear();
            do
            {
                closeCondition = ChoosePlaceToPlantBomb();

                _brain._currentPlayerNo = _brain.OneToZero(_brain._currentPlayerNo);
            } while (closeCondition == 0);

            _brain._currentPlayerNo = _brain.OneToZero(_brain._currentPlayerNo);
            
            string returnString = MainUI.GameClosure(_brain, closeCondition);
            return returnString;
        }
        
        // Load Game Config (We check that the config exists and if exists, we update current config to the loaded config).
        // We always grab config from local .json file, which could be set up from the Config Options.
        public void LoadGameConfig()
        {
            string confFile = MainUI.LoadLocalConfigUi();
            var confText = File.ReadAllText(confFile);
            _conf = JsonSerializer.Deserialize<GameConfig>(confText);
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
            bool hitCondition = false;
            do
            {
                // Check for win conditions start
                var winCondition = _brain.GameWinCondition();
                if (winCondition) return 1;
                // Check for win conditions end
                
                // DECLARATIONS START
                // correctLocation is used to check if the place is not already bombed. If it is, then ask again.
                bool correctLocation = false;
                // xyLocationConverted is used to save X and Y coordinates for the bomb in the array list.
                int[] xyLocationConverted = new int[2];
                // DECLARATIONS END
                
                Console.Clear();
                if (hitCondition)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("You shot the ship! Choose the next place to shoot.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("Current Player: {0}", _brain._currentPlayerNo + 1);
                BSConsoleUI.DrawBoard(true, _brain.GetEnemyBoard());
                Console.WriteLine("Write e to exit the game. Write s to exit and save the game.");

                do
                {
                    if(correctLocation) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Please input place that you did not bomb yet!"); Console.ForegroundColor = ConsoleColor.White; }
                    xyLocationConverted = BombLocationXy(0);
                    correctLocation = _brain.CheckForBomb(xyLocationConverted[0], xyLocationConverted[1]);
                } while (correctLocation);

                for (int i = 0; i < 2; i++)
                {
                    if (xyLocationConverted[i] == -1) return -1;
                    if (xyLocationConverted[i] == -2) return -2;
                }

                hitCondition = _brain.PlaceBomb(xyLocationConverted);
            } while (hitCondition);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You missed! Please give the second user control. Click enter after that.");
            Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            
            return 0;
        }
        public int[] BombLocationXy(int i)
        {
            int[] xy = new int[2];
            string[] location = {"x", "y"};
            int[] maxXy = {_conf.BoardSizeX, _conf.BoardSizeY};

            for (; i < 2; i++)
            {
                Console.WriteLine("Input bomb location on {0} coordinate:", location[i]);
                var x = Console.ReadLine()?.Trim();

                // Since I am using int[], I have decided to transform exit and save to -1 and -2.
                // From my point of view, there is no problem with that. Even though player may manually exit the game inputting -1 and -2, not letters 'e' or 's'.  
                if (x?.ToLower() == "e")
                {
                    xy[i] = -1;
                    break;
                }

                if (x?.ToLower() == "s")
                {
                    xy[i] = -2;
                    break;
                }

                var success = int.TryParse(x, out var element);
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