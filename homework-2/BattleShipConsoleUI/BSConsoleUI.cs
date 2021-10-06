using System;
using BattleShipBrain;

namespace BattleShipConsoleUI
{
    public class BSConsoleUI
    {
        public static void DrawBoard(BoardSquareState[,] board)
        {
            // X loop
            Console.Write("\n  ");
            for (int xLoop = 0; xLoop < board.GetLength(0); xLoop++)
            {
                Console.Write("  {0} ", xLoop);
            }
            Console.Write("\n  ");
            for (int xLoop = 0; xLoop < board.GetLength(0); xLoop++)
            {
                Console.Write("+ - ");
            }
            Console.WriteLine("+");
            
            // Y loop
            for (var yLoop = 0; yLoop < board.GetLength(1); yLoop++)
            {
                Console.Write("{0} ", yLoop);
                for (var xLoop = 0; xLoop < board.GetLength(0); xLoop++)
                {
                    Console.Write("| ");
                    Console.Write(board[xLoop,yLoop]);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" ");
                }
                Console.WriteLine("|");
            }
            
            Console.Write("  ");
            for (int xLoop = 0; xLoop < board.GetLength(0); xLoop++)
            {
                Console.Write("+ - ");
            }
            Console.WriteLine("+");
        }
    }
    
}