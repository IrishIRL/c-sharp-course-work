#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace BattleShipBrain
{
    // DTO - Data Transfer Object
    public class SaveGameDto
    {
        public int CurrentPlayerNo { get; set; }
        public GameBoardDto[] GameBoards  { get; set; } = new GameBoardDto[2];
        
        public class GameBoardDto
        {
            public List<List<BoardSquareState>>? Board { get; set; } = new List<List<BoardSquareState>>();
            public List<Ship>? Ships { get; set; } = new List<Ship>(); //TODO: We should save the whole config, not the ships.
        }

        public void SetGameBoard(GameBoard[] gameBoards)
        {
            for (int i = 0; i < 2; i++)
            {
                GameBoards[i] = new GameBoardDto();
                var board = gameBoards[i].Board;

                var toList = Enumerable.Range(0, board.GetLength(0))
                    .Select(row => Enumerable.Range(0, board.GetLength(1))
                        .Select(col => board[row, col]).ToList()).ToList();

                GameBoards[i].Board = toList;
                GameBoards[i].Ships = gameBoards[i].Ships;
            }
        }
        
        public GameBoard[] GetGameBoard()
        {
            var gameBoard = new GameBoard[2];

            for (int i = 0; i < 2; i++)
            {
                gameBoard[i] = new GameBoard();
                var arrayGameBoard = new BoardSquareState[GameBoards[i].Board!.Count, GameBoards[i].Board![i].Count];

                for (int x = 0; x < GameBoards[i].Board!.Count; x++)
                {
                    for (int y = 0; y < GameBoards[i].Board![y].Count-1; y++)
                    {
                        arrayGameBoard[x, y] = GameBoards[i].Board![x][y];
                    }
                }
            
                gameBoard[i].Board = arrayGameBoard;
                gameBoard[i].Ships = GameBoards[i].Ships;
            }
            
            return gameBoard;
        }
        
    }
}