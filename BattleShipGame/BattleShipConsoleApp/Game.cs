using System;
using System.IO;
using System.Text.Json;
using BattleShipBrain;
using BattleShipConsoleUI;

namespace BattleShipConsoleApp
{
    public class Game
    {
        private static GameConfig _conf = new GameConfig();
        private static BsBrain _brain = new BsBrain(_conf);
        public static string PlayGame()
        {
            
            int closeCondition = 0;
            int loadSavedGame = LoadSavedGame();
            
            LoadGameConfig(); // Currently the logic works in such way, that the person should know if there was a custom config used for the game
                              // It is used for limiting the sizes of the board and may be used later for knowing win conditions?
                              
            int xConverted = _conf.BoardSizeX;
            int yConverted = _conf.BoardSizeY;
                              
            if (loadSavedGame != 0)
            {
                MainUI.PlaceShipUi(_conf);
            }

            Console.Clear();
            do
            {
                Console.WriteLine("Write e to exit the game. Write s to exit and save the game.");
                closeCondition = ChoosePlaceToPlantBomb(xConverted, yConverted);
                BSConsoleUI.DrawBoard(_brain.GetEnemyBoard());
                if (_brain._currentPlayerNo == 1) _brain._currentPlayerNo = 0; // change player 
                else _brain._currentPlayerNo = 1;
            } while (closeCondition == 0);

            string returnString = MainUI.GameClosure(_brain, closeCondition);
            return returnString;
        }
        
        // Load Game Config (We check that the config exists and if exists, we update current config to the loaded config).
        // We always grab config from local .json file, which could be set up from the Config Options.
        public static void LoadGameConfig()
        {
            string confFile = MainUI.LoadLocalConfigUi();
            var confText = File.ReadAllText(confFile);
            _conf = JsonSerializer.Deserialize<GameConfig>(confText);
        }

        // Load Saved Game
        public static int LoadSavedGame()
        {
            // returnResult will decide if we need to make a new board with new ships.
            // If it is not eq to 0, then we should.
            // If it is eq to 0, no new board should be made.
            int returnResult = 1;
            Console.WriteLine("Do you want to load saved game? (y/N)");
            var yesNo = Console.ReadKey(true);
            if (yesNo.Key.ToString().ToLower() == "y")
            {
                returnResult = MainUI.LoadSavedGameUi(_brain); 
            }
            else _brain = new BsBrain(_conf);

            return returnResult;
        }
        
        // Function that asks player where to place the bomb.
        public static int ChoosePlaceToPlantBomb(int maxX, int maxY)
        {
            int[] xyLocationConverted = LocationXy(0);
            int[] maxXy = { maxX, maxY };

            for (int i = 0; i < 2; i++)
            {
                if (xyLocationConverted[i] == -1) return -1;
                if (xyLocationConverted[i] == -2) return -2;
                
                if (xyLocationConverted[i] > maxXy[i] - 1)
                {
                    Console.WriteLine("Please input number less than {0}", maxXy[i]-1);
                    xyLocationConverted = LocationXy(i);
                    i--;
                }
            }
            
            _brain.PlaceBomb(xyLocationConverted);
            return 0;
        }
        public static int[] LocationXy(int i)
        {
            int[] xy = new int[2];
            string[] location = {"x", "y"};
            
            for (; i < 2; i++)
            {
                Console.WriteLine("Current Player: {0}", _brain._currentPlayerNo + 1);
                Console.WriteLine("Input bomb location on {0} coordinate:", location[i]);
                var x = Console.ReadLine()?.Trim();

                if (x?.ToLower() == "e") { xy[i] = -1; break; }

                if (x?.ToLower() == "s") { xy[i] = -2; break; }
                
                var success = int.TryParse(x, out var element);
                if (!success) { Console.WriteLine("Please input correct number!"); i--; }
                else xy[i] = element;
            }
            return xy;
        }
    }
}