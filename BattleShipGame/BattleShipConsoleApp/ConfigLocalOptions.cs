using System;
using System.IO;
using System.Text.Json;
using BattleShipBrain;

namespace BattleShipConsoleApp
{
    public class ConfigLocalOptions
    {
        public static string RemoveConfig()
        {
            // DECLARATIONS START
            int amountOfConfigsFound = 0;
            string[] files = Directory.GetFiles(GlobalVariables.ReturnConfigFolderLocation());
            int longestEntryId = 0;
            int longestEntryName = 0;
            // DECLARATIONS OVER

            foreach (var entry in GlobalVariables.ReturnCutNames(0))
            {
                amountOfConfigsFound++;
                if (amountOfConfigsFound.ToString().Length > longestEntryId) longestEntryId = amountOfConfigsFound.ToString().Length;
                if (entry.Length > longestEntryName) longestEntryName = entry.Length;
            }

            amountOfConfigsFound = 0;

            Console.Clear();
            Console.WriteLine("====> Config Menu - Local <====\n-------------------");
            if (files.Length != 0)
            {
                Console.Write("+-No");
                for (int i = "+-No".Length; i < longestEntryId + 5; i++)
                {
                    Console.Write("-");
                }

                Console.Write("Config Name");
                for (int i = "Config Name".Length; i < longestEntryName + 4; i++)
                {
                    Console.Write("-");
                }

                Console.Write("+\n");
                foreach (var file in GlobalVariables.ReturnCutNames(0))
                {
                    amountOfConfigsFound++;
                    Console.Write($"| {amountOfConfigsFound}");
                    for (int i = amountOfConfigsFound.ToString().Length; i < longestEntryId + 4; i++)
                    {
                        Console.Write(" ");
                    }

                    Console.Write(file);
                    for (int i = file.Length; i < longestEntryName + 4; i++)
                    {
                        Console.Write(" ");
                    }

                    Console.Write("|\n");
                }

                Console.Write("+");
                for (int i = 0; i < longestEntryId + longestEntryName + 8; i++)
                {
                    Console.Write("-");
                }
                
                Console.WriteLine("+");
                
                Console.WriteLine("Please input config No. you want to delete: (or write \"A\" - to delete all)");
                var configToDeleteNumber = Console.ReadLine()?.Trim();
                int.TryParse(configToDeleteNumber, out var configToDeleteNumberConverted);

                if (configToDeleteNumber!.ToUpper() == "A")
                {
                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }

                    return "All configs were Removed!";
                }
                
                amountOfConfigsFound = 0;
                if (configToDeleteNumberConverted >= amountOfConfigsFound)
                {
                    foreach (string file in files)
                    {
                        amountOfConfigsFound++;
                        if (amountOfConfigsFound == configToDeleteNumberConverted)
                        {
                            string fileNameStandardConfig = file;
                            File.Delete(fileNameStandardConfig);
                            break;
                        }
                    }
                }
                else
                {
                    return "No such config found!";
                }
            }
            else
            {
                Console.WriteLine("It looks like you have no locally saved configs yet!\nYou can do it with \"Build custom config\" option.");
                Console.ReadLine();
                return "Configs not found!";
            }

            return "Config Removed!";
        }

        public static string DisplayLocallySavedConfigs()
        {
            // DECLARATIONS START
            int amountOfConfigsFound = 0;
            string[] files = Directory.GetFiles(GlobalVariables.ReturnConfigFolderLocation());
            int longestEntryId = 0;
            int longestEntryName = 0;
            // DECLARATIONS OVER

            foreach (var entry in GlobalVariables.ReturnCutNames(0))
            {
                amountOfConfigsFound++;
                if (amountOfConfigsFound.ToString().Length > longestEntryId) longestEntryId = amountOfConfigsFound.ToString().Length;
                if (entry.Length > longestEntryName) longestEntryName = entry.Length;
            }

            amountOfConfigsFound = 0;

            Console.Clear();
            Console.WriteLine("====> Config Menu - Local <====\n-------------------");
            if (files.Length != 0)
            {
                Console.Write("+ID");
                for (int i = "+ID".Length; i < longestEntryId + 5; i++)
                {
                    Console.Write("-");
                }

                Console.Write("Config Name");
                for (int i = "Config Name".Length; i < longestEntryName + 4; i++)
                {
                    Console.Write("-");
                }

                Console.Write("+\n");
                foreach (var file in GlobalVariables.ReturnCutNames(0))
                {
                    amountOfConfigsFound++;
                    Console.Write($"|{amountOfConfigsFound}");
                    for (int i = amountOfConfigsFound.ToString().Length; i < longestEntryId + 4; i++)
                    {
                        Console.Write(" ");
                    }

                    Console.Write(file);
                    for (int i = file.Length; i < longestEntryName + 4; i++)
                    {
                        Console.Write(" ");
                    }

                    Console.Write("|\n");
                }

                Console.Write("+");
                for (int i = 0; i < longestEntryId + longestEntryName + 8; i++)
                {
                    Console.Write("-");
                }
                
                Console.WriteLine("+");
                Console.WriteLine("Click enter when ready.");
            }
            else
            {
                Console.WriteLine("It looks like you have no locally saved configs yet!\nYou can do it with \"Build custom config\" option.");
            }
            Console.ReadLine();
            return "Configs Viewed!";
        }

        public static string SaveStandardConfig()
        {
            GameConfig conf = new GameConfig();
            
            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var confJsonStr = JsonSerializer.Serialize(conf, jsonOptions);
            
            File.WriteAllText(GlobalVariables.fileNameStandardConfig, confJsonStr);
            return "Config Saved with name _StandardConfig.conf!";
        }
    }
}