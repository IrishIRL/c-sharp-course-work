using System;
using BattleShipBrain;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GameConfig = BattleShipBrain.GameConfig;

namespace WebApp.Pages.Game
{
    public class Index : PageModel
    {
        public static GameConfig _conf = new GameConfig();
        public static BsBrain _brain = new BsBrain(_conf);
        
        public int GameId { get; set; } = default!;
        
        public void OnGet(int gameId)
        {
            if (gameId != null && gameId != 0)
            {
                // loadWay 2 means load from database. savedGame takes id of the game in database.
                // runMethod could be both 0 or 1, but since we will try to load game only once, there is no need to set it to 0.
                GameId = gameId;
                int errorLogging = Index._brain.RestoreBrainFromJson(2, gameId.ToString(), 1);
                Console.WriteLine(errorLogging); // just for us to know, if this is eq to 0, then everything went well. 1 means error occured.
            }
        }
    }
}