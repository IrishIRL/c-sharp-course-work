using System;

namespace BattleShipBrain
{
    public class BSBrain
    {

        private static BoardSquareState[,] _boardA;
        private static BoardSquareState[,] _boardB;
        
        private readonly Random _rnd = new Random();
        
        public BSBrain(int xSize, int ySize)
        {
            _boardA = new BoardSquareState[xSize,ySize];
            _boardB = new BoardSquareState[xSize,ySize];

            for (var x = 0; x < xSize; x++)
            {
                for (var y = 0; y < ySize; y++)
                {
                    _boardA[x, y] = new BoardSquareState
                    {
                        IsShip = _rnd.Next(0, 2) != 0
                    };
                    
                    _boardB[x, y] = new BoardSquareState
                    {
                        IsShip = _rnd.Next(0, 2) != 0
                    };
                }
            }
        }

        public static void PlantBomb(int x, int y, int player)
        {
            switch (player)
            {
                case 0: 
                    _boardA[x, y].IsBomb = true;
                    break;
                case 1:
                    _boardB[x, y].IsBomb = true;
                    break;
            }

        }
        public BoardSquareState[,] GetBoard(int playerNo)
        {
            if (playerNo == 0) return _boardA;
            return _boardB;
        }
    }
}