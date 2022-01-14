using System;
using System.Collections.Generic;
using BattleShipBrain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using DAL;

namespace WebApp.Pages.Game
{
    public class SetShips : PageModel
    {
        public bool ShipIdSelect { get; set; }
        //public bool shipPlaceSelect { get; set; } = false;
        public bool WrongLocation { get; set; }
        public bool WrongShip { get; set; }
        public char[] Letters { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public Domain.GameConfig Conf = default!;
        public BsBrain Brain = default!;
        public GameConfig config = default!;

        public int GameId { get; set; }
        private readonly DAL.ApplicationDbContext _context;

        public SetShips(DAL.ApplicationDbContext context)
        {
            _context = context;
        }
        
        // resetGame is used to reset BsBrain in case we start a new game.
        // playerChange is used to change between player 0 and player 1.
        // configId is used to determine which config to use for the game.
        // cellId is used to place ships.
        // shipName is used to determine which ship we are placing.
        // ready is used to determine when we can start the game.
        // wrongLocation is used to determine if the user placed the ship on the impossible location.
        public void OnGet(bool resetGame, int player, int cellId, string shipName, bool ready,
            bool wrongLocation, bool wrongShip, bool randomShipPlacement, int gameId, int confId)
        {
            // Load config
            Conf = _context.GameConfigs.Find(confId);

            config = JsonSerializer.Deserialize<GameConfig>(Conf.ConfigJson)!;

            Brain = new BsBrain(config); 
            
            //Brain.DatabaseGameLoad(gameId);
            Brain.RestoreBrainFromJson(2, gameId.ToString(), 1);
            

            GameId = gameId;

        if (randomShipPlacement)
            {
                Brain = Brain.RandomShipPlacement(config);
                // When we start the game, we also make a save of it. Not sure why so, but let it be here.
                Brain.GetBrainJson(2, ""); // Decision 2 -> Save to database. We do not need name, when saving to database (since no name is saved), so it is left blank.
                Response.Redirect("../Game/Play?gameId=" + gameId + "&confId=" + confId);
            }
            
            if (ready)
            {
                Brain.CurrentPlayerNo = 0;
                // When we start the game, we also make a save of it. Not sure why so, but let it be here.
                Brain.GetBrainJson(2, ""); // Decision 2 -> Save to database. We do not need name, when saving to database (since no name is saved), so it is left blank.
                Response.Redirect("../Game/Play?gameId=" + gameId + "&confId=" + confId);
            }
            
            if (resetGame)
            {
                Brain = new BsBrain(config);
            }
            
            if (wrongLocation)
            {
                WrongLocation = true;
            }

            if (wrongShip)
            {
                WrongShip = true;
            }

            //config = JsonSerializer.Deserialize<GameConfig>(Index.ConfigText)!;
            Brain.CurrentPlayerNo = player;
            //Response.Redirect("../Game/SetShips?gameId=" + gameId + "&confId=" + confId);
            
            
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
                    

                    for (int id = 0; id < config.ShipConfigs.Count; id++)
                    {
                        if (config.ShipConfigs[id].Name == shipName)
                        {
                           shipId = id;
                        }
                        else
                        {
                            // This "if" statement works only on the last loop, if it still has the wrong name.
                            // (checks amount of ships and how many loops were already done).
                            if (id == config.ShipConfigs.Count - 1)
                            {
                                Response.Redirect("../Game/SetShips?gameId=" + gameId + "&confId=" + confId);
                            }
                        }
                    }
                    
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

                    if (config.ShipConfigs[shipId].Quantity > 0)
                    {
                        for (int sizeX = 0; sizeX < config.ShipConfigs[shipId].ShipSizeX; sizeX++)
                        {
                            for (int sizeY = 0; sizeY < config.ShipConfigs[shipId].ShipSizeY; sizeY++)
                            {
                                list.Add(new Coordinate(
                                    xy[1] + sizeY,
                                    xy[0] + sizeX)
                                );
                            }
                        }

                        bool check = Brain.CheckCondition(list, config);

                        if (check)
                        {
                            Brain.PlaceShip(shipName, list);
                            
                            
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
                            
                            
                            Brain.GetBrainJson(1,"");
                            config.ShipConfigs[shipId].Quantity -= 1;
                            Response.Redirect("../Game/SetShips?gameId=" + gameId + "&confId=" + confId);
                        }
                        else
                        {
                            Response.Redirect("../Game/SetShips?wrongLocation=true&gameId=" + gameId + "&confId=" + confId);
                        }
                    }
                    else
                    {
                        Response.Redirect("../Game/SetShips?wrongShip=true&?gameId=" + gameId + "&confId=" + confId);
                    }
                }
            }
        }
    }
}