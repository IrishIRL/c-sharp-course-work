using System.Collections.Generic;

namespace BattleShipBrain
{
    public class GameBoard
    {
        public BoardSquareState[,] Board { get; set; } = default!;
        public List<Ship> Ships { get; set; } = new List<Ship>();
    }
}