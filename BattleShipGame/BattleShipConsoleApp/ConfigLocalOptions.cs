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
            string[] files = Directory.GetFiles(GlobalVariables.ReturnConfigFolderLocation());
            int amountOfConfigsFound = 0;
            int counter = 0;
            
            Console.WriteLine("No. | Config Name");
            foreach (string file in files)
            {
                amountOfConfigsFound++;
                Console.WriteLine(amountOfConfigsFound + " | " + file);
            }
            
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

            if (configToDeleteNumberConverted <= amountOfConfigsFound)
            {
                foreach (string file in files)
                {
                    counter++;
                    if (counter == configToDeleteNumberConverted)
                    {
                        string fileNameStandardConfig = file;
                        File.Delete(fileNameStandardConfig);
                    }
                }
            }
            else
            {
                return "No such config found!";
            }

            return "Config Removed!";
        }
        public static string DisplayConfig()
        {
            Console.WriteLine("Loading config...");
            var confText = File.ReadAllText(GlobalVariables.fileNameStandardConfig);
            var conf = JsonSerializer.Deserialize<GameConfig>(confText);
            Console.WriteLine(conf);
            Console.WriteLine("Click Enter after finished!");
            Console.ReadLine();
            return "Config Viewed!";
        }
    }
}