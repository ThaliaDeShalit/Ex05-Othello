using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05.GameLogic
{
    // This class holds the data of each player
    class Player
    {
        private readonly string m_Name;
        private eColor m_Color;
        private int m_Score;
        private List<sMatrixCoordinate> m_ValidMoves = new List<sMatrixCoordinate>();
        private List<sMatrixCoordinate> m_CellsOccupied = new List<sMatrixCoordinate>();
        private Random m_Rnd;

        // ctor - adding to the cellsOccupied list the first two coins set on the game board
        public Player(string i_Name, eColor i_Color, sMatrixCoordinate i_FirstCoinPosition, sMatrixCoordinate i_SecondCoindPosition)
        {
            m_Name = i_Name;
            m_Color = i_Color;
            m_CellsOccupied.Add(i_FirstCoinPosition);
            m_CellsOccupied.Add(i_SecondCoindPosition);
            m_Score = 0;

            m_Rnd = new Random();
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public int Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value;
            }
        }

        public eColor Color
        {
            get
            {
                return m_Color;
            }
        }

        public List<sMatrixCoordinate> CellsOccupied
        {
            get
            {
                return m_CellsOccupied;
            }
        }

        public List<sMatrixCoordinate> ValidMoves
        {
            get
            {
                return m_ValidMoves;
            }
        }

        // Returns true if the player has any valid moves left, false otherwise
        public bool HasValidMoves()
        {
            bool hasMoves = m_ValidMoves.Count != 0;

            return hasMoves;
        }

        // In case another game is started, this method clears the data in order for the new game to begin
        public void Restart(sMatrixCoordinate i_FirstCoinPosition, sMatrixCoordinate i_SecondCoindPosition)
        {
            // clearing the lists of matrix coordinate
            m_ValidMoves.Clear();
            m_CellsOccupied.Clear();

            // re adds the first two coins set on the game board to the cellOccupied list
            m_CellsOccupied.Add(i_FirstCoinPosition);
            m_CellsOccupied.Add(i_SecondCoindPosition);
        }

        // in case player is played by computer, this method decides randomly which possible move to make
        public sMatrixCoordinate MakeMove()
        {
            int randomNumber = m_Rnd.Next() % m_ValidMoves.Count;

            return m_ValidMoves[randomNumber];
        }
    }
}
