using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05.GameLogic
{
    // this class is in charge of all the different game operations
    public class GameOperations
    {
        private GameState m_CurrentGameState;

        private sMatrixCoordinate m_UpLeft = new sMatrixCoordinate(-1, -1);
        private sMatrixCoordinate m_Up = new sMatrixCoordinate(0, -1);
        private sMatrixCoordinate m_UpRight = new sMatrixCoordinate(1, -1);
        private sMatrixCoordinate m_Left = new sMatrixCoordinate(-1, 0);
        private sMatrixCoordinate m_Right = new sMatrixCoordinate(1, 0);
        private sMatrixCoordinate m_DownLeft = new sMatrixCoordinate(-1, 1);
        private sMatrixCoordinate m_Down = new sMatrixCoordinate(0, 1);
        private sMatrixCoordinate m_DownRight = new sMatrixCoordinate(1, 1);

        private sMatrixCoordinate[] m_Directions; // this array will hold all the direction coordinates

        public GameOperations(GameState i_GameState)
        {
            m_CurrentGameState = i_GameState;

            m_Directions = new sMatrixCoordinate[8] { m_UpLeft, m_Up, m_UpRight, m_Left, m_Right, m_DownLeft, m_Down, m_DownRight };
        }

        // this method updates the game once a valid mmove is chosen
        public void UpdateGame(sMatrixCoordinate i_Move)
        {
            // sets the chosen cell the right color and adds it to the cellsOccupied list of the relevant player
            m_CurrentGameState.CurrentBoard.GameBoard[i_Move.X, i_Move.Y] = (eBoardCell)m_CurrentGameState.CurrentPlayer.Color;
            m_CurrentGameState.CurrentPlayer.CellsOccupied.Add(i_Move);

            // check the cell in every direction (8 directions). If adjacent coin is of the other color, send to checkIfCouldFlipInDirection with that direction
            foreach (sMatrixCoordinate direction in m_Directions)
            {
                sMatrixCoordinate adjacentCoordinate = i_Move + direction;

                if (isInBounds(adjacentCoordinate, m_CurrentGameState.BoardSize))
                {
                    eBoardCell adjacentCell = m_CurrentGameState.CurrentBoard.GameBoard[adjacentCoordinate.X, adjacentCoordinate.Y];

                    // checks if not empty and not in the same color as the move cell (therfor, in the other color)
                    if (!adjacentCell.Equals(eBoardCell.Empty) && !adjacentCell.Equals((eBoardCell)m_CurrentGameState.CurrentPlayer.Color))
                    {
                        // checks if there is a coin of my color on the other side of the vector created from move coordinate and the direction
                        sMatrixCoordinate? tempCoordToCheckForFlipPossibility = CheckIfCouldFlipInDirection(adjacentCoordinate, direction, (eBoardCell)m_CurrentGameState.CurrentPlayer.Color);

                        // if there is a coin, flip in that direction
                        if (tempCoordToCheckForFlipPossibility != null)
                        {
                            Flip(adjacentCoordinate, direction, adjacentCell);
                        }
                    }
                }
            }

            // sets the last move and name of last player in order to show to user the updated status of board
            m_CurrentGameState.LastMovePlayed = i_Move;
            m_CurrentGameState.NameOfLastPlayerThatPlayed = m_CurrentGameState.CurrentPlayer.Name;
        }

        // checks wether a matrix coordinate is within the bounds of the board
        private bool isInBounds(sMatrixCoordinate i_Coordinate, int i_SizeOfBoard)
        {
            bool isWithinRange = true;

            if (i_Coordinate.X < 0 || i_Coordinate.X >= i_SizeOfBoard || i_Coordinate.Y < 0 || i_Coordinate.Y >= i_SizeOfBoard)
            {
                isWithinRange = false;
            }

            return isWithinRange;
        }

        // calculating the scores of both players
        public void CalcScore()
        {
            m_CurrentGameState.FirstPlayer.Score = 0;
            m_CurrentGameState.SecondPlayer.Score = 0;

            // runs on the entire board and accumlates the score of the relevant player
            foreach (eBoardCell cell in m_CurrentGameState.CurrentBoard.GameBoard)
            {
                switch (cell)
                {
                    case eBoardCell.Black:
                        m_CurrentGameState.FirstPlayer.Score++;
                        break;
                    case eBoardCell.White:
                        m_CurrentGameState.SecondPlayer.Score++;
                        break;
                }
            }
        }

        // runs recursivly and flips all coins that are in color of the cell in currentCoordinate
        public void Flip(sMatrixCoordinate i_CurrentCoordinate, sMatrixCoordinate i_Direction, eBoardCell i_CellType)
        {
            eBoardCell currentBoardCell = m_CurrentGameState.CurrentBoard.GameBoard[i_CurrentCoordinate.X, i_CurrentCoordinate.Y];
            sMatrixCoordinate newCoordinate = i_CurrentCoordinate + i_Direction;
            eBoardCell adjacentCell = m_CurrentGameState.CurrentBoard.GameBoard[newCoordinate.X, newCoordinate.Y];

            // check if next cell is of same color - if yes, send recursively. if not, we flipped all in this direction and can stop
            if (adjacentCell.Equals(currentBoardCell))
            {
                Flip(newCoordinate, i_Direction, adjacentCell);
            }

            // flip this cell - update both players cell lists
            if (currentBoardCell.Equals(eBoardCell.Black))
            {
                m_CurrentGameState.CurrentBoard.GameBoard[i_CurrentCoordinate.X, i_CurrentCoordinate.Y] = eBoardCell.White;
            }
            else
            {
                m_CurrentGameState.CurrentBoard.GameBoard[i_CurrentCoordinate.X, i_CurrentCoordinate.Y] = eBoardCell.Black;
            }

            if (m_CurrentGameState.CurrentPlayer.Equals(m_CurrentGameState.FirstPlayer))
            {
                m_CurrentGameState.SecondPlayer.CellsOccupied.Remove(i_CurrentCoordinate);
            }
            else
            {
                m_CurrentGameState.FirstPlayer.CellsOccupied.Remove(i_CurrentCoordinate);
            }

            m_CurrentGameState.CurrentPlayer.CellsOccupied.Add(i_CurrentCoordinate);
        }

        // calculating all the valid moves of player
        public void CalcValidMoves(Player i_Player)
        {
            // clearing all the moves of player 
            i_Player.ValidMoves.Clear();

            sMatrixCoordinate? tempCoordToCheckForValidMovePossibility = null;

            // runs on all the cells occupied by player and cheks to see if from them we can find a cell to 
            // put a coin on, that'll flip all the coins on the way
            foreach (sMatrixCoordinate cellOccupied in i_Player.CellsOccupied)
            {
                eBoardCell currentBoardCell = m_CurrentGameState.CurrentBoard.GameBoard[cellOccupied.X, cellOccupied.Y];

                // for each cell occupied, checks all 8 directions to find places to put coins
                foreach (sMatrixCoordinate direction in m_Directions)
                {
                    sMatrixCoordinate adjacentCoordinate = cellOccupied + direction;

                    // checking that new coordinate is within the boinds of the board
                    if (isInBounds(adjacentCoordinate, m_CurrentGameState.BoardSize))
                    {
                        eBoardCell adjacentCell = m_CurrentGameState.CurrentBoard.GameBoard[adjacentCoordinate.X, adjacentCoordinate.Y];

                        // checking if there is a valid move at the end of this vector, starting from current coord going in direction
                        if (!currentBoardCell.Equals(adjacentCell) && !adjacentCell.Equals(eBoardCell.Empty))
                        {
                            tempCoordToCheckForValidMovePossibility = CheckIfCouldFlipInDirection(adjacentCoordinate, direction, eBoardCell.Empty);
                        }

                        // the matrix coord isnt null - we found a valid move. adding the cooridante of that valid move to our valid moves list
                        if (tempCoordToCheckForValidMovePossibility != null)
                        {
                            if (!i_Player.ValidMoves.Contains((sMatrixCoordinate)tempCoordToCheckForValidMovePossibility))
                            {
                                i_Player.ValidMoves.Add((sMatrixCoordinate)tempCoordToCheckForValidMovePossibility);
                            }
                        }
                    }
                }
            }
        }

        // runs recursivly trying to find a coordinate that has i_cellType type on it
        public sMatrixCoordinate? CheckIfCouldFlipInDirection(sMatrixCoordinate i_CurrentCoordinate, sMatrixCoordinate i_Direction, eBoardCell i_CellType)
        {
            sMatrixCoordinate? returnedCoordinate = null;
            sMatrixCoordinate adjacentCoordinate = i_CurrentCoordinate + i_Direction;

            int boardSize = m_CurrentGameState.BoardSize - 1;
            if (isInBounds(adjacentCoordinate, m_CurrentGameState.BoardSize))
            {
                eBoardCell adjacentCell = m_CurrentGameState.CurrentBoard.GameBoard[adjacentCoordinate.X, adjacentCoordinate.Y];

                // if adjacent cell in given direction is a coin of cellType - return MatrixCoordinate of that cell
                if (adjacentCell.Equals(i_CellType))
                {
                    returnedCoordinate = adjacentCoordinate;
                }
                else if (!adjacentCell.Equals(m_CurrentGameState.CurrentBoard.GameBoard[i_CurrentCoordinate.X, i_CurrentCoordinate.Y]))
                {
                    // if adjacent cell is the third kind of cell (not same as me, not same as cellType) or out of board bounds - return null
                    returnedCoordinate = null;
                }
                else
                {
                    // if adjacent cell is same color - send recursively with updated currentCoordinate (of the adjacent cell) and same direction
                    returnedCoordinate = CheckIfCouldFlipInDirection(adjacentCoordinate, i_Direction, i_CellType);
                }
            }

            return returnedCoordinate;
        }
    }
}
