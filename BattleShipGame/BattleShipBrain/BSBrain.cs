using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using BattleShipConsoleApp;
using DAL;

namespace BattleShipBrain
{
    public class BsBrain
    {
        public int _currentPlayerNo = 0;
        private static GameBoard[] _gameBoards = new GameBoard[2];
        
        private readonly Random _rnd = new Random();
        
        public BsBrain(GameConfig config)
        {
            _gameBoards[0] = new GameBoard();
            _gameBoards[1] = new GameBoard();
            
            _gameBoards[0].Board = new BoardSquareState[config.BoardSizeX, config.BoardSizeY];
            _gameBoards[1].Board = new BoardSquareState[config.BoardSizeX, config.BoardSizeY];

            /* Random ship placement
            for (var x = 0; x < config.BoardSizeX; x++)
            {
                for (var y = 0; y < config.BoardSizeY; y++)
                {
                    _gameBoards[0].Board[x, y] = new BoardSquareState
                    {
                        IsShip = _rnd.Next(0, 2) != 0
                    };
                    
                    _gameBoards[1].Board[x, y] = new BoardSquareState
                    {
                        IsShip = _rnd.Next(0, 2) != 0
                    };
                }
            }
            */
        }

        // Placing ships logic
        public void PlaceShip(int[] xy)
        {
            switch (_currentPlayerNo)
            {
                case 0:
                    _gameBoards[0].Board[xy[0], xy[1]].IsShip = true;
                    break;
                case 1:
                    _gameBoards[1].Board[xy[0], xy[1]].IsShip = true;
                    break;
            }
        }
        
        // Placing bomb
        public void PlaceBomb(int[] xy)
        {
            switch (_currentPlayerNo)
            {
                case 0:
                    _gameBoards[0].Board[xy[0], xy[1]].IsBomb = true;
                    break;
                case 1:
                    _gameBoards[1].Board[xy[0], xy[1]].IsBomb = true;
                    break;
            }
        }
        
        // Returning enemies game board
        public BoardSquareState[,] GetEnemyBoard()
        {
            return _gameBoards[_currentPlayerNo].Board;
        }
        
        // Return user own game board
        public BoardSquareState[,] GetUserBoard()
        {
            if(_currentPlayerNo == 0) return _gameBoards[1].Board;
            if(_currentPlayerNo == 1) return _gameBoards[0].Board;
            else
            {
                return _gameBoards[_currentPlayerNo].Board;
            }
        }
        
        // Save game to local/ database
        public void GetBrainJson(int saveDecisionParsed, string nameOfGame)
        { 
            string savedGameFile = GlobalVariables.ReturnGameSaveFolderLocation() + Path.DirectorySeparatorChar + nameOfGame + ".json";;
            
            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var dto = new SaveGameDto();

            dto.SetGameBoard(_gameBoards);
            dto.CurrentPlayerNo = _currentPlayerNo;
            
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
        // savedGame = full location to the game save file in case we parse 
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
                _gameBoards[0] = gameBoardArray[0];
                _gameBoards[1] = gameBoardArray[1];
                _currentPlayerNo = dto.CurrentPlayerNo;
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
        private static string DatabaseGameLoad(string idOfTheSave)
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
    }
}