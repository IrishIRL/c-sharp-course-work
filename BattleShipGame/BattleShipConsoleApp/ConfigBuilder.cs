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
            string confJsonStr = ChangeDimensions();
            Console.WriteLine("Please input name of new config");
            var configName = Console.ReadLine()!.Trim();
            configName += ".json";
            string fullNewConfig = GlobalVariables.ReturnConfigFolderLocation() + Path.DirectorySeparatorChar + configName;
            System.IO.File.WriteAllText(fullNewConfig, confJsonStr);
            
            return "New config saved!";
        }

        private static string ChangeDimensions()
        {
            var conf = new GameConfig();

            var boardXyGrabbed = BoardDimensionQuestions();
            
            conf.BoardSizeX = boardXyGrabbed[0];
            conf.BoardSizeY = boardXyGrabbed[1];

            for (int i = 0; i < 5; i++)
            {
                var xyzGrabbed = SizeQuestions(i);
                            
                conf.ShipConfigs[i].ShipSizeX = xyzGrabbed[0];
                conf.ShipConfigs[i].ShipSizeY = xyzGrabbed[1];
            }
            
            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var confJsonStr = JsonSerializer.Serialize(conf, jsonOptions);

            return confJsonStr;
        }

        private static int[] BoardDimensionQuestions()
        {
            Console.WriteLine("Input X Board Dimension");
            var x = Console.ReadLine()?.Trim();
            int.TryParse(x, out var xConverted);
            Console.WriteLine("Input Y Board Dimension");
            var y = Console.ReadLine()?.Trim();
            int.TryParse(y, out var yConverted);

            int[] xy = new int[2];
            xy[0] = xConverted;
            xy[1] = yConverted;
            return xy;
        }

        private static int[] SizeQuestions(int i)
        {
            var conf = new GameConfig();
            string ship = "";
            int quantity = 0;
            int shipSizeX = 0;
            int shipSizeY = 0;

            switch (i)
            {
                case 0:
                    ship = conf.ShipConfigs[i].Name;
                    quantity = conf.ShipConfigs[i].Quantity;
                    shipSizeX = conf.ShipConfigs[i].ShipSizeX;
                    shipSizeY = conf.ShipConfigs[i].ShipSizeY;
                    break;
                case 1:
                    ship = conf.ShipConfigs[i].Name;
                    quantity = conf.ShipConfigs[i].Quantity;
                    shipSizeX = conf.ShipConfigs[i].ShipSizeX;
                    shipSizeY = conf.ShipConfigs[i].ShipSizeY;
                    break;
                case 2:
                    ship = conf.ShipConfigs[i].Name;
                    quantity = conf.ShipConfigs[i].Quantity;
                    shipSizeX = conf.ShipConfigs[i].ShipSizeX;
                    shipSizeY = conf.ShipConfigs[i].ShipSizeY;
                    break;
                case 3:
                    ship = conf.ShipConfigs[i].Name;
                    quantity = conf.ShipConfigs[i].Quantity;
                    shipSizeX = conf.ShipConfigs[i].ShipSizeX;
                    shipSizeY = conf.ShipConfigs[i].ShipSizeY;
                    break;
                case 4:
                    ship = conf.ShipConfigs[i].Name;
                    quantity = conf.ShipConfigs[i].Quantity;
                    shipSizeX = conf.ShipConfigs[i].ShipSizeX;
                    shipSizeY = conf.ShipConfigs[i].ShipSizeY;
                    break;
            }
            
            
            Console.WriteLine("Input Size X for ship: {0}. Normally X is: {1}", ship, shipSizeX);
            var x = Console.ReadLine()?.Trim();
            int.TryParse(x, out var xConverted);
            Console.WriteLine("Input Size Y for ship: {0}. Normally Y is: {1}", ship, shipSizeY);
            var y = Console.ReadLine()?.Trim();
            int.TryParse(y, out var yConverted);
            Console.WriteLine("Input Quantity of ships: {0}. Normally Quantity is: {1}", ship, quantity);
            var q = Console.ReadLine()?.Trim();
            int.TryParse(q, out var qConverted);
            
            int[] xyq = new int[3];
            xyq[0] = xConverted;
            xyq[1] = yConverted;
            xyq[2] = qConverted;
            return xyq;
        }
    }
}