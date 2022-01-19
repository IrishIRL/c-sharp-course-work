using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class Win : PageModel
    {
        public int CurrentPlayerNo { get; set; } = default!;
        
        public void OnGet(int currentPlayerNumber)
        {
            CurrentPlayerNo = currentPlayerNumber;
        }
    }
}