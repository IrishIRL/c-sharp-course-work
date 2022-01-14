using System.Collections.Generic;
using System.Threading.Tasks;
using BattleShipConsoleApp;
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
        public string[] LocalSavedFiles { get; set; } = GlobalVariables.ReturnCutNames(0);
        
        public async Task OnGetAsync()
        {
            GameConfig = await _context.GameConfigs.ToListAsync();
        }
    }
}