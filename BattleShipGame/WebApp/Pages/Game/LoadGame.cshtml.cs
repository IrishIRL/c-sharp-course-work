using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
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

        public async Task OnGetAsync()
        {
            GameSave = await _context.SavedGames.ToListAsync();
        }
    }
}