using System;
using System.Collections.Generic;
using BattleShipBrain;
using BattleShipConsoleApp;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Game
{
    public class SetShips : PageModel
    {
        public bool ShipIdSelect { get; set; } = false;
        //public bool shipPlaceSelect { get; set; } = false;
        public bool WrongLocation { get; set; }
        public char[] Letters { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        
        public void OnGet(bool resetGame, bool playerChange, int cellId, string shipName, bool ready)
        {
            if (ready)
            {
                Index._brain.GetBrainJson(2, ""); // Decision 2 -> Save to database. We do not need name, when saving to database (since no name is saved), so it is left blank.
                string url = "../Game/Play";
                Response.Redirect(url);
            }
            
            if (resetGame)
            {
                Index._brain = new BsBrain(Index._conf);
            }
            
            var list = new List<Coordinate>();
            int shipId = 0;

            if (shipName != null && shipName != "")
            {
                if (ShipIdSelect == false)
                {
                    ShipIdSelect = true;
                }

                if (cellId > 0)
                {
                    int[] xy = new int[2];
                    int counter = 1;
                    

                    for (int id = 0; id < Index._conf.ShipConfigs.Count; id++)
                    {
                        if (Index._conf.ShipConfigs[id].Name == shipName)
                        {
                            shipId = id;
                        }
                        else
                        {
                            if (id == Index._conf.ShipConfigs.Count - 1)
                            {
                                Response.Redirect("../Game/SetShips");
                            }
                        }
                    }
                    
                    
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
                    
                    for (int sizeX = 0; sizeX < Index._conf.ShipConfigs[shipId].ShipSizeX; sizeX++)
                    {
                        for (int sizeY = 0; sizeY < Index._conf.ShipConfigs[shipId].ShipSizeY; sizeY++)
                        {
                            list.Add(new Coordinate(
                                xy[1] + sizeY,
                                xy[0] + sizeX)
                            );
                        }
                    }

                    bool check = Index._brain.CheckCondition(list);

                    if (check)
                    {
                        Index._brain.PlaceShip(shipName, list);
                    }
                    else
                    {
                        WrongLocation = true;
                    }
                    Response.Redirect("../Game/SetShips");
                }
            }

            if (playerChange)
            {
                Index._brain._currentPlayerNo = Index._brain._currentPlayerNo == 0 ? 1 : 0;
                Response.Redirect("../Game/SetShips");
            }
        }
    }
}