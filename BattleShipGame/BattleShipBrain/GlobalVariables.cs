using System;
using System.IO;
using BattleShipBrain;

namespace BattleShipConsoleApp
{
    public class GlobalVariables
    {
        // Checks the standard folder location. If there is none, then make such dir.
        public static string ReturnFolderLocation()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string standardLocalFolder = appData + Path.DirectorySeparatorChar + "BattleShipGame";
            if (!File.Exists(standardLocalFolder))
            {
                Directory.CreateDirectory(standardLocalFolder);
            }
            return standardLocalFolder;
        }
        
        public static string ReturnConfigFolderLocation()
        {
            string standardLocalFolder = ReturnFolderLocation() + Path.DirectorySeparatorChar + "Configs";
            if (!File.Exists(standardLocalFolder))
            {
                Directory.CreateDirectory(standardLocalFolder);
            }
            return standardLocalFolder;
        }
        public static string ReturnGameSaveFolderLocation()
        {
            string standardLocalFolder = ReturnFolderLocation() + Path.DirectorySeparatorChar + "SavedGames";
            if (!File.Exists(standardLocalFolder))
            {
                Directory.CreateDirectory(standardLocalFolder);
            }
            return standardLocalFolder;
        }
        public static string fileNameStandardConfig =  ReturnConfigFolderLocation() + Path.DirectorySeparatorChar + "standardConfig.json";
        //public static string savedGameFile = BsBrain.ReturnFolderLocation() + Path.DirectorySeparatorChar + "savedGame.json";
    }
}