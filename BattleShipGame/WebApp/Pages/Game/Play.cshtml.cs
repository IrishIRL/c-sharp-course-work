using System.Threading.Tasks;
using BattleShipBrain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace WebApp.Pages.Game
{
    public class Play : PageModel
    {
        public char[] letters { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        
        public void OnGet(int id, int cellId, int quit)
        {
            bool hitCondition = false;
            
            // Check for quit conditions
            if (quit == 1)
            {
                Index._brain.GetBrainJson(2, ""); // Decision 2 -> Save to database. We do not need name, when saving to database (since no name is saved), so it is left blank.
                string url = "../";
                Response.Redirect(url);
            }
            if (quit == 2)
            {
                string url = "../";
                Response.Redirect(url);
            }

            if (cellId > 0)
            {
                int[] xy = new int[2];
                int counter = 1;

                for (int y = 0; y < Index._brain.GetUserBoard().GetLength(1); y++)
                {
                    for (int x = 0; x < Index._brain.GetUserBoard().GetLength(0); x++)
                    {
                        if (counter == cellId)
                        {
                            xy[0] = x;
                            xy[1] = y;
                        }

                        counter++;
                    }
                }

                // Placing bombs
                hitCondition = Index._brain.PlaceBomb(xy);

                // Check for win conditions
                var winCondition = Index._brain.GameWinCondition();
                if (winCondition) Response.Redirect("../win");

                // Check for hit
                if (!hitCondition)
                {
                    // Change player if the bomb did not hit the target
                    Index._brain._currentPlayerNo = Index._brain._currentPlayerNo == 0 ? 1 : 0;

                }
            }
        }
    }
}