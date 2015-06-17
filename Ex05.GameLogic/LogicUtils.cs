using System;
using System.Collections.Generic;
using System.Text;

// this file hold all the logic componenets that arent classes
namespace Ex05.GameLogic
{
    // This struct holds two int values representing a coordinate inside a matrix, and for this game's purposes
    // holds the coordinates inside the game board
    public struct sMatrixCoordinate
    {
        private int m_X;
        private int m_Y;

        public sMatrixCoordinate(int i_X, int i_Y)
        {
            m_X = i_X;
            m_Y = i_Y;
        }

        public int X
        {
            get
            {
                return m_X;
            }
        }

        public int Y
        {
            get
            {
                return m_Y;
            }
        }

        // adds two matrix coordinates to create a new one
        public static sMatrixCoordinate operator +(sMatrixCoordinate i_FirstMatrixCoordiante, sMatrixCoordinate i_SecondMatrixCoordinate)
        {
            return new sMatrixCoordinate(i_FirstMatrixCoordiante.X + i_SecondMatrixCoordinate.X, i_FirstMatrixCoordiante.Y + i_SecondMatrixCoordinate.Y);
        }
    }

    // holds the different board cell types
    public enum eBoardCell
    {
        Black,
        White,
        Empty
    }

    // holds the different color types
    public enum eColor
    {
        Black,
        White
    }
}
