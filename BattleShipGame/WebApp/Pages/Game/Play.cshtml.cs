using System.Threading.Tasks;
using BattleShipBrain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace WebApp.Pages.Game
{
    public class Play : PageModel
    {
        
        /*
        public async Task OnGetAsync(int id, int x, int y, bool qs)
        {
            int[] xy = new int[] {x, y};
            Index._brain.PlaceBomb(xy);
            
            if (Index._brain._currentPlayerNo == 0)
            {
                Index._brain._currentPlayerNo = 1;
            }
            else
            {
                Index._brain._currentPlayerNo = 0;
            }

            if (qs)
            {
                qs = false;
                //Index._brain.GetBrainJson(2, ""); // Decision 2 -> Save to database. We do not need name, when saving to database (since no name is saved), so it is left blank.
                string url = "https://google.com";
                Response.Redirect(url);
            }
        }
    }*/
    
        public void OnGet(int id, int x, int y, bool qs)
        {
            int[] xy = new int[] {x, y};
            Index._brain.PlaceBomb(xy);
            
            if (Index._brain._currentPlayerNo == 0)
            {
                Index._brain._currentPlayerNo = 1;
            }
            else
            {
                Index._brain._currentPlayerNo = 0;
            }

            if (qs)
            {
                Index._brain.GetBrainJson(2, ""); // Decision 2 -> Save to database. We do not need name, when saving to database (since no name is saved), so it is left blank.
                string url = "../";
                Response.Redirect(url);
            }
        }
    }
}