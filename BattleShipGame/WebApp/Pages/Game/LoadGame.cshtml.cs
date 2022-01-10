using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BattleShipConsoleApp;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp;
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
        public string[] LocalSavedFiles { get; set; } = GlobalVariables.ReturnCutNames(1);
        public async Task OnGetAsync()
        {
            GameSave = await _context.SavedGames.ToListAsync();
        }
    }
}