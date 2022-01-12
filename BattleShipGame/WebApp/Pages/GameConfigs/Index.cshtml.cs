using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace WebApp.Pages_GameConfigs
{
    public class IndexModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public IndexModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<GameConfig> GameConfig { get; set; } = default!;
        public static BattleShipBrain.GameConfig Conf { get; set; } = new  BattleShipBrain.GameConfig();
        public async Task OnGetAsync()
        {
            GameConfig = await _context.GameConfigs.ToListAsync();
        }
    }
}
