using System;
using System.Text.Json;
using BattleShipBrain;
using BattleShipConsoleApp;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GameConfig = BattleShipBrain.GameConfig;

namespace WebApp.Pages.Game
{
    public class Index : PageModel
    {
        public static string ConfigText = "";
        public int GameId { get; set; }
        
        private readonly DAL.ApplicationDbContext _context;
        public static int TurnCounter { get; set; }
        public static int NowTurn { get; set; }
        
        public Index(DAL.ApplicationDbContext context)
        {
            _context = context;
        }
        /*
        public void OnGet(string gameId, string configId)
        {
            int gameIdNumber;
            bool isParsableGameNo = Int32.TryParse(gameId, out gameIdNumber);
            bool zeroGame = isParsableGameNo && gameIdNumber == 0;
            
            int configIdNumber;
            bool isParsableConfigNo = Int32.TryParse(configId, out configIdNumber);
            bool zeroConfig = isParsableConfigNo && configIdNumber == 0;

            if (gameId != null && !zeroGame)
            {
                int loadWay;
                // loadWay 1 means load from local.
                // loadWay 2 means load from database.
                // runMethod could be both 0 or 1, but since we will try to load game only once, there is no need to set it to 0.
                if (isParsableGameNo)
                {
                    GameId = gameIdNumber;
                    loadWay = 2;
                }
                else
                {
                    GameId = 1;
                    loadWay = 1;
                    gameId = GlobalVariables.ReturnGameSaveFolderLocation() + @"\" + gameId;
                }
                
                int errorLogging = Brain.RestoreBrainFromJson(loadWay, gameId, 1);
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
                    ConfigText = confText;
                    Conf = JsonSerializer.Deserialize<GameConfig>(ConfigText)!;
                    Response.Redirect("Game/SetShips?resetGame=true");
                }
            }
        }*/
    }
}