using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleShipBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using GameConfig = Domain.GameConfig;


namespace WebApp.Pages_GameConfigs
{
    public class CreateModel : PageModel
    {
        public static BattleShipBrain.GameConfig _conf = new BattleShipBrain.GameConfig();
        public static BattleShipBrain.BsBrain _brain = new BattleShipBrain.BsBrain(_conf);

        private int BoardSizeX { get; set; } = 10;
        private int BoardSizeY { get; set; } = 10;
        private int EShipTouchRuleInt { get; set; } = 0;
        private string ShipName { get; set; } = "";
        
        private int ShipSizeX { get; set; } = 1;
        private int ShipSizeY { get; set; } = 1;
        private int ShipCount { get; set; } = 1;
        
        
        private readonly DAL.ApplicationDbContext _context;

        public CreateModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public GameConfig GameConfig { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            _conf.BoardSizeX = BoardSizeX;
            _conf.BoardSizeY = BoardSizeY;
            switch (EShipTouchRuleInt)
            {
                case 0:
                    _conf.EShipTouchRule = EShipTouchRule.NoTouch;
                    break;
                case 1:
                    _conf.EShipTouchRule = EShipTouchRule.CornerTouch;
                    break;
                case 2:
                    _conf.EShipTouchRule = EShipTouchRule.SideTouch;
                    break;
            }
            
            var shipConfig = new BattleShipBrain.ShipConfig()
            {
                Name = ShipName,
                ShipSizeX = ShipSizeX,
                ShipSizeY = ShipSizeY,
                Quantity = ShipCount
            };
            
            _conf.ShipConfigs.Add(shipConfig);

            _context.GameConfigs.Add(GameConfig);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
