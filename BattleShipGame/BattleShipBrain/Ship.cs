using System.Collections.Generic;
using System.Linq;

namespace BattleShipBrain
{
    public class Ship
    {
        public string Name { get; private set; }
        
        private readonly List<Coordinate> _coordinates;

        public Ship(string name, List<Coordinate> position)
        {
            Name = name;
            _coordinates = position;
        }

        public int GetShipSize() => _coordinates.Count;
        
        public int GetShipDamageCount(BoardSquareState[,] board) =>
            // count all the items that match the predicate
            _coordinates.Count(coordinate => board[coordinate.X, coordinate.Y].IsBomb);

        public bool IsShipSunk(BoardSquareState[,] board) =>
            // returns true when all the items in the list match predicate
            _coordinates.All(coordinate => board[coordinate.X, coordinate.Y].IsBomb);
    }
}