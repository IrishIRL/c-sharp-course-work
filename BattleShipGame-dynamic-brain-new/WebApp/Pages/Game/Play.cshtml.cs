using System;
using System.IO;
using System.Threading.Tasks;
using BattleShipBrain;
using BattleShipConsoleApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public int NowTurn { get; set; }

        [BindProperty]
        public int GameId { get; set; }
        public int Quit { get; set; }

        public GameBoard CurrentPlayerGameBoard { get; set; } = default!;
        public GameBoard OppositePlayerGameBoard { get; set; } = default!;

        public int CurrentPlayerNumber { get; set; }
        
        public IActionResult OnGetAsync(int? gameId, bool incorrectLocation)
        {
            //if (gameId == null) return NotFound();
            if (incorrectLocation) IncorrectLocation = true;
            GameId = (int)gameId;

            var brain = new DBAccess().RestoreGameFromDb((int)gameId).Result;
            CurrentPlayerGameBoard = brain.GetCurrentBoard();
            OppositePlayerGameBoard = brain.GetOppositeBoard();
            CurrentPlayerNumber = brain.CurrentPlayerNo;
            
            return Page();
        }
        
        [BindProperty]
        public int X { get; set; }
        [BindProperty]
        public int Y { get; set; }

        public async Task<IActionResult> OnPostExitGame()
        {
            if (Quit == 1)
            {
                var brain = new DBAccess().RestoreGameFromDb(GameId).Result;
                new DBAccess().UpdateGame(brain, GameId);
                //brain.GetBrainJson(2, "", false); // Decision 2 -> Save to database. We do not need name, when saving to database (since no name is saved), so it is left blank.
                RedirectToPage("./Index");
            }
            
            return RedirectToPage("./Index");
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            var brain = new DBAccess().RestoreGameFromDb(GameId).Result;
            
            int[] coords = new[] {X, Y};
            var checker = brain.CheckForBomb(X, Y);
            if (checker)
            {
                return RedirectToPage("./Play", new {gameId = GameId, IncorrectLocation = true});
            }
            var result = brain.PlaceBomb(coords);
            new DBAccess().UpdateGame(brain, GameId);
            var winCondition = brain.GameWinCondition();
            if (winCondition) return RedirectToPage("../Win", new {currentPlayerNumber = brain.CurrentPlayerNo});

            if (result)
            {
                new DBAccess().UpdateGame(brain, GameId);
                return RedirectToPage("./Play", new {gameId = GameId});
            }
            brain.SwitchPlayers();
            new DBAccess().UpdateGame(brain, GameId);
            return RedirectToPage("./ChangeTurn", new {gameId = GameId});
        }
        /*
        public void OnGet(bool changeTurn, int cellId, int quit, bool goBack, bool goFront)
        {
            // Make one turn back
            if (goBack && Index.TurnCounter > 0)
            {
                Index.TurnCounter -= 1;
                string savedGameFile = GlobalVariables.ReturnGameTempFolderLocation() + Path.DirectorySeparatorChar + "TempGameSaver_" + (Index.TurnCounter-1) + ".json";
                Index.Brain.RestoreBrainFromJson(1, savedGameFile, 1);
                //Index.Brain.CurrentPlayerNo = Index.Brain.CurrentPlayerNo == 0 ? 1 : 0;
            }
                
            // Make one turn "front"
            if (goFront && NowTurn < Index.TurnCounter)
            {
                Index.TurnCounter += 1;
                string savedGameFile = GlobalVariables.ReturnGameTempFolderLocation() + Path.DirectorySeparatorChar + "TempGameSaver_" + (Index.TurnCounter) + ".json";
                Index.Brain.RestoreBrainFromJson(1, savedGameFile, 1);
                //Index.Brain.CurrentPlayerNo = Index.Brain.CurrentPlayerNo == 0 ? 1 : 0;
            }
            
            // Check for quit conditions
            if (quit == 1)
            {
                Index.Brain.GetBrainJson(2, "", false); // Decision 2 -> Save to database. We do not need name, when saving to database (since no name is saved), so it is left blank.
                Response.Redirect("../");
            }
            if (quit == 2)
            {
                Response.Redirect("../");
            }

            if (changeTurn)
            {
                ChangeTurn = false;
                Index.Brain.CurrentPlayerNo = Index.Brain.CurrentPlayerNo == 0 ? 1 : 0;
            }

            if (cellId > 0)
            {
                int[] xy = new int[2];
                int counter = 1;

                for (int y = 0; y < Index.Brain.GetUserBoard().GetLength(1); y++)
                {
                    for (int x = 0; x < Index.Brain.GetUserBoard().GetLength(0); x++)
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
                IncorrectLocation = Index.Brain.CheckForBomb(xy[0], xy[1]);

                // Placing bombs
                HitCondition = Index.Brain.PlaceBomb(xy);

                if (!IncorrectLocation)
                {
                    // Make a temporary game save
                    Index.Brain.GetBrainJson(1, "TempGameSaver_" + Index.TurnCounter, true);
                }
                
                // Check for win conditions
                var winCondition = Index.Brain.GameWinCondition();
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
                    SizeLeft = Index.Brain.LeftShipSize(xy);
                    ShipHit = true;
                }
            }
           
        }
    */
    }
}