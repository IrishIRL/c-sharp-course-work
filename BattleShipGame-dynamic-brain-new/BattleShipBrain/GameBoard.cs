using System.Collections.Generic;
using System.Linq;

namespace BattleShipBrain
{
    public class GameBoard
    {
        public BoardSquareState[,] Board { get; set; } = default!;
        public List<Ship> Ships { get; set; } = new List<Ship>();

        // find all ships based on ship config. Returns number of placed ships.
        public int CheckShipPlacement(ShipConfig shipConfig)
        {
            return Ships.Count(ship => ship.Name == shipConfig.Name);
        }
    }
}