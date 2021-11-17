using System;
using BattleShipConsoleApp;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Game
{
    public class SetShips : PageModel
    {
        /*
        private static int _shipCount = Index._conf.ShipConfigs.Count;
        private static int _shipQuantity = Index._conf.ShipConfigs[_shipCount].Quantity;
        private static int _sizeX = Index._conf.ShipConfigs[_shipCount].ShipSizeX;
        private static int _sizeY = Index._conf.ShipConfigs[_shipCount].ShipSizeY;
        */
        public void OnGet(bool playerChange, int x, int y)
        {
            int[] xy = new int[] {x, y};
            Index._brain.PlaceShip(xy);
            if (playerChange)
            {
                if(Index._brain._currentPlayerNo ==0) Index._brain._currentPlayerNo = 1;
                else Index._brain._currentPlayerNo = 0;
                Response.Redirect("../Game/SetShips");
            }
        }
    }
}