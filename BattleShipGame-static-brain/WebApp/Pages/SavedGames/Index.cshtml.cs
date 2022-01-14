using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_SavedGames
{
    public class IndexModel : PageModel
    {
        public static BattleShipBrain.GameConfig _conf = new BattleShipBrain.GameConfig();
        public static BattleShipBrain.BsBrain _brain = new BattleShipBrain.BsBrain(_conf);
        public int SuccessfullySavedGame { get; set; }

        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<GameSave> GameSave { get;set; } = default!;

        public async Task OnGetAsync(bool saveToLocal, int gameId, int saveStatus)
        {
            if (saveStatus == 1)
            {
                SuccessfullySavedGame = 1;
            }
            else if (saveStatus == 2)
            {
                SuccessfullySavedGame = 2; 
            }
            
            if (saveToLocal)
            {
                if (gameId != null && gameId != 0)
                {
                    string nameOfGame = "DBSave_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss") + "_id_" + gameId;
                    int success = _brain.RestoreBrainFromJson(2, gameId.ToString(), 1);
                    Console.WriteLine("success: " + success + ". If 0 then success, if 1 then no success");
                    _brain.GetBrainJson(1, nameOfGame, false);
                    Console.WriteLine("Saved game " + gameId + " with name: " + nameOfGame);

                    Response.Redirect("SavedGames?saveStatus=" + success+1);
                }
            }
            GameSave = await _context.SavedGames.ToListAsync();
        }
    }
}
