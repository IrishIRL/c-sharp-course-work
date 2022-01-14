using System.Collections.Generic;
using System.Linq;

namespace BattleShipBrain
{
    public class Ship
    {
        public string Name { get; set; }
        public List<Coordinate> Coordinates { get; set; } = new List<Coordinate>();
        public Ship(string name, List<Coordinate> coordinates)
        {
            Name = name;
            Coordinates = coordinates;
        }
        public int GetShipSize() => Coordinates.Count;
        
        public int GetShipDamageCount(BoardSquareState[,] board) =>
            // count all the items that match the predicate
            Coordinates.Count(coordinate => board[coordinate.X, coordinate.Y].IsBomb);

        public bool IsShipSunk(BoardSquareState[,] board) =>
            // returns true when all the items in the list match predicate
            Coordinates.All(coordinate => board[coordinate.X, coordinate.Y].IsBomb);
    }
}