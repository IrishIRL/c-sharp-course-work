using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using BattleShipBrain;
using BattleShipConsoleApp;
using DAL;

namespace BattleShipConsoleUI
{
    public class MainUI
    {
        // Different ways of game closure (Win/ Exit without save/ Exit with save)
        public static string GameClosure(BsBrain brain, int closeCondition)
        {
            int currentPlayer = brain.CurrentPlayerNo + 1;
            if (closeCondition == 1) return ("Congratulations Player " + currentPlayer + ", you won!");
            if (closeCondition == -1) return "Exited Game.";
            if (closeCondition == -2) SaveGameUi(brain);
            return "Exited and Saved the Game!";
        }

        // Exit with save opens -> saveGameUi
        public static void SaveGameUi(BsBrain brain)
        {
            Console.WriteLine(
                "Do you want to make:\n1. Local save\n2. Database save\n3. Both Local and Database saves.\nIn case you do not want to save the game - pick any other number.");
            var saveDecision = Console.ReadLine();
            int.TryParse(saveDecision, out var saveDecisionParsed);

            Console.WriteLine("Which name would you like to set for the Game Save?");
            string nameOfGame = Console.ReadLine()!;

            brain.GetBrainJson(saveDecisionParsed, nameOfGame, false);
        }

        // Load Saved Game from both Local and Database copies 
        public static int LoadSavedGameUi(BsBrain brain)
        {
            string nameOfGame = "";

            // If return success is 1, then it means that something failed. It is set to fail until the code gets the correct result (0).
            // If it fails, we will ask user if he wants to try to download from another source. (Database if it was Local and otherwise).
            int returnSuccess = 1;

            Console.WriteLine(
                "Would you like to load game from:\n1. Local save\n2. Database save\nChoose any other number to skip load.");
            var saveDecision = Console.ReadLine();
            int.TryParse(saveDecision, out var saveDecisionParsed);

            switch (saveDecisionParsed)
            {
                // If we choose local save, then we also want to know the name (with location) to the file. Here we ask that.
                case 1:
                    nameOfGame = LoadSavedGameFromLocalFileUi();
                    break;

                // If we choose database save, then we also want to know the name of the file. Here we ask that.
                case 2:
                    nameOfGame = LoadSavedGameFromDatabaseFileUi();
                    break;

                // Default is set to skip the load.
                default:
                    Console.WriteLine("Skipping...");
                    returnSuccess = 2;
                    break;
            }

            if (nameOfGame != "")
            {
                returnSuccess = brain.RestoreBrainFromJson(saveDecisionParsed, nameOfGame, 0);
            }

            if (returnSuccess == 2)
            {
                Console.WriteLine("You will start from the new game. Please click any button to continue.");
                Console.ReadLine();
                return returnSuccess;
            }

            if (returnSuccess == 1)
            {
                Console.WriteLine(
                    "No configs were found. Would you like to try to load game from another source (Local / Database) (Y/n)");
                var yesNo = Console.ReadKey(true);
                if (yesNo.Key.ToString().ToLower() != "n")
                {
                    // Change decision to another one (from local -> database, from database -> local)
                    if (saveDecisionParsed == 1) saveDecisionParsed = 0;
                    if (saveDecisionParsed == 0) saveDecisionParsed = 1;

                    switch (saveDecisionParsed)
                    {
                        case 1:
                            nameOfGame = LoadSavedGameFromLocalFileUi();
                            break;

                        case 2:
                            nameOfGame = LoadSavedGameFromDatabaseFileUi();
                            break;
                    }

                    returnSuccess = brain.RestoreBrainFromJson(saveDecisionParsed, nameOfGame, 1);
                }
            }

            // Last step, we check once again if there were found any configs. If no, returnSuccess will be equal to 1. Otherwise to 0.
            if (returnSuccess == 1)
            {
                Console.WriteLine(
                    "No configs were found. You will start from the new game. Please click any button to continue.");
                Console.ReadLine();
                return returnSuccess;
            }
            else
            {
                Console.WriteLine("Config was successfully loaded! Press any button to continue.");
                Console.ReadLine();
                return returnSuccess;
            }
        }

        // In this method we try to get the correct file location of the saved game from local folder.
        public static string LoadSavedGameFromLocalFileUi()
        {
            Console.Clear();

            int counter = 0;
            int amountOfGameSavesFound = 0;
            string[] files = Directory.GetFiles(GlobalVariables.ReturnGameSaveFolderLocation());
            int longestEntryId = 0;
            int longestEntryName = 0;
            
            if (files.Length != 0)
            {
                Console.WriteLine(
                    "There are {0} locally saved games. Would you like to show list of saved games? (Y/n)",
                    files.Length);
                var yesNoSeeGameSave = Console.ReadKey(true);
                if (yesNoSeeGameSave.Key.ToString().ToLower() != "n")
                {
                    foreach (var entry in GlobalVariables.ReturnCutNames(1))
                    {
                        amountOfGameSavesFound++;
                        if (amountOfGameSavesFound.ToString().Length > longestEntryId) longestEntryId = amountOfGameSavesFound.ToString().Length;
                        if (entry.Length > longestEntryName) longestEntryName = entry.Length;
                    }
                    
                    Console.Write("+-No");
                    for (int i = "+-No".Length; i < longestEntryId + 6; i++)
                    {
                        Console.Write("-");
                    }
                    Console.Write("Game Save Name");
                    for (int i = "Game Save Name".Length; i < longestEntryName + 4; i++)
                    {
                        Console.Write("-");
                    }
                    Console.Write("+\n");

                    amountOfGameSavesFound = 0;
                    
                    foreach (var file in GlobalVariables.ReturnCutNames(1))
                    {
                        amountOfGameSavesFound++;
                        Console.Write($"| {amountOfGameSavesFound}");
                        for (int i = amountOfGameSavesFound.ToString().Length; i < longestEntryId + 4; i++)
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
                    for (int i = 0; i < longestEntryId + longestEntryName + 9; i++)
                    {
                        Console.Write("-");
                    }
                    Console.WriteLine("+");
                    
                    
                    Console.WriteLine("Please input game save No. you want to use:");
                    var gameSaveToUseNumber = Console.ReadLine()?.Trim();
                    int.TryParse(gameSaveToUseNumber, out var gameSaveToUseNumberConverted);
                    if (gameSaveToUseNumberConverted <= amountOfGameSavesFound)
                    {
                        foreach (string file in files)
                        {
                            counter++;
                            if (counter == gameSaveToUseNumberConverted)
                            {
                                return file;
                            }
                        }
                    }
                }
            }

            return "";
        }

        // In this method we try to get the correct file id of the saved game from database. We return it as a string here, but will parse to int in the BsBrain logic.
        public static string LoadSavedGameFromDatabaseFileUi()
        {
            Console.Clear();
            string idOfTheSave = "";

            using (var db = new ApplicationDbContext())
            {
                if (db.SavedGames.Any())
                {
                    Console.WriteLine("Currently there are {0} in the database. Do you want to see all of them? (Y/n)",
                        db.SavedGames.Count());
                    var yesNo = Console.ReadKey(true);
                    if (yesNo.Key.ToString().ToLower() != "n")
                    {
                        
                        int longestEntryId = 0;
                        int longestGameSaveDate = 0;
                        
                        foreach (var entry in db.SavedGames)
                        {
                            if (entry.SavedGameId.ToString().Length > longestEntryId) longestEntryId = entry.SavedGameId.ToString().Length;
                            if (entry.GameSaveDate.ToString().Length > longestGameSaveDate) longestGameSaveDate = entry.GameSaveDate.ToString().Length;
                        }

                        Console.Write("+-Game ID");
                        for (int i = 0; i < longestEntryId; i++)
                        {
                            Console.Write("-");
                        }

                        Console.Write("Game Save Date");
                        for (int i = "Game Save Date".Length; i < longestGameSaveDate + 2; i++)
                        {
                            Console.Write("-");
                        }

                        Console.Write("+\n");
                        foreach (var entry in db.SavedGames)
                        {
                            Console.Write($"| {entry.SavedGameId}");
                            for (int i = entry.SavedGameId.ToString().Length; i < longestEntryId + "Game ID".Length; i++)
                            {
                                Console.Write(" ");
                            }

                            Console.Write($"{entry.GameSaveDate}");
                            for (int i = entry.GameSaveDate.ToString().Length; i < longestGameSaveDate + 1; i++)
                            {
                                Console.Write(" ");
                            }

                            Console.Write(" |\n");
                        }
                        Console.Write("+");
                        for (int i = 0; i < longestEntryId + longestGameSaveDate + " Game ID".Length + 2; i++)
                        {
                            Console.Write("-");
                        }
                        Console.WriteLine("+");
                    }

                    Console.WriteLine("Which one would you like to pick? Input correct No.");
                    idOfTheSave += Console.ReadLine();
                    return idOfTheSave;
                }
            }

            return idOfTheSave;
        }

        // Load config from local file.
        // PS. We always load a local config. In case we want to start with database config, we still should download it to the local folder first and then choose from local configs.
        public static string LoadLocalConfigUi()
        {
            // DECLARATIONS START
            int amountOfConfigsFound = 0;
            int counter = 0;
            string[] files = Directory.GetFiles(GlobalVariables.ReturnConfigFolderLocation());
            // DECLARATIONS OVER

            if (files.Length != 0)
            {
                Console.WriteLine(
                    "You have {0} different custom configs in your folder. Would you like to load custom config? (y/N)",
                    files.Length);
                var yesNo = Console.ReadKey(true);
                if (yesNo.Key.ToString().ToLower() == "y")
                {
                    Console.WriteLine("No. | Config Name");
                    foreach (string file in files)
                    {
                        amountOfConfigsFound++;
                        Console.WriteLine(amountOfConfigsFound + " | " + file);
                    }

                    Console.WriteLine("Please input config No. you want to use:");
                    var configToUseNumber = Console.ReadLine()?.Trim();
                    int.TryParse(configToUseNumber, out var configToUseNumberConverted);
                    if (configToUseNumberConverted <= amountOfConfigsFound)
                    {
                        foreach (string file in files)
                        {
                            counter++;
                            if (counter == configToUseNumberConverted)
                            {
                                return file;
                            }
                        }
                    }
                }
                else
                {
                    // If we are loading default config, then I decided to reload it each time the game starts (so user cannot make a fake default conf).
                    ReloadStandardConfig();
                }
            }
            else
            {
                // If we are loading default config, then I decided to reload it each time the game starts (so user cannot make a fake default conf).
                ReloadStandardConfig();
            }

            return GlobalVariables.fileNameStandardConfig;
        }

        // Just a small help method that will declares a new config (default one) and saves it to the correct location.
        public static void ReloadStandardConfig()
        {
            var conf = new GameConfig();

            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var confJsonStr = JsonSerializer.Serialize(conf, jsonOptions);

            File.WriteAllText(GlobalVariables.fileNameStandardConfig, confJsonStr);
        }

        // Method that asks all the needed information before we could upload config to database using UploadCurrentConfig().
        public static string UploadCurrentConfigUi()
        {
            // DECLARATIONS START
            string confLocation = "";
            int amountOfConfigsFound = 0;
            int counter = 0;
            string[] files = Directory.GetFiles(GlobalVariables.ReturnConfigFolderLocation());
            // DECLARATIONS OVER

            // Getting the name for the config
            Console.WriteLine("How do you want to name the config?");
            string nameOfConfig = Console.ReadLine()!.Trim();

            // Getting the full location for the config
            Console.WriteLine("You have {0} different custom configs in your folder.", files.Length);
            Console.WriteLine("No. | Config Name");
            foreach (string file in files)
            {
                amountOfConfigsFound++;
                Console.WriteLine(amountOfConfigsFound + " | " + file);
            }

            Console.WriteLine("Please input config No. you want to upload:");
            var configToUseNumber = Console.ReadLine()?.Trim();
            int.TryParse(configToUseNumber, out var configToUseNumberConverted);
            if (configToUseNumberConverted <= amountOfConfigsFound)
            {
                foreach (string file in files)
                {
                    counter++;
                    if (counter == configToUseNumberConverted)
                    {
                        confLocation = file;
                    }
                }
            }

            if (confLocation != "")
            {
                BsBrain.UploadCurrentConfig(nameOfConfig, confLocation);
                return "Successfully uploaded to database!";
            }
            else
            {
                return "Something went wrong while upload.";
            }
        }

        // SHIP PLACEMENT UI
        public static BsBrain PlaceShipUi(GameConfig conf)
        {
            BsBrain brain = new BsBrain(conf);
            for (int playerNo = 0; playerNo < 2; playerNo++)
            {
                brain.CurrentPlayerNo = playerNo;
                for (int shipCount = 0; shipCount < conf.ShipConfigs.Count; shipCount++)
                {
                    for (int shipQuantity = 0; shipQuantity < conf.ShipConfigs[shipCount].Quantity; shipQuantity++)
                    {
                        var placedShip = false;
                        do
                        {
                            bool done;
                            char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                            var list = new List<Coordinate>();
                            int i = 0; // small counter to check if the user did not input the correct coordinates
                            do
                            {
                                list = new List<Coordinate>();
                                int[] xy = new int[2];

                                Console.Clear();
                                Console.WriteLine("Current Player: {0}", playerNo + 1);
                                BSConsoleUI.DrawBoard(false,brain.GetUserBoard());
                                if (i != 0) { 
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Please input the correct coordinates!");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }

                                Console.WriteLine("Current ship:\nName: {0}, Quantity: {1}",
                                    conf.ShipConfigs[shipCount].Name,
                                    conf.ShipConfigs[shipCount].Quantity);
                                Console.WriteLine("Input X");
                                var x = Console.ReadLine()?.Trim();
                                bool tryParse = int.TryParse(x, out var xConverted);
                                Console.WriteLine("Input Y");
                                var y = Console.ReadLine()?.Trim();
                                int.TryParse(y, out var yConverted);

                                if (tryParse)
                                {
                                    xy[0] = xConverted;
                                }
                                else
                                {
                                    for (int loop = 0; loop < conf.BoardSizeX; loop++)
                                    {
                                        if (x?.ToUpper() == letters[loop].ToString().ToUpper())
                                        {
                                            xy[0] = loop;
                                            break;
                                        }
                                    } 
                                }
                                xy[1] = yConverted;

                                for (int sizeX = 0; sizeX < conf.ShipConfigs[shipCount].ShipSizeX; sizeX++)
                                {
                                    for (int sizeY = 0; sizeY < conf.ShipConfigs[shipCount].ShipSizeY; sizeY++)
                                    {
                                        list.Add(new Coordinate(
                                            xy[1] + sizeY,
                                            xy[0] + sizeX)
                                        );
                                    }
                                }

                                done = brain.CheckCondition(list, conf);
                                i++;
                            } while (!done); 
                            
                            brain.PlaceShip(conf.ShipConfigs[shipCount].Name, list);

                            Console.Clear(); // Clears the board again and previews the ship.
                            Console.WriteLine("Current Player: {0}", playerNo + 1);
                            BSConsoleUI.DrawBoard(false,brain.GetUserBoard());
                            Console.WriteLine("Is this location suitable for you? (Y/n)");
                            var yesNo = Console.ReadKey(true);
                            if (yesNo.Key.ToString().ToLower() != "n")
                            {
                                placedShip = true;
                            }
                            else
                            {
                                brain.RemoveShip(conf.ShipConfigs[shipCount].Name, list);
                            }
                            
                        } while (!placedShip);
                    }
                }
            }
            
            brain.CurrentPlayerNo = 0;
            return brain;
        }
        
    }
}