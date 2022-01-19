using System;

namespace BattleShipBrain
{
    public struct BoardSquareState
    {
        public bool IsShip { get; set; }
        public bool IsBomb { get; set; }

        public override string ToString()
        {
            switch (IsEmpty: IsShip, IsBomb)
            {
                case (false, false):
                    return " "; //nothing
                case (false, true):
                    return "-"; //miss
                case (true, false):
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    return "8"; //ship
                case (true, true):
                    Console.ForegroundColor = ConsoleColor.Red;
                    return "X"; //bombed
            }
        }
    }
    
    
}