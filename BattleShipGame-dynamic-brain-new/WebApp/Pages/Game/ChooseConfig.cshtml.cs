using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BattleShipBrain;
using BattleShipConsoleApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Game
{
    public class ChooseConfig : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public ChooseConfig(DAL.ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<Domain.GameConfig> GameConfig { get; set; } = default!;
        [BindProperty]
        public string ConfigName { get; set; } = default!;
        public string[] LocalSavedFiles { get; set; } = GlobalVariables.ReturnCutNames(0);
        
        public async Task OnGetAsync()
        {
            GameConfig = await _context.GameConfigs.ToListAsync();
        }

        [BindProperty]
        public int ConfigId { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            var gameId = new DBAccess().BuildNewGame(ConfigId).Result;
            return RedirectToPage("./SetShips", new {gameId = gameId, configId = ConfigId});
        }
        
        public async Task<IActionResult> OnPostLoadLocalConf()
        {
            if (ConfigName != null || ConfigName != "")
            {
                var localConf = GlobalVariables.ReturnConfigFolderLocation() + @"\" + ConfigName; 
                ConfigId = new DBAccess().LocalConfLoad(localConf);
                var gameId = new DBAccess().BuildNewGame(ConfigId).Result;
                return RedirectToPage("./SetShips", new {gameId = gameId, configId = ConfigId});
            }
            return RedirectToPage("./ChooseConfig", new {gameId = 0});
        }
        
    }
}