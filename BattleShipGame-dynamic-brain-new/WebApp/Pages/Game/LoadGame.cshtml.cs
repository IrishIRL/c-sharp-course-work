using System.Collections.Generic;
using System.Threading.Tasks;
using BattleShipConsoleApp;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Game
{
    public class LoadGame : PageModel
    { 
        private readonly DAL.ApplicationDbContext _context;

        public LoadGame(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<GameSave> GameSave { get;set; } = default!;
        [BindProperty]
        public string GameName { get; set; } = default!;
        public string[] LocalSavedFiles { get; set; } = GlobalVariables.ReturnCutNames(1);
        public async Task OnGetAsync()
        {
            GameSave = await _context.SavedGames.ToListAsync();
        }

        public async Task<IActionResult> OnPostLoadLocalGame()
        {
            if (GameName != null || GameName != "")
            {
                var localGameName = GlobalVariables.ReturnGameSaveFolderLocation() + @"\" + GameName; 
                int gameId = new DBAccess().LocalGameLoad(localGameName);
                return RedirectToPage("./Play", new {gameId = gameId});
            }
            return RedirectToPage("./LoadGame", new {gameId = 0});
        }
    }
}