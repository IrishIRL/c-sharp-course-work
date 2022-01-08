using System;
using System.Collections.Generic;
using BattleShipConsoleUI;
using MenuSystem;

namespace BattleShipConsoleApp
{
    class Program
    {
        static void Main()
        {
            try {
                var playGame = new Game();
                var mainMenu = new Menu("BattleShip Main", EMenuLevel.Root);
                mainMenu.AddMenuItems(new List<MenuItem>()
                {
                    new("P", "Play a Game", playGame.PlayGame),
                    new("C", "Config Options", ConfigOptionsMenu),
                });
                mainMenu.Run();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }

        private static string ConfigOptionsMenu()
        {
            var menu = new Menu("Config Menu", EMenuLevel.First);
            menu.AddMenuItems(new List<MenuItem>()
            {
                new("L", "Local config options", LocalConfigMenu),
                new("D", "Database config options", DatabaseConfigMenu)
            });
            return menu.Run();
        }

        private static string LocalConfigMenu()
        {
            var menu = new Menu("Config Menu - Local", EMenuLevel.SecondOrMore);
            menu.AddMenuItems(new List<MenuItem>()
            {
                new("C", "Make CUSTOM config", ConfigBuilder.ConfigAssembler),
                new("RR", "Remove configs from computer", ConfigLocalOptions.RemoveConfig)
            });
            return menu.Run();
        }
        
        private static string DatabaseConfigMenu()
        {
            var menu = new Menu("Config Menu - Database", EMenuLevel.SecondOrMore);
            menu.AddMenuItems(new List<MenuItem>()
            {
                new("U", "Upload current config to database", MainUI.UploadCurrentConfigUi),
                new("RE", "Retrieve current config from database", ConfigDatabaseOptions.RetrieveStandardConfig)
            });
            return menu.Run();
        }
    }
}