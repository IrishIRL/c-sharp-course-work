using System;
using System.Runtime.CompilerServices;
using BattleShipBrain;

namespace BattleShipConsoleUI
{
    public class BSConsoleUI
    {
        public static void DrawBoard(bool enemy, BoardSquareState[,] board)
        {
            char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            // X loop
            Console.Write("\n  ");
            for (int xLoop = 0; xLoop < board.GetLength(0); xLoop++)
            {
                Console.Write("  {0} ", letters[xLoop % 26]);
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
                if (yLoop < 10)
                {
                    Console.Write("0{0} ", yLoop);
                }
                else {
                    Console.Write("{0} ", yLoop); 
                }
                for (var xLoop = 0; xLoop < board.GetLength(0); xLoop++)
                {
                    Console.Write("| ");
                    if (enemy)
                    {
                        if (board[xLoop, yLoop].IsShip && !board[xLoop, yLoop].IsBomb)
                        {
                            Console.Write(" ");
                        }
                        else
                        {
                            Console.Write(board[xLoop,yLoop]);
                        }
                    }
                    else
                    {
                        Console.Write(board[xLoop,yLoop]);
                    }
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