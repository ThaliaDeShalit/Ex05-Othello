using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05.GameLogic
{
    // this class holds the data of the game - the game board, the players, and any data refernce we might need
    // during the game
    public class GameState
    {
        private Player m_FirstPlayer;
        private Player m_SecondPlayer;
        private Player m_CurrentPlayer; // keeps a pointer to current player playing the game
        private bool m_GameAgainstComputer; // keeps the value of whether the game is against computer or not
        private Board m_GameBoard;
        private sMatrixCoordinate? m_LastMove; // keeps the last move played (to be able to show to user) - is nullable in case no move was made
        private string m_NameOfPlayerThatPlayedLastMove; // keeps the name of the person who played the last move played (same reason as LastMove)

        public GameState(string i_FirstPlayerName, string i_SecondPlayerName, int i_BoardSize, bool i_AgainstComputer)
        {
            // setting the first player with the first two coins in the middle of the board
            m_FirstPlayer = new Player(i_FirstPlayerName, eColor.Black, new sMatrixCoordinate((i_BoardSize / 2) - 1, i_BoardSize / 2), new sMatrixCoordinate(i_BoardSize / 2, (i_BoardSize / 2) - 1));

            // setting the second player with his first two coins
            m_SecondPlayer = new Player(i_SecondPlayerName, eColor.White, new sMatrixCoordinate((i_BoardSize / 2) - 1, (i_BoardSize / 2) - 1), new sMatrixCoordinate(i_BoardSize / 2, i_BoardSize / 2));

            // randomly choosing who is the first player to play
            m_CurrentPlayer = getRandomPlayer();

            // setting wheter the game is against computer
            m_GameAgainstComputer = i_AgainstComputer;

            // creating a game board with size board size
            m_GameBoard = new Board(i_BoardSize);
        }

        // randomly returns a player
        private Player getRandomPlayer()
        {
            Player randomPlayer;
            Random rnd = new Random();
            int randomValue = rnd.Next() % 2;

            if (randomValue == 0)
            {
                randomPlayer = m_FirstPlayer;
            }
            else
            {
                randomPlayer = m_SecondPlayer;
            }

            return randomPlayer;
        }

        public int BoardSize
        {
            get
            {
                return (int)Math.Sqrt(CurrentBoard.GameBoard.Length);
            }
        }

        public Board CurrentBoard
        {
            get
            {
                return m_GameBoard;
            }
        }

        public sMatrixCoordinate? LastMovePlayed
        {
            get
            {
                return m_LastMove;
            }

            set
            {
                m_LastMove = value;
            }
        }

        public string NameOfLastPlayerThatPlayed
        {
            get
            {
                return m_NameOfPlayerThatPlayedLastMove;
            }

            set
            {
                m_NameOfPlayerThatPlayedLastMove = value;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return m_CurrentPlayer;
            }
        }

        public Player FirstPlayer
        {
            get
            {
                return m_FirstPlayer;
            }
        }

        public Player SecondPlayer
        {
            get
            {
                return m_SecondPlayer;
            }
        }

        public bool IsAgainstComputer
        {
            get
            {
                return m_GameAgainstComputer;
            }
        }

        // checks if none of the players have any valid moves left. if so - return true
        public bool GameOver()
        {
            bool isGameOver = true;

            if (m_FirstPlayer.HasValidMoves() || m_SecondPlayer.HasValidMoves())
            {
                isGameOver = false;
            }

            return isGameOver;
        }

        // switching the current player to the player not playing
        public void NextTurn()
        {
            if (m_CurrentPlayer.Name == m_FirstPlayer.Name)
            {
                m_CurrentPlayer = m_SecondPlayer;
            }
            else
            {
                m_CurrentPlayer = m_FirstPlayer;
            }
        }

        // returns the player with the higher score or null if there is no leader
        public Player GetLeader()
        {
            Player leader = null;

            if (m_FirstPlayer.Score > m_SecondPlayer.Score)
            {
                leader = m_FirstPlayer;
            }
            else if (m_FirstPlayer.Score < m_SecondPlayer.Score)
            {
                leader = m_SecondPlayer;
            }

            return leader;
        }

        // restarting the instance in case the game is being played once more from the begining
        public void Restart()
        {
            int sizeOfBoard = BoardSize;
            m_GameBoard = new Board(sizeOfBoard);

            m_FirstPlayer.Restart(new sMatrixCoordinate((sizeOfBoard / 2) - 1, (sizeOfBoard / 2) - 1), new sMatrixCoordinate(sizeOfBoard / 2, sizeOfBoard / 2));
            m_SecondPlayer.Restart(new sMatrixCoordinate((sizeOfBoard / 2) - 1, sizeOfBoard / 2), new sMatrixCoordinate(sizeOfBoard / 2, (sizeOfBoard / 2) - 1));

            m_CurrentPlayer = getRandomPlayer();

            m_LastMove = null;
        }
    }
}
