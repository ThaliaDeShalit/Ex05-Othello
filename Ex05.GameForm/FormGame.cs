using Ex05.GameLogic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ex05.GameForm
{
    class FormGame : Form
    {
        private const string k_Title = "Othello - {0}'s turn";

        private const int k_ButtonSize = 50;
        private const int k_ButtonMargin = 4;
        private const int k_EdgeMargin = 10;

        private int m_BoardSize;
        private GameButton[,] m_BoardCells;
        private GameState m_CurrentGameState;
        private GameOperations m_GameOperator;

        private int m_NumOfGamesPlayed;

        public FormGame(int i_BoardSize, bool i_AgainstComputer)
        {
            ClientSize = new Size(2 * k_EdgeMargin + i_BoardSize * k_ButtonSize + (i_BoardSize - 1) * k_ButtonMargin,
                2 * k_EdgeMargin + i_BoardSize * k_ButtonSize + (i_BoardSize - 1) * k_ButtonMargin);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Fixed3D;

            m_BoardSize = i_BoardSize;
            m_BoardCells = new GameButton[m_BoardSize, m_BoardSize];

            m_CurrentGameState = new GameState(i_BoardSize, i_AgainstComputer);
            m_GameOperator = new GameOperations(m_CurrentGameState);

            addButtons();

            runGame();
        }

        private void runGame()
        {
            m_GameOperator.CalcValidMoves(m_CurrentGameState.FirstPlayer);
            m_GameOperator.CalcValidMoves(m_CurrentGameState.SecondPlayer);

            updateBoard();
            if (m_CurrentGameState.FirstPlayer.HasValidMoves() || m_CurrentGameState.SecondPlayer.HasValidMoves())
            {
                m_CurrentGameState.NextTurn();

                if (m_CurrentGameState.CurrentPlayer.HasValidMoves())
                {
                    if ((m_CurrentGameState.CurrentPlayer == m_CurrentGameState.SecondPlayer) && m_CurrentGameState.IsAgainstComputer)
                    {
                        sMatrixCoordinate move = m_CurrentGameState.CurrentPlayer.MakeMove();
                        m_GameOperator.UpdateGame(move);

                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        MessageBox.Show(string.Format("Computer played {0},{1}", (move.x + 1), (move.y + 1)), "Othello", buttons);

                        runGame();
                    }
                    else
                    {
                        setPossibleMoves();
                    }
                }
                else
                {
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(string.Format("{0} has no moves!", m_CurrentGameState.CurrentPlayer.Name), "Othello", buttons);
                    runGame();
                }
            }
            else
            {
                gameOver();
            }
        }

        private void gameOver()
        {
            m_GameOperator.CalcScore();

            Player winner = null;
            Player loser = null;

            bool tie = false;

            if (m_CurrentGameState.FirstPlayer.Score > m_CurrentGameState.SecondPlayer.Score)
            {
                winner = m_CurrentGameState.FirstPlayer;
                loser = m_CurrentGameState.SecondPlayer;
            }
            else if (m_CurrentGameState.FirstPlayer.Score < m_CurrentGameState.SecondPlayer.Score)
            {
                winner = m_CurrentGameState.SecondPlayer;
                loser = m_CurrentGameState.FirstPlayer;
            }
            else
            {
                tie = true;
            }

            string messageBoxMessage;

            m_NumOfGamesPlayed++;

            if (!tie)
            {
                winner.GamesWon++;

                messageBoxMessage = string.Format(
@"{0} Won!! ({1}/{2}) ({3}/{4})
Would you like another round?", winner.Name, winner.Score, loser.Score, winner.GamesWon, m_NumOfGamesPlayed);
            }
            else
            {
                messageBoxMessage = string.Format(
@"It's a tie!! ({0}/{0})
Would you like another round?", m_CurrentGameState.FirstPlayer.Score);
            }

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(messageBoxMessage, "Othello", buttons);

            if (result == DialogResult.Yes)
            {
                m_CurrentGameState.Restart();
                runGame();
            }
            else
            {
                Close();
            }
        }

        private bool checkIfGameIsOver()
        {
            bool gameIsOver = false;
            if (m_CurrentGameState.GameOver())
            {
                gameIsOver = true;
            }

            return gameIsOver;
        }

        private void addButtons()
        {
            int rowOffset = 10;
            int lineOffset;
            for (int row = 0; row < m_BoardSize; row++)
            {
                lineOffset = 10;
                for (int line = 0; line < m_BoardSize; line++)
                {
                    GameButton button = new GameButton(row, line);
                    button.Size = new Size(k_ButtonSize, k_ButtonSize);

                    int rowMargin = k_ButtonMargin * row;
                    int lineMargin = k_ButtonMargin * line;

                    button.Location = new Point(rowOffset + rowMargin, lineOffset + lineMargin);
                    button.Enabled = false;
                    Controls.Add(button);
                    lineOffset += k_ButtonSize;

                    m_BoardCells[row, line] = button;
                }

                rowOffset += k_ButtonSize;
            }
        }

        private void updateBoard()
        {
            for (int i = 0; i < m_BoardSize; i++)
            {
                for (int j = 0; j < m_BoardSize; j++)
                {
                    GameButton currButton = m_BoardCells[i, j];

                    currButton.Click -= buttonToChoose_Click;

                    switch (m_CurrentGameState.CurrentBoard.GameBoard[i, j])
                    {
                        case eBoardCell.Black:
                            currButton.BackColor = Color.Black;
                            currButton.Text = "O";
                            break;
                        case eBoardCell.White:
                            currButton.BackColor = Color.White;
                            currButton.Text = "O";
                            break;
                        case eBoardCell.Empty:
                            currButton.BackColor = default(Color);
                            currButton.Text = string.Empty;
                            break;
                    }

                    currButton.Enabled = false;
                }
            }
        }

        private void setPossibleMoves()
        {
            string currentPlayerName = m_CurrentGameState.CurrentPlayer.Name;
            Text = string.Format(k_Title, currentPlayerName);
            List<sMatrixCoordinate> possibleMoves = m_CurrentGameState.CurrentPlayer.ValidMoves;

            foreach (sMatrixCoordinate coord in possibleMoves)
            {
                GameButton buttonToChoose = m_BoardCells[coord.x, coord.y];
                buttonToChoose.BackColor = Color.LightGreen;
                buttonToChoose.Enabled = true;
                buttonToChoose.Click += buttonToChoose_Click;
            }
        }

        private void buttonToChoose_Click(object sender, EventArgs e)
        {
            GameButton button = sender as GameButton;
            m_GameOperator.UpdateGame(new sMatrixCoordinate(button.X, button.Y));
            runGame();
        }
    }
}