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
        public void OnGet(bool changeTurn, int cellId, int quit)
        {
            // Check for quit conditions
            if (quit == 1)
            {
                Index.Brain.GetBrainJson(2, ""); // Decision 2 -> Save to database. We do not need name, when saving to database (since no name is saved), so it is left blank.
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
    }
}