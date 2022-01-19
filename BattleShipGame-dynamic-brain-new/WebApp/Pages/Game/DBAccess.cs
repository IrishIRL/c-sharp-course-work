using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using BattleShipBrain;
using BattleShipConsoleApp;
using DAL;
using Domain;
using GameConfig = BattleShipBrain.GameConfig;

namespace WebApp.Pages.Game
{
    public class DBAccess
    {
        // returns created game ID
        public async Task<int> CreateGameInDb(BsBrain brain)
        {
            await using var db = new ApplicationDbContext();
            var gameSave = new GameSave
            {
                SavedGame = brain.GetBrainJson2(),
                GameSaveDate = DateTime.Now
            };
            await db.SavedGames.AddAsync(gameSave);
            await db.SaveChangesAsync();
            return gameSave.SavedGameId;
        }
        
        // returns created conf ID
        public async Task<int> CreateConfInDb(GameConfig config)
        {
            await using var db = new ApplicationDbContext();
            
            
            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var confJsonStr = JsonSerializer.Serialize(config, jsonOptions);

            var gameConfig = new Domain.GameConfig
            {
                ConfigJson = confJsonStr,
                ConfigName = "Restored "+DateTime.Now,
                ConfigBuildDate = DateTime.Now
            };
            await db.GameConfigs.AddAsync(gameConfig);
            await db.SaveChangesAsync();
            return gameConfig.ConfigId;
        }
        
        // transforn DB config into battleship config
        public async Task<GameConfig> GetBrainConfigurationFromDbConfiguration(int configId)
        {
            await using var db = new ApplicationDbContext();
            var gameConfigFromDb = db.GameConfigs.FindAsync(configId).Result;
            return JsonSerializer.Deserialize<GameConfig>(gameConfigFromDb.ConfigJson)!;
        }
        
        public async Task<int> BuildNewGame(int configId)
        {
            var gameConfig = GetBrainConfigurationFromDbConfiguration(configId);
            var brain = new BsBrain(gameConfig.Result);
            return await CreateGameInDb(brain);
        }
        
        // restores brain from db (json)
        public async Task<BsBrain> RestoreGameFromDb(int gameId)
        {
            await using var db = new ApplicationDbContext();
            var gameSave = db.SavedGames.FindAsync(gameId).Result;
            var brain = new BsBrain(new GameConfig());
            brain.RestoreGameFromJson2(gameSave.SavedGame);
            return brain;
        }
        
        // Updates game
        public void UpdateGame(BsBrain brain, int gameId)
        {
            using var db = new ApplicationDbContext();
            var gameSave = db.SavedGames.Find(gameId);
            gameSave.SavedGame = brain.GetBrainJson2();
            db.SavedGames.Update(gameSave); 
            db.SaveChanges();
        }

        public int LocalGameLoad(string confText)
        {
            var brain = new BsBrain(new GameConfig());
            
            string json = "";
            if (File.Exists(confText))
            {
                json = File.ReadAllText(confText);
            }

            if (json != "")
            {
                var dto = JsonSerializer.Deserialize<SaveGameDto>(json);
                brain.GameBoards = dto!.GetGameBoard();
                brain.CurrentPlayerNo = dto!.CurrentPlayerNo;
                return CreateGameInDb(brain).Result;
            }
            
            return 0;
        }
        
        public int LocalConfLoad(string confText)
        {
            var config = new GameConfig();
            string json = "";
            if (File.Exists(confText))
            {
                json = File.ReadAllText(confText);
            }

            if (json != "")
            {
                var dto = JsonSerializer.Deserialize<GameConfig>(json);
                config.BoardSizeX = dto!.BoardSizeX;
                config.BoardSizeY = dto.BoardSizeY;
                config.ShipConfigs = dto.ShipConfigs;
                
                return CreateConfInDb(config).Result;
            }
            
            return 0;
        }
    }
}