using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using BattleShipConsoleApp;
using DAL;

namespace BattleShipBrain
{
    public class BsBrain
    {
        public int CurrentPlayerNo;
        public GameBoard[] GameBoards = new GameBoard[2];
        private EShipTouchRule _rule = new EShipTouchRule();
        public readonly Random Rnd = new Random();
        
        public BsBrain(GameConfig config)
        {
            GameBoards[0] = new GameBoard();
            GameBoards[1] = new GameBoard();

            GameBoards[0].Board = new BoardSquareState[config.BoardSizeX, config.BoardSizeY];
            GameBoards[1].Board = new BoardSquareState[config.BoardSizeX, config.BoardSizeY];
        }

        // Function that converts Zero to One and One to Zero
        public int OneToZero(int i)
        {
            return i == 0 ? 1 : 0;
        }
        
        // Placing ships logic
        public void PlaceShip(string shipName, List<Coordinate> coordinates)
        {
            GameBoards[CurrentPlayerNo].Ships.Add(new Ship(shipName, coordinates));
            foreach (var coordinate in coordinates)
            {
                GameBoards[CurrentPlayerNo].Board[coordinate.X, coordinate.Y].IsShip = true;
            }
        }

        // Removing ships logic is used while deciding where to place the ships
        public void RemoveShip(string shipName, List<Coordinate> coordinates)
        {
            GameBoards[CurrentPlayerNo].Ships.Remove(new Ship(shipName, coordinates));

            foreach (var coordinate in coordinates)
            {
                GameBoards[CurrentPlayerNo].Board[coordinate.X, coordinate.Y].IsShip = false;
            }
        }

        // Check condition is used to check if it is possible to place ships in such place or not.
        public bool CheckCondition(List<Coordinate> coordinates, GameConfig config)
        {
            foreach (var coordinate in coordinates)
            {
                try
                {
                    if (GameBoards[CurrentPlayerNo].Board[coordinate.X, coordinate.Y].IsShip) return false;
                }
                catch
                {
                    return false;
                }

                switch (_rule)
                {
                    case EShipTouchRule.NoTouch:
                        try
                        {
                            if (coordinate.X+1 < config.BoardSizeX && GameBoards[CurrentPlayerNo].Board[coordinate.X + 1, coordinate.Y].IsShip)
                                return false;
                            if (coordinate.Y+1 < config.BoardSizeY && GameBoards[CurrentPlayerNo].Board[coordinate.X, coordinate.Y + 1].IsShip)
                                return false;
                            if (coordinate.X-1 >= 0 && GameBoards[CurrentPlayerNo].Board[coordinate.X - 1, coordinate.Y].IsShip)
                                return false;
                            if (coordinate.Y-1 >= 0 && GameBoards[CurrentPlayerNo].Board[coordinate.X, coordinate.Y - 1].IsShip)
                                return false;
                            if (coordinate.X+1 < config.BoardSizeX && coordinate.Y+1 < config.BoardSizeY && GameBoards[CurrentPlayerNo].Board[coordinate.X + 1, coordinate.Y + 1].IsShip)
                                return false;
                            if (coordinate.X+1 < config.BoardSizeX && coordinate.Y-1 >= 0 && GameBoards[CurrentPlayerNo].Board[coordinate.X + 1, coordinate.Y - 1].IsShip)
                                return false;
                            if (coordinate.X-1 >= 0 && coordinate.Y+1 < config.BoardSizeY && GameBoards[CurrentPlayerNo].Board[coordinate.X - 1, coordinate.Y + 1].IsShip)
                                return false;
                            if (coordinate.X-1 >= 0 && coordinate.Y-1 >= 0 && GameBoards[CurrentPlayerNo].Board[coordinate.X - 1, coordinate.Y - 1].IsShip)
                                return false;
                        }
                        catch
                        {
                            return false;
                        }
                        break;

                    case EShipTouchRule.CornerTouch:
                        try
                        {
                            if (coordinate.X+1 < config.BoardSizeX && GameBoards[CurrentPlayerNo].Board[coordinate.X + 1, coordinate.Y].IsShip)
                                return false;
                            if (coordinate.Y+1 < config.BoardSizeY && GameBoards[CurrentPlayerNo].Board[coordinate.X, coordinate.Y + 1].IsShip)
                                return false;
                            if (coordinate.X-1 >= 0 && GameBoards[CurrentPlayerNo].Board[coordinate.X - 1, coordinate.Y].IsShip)
                                return false;
                            if (coordinate.Y-1 >= 0 && GameBoards[CurrentPlayerNo].Board[coordinate.X, coordinate.Y - 1].IsShip)
                                return false;
                            if (coordinate.X+1 < config.BoardSizeX && coordinate.Y+1 < config.BoardSizeY && GameBoards[CurrentPlayerNo].Board[coordinate.X + 1, coordinate.Y + 1].IsShip)
                                return false;
                            if (coordinate.X-1 >= 0 && coordinate.Y-1 >= 0 && GameBoards[CurrentPlayerNo].Board[coordinate.X - 1, coordinate.Y - 1].IsShip)
                                return false;
                        }
                        catch
                        {
                            return false;
                        }
                        break;
                }
            }
            return true;
        }

        // Placing bomb
        public bool PlaceBomb(int[] xy)
        {
            GameBoards[OneToZero(CurrentPlayerNo)].Board[xy[0], xy[1]].IsBomb = true;
            if (GameBoards[OneToZero(CurrentPlayerNo)].Board[xy[0], xy[1]].IsShip) return true;

            return false;
        }

        // Check if the following coordinate has bomb or not. 
        public bool CheckForBomb(int x, int y)
        {
            if (x > -1 && y > -1)
            {
                if (GameBoards[OneToZero(CurrentPlayerNo)].Board[x, y].IsBomb) return true;
            }
            return false;
        }

        // Returning enemies game board
        public BoardSquareState[,] GetEnemyBoard()
        {
            return GameBoards[OneToZero(CurrentPlayerNo)].Board;
        }

        // Return user own game board
        public BoardSquareState[,] GetUserBoard()
        {
            return GameBoards[CurrentPlayerNo].Board;
        }

        // Check if ship is fully shot
        public bool IsShipSunk(int[] xy)
        {
            // Get the ship that is on these coordinates
            foreach (var ship in GameBoards[OneToZero(CurrentPlayerNo)].Ships)
            {
                if (ship.Coordinates.Any(coordinate => coordinate.X.Equals(xy[0]) && coordinate.Y.Equals(xy[1])))
                {
                    return ship.IsShipSunk(GameBoards[OneToZero(CurrentPlayerNo)].Board);
                }
            }

            return false;
        }
        
        // Get full size of ship by coordinate
        public int ShipSize(int[] xy)
        {
            // Get the ship that is on these coordinates
            foreach (var ship in GameBoards[OneToZero(CurrentPlayerNo)].Ships)
            {
                if (ship.Coordinates.Any(coordinate => coordinate.X.Equals(xy[0]) && coordinate.Y.Equals(xy[1])))
                {
                    return ship.GetShipSize();
                }
            }
            return 0;
        }
        
        // Get left size of ship by coordinate
        public int LeftShipSize(int[] xy)
        {
            // Get the ship that is on these coordinates
            foreach (var ship in GameBoards[OneToZero(CurrentPlayerNo)].Ships)
            {
                if (ship.Coordinates.Any(coordinate => coordinate.X.Equals(xy[0]) && coordinate.Y.Equals(xy[1])))
                {
                    return (ship.GetShipSize() - ship.GetShipDamageCount(GameBoards[OneToZero(CurrentPlayerNo)].Board));
                }
            }
            return 0;
        }
        
        // If true, then won
        public bool GameWinCondition()
        {
            return GameBoards[OneToZero(CurrentPlayerNo)].Ships.All(ship => ship.IsShipSunk(GetEnemyBoard()));
        }
        
        // Save game to local/ database
        public void GetBrainJson(int saveDecisionParsed, string nameOfGame)
        { 
            string savedGameFile = GlobalVariables.ReturnGameSaveFolderLocation() + Path.DirectorySeparatorChar + nameOfGame + ".json";
            
            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var dto = new SaveGameDto();

            dto.SetGameBoard(GameBoards);
            dto.CurrentPlayerNo = CurrentPlayerNo;
            
            var jsonStr = JsonSerializer.Serialize(dto, jsonOptions);
            
            if (saveDecisionParsed == 1)
            {
                File.WriteAllText(savedGameFile, jsonStr);
            }
            else if (saveDecisionParsed == 2)
            {
                SaveGameToDatabase(jsonStr);
            }
            else if (saveDecisionParsed == 3)
            {
                File.WriteAllText(savedGameFile, jsonStr);
                SaveGameToDatabase(jsonStr);
            }
        }
        
        // Saves Every Game State
        public void GameSaveState(Stack gameSaveState)
        {
            //Stack GameSaveState = new Stack();
            
            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var dto = new SaveGameDto();

            dto.SetGameBoard(GameBoards);
            dto.CurrentPlayerNo = CurrentPlayerNo;
            
            var jsonStr = JsonSerializer.Serialize(dto, jsonOptions);
           
            gameSaveState.Push(jsonStr);
        }
        
        // Save game to database
        public void SaveGameToDatabase(string jsonStr)
        {
            using (var db = new ApplicationDbContext())
            {
                var gameConfigDatabase = new Domain.GameSave();
                gameConfigDatabase.SavedGame = jsonStr;
                gameConfigDatabase.GameSaveDate = DateTime.Now;
                db.SavedGames.Add(gameConfigDatabase);
                db.SaveChanges();
            }
        }
        
        // Restore game from Game Save - both Local and Database (depending on which you choose on start).
        // The logic is: we pass savedGameFile and loadWay.
        // loadWay: on first, if it is eq 1, then load from local. If it is eq 2, then load from database. 
        // savedGame = full location to the game save file in case we parse from local. In case we parse from the Databse, then it is the id of the game.
        // runMethod is the way of running. First time we run it with 0. If we did not succeed, then player could try to search for the saved game with another method (if it was local -> database, if database -> local).
        // return integer is used, to understand success of the operation.
        public int RestoreBrainFromJson(int loadWay, string savedGame, int runMethod)
        {
            string json = "";

            // Initial load using the way the user chose (Local / Database).
            if (runMethod == 0)
            {
                switch (loadWay)
                {
                    case 1:
                        json = LocalGameLoad(savedGame); // In case of Local, we parse the full location of the saved game to the method.
                        break;
                    case 2:
                        json = DatabaseGameLoad(savedGame); // In case of Database, we parse the id of the saved game to the method. 
                        break;
                }
            }

            // If the Game Config was not found we may ask to try to load from different source.
            if (runMethod == 1)
            {
                switch (loadWay)
                    {
                        case 1:
                            json = LocalGameLoad(savedGame);
                            break;
                        case 2:
                            json = DatabaseGameLoad(savedGame);
                            break; 
                    }
            }

            // Checking for json still eq. "". If so, then skipping loading.
            if (json != "")
            {
                var dto = JsonSerializer.Deserialize<SaveGameDto>(json);
                var gameBoardArray = dto!.GetGameBoard();
                GameBoards[0] = gameBoardArray[0];
                GameBoards[1] = gameBoardArray[1];
                CurrentPlayerNo = dto.CurrentPlayerNo;
                return 0; // Success
            }
            else
            {
                return 1; // No success
            }
        }

        // Load game save from local file. 
        private static string LocalGameLoad(string confText)
        {
            string json = "";
            if (File.Exists(confText))
            {
                json = File.ReadAllText(confText);
            }
            return json;
        }
        
        // Load game save from database file.
        public static string DatabaseGameLoad(string idOfTheSave)
        {
            int idOfTheSaveParsed = int.Parse(idOfTheSave);
            string json = "";
            
            using (var db = new ApplicationDbContext())
            {
                if (db.SavedGames.Any())
                {
                    foreach (var entry in db.SavedGames)
                    {
                        if (entry.SavedGameId == idOfTheSaveParsed)
                        {
                            json = entry.SavedGame;
                        }
                    }
                }
            }
            return json;
        }
        
        // Proper JsonBuilder
        public static string JsonBuilder(int[] boardXyGrabbed, int touchRule, GameConfig conf)
        {
            conf.BoardSizeX = boardXyGrabbed[0];
            conf.BoardSizeY = boardXyGrabbed[1];
            
            switch (touchRule)
            {
                case 1:
                    conf.EShipTouchRule = EShipTouchRule.NoTouch;
                    break;
                case 2:
                    conf.EShipTouchRule = EShipTouchRule.CornerTouch;
                    break;
                case 3:
                    conf.EShipTouchRule = EShipTouchRule.SideTouch;
                    break;
            }
            
            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var confJsonStr = JsonSerializer.Serialize(conf, jsonOptions);
            
            return confJsonStr;
        }
        
        // Upload Current Config To DataBase
        public static void UploadCurrentConfig(string configName, string confLocation)
        { 
            var gameConfigDatabase = new Domain.GameConfig();
            string json = File.ReadAllText(confLocation);
            
            using (var db = new ApplicationDbContext())
            {
                gameConfigDatabase.ConfigName = configName;
                gameConfigDatabase.ConfigJson = json;
                gameConfigDatabase.ConfigBuildDate = DateTime.Now;
                
                db.GameConfigs.Add(gameConfigDatabase);
                db.SaveChanges();
            }
        }
        
        // Random ship placement
        public BsBrain RandomShipPlacement(GameConfig config)
        { 
            BsBrain brain = new BsBrain(config);
            bool brokenLoop;
            do
            {
                brokenLoop = false;
                brain = new BsBrain(config);
                for (int playerNo = 0; playerNo < 2; playerNo++)
                {
                    brain.CurrentPlayerNo = playerNo;
                    foreach (var ship in config.ShipConfigs)
                    {
                        for (int shipQuantity = 0; shipQuantity < ship.Quantity; shipQuantity++)
                        {
                            bool done;
                            var list = new List<Coordinate>();
                            int checkForPossibility = 0;

                            do
                            {
                                list = new List<Coordinate>();
                                int[] xy = new int[2];

                                xy[0] = brain.Rnd.Next(0, config.BoardSizeX-ship.ShipSizeX);
                                xy[1] = brain.Rnd.Next(0, config.BoardSizeY-ship.ShipSizeY+1);

                                for (int sizeX = 0; sizeX < ship.ShipSizeX; sizeX++)
                                {
                                    for (int sizeY = 0;
                                        sizeY < ship.ShipSizeY;
                                        sizeY++)
                                    {
                                        list.Add(new Coordinate(
                                            xy[1] + sizeY,
                                            xy[0] + sizeX)
                                        );
                                    }
                                }
                                done = brain.CheckCondition(list, config);
                                
                                if (!done)
                                {
                                    if (checkForPossibility > 10)
                                    {
                                        brokenLoop = true;
                                        break;
                                    }

                                    checkForPossibility++;
                                }
                            } while (!done);
                            
                            if (!brokenLoop)
                            {
                                brain.PlaceShip(ship.Name, list);
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (brokenLoop)
                        {
                            break;
                        }
                    }
                    if (brokenLoop)
                    {
                        break;
                    }
                }
            } while (brokenLoop);
            
            brain.CurrentPlayerNo = 0;
            return brain;
        }
        
        // Tried to make up a formula, which will logically test everything.
        // However, I did not succeed with that, so decided to test the config by trying to randomly generate the working game board.
        // If it fails after 50 time, then we suppose the config is impossible to play.
        public static bool ShipTester(GameConfig config)
        {
            BsBrain brain = new BsBrain(config);
            bool brokenLoop;
            do
            {
                brokenLoop = false;
                brain = new BsBrain(config);
                foreach (var ship in config.ShipConfigs)
                {
                    for (int shipQuantity = 0; shipQuantity < ship.Quantity; shipQuantity++)
                    {
                        bool done;
                        var list = new List<Coordinate>();
                        int checkForPossibility = 0;

                        do
                        {
                            list = new List<Coordinate>();
                            int[] xy = new int[2];

                            xy[0] = brain.Rnd.Next(0, config.BoardSizeX - ship.ShipSizeX);
                            xy[1] = brain.Rnd.Next(0, config.BoardSizeY - ship.ShipSizeY + 1);

                            for (int sizeX = 0; sizeX < ship.ShipSizeX; sizeX++)
                            {
                                for (int sizeY = 0; sizeY < ship.ShipSizeY; sizeY++)
                                {
                                    list.Add(new Coordinate(
                                        xy[1] + sizeY,
                                        xy[0] + sizeX)
                                    );
                                }
                            }

                            done = brain.CheckCondition(list, config);
                            brain.PlaceShip(ship.Name, list);
                                    
                            if (!done)
                            {
                                if (checkForPossibility > 50)
                                {
                                    return false;
                                }
                                checkForPossibility++;
                            }
                        } while (!done);
                    }
                }
            } while (brokenLoop);

            return true;
        }
    }
}