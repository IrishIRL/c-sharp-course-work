using System;
using System.IO;
using System.Linq;
using DAL;

namespace BattleShipConsoleApp
{
    public class ConfigDatabaseOptions
    {
       // We are downloading the config from Database and saving it as local standard config. 
        public static string RetrieveStandardConfig()
        {
            string configName = "default";
            string configJson = "";
            bool success = true;
            
            using (var db = new ApplicationDbContext())
            {

                Console.WriteLine(
                    "Currently there are {0} configs in the database. Do you want to see all of them? (Y/n)",
                    db.GameConfigs.Count());
                var yesNo1 = Console.ReadKey(true);
                if (yesNo1.Key.ToString().ToLower() != "n")
                {
                    int longestEntryId = 0;
                    int longestEntryName = 0;
                    int longestDatabaseEntry = 0;
                    foreach (var entry in db.GameConfigs)
                    {
                        if (entry.ConfigId.ToString().Length > longestEntryId) longestEntryId = entry.ConfigId.ToString().Length;
                        if (entry.ConfigName.Length > longestEntryName) longestEntryName = entry.ConfigName.Length;
                        if (entry.ConfigBuildDate.ToString().Length > longestDatabaseEntry) longestDatabaseEntry = entry.ConfigBuildDate.ToString().Length;
                    }
                    
                    Console.Write("+ID");
                    for (int i = "+ID".Length; i < longestEntryId+5; i++)
                    {
                        Console.Write("-");   
                    }
                    Console.Write("Config Name");
                    for (int i = "Config Name".Length; i < longestEntryName+4; i++)
                    {
                        Console.Write("-");   
                    }
                    Console.Write("Config Build Date");
                    for (int i = "Config Build Date".Length; i < longestDatabaseEntry; i++)
                    {
                        Console.Write("-");   
                    }
                    Console.Write("+\n");
                    foreach (var entry in db.GameConfigs)
                    {
                        Console.Write($"|{entry.ConfigId}");
                        for (int i = entry.ConfigId.ToString().Length; i < longestEntryId+4; i++)
                        {
                            Console.Write(" ");   
                        }
                        Console.Write(entry.ConfigName);
                        for (int i = entry.ConfigName.Length; i < longestEntryName+4; i++)
                        {
                            Console.Write(" ");
                        }
                        Console.Write($"{entry.ConfigBuildDate}");
                        for (int i = entry.ConfigBuildDate.ToString().Length; i < longestDatabaseEntry; i++)
                        {
                            Console.Write(" ");
                        }
                        Console.Write("|\n");
                    } 
                    Console.Write("+");
                    for (int i = 0; i < longestEntryId + longestEntryName + longestDatabaseEntry + 8; i++)
                    {
                        Console.Write("-");
                    }
                    Console.WriteLine("+");
                }
                bool foundEntry = false;
                do
                {
                    Console.WriteLine("\n\nWhich config would you like to choose? (Input id)"); 
                    var configId = Console.ReadLine(); 
                    int.TryParse(configId, out var configIdParsed); 
                    foreach (var entry in db.GameConfigs)
                    {
                        if (entry.ConfigId == configIdParsed)
                        {
                            configName = entry.ConfigName;
                            configJson = entry.ConfigJson;
                            foundEntry = true;
                            break;
                        }
                    }
                } while (!foundEntry);
            }

            do
            {
                if (!success)
                {
                    configName = "";
                    Console.WriteLine("Please input name of the new config");
                    configName += Console.ReadLine();
                }
                
                configName += ".json";
                string fullNewConfig = GlobalVariables.ReturnConfigFolderLocation() + Path.DirectorySeparatorChar + configName;

                if (!File.Exists(fullNewConfig))
                {
                    File.WriteAllText(fullNewConfig, configJson);
                    success = true;
                }
                else
                {
                    success = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Config with this name already exists, please choose another name!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                
            } while (!success);

            return "Config retrieved and saved!";
            
        }
    }
}