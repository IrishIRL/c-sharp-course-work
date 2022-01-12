using System;
using System.Text.Json;
using BattleShipBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;
using GameConfig = Domain.GameConfig;


namespace WebApp.Pages_GameConfigs
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public EShipTouchRule EShipTouchRuleReceived { get; set; }
        
        [BindProperty]
        public int BoardSizeX { get; set; } = 10;
        
        [BindProperty]
        public int BoardSizeY { get; set; } = 10;
        
        [BindProperty]
        public string ShipName { get; set; } = "";
        
        [BindProperty]
        public int ShipSizeX { get; set; } = 1;
        
        [BindProperty]
        public int ShipSizeY { get; set; } = 1;
        
        [BindProperty]
        public int ShipCount { get; set; } = 1;
        
        public bool FirstTime { get; set; } = true;

        [BindProperty]
        public GameConfig GameConfig { get; set; } = default!;
        
        public bool ConfigTested { get; set; }
        public bool Ready { get; set; }
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        public void OnPostBaseParams()
        {
            IndexModel.Conf.ShipConfigs.Clear();
            
            IndexModel.Conf.BoardSizeX = BoardSizeX;
            IndexModel.Conf.BoardSizeY = BoardSizeY;
            IndexModel.Conf.EShipTouchRule = EShipTouchRuleReceived;
            FirstTime = false;
        }
        public void OnPostAddShip()
        {
            var shipConfig = new ShipConfig()
            {
                Name = ShipName,
                ShipSizeX = ShipSizeX,
                ShipSizeY = ShipSizeY,
                Quantity = ShipCount
            };
            
            IndexModel.Conf.ShipConfigs.Add(shipConfig);
            FirstTime = false;
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        // public async Task<IActionResult> OnPostUploadConfig()
        public void OnPostUploadConfig()
        {
            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            ConfigTested = BsBrain.ShipTester(IndexModel.Conf);
            if (ConfigTested)
            {
                Ready = true;
                var confJsonStr = JsonSerializer.Serialize(IndexModel.Conf, jsonOptions);

                GameConfig.ConfigJson = confJsonStr;
                GameConfig.ConfigBuildDate = DateTime.Now;

                _context.GameConfigs.Add(GameConfig);
                _context.SaveChanges();   
            }
            else
            {
                Ready = true;
            }

            //await _context.SaveChangesAsync();

            //return RedirectToPage("./Index");
        }
    }
}
