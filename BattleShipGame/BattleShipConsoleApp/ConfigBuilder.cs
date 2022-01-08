using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BattleShipBrain;

namespace BattleShipConsoleApp
{
    public class ConfigBuilder
    {
        public static string ConfigAssembler()
        {
            var conf = new GameConfig();
            
            int[] boardXyGrabbed = BoardDimensionQuestions();
            int touchRule = TouchRuleQuestion();
            ShipConfigQuestions(conf);
            
            string confJsonStr = BsBrain.JsonBuilder(boardXyGrabbed, touchRule, conf);
            Console.WriteLine("Please input name of new config");
            var configName = Console.ReadLine()!.Trim();
            configName += ".json";
            string fullNewConfig = GlobalVariables.ReturnConfigFolderLocation() + Path.DirectorySeparatorChar + configName;
            System.IO.File.WriteAllText(fullNewConfig, confJsonStr);
            
            return "New config saved!";
        }

        private static int[] BoardDimensionQuestions()
        {
            bool checker = true;
            int[] standardBoard = {10, 10};
            do
            {
                Console.WriteLine("Input X Board Dimension");
                var x = Console.ReadLine()?.Trim();
                int.TryParse(x, out var xConverted);
                Console.WriteLine("Input Y Board Dimension");
                var y = Console.ReadLine()?.Trim();
                int.TryParse(y, out var yConverted);

                if (xConverted is > 4 and < 26 && yConverted is > 4 and < 26)
                {
                    int[] xy = new int[2];
                    xy[0] = xConverted;
                    xy[1] = yConverted;
                    return xy;   
                }
                else
                {
                    // I do not think that there is a point to play a smaller or a bigger board. But it can be changes if wanted.
                    Console.WriteLine("Please make your board at least 4 and at most 26 in sizes.");
                }
            } while (checker);
            return standardBoard;
        }
        
        private static int TouchRuleQuestion()
        {
            bool checker = true;
            do
            {
                Console.WriteLine("Choose touch rule:\n1. No Touch\n2. Corner Touch\n3. Side Touch");
                var x = Console.ReadLine()?.Trim();
                int.TryParse(x, out var touchRule);

                if (touchRule is 1 or 2 or 3)
                {
                    return touchRule;
                }
                else
                {
                    Console.WriteLine("Please input the correct number!");
                }
            } while (checker);
            
            return 1;
        }

        private static void ShipConfigQuestions(GameConfig conf)
        {
            int shipCounter = 5;
            conf.ShipConfigs.Clear();
            
            Console.WriteLine("How many different ships do you want to build (default 5):");
            var shipCount = Console.ReadLine()?.Trim();
            int.TryParse(shipCount, out var shipCountConverted);

            if (shipCountConverted > 0)
            {
                shipCounter = shipCountConverted;
            }
            
            for (int i = 0; i < shipCounter; i++)
            {
                bool questionChecker = true;
                do
                {
                    Console.WriteLine("Input name for the ship:");
                    var shipName = Console.ReadLine()?.Trim();
                    Console.WriteLine("Input Size X for ship:");
                    var x = Console.ReadLine()?.Trim();
                    int.TryParse(x, out var xConverted);
                    Console.WriteLine("Input Size Y for ship:");
                    var y = Console.ReadLine()?.Trim();
                    int.TryParse(y, out var yConverted);
                    Console.WriteLine("Input Quantity of ships:");
                    var q = Console.ReadLine()?.Trim();
                    int.TryParse(q, out var qConverted);

                    if (conf.BoardSizeX > xConverted && xConverted > 0 && conf.BoardSizeY > yConverted &&
                        yConverted > 0 && qConverted > 0 && shipName != null)
                    {
                        var shipConfig = new ShipConfig
                        {
                            Name = shipName,
                            ShipSizeX = xConverted,
                            ShipSizeY = yConverted,
                            Quantity = qConverted
                        };
                        conf.ShipConfigs.Add(shipConfig);
                        questionChecker = false;
                    }
                    else
                    {
                        Console.WriteLine("Please input the correct numbers!");
                    }
                } while (questionChecker);
            }
        }
        
        /*
         * private static int[] SizeQuestions(int i)
        {
            var conf = new GameConfig();
            string ship = "";

            // DefaultSizes is a default ship which logically should be never built.
            // But for security reasons let it be here.
            int[] defaultSizes = {1, 1, 1};
            
            switch (i)
            {
                case 0:
                    ship = conf.ShipConfigs[i].Name;
                    defaultSizes[0] = conf.ShipConfigs[i].Quantity;
                    defaultSizes[1] = conf.ShipConfigs[i].ShipSizeX;
                    defaultSizes[2] = conf.ShipConfigs[i].ShipSizeY;
                    break;
                case 1:
                    ship = conf.ShipConfigs[i].Name;
                    defaultSizes[0] = conf.ShipConfigs[i].Quantity;
                    defaultSizes[1] = conf.ShipConfigs[i].ShipSizeX;
                    defaultSizes[2] = conf.ShipConfigs[i].ShipSizeY;
                    break;
                case 2:
                    ship = conf.ShipConfigs[i].Name;
                    defaultSizes[0] = conf.ShipConfigs[i].Quantity;
                    defaultSizes[1] = conf.ShipConfigs[i].ShipSizeX;
                    defaultSizes[2] = conf.ShipConfigs[i].ShipSizeY;
                    break;
                case 3:
                    ship = conf.ShipConfigs[i].Name;
                    defaultSizes[0] = conf.ShipConfigs[i].Quantity;
                    defaultSizes[1] = conf.ShipConfigs[i].ShipSizeX;
                    defaultSizes[2] = conf.ShipConfigs[i].ShipSizeY;
                    break;
                case 4:
                    ship = conf.ShipConfigs[i].Name;
                    defaultSizes[0] = conf.ShipConfigs[i].Quantity;
                    defaultSizes[1] = conf.ShipConfigs[i].ShipSizeX;
                    defaultSizes[2] = conf.ShipConfigs[i].ShipSizeY;
                    break;
            }
            
            bool checker = true;
            do
            {
                Console.WriteLine("Input name for the ship: {0}. Normally X is: {1}", ship, defaultSizes[0]);
                var shipName = Console.ReadLine()?.Trim();
                Console.WriteLine("Input Size X for ship: {0}. Normally X is: {1}", ship, defaultSizes[0]);
                var x = Console.ReadLine()?.Trim();
                int.TryParse(x, out var xConverted);
                Console.WriteLine("Input Size Y for ship: {0}. Normally Y is: {1}", ship, defaultSizes[1]);
                var y = Console.ReadLine()?.Trim();
                int.TryParse(y, out var yConverted);
                Console.WriteLine("Input Quantity of ships: {0}. Normally Quantity is: {1}", ship, defaultSizes[2]);
                var q = Console.ReadLine()?.Trim();
                int.TryParse(q, out var qConverted);

                if (conf.BoardSizeX > xConverted && xConverted > 0 && conf.BoardSizeY > yConverted && yConverted > 0 && qConverted > 0)
                {
                    int[] xyq = new int[3];
                    xyq[0] = xConverted;
                    xyq[1] = yConverted;
                    xyq[2] = qConverted;
                    //return xyq;
                    
                    // conf.ShipConfigs[i].ShipSizeX = xyzGrabbed[0];
                    // conf.ShipConfigs[i].ShipSizeY = xyzGrabbed[1];
                    
                    var ship = new Ship();
                }
                else
                {
                    Console.WriteLine("Please input the correct numbers!");
                }
            } while (checker);

            return defaultSizes;
        }
         */
    }
}