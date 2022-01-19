using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Game
{
    public class ChangeTurn : PageModel
    {
        public int GameId { get; set; }
        public void OnGet(int gameId)
        {
            GameId = gameId;
        }
    }
}