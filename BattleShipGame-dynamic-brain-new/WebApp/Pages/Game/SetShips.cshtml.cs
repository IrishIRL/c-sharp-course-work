using System;
using System.Collections.Generic;
using System.IO;
using BattleShipBrain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Threading.Tasks;
using BattleShipConsoleApp;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Pages.Game
{
    public class SetShips : PageModel
    {
        public bool ShipIdSelect { get; set; }
        //public bool shipPlaceSelect { get; set; } = false;
        public bool WrongLocation { get; set; }
        public bool WrongShip { get; set; }
        public char[] Letters { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        
        // resetGame is used to reset BsBrain in case we start a new game.
        // playerChange is used to change between player 0 and player 1.
        // configId is used to determine which config to use for the game.
        // cellId is used to place ships.
        // shipName is used to determine which ship we are placing.
        // ready is used to determine when we can start the game.
        // wrongLocation is used to determine if the user placed the ship on the impossible location.
        [BindProperty]
        public GameBoard GameBoard { get; set; } = default!;
        [BindProperty]
        public GameConfig GameConfig { get; set; } = default!;

        public int CurrentPlayerNumber { get; set; }
        [BindProperty]
        public int GameId { get; set; }
        [BindProperty]
        public int ConfigId { get; set; }
        
        public IActionResult OnGet(int? gameId, int? configId, bool wrongLocation, bool wrongShip)
        {
            if (gameId == null || configId == null) return NotFound();
            if (wrongLocation) WrongLocation = true;
            if (wrongShip) WrongShip = true; 
            GameId = (int)gameId;
            ConfigId = (int)configId;
            var brain = new DBAccess().RestoreGameFromDb((int)gameId).Result;
            GameBoard = brain.GetCurrentBoard();
            CurrentPlayerNumber = brain.CurrentPlayerNo;
            GameConfig = new DBAccess().GetBrainConfigurationFromDbConfiguration((int)configId).Result;
            return Page();
        }

        public int CountShipsLeft(ShipConfig shipConfig)
        {
            return GameBoard.CheckShipPlacement(shipConfig);
        }
        
        [BindProperty]
        public string SelectedShipName { get; set; } = default!;
        [BindProperty]
        public int X { get; set; }
        [BindProperty]
        public int Y { get; set; }
        
        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("X : " + X + "Y: " + Y);
            var brain = new DBAccess().RestoreGameFromDb(GameId).Result;
            var gameConfig = new DBAccess().GetBrainConfigurationFromDbConfiguration(ConfigId).Result;
            var shipConfig = gameConfig.ShipConfigs.Find(config => config.Name == SelectedShipName);
            var list = new List<Coordinate>();

            if (shipConfig != null)
            {
                if (shipConfig.Quantity < 1){
                    return RedirectToPage("./SetShips",
                        new {gameId = GameId, configId = ConfigId, WrongShip = true});
                }
            }

            for (int sizeX = 0; sizeX < shipConfig!.ShipSizeX; sizeX++)
            {
                for (int sizeY = 0; sizeY < shipConfig.ShipSizeY; sizeY++)
                {
                    list.Add(new Coordinate(
                            Y + sizeY,
                            X + sizeX
                        )
                    );
                }
            }

            bool check = brain.CheckCondition(list, gameConfig);
            if (check)
            {
                brain.PlaceShip(shipConfig.Name, list);
                new DBAccess().UpdateGame(brain, GameId);
                return RedirectToPage("./SetShips", new {gameId = GameId, configId = ConfigId});
            }
            else
            {
                //Response.Redirect("../Game/SetShips?wrongLocation=true");
                return RedirectToPage("./SetShips",
                    new {gameId = GameId, configId = ConfigId, WrongLocation = true});
            }
            

            return RedirectToPage("./SetShips", new {gameId = GameId, configId = ConfigId});
        }

        public async Task<IActionResult> OnPostChangePlayer()
        {
            var brain = new DBAccess().RestoreGameFromDb(GameId).Result;
            brain.CurrentPlayerNo = 1;
            new DBAccess().UpdateGame(brain, GameId);
            return RedirectToPage("./SetShips", new {gameId = GameId, configId = ConfigId});
        }
        
        public async Task<IActionResult> OnPostRandomShipPlacement()
        {
            var brain = new DBAccess().RestoreGameFromDb(GameId).Result;
            var config = new DBAccess().GetBrainConfigurationFromDbConfiguration(ConfigId).Result;
            brain = brain.RandomShipPlacement(config);
            //brain.CurrentPlayerNo = 1;
            new DBAccess().UpdateGame(brain, GameId);
            return RedirectToPage("./SetShips", new {gameId = GameId, configId = ConfigId});
        }
        
        /*
        public void OnGet(bool resetGame, bool playerChange, int cellId, string shipName, bool ready, bool wrongLocation, bool wrongShip, bool randomShipPlacement)
        {
            if (randomShipPlacement)
            {
                Index.Brain = Index.Brain.RandomShipPlacement(Index.Conf);
                // When we start the game, we also make a save of it. Not sure why so, but let it be here.
                Index.Brain.GetBrainJson(1, "TempGameSaver_" + Index.TurnCounter, true);
                //Index.Brain.GetBrainJson(2, "", true); // Decision 2 -> Save to database. We do not need name, when saving to database (since no name is saved), so it is left blank.
                Response.Redirect("../Game/Play");
            }
            
            if (ready)
            {
                Index.Brain.CurrentPlayerNo = 0;
                // When we start the game, we also make a save of it. Not sure why so, but let it be here.
                Index.Brain.GetBrainJson(1, "TempGameSaver_" + Index.TurnCounter, true);
                //Index.Brain.GetBrainJson(2, "", true); // Decision 2 -> Save to database. We do not need name, when saving to database (since no name is saved), so it is left blank.
                Response.Redirect("../Game/Play");
            }
            
            if (resetGame)
            {
                Index.Brain = new BsBrain(Index.Conf);
            }
            
            if (wrongLocation)
            {
                WrongLocation = true;
            }

            if (wrongShip)
            {
                WrongShip = true;
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
                    

                    for (int id = 0; id < Index.Conf.ShipConfigs.Count; id++)
                    {
                        if (Index.Conf.ShipConfigs[id].Name == shipName)
                        {
                           shipId = id;
                        }
                        else
                        {
                            // This "if" statement works only on the last loop, if it still has the wrong name.
                            // (checks amount of ships and how many loops were already done).
                            if (id == Index.Conf.ShipConfigs.Count - 1)
                            {
                                Response.Redirect("../Game/SetShips");
                            }
                        }
                    }
                    
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

                    if (Index.Conf.ShipConfigs[shipId].Quantity > 0)
                    {
                        for (int sizeX = 0; sizeX < Index.Conf.ShipConfigs[shipId].ShipSizeX; sizeX++)
                        {
                            for (int sizeY = 0; sizeY < Index.Conf.ShipConfigs[shipId].ShipSizeY; sizeY++)
                            {
                                list.Add(new Coordinate(
                                    xy[1] + sizeY,
                                    xy[0] + sizeX)
                                );
                            }
                        }

                        bool check = Index.Brain.CheckCondition(list, Index.Conf);

                        if (check)
                        {
                            Index.Brain.PlaceShip(shipName, list);
                            Index.Conf.ShipConfigs[shipId].Quantity -= 1;
                            Response.Redirect("../Game/SetShips");
                        }
                        else
                        {
                            Response.Redirect("../Game/SetShips?wrongLocation=true");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Game/SetShips?wrongShip=true");
                    }
                }
            }
            if (playerChange)
            {
                Index.Conf = JsonSerializer.Deserialize<GameConfig>(Index.ConfigText)!;
                Index.Brain.CurrentPlayerNo = Index.Brain.CurrentPlayerNo == 0 ? 1 : 0;
                Response.Redirect("../Game/SetShips");
            }
        }
         */
    }
}