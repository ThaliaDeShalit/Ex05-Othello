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
        private const string k_Othello = "Othello";
        private const string k_ComputerMove = "Computer played {0},{1}";
        private const string k_NoPossibleMoves = "{0} has no moves!";
        private const string k_WinMessage = 
@"{0} Won!! ({1}/{2}) ({3}/{4})
Would you like another round?";
        private const string k_TieMessage =
@"It's a tie!! ({0}/{0})
Would you like another round?";
        private const string k_ButtonText = "O";

        private const int k_ButtonSize = 50;
        private const int k_ButtonMargin = 4;
        private const int k_EdgeMargin = 10;

        private readonly int m_BoardSize;
        private GameButton[,] m_BoardCells;
        private GameState m_CurrentGameState;
        private GameOperations m_GameOperator;

        private int m_NumOfGamesPlayed;

        public FormGame(int i_BoardSize, bool i_AgainstComputer)
        {
            // Dynamically calculate the form size, according to the set button sizes (k_ButtonSize),
            // the margins between the buttons (k_ButtonMargin) and the edge margins (k_EdgeMargin) and
            // of course the size of the board (i_BoardSize)
            ClientSize = new Size(2 * k_EdgeMargin + i_BoardSize * k_ButtonSize + (i_BoardSize - 1) * k_ButtonMargin,
                2 * k_EdgeMargin + i_BoardSize * k_ButtonSize + (i_BoardSize - 1) * k_ButtonMargin);

            // Set the form in the center of the display, set the style and fix the size (Not resizable)
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Fixed3D;

            m_BoardSize = i_BoardSize;
            m_BoardCells = new GameButton[m_BoardSize, m_BoardSize];

            // Intiate a GameState and a GameOperations objects to run the game logic
            m_CurrentGameState = new GameState(i_BoardSize, i_AgainstComputer);
            m_GameOperator = new GameOperations(m_CurrentGameState);

            // Add all the board pieces to the board
            addButtons();

            // Start the game
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
                        MessageBox.Show(string.Format(k_ComputerMove, (move.x + 1), (move.y + 1)), k_Othello, buttons);

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
                    MessageBox.Show(string.Format(k_NoPossibleMoves, m_CurrentGameState.CurrentPlayer.Name), k_Othello, buttons);
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

                messageBoxMessage = string.Format(k_WinMessage , winner.Name, winner.Score, loser.Score, winner.GamesWon, m_NumOfGamesPlayed);
            }
            else
            {
                messageBoxMessage = string.Format(k_TieMessage , m_CurrentGameState.FirstPlayer.Score);
            }

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(messageBoxMessage, k_Othello, buttons);

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

        // Initiale creation of all the board buttons and adding them to the board matrix
        // and Controls
        private void addButtons()
        {
            int rowOffset = k_EdgeMargin;
            int lineOffset;
            for (int row = 0; row < m_BoardSize; row++)
            {
                lineOffset = k_EdgeMargin;
                for (int line = 0; line < m_BoardSize; line++)
                {
                    // Create a new button and set it's coordinates on the board
                    GameButton button = new GameButton(row, line);
                    // Set it's size according to the desired button size (k_ButtonSize)
                    button.Size = new Size(k_ButtonSize, k_ButtonSize);

                    // Dynamically calculate it's position on the form
                    int rowMargin = k_ButtonMargin * row;
                    int lineMargin = k_ButtonMargin * line;

                    // Set it's location, set its Enabled status to false and add it to the form Controls
                    button.Location = new Point(rowOffset + rowMargin, lineOffset + lineMargin);
                    button.Enabled = false;
                    Controls.Add(button);
                    // Update the line offset for the next button
                    lineOffset += k_ButtonSize;

                    // Finally, add it to the board matrix in the proper position
                    m_BoardCells[row, line] = button;
                }

                // Update the row offset for the next line of buttons
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
                            currButton.Text = k_ButtonText;
                            break;
                        case eBoardCell.White:
                            currButton.BackColor = Color.White;
                            currButton.Text = k_ButtonText;
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