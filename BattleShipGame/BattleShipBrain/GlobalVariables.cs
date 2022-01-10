using System;
using System.IO;
using System.Linq;
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

        // Returns only name of the file.
        // int place = 0 for Config
        // int place = 1 for Game Save
        public static string[] ReturnCutNames(int place)
        {
            string[] uncutFiles;
            switch (place)
            {
                case 1:
                    uncutFiles = Directory.GetFiles(ReturnGameSaveFolderLocation());
                    break;
                default:
                    uncutFiles = Directory.GetFiles(ReturnConfigFolderLocation());
                    break;
            }

            string[] cutFiles = new string[uncutFiles.Length];
            int counter = 0;
            foreach (var cut in uncutFiles)
            {
                string cutReady = cut.Split(@"\").Last();
                cutFiles[counter] = cutReady;
                counter++;
            }
            
            return cutFiles;
        }


        public static string fileNameStandardConfig =  ReturnConfigFolderLocation() + Path.DirectorySeparatorChar + "standardConfig.json";
        //public static string savedGameFile = BsBrain.ReturnFolderLocation() + Path.DirectorySeparatorChar + "savedGame.json";
    }
}