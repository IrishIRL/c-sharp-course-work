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
            
            string confJsonStr = BsBrain.JsonBuilder(boardXyGrabbed, touchRule, conf); // shouldn't be var _brain = new BsBrain() and _brain.JsonBuilder ???
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
    }
}