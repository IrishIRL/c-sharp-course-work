using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BattleShipBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using GameConfig = Domain.GameConfig;
using JsonSerializer = System.Text.Json.JsonSerializer;
using ShipConfig = Domain.ShipConfig;

namespace WebApp.Pages_GameConfigs
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public EShipTouchRule EShipTouchRuleReceived { get; set; }
        
        [BindProperty]
        public int BoardSizeX { get; set; } = 10;
        
        [BindProperty]
        public int BoardSizeY { get; set; } = 10;

        [BindProperty] public List<ShipConfig> ShipConfigs { get; set; } = new List<ShipConfig>();
        public string ShipName { get; set; } = "";
        
        [BindProperty]
        public int ShipSizeX { get; set; } = 1;
        
        [BindProperty]
        public int ShipSizeY { get; set; } = 1;
        
        [BindProperty]
        public int ShipCount { get; set; } = 1;
        
        public DetailsModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public GameConfig GameConfig { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GameConfig = await _context.GameConfigs.FirstOrDefaultAsync(m => m.ConfigId == id);
            
            if (GameConfig == null)
            {
                return NotFound();
            }
            
            IndexModel.Conf = JsonSerializer.Deserialize<BattleShipBrain.GameConfig>(GameConfig.ConfigJson);

            if (IndexModel.Conf != null)
            {
                BoardSizeX = IndexModel.Conf.BoardSizeX;
                BoardSizeY = IndexModel.Conf.BoardSizeY;
                EShipTouchRuleReceived = IndexModel.Conf.EShipTouchRule;
                Console.WriteLine(IndexModel.Conf.ShipConfigs.Count);
                Console.WriteLine(IndexModel.Conf.ShipConfigs[0].Name);
                Console.WriteLine(IndexModel.Conf.ShipConfigs[1].Name);
                foreach (var ship in IndexModel.Conf.ShipConfigs)
                {
                    var shipConfig = new ShipConfig
                    {
                        ShipName = ship.Name,
                        ShipSizeX = ship.ShipSizeX,
                        ShipSizeY = ship.ShipSizeY,
                        ShipQuantity = ship.Quantity
                    };
                    ShipConfigs.Add(shipConfig);
                }
            }
            else
            {
                return NotFound();
            }
            
            return Page();
        }
    }
}
