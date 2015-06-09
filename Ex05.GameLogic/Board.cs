using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05.GameLogic
{
    class Board
    {
        private eBoardCell[,] m_GameBoard;

        public Board(int i_BoardSize)
        {
            m_GameBoard = instantiateBoard(i_BoardSize);
        }

        // creating a game board according to the size given
        private eBoardCell[,] instantiateBoard(int i_BoardSize)
        {
            eBoardCell[,] newBoard = new eBoardCell[i_BoardSize, i_BoardSize];

            for (int i = 0; i < i_BoardSize; i++)
            {
                for (int j = 0; j < i_BoardSize; j++)
                {
                    // setting all the inital coins on the board
                    if ((i == (i_BoardSize / 2) - 1 || i == i_BoardSize / 2) && i == j)
                    {
                        newBoard[i, j] = eBoardCell.White;
                    }
                    else if ((i == (i_BoardSize / 2) - 1 && j == i_BoardSize / 2) || (i == i_BoardSize / 2 && j == (i_BoardSize / 2) - 1))
                    {
                        newBoard[i, j] = eBoardCell.Black;
                    }
                    else
                    {
                        newBoard[i, j] = eBoardCell.Empty;
                    }
                }
            }

            return newBoard;
        }

        public eBoardCell[,] Board
        {
            get
            {
                return m_GameBoard;
            }
        }
    }
}
