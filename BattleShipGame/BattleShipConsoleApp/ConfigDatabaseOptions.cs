using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using DAL;

 /*
  TODO 1: Clear Repeated Code, make it easier to read
  TODO 2: Rewrite database logic, so we have pointers to ships, not fully saved ships
  TODO 3: Rewrite database logic, so we only upload new ships and do not upload ships that are identical to the database received info
 */

namespace BattleShipConsoleApp
{
    public class ConfigDatabaseOptions
    {
       // We are downloading the config from Database and saving it as local standard config. 
        public static string RetrieveStandardConfig()
        {
            string configName = "default";
            string configJson = "";
            
            using (var db = new ApplicationDbContext())
            {

                Console.WriteLine(
                    "Currently there are {0} configs in the database. Do you want to see all of them? (Y/n)",
                    db.GameConfigs.Count());
                var yesNo1 = Console.ReadKey(true);
                if (yesNo1.Key.ToString().ToLower() != "n")
                {
                    Console.WriteLine("ID - Config Name - Config Build Date");
                    foreach (var entry in db.GameConfigs)
                    {
                        Console.WriteLine($" {entry.ConfigId} - {entry.ConfigName} {entry.ConfigBuildDate}");
                    }
                }

                Console.WriteLine("Which config would you like to choose?");
                var configId = Console.ReadLine();
                int.TryParse(configId, out var configIdParsed);
                foreach (var entry in db.GameConfigs)
                {
                    if (entry.ConfigId == configIdParsed)
                    {
                        configName = entry.ConfigName;
                        configJson = entry.ConfigJson;
                    }
                }
            }

            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            
            configName += ".json";
            string fullNewConfig = GlobalVariables.ReturnConfigFolderLocation() + Path.DirectorySeparatorChar + configName;
            System.IO.File.WriteAllText(fullNewConfig, configJson);
            
            return "Config retrieved and saved!";
            
        }
    }
}