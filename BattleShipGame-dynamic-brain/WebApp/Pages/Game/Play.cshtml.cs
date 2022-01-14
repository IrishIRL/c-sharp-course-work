using BattleShipBrain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using DAL;

namespace WebApp.Pages.Game
{
    public class Play : PageModel
    {
        public char[] Letters { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        public int SizeLeft { get; set; }
        public bool ShipHit { get; set; }
        public bool IncorrectLocation { get; set; }
        public bool HitCondition { get; set; }
        public bool ChangeTurn { get; set; }
        public BsBrain Brain = default!;
        public int GameId { get; set; }
        public Domain.GameConfig Conf = default!;
        private readonly DAL.ApplicationDbContext _context;

        public Play(DAL.ApplicationDbContext context)
        {
            _context = context;
        }
        
        public void OnGet(bool changeTurn, int cellId, int quit, int gameId)
        {
            // Load config
            var config = new GameConfig();
            Brain = new BsBrain(config);
            
            //Brain.DatabaseGameLoad(gameId);
            Brain.RestoreBrainFromJson(2, gameId.ToString(), 1);

            GameId = gameId;
            
            //var Conf = new GameConfig(); 
            Brain.RestoreBrainFromJson(2, gameId.ToString(), 1);
            
            // Check for quit conditions
            if (quit == 1)
            {
                Brain.GetBrainJson(2, ""); // Decision 2 -> Save to database. We do not need name, when saving to database (since no name is saved), so it is left blank.
                Response.Redirect("../");
            }
            if (quit == 2)
            {
                Response.Redirect("../");
            }

            if (changeTurn)
            {
                ChangeTurn = false;
                Brain.CurrentPlayerNo = Brain.CurrentPlayerNo == 0 ? 1 : 0;
            }
            
            if (cellId > 0)
            {
                int[] xy = new int[2];
                int counter = 1;

                for (int y = 0; y < Brain.GetUserBoard().GetLength(1); y++)
                {
                    for (int x = 0; x < Brain.GetUserBoard().GetLength(0); x++)
                    {
                        if (counter == cellId)
                        {
                            xy[0] = x;
                            xy[1] = y;
                        }

                        counter++;
                    }
                }

                // Check if the location has the bomb already, so a player does not place bomb to the same place
                IncorrectLocation = Brain.CheckForBomb(xy[0], xy[1]);
                
                // Placing bombs
                HitCondition = Brain.PlaceBomb(xy);

                var jsonOptions = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };

                var dto = new SaveGameDto();

                dto.SetGameBoard(Brain.GameBoards);
                
                dto.CurrentPlayerNo = Brain.CurrentPlayerNo;
            
                var jsonStr = JsonSerializer.Serialize(dto, jsonOptions);
                
                using (var db = new ApplicationDbContext())
                {
                    var gameSave = _context.SavedGames.Find(GameId);
                    gameSave.SavedGame = jsonStr;
                    _context.SavedGames.Update(gameSave);
                    _context.SaveChanges();
                }
                
                // Check for win conditions
                var winCondition = Brain.GameWinCondition();
                if (winCondition) Response.Redirect("../win");

                // Check for hit
                if (!HitCondition && !IncorrectLocation)
                {
                    // Change player if the bomb did not hit the target
                    ChangeTurn = true;
                    // Index.Brain.CurrentPlayerNo = Index.Brain.CurrentPlayerNo == 0 ? 1 : 0;
                }
                else if(!IncorrectLocation)
                {
                    SizeLeft = Brain.LeftShipSize(xy);
                    ShipHit = true;
                }
            }
        }
    }
}