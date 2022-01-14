using System;
using System.Text.Json;
using BattleShipBrain;
using BattleShipConsoleApp;
using DAL;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GameConfig = BattleShipBrain.GameConfig;

namespace WebApp.Pages.Game
{
    public class Index : PageModel
    {
        public Domain.GameConfig GameConfig { get; set; } = default!;
        //public static string ConfigText = "";
        public int GameId { get; set; }
        
        private readonly DAL.ApplicationDbContext _context;

        public Index(DAL.ApplicationDbContext context)
        {
            _context = context;
        }
        
        public void OnGet(string gameId, string configId)
        {
            var Conf = new GameConfig(); 
            var Brain = new BsBrain(Conf);
            int gameIdNumber;

            int configIdNumber;
            bool isParsableConfigNo = Int32.TryParse(configId, out configIdNumber);
            bool zeroConfig = isParsableConfigNo && configIdNumber == 0;

            
                var jsonOptions = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };

                var dto = new SaveGameDto();
                dto.SetGameBoard(Brain.GameBoards);
                dto.CurrentPlayerNo = Brain.CurrentPlayerNo;
            
                var jsonStr = JsonSerializer.Serialize(dto, jsonOptions);
                
                using (var db = new ApplicationDbContext())
                {
                    var gameConfigDatabase = new Domain.GameSave();
                    gameConfigDatabase.SavedGame = jsonStr;
                    gameConfigDatabase.GameSaveDate = DateTime.Now;
                    db.SavedGames.Add(gameConfigDatabase);
                    db.SaveChanges();
                    GameId = gameConfigDatabase.SavedGameId;
                    gameId = GameId.ToString();
                }
            
            
                bool isParsableGameNo = Int32.TryParse(gameId, out gameIdNumber);
                bool zeroGame = isParsableGameNo && gameIdNumber == 0;
                
            if (gameId != null && !zeroGame)
            {
                int loadWay;
                // loadWay 1 means load from local.
                // loadWay 2 means load from database.
                // runMethod could be both 0 or 1, but since we will try to load game only once, there is no need to set it to 0.
                if (isParsableGameNo)
                {
                    //GameId = gameIdNumber;
                    loadWay = 2;
                }
                else
                {
                    //GameId = 1;
                    loadWay = 1;
                    gameId = GlobalVariables.ReturnGameSaveFolderLocation() + @"\" + gameId;
                }

                Console.WriteLine(gameId);
                int errorLogging = Brain.RestoreBrainFromJson(loadWay, gameId, 1);
                Console.WriteLine(errorLogging); // just for us to know, if this is eq to 0, then everything went well. 1 means error occured.
            }

            if (configId != null && !zeroConfig)
            {
                string confText;

                if (isParsableConfigNo)
                {
                    var config = _context.GameConfigs.Find(configIdNumber);
                    confText = config.ConfigJson;
                }
                else
                {
                    var configFile = GlobalVariables.ReturnConfigFolderLocation() + @"\" + configId;
                    confText = System.IO.File.ReadAllText(configFile);
                }
                
                if (confText != "")
                {
                    //ConfigText = confText;
                    Conf = JsonSerializer.Deserialize<GameConfig>(confText)!;
                    Response.Redirect("Game/SetShips?resetGame=true&confId="+configId+"&gameId="+gameId);
                }
            }
        }
    }
}