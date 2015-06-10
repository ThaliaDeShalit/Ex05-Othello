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
        
        private int k_buttonSize = 50;
        private int k_buttonMargin = 4;
        private int k_edgeMargin = 10;

        private int m_BoardSize;
        private Button[,] m_BoardCells;
        private GameState m_CurrentGameState;
        private GameOperations m_GameOperator;

        private sMatrixCoordinate m_Move;

        public FormGame(int i_BoardSize, bool i_AgainstComputer)
        {
            ClientSize = new Size(2 * k_edgeMargin + i_BoardSize * k_buttonSize + (i_BoardSize - 1) * k_buttonMargin,
                2 * k_edgeMargin + i_BoardSize * k_buttonSize + (i_BoardSize - 1) * k_buttonMargin);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Fixed3D;

            m_BoardSize = i_BoardSize;
            m_BoardCells = new Button[m_BoardSize, m_BoardSize];

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
                if (!checkIfGameIsOver())
                {
                    if (m_CurrentGameState.CurrentPlayer.HasValidMoves())
                    {
                        m_CurrentGameState.NextTurn();
                        setPossibleMoves();                      
                    }
                    else
                    {
                        //MessageBox show no moves;
                        m_CurrentGameState.NextTurn();
                    }
                }
        }

        private bool checkIfGameIsOver()
        {
            bool gameIsOver = false;
            //if (m_CurrentGameState.GameOver())
            //{
            //    m_GameOperator.CalcScore();
            //    MessageBox = Show score and ask for second game;
            //    gameIsOver = true;
            //}

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
                    Button button = new Button();
                    button.Size = new Size(k_buttonSize, k_buttonSize);

                    int rowMargin = k_buttonMargin * row;
                    int lineMargin = k_buttonMargin * line;

                    button.Location = new Point(rowOffset + rowMargin, lineOffset + lineMargin);
                    button.Enabled = false;
                    Controls.Add(button);
                    lineOffset += k_buttonSize;

                    m_BoardCells[row, line] = button; 
                }

                rowOffset += k_buttonSize;
            }
        }

        private void updateBoard()
        {
            
            for (int i = 0; i < m_BoardSize; i++)
            {
                for (int j = 0; j < m_BoardSize; j++)
                {
                    Button currButton = m_BoardCells[i, j];

                    currButton.Click -= currButton_Click;

                    switch (m_CurrentGameState.CurrentBoard.GameBoard[i, j])
                    {
                        case eBoardCell.Black:
                            currButton.BackColor = Color.Black;
                            currButton.Text = "O";
                            //currbutton.enabled = true;
                            currButton.ForeColor = Color.White;
                            break;
                        case eBoardCell.White:
                            currButton.BackColor = Color.White;
                            currButton.Text = "O";
                            currButton.ForeColor = Color.Black;
                            break;
                        case eBoardCell.Empty:
                            currButton.BackColor = default(Color);
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

            for (int i = 0; i < m_BoardSize; i++)
            {
                for (int j = 0; j < m_BoardSize; j++)
                {
                    if (possibleMoves.Contains(new sMatrixCoordinate(i, j)))
                    {
                        Button currButton = m_BoardCells[i, j];
                        currButton.BackColor = Color.LightGreen;
                        currButton.Enabled = true;
                        //Controls.Add(currButton);
                        currButton.Click += currButton_Click;
                    }
                }
            }

            //if (possibleMoves.Count == 0)
            //{
            //    MessageBoxButtons buttons = MessageBoxButtons.OK;
            //    MessageBox.Show("Othello", string.Format("{0} has no moves!", currentPlayerName), buttons);
            //}
            
        }

        private void currButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < m_BoardSize; i++)
            {
                for (int j = 0; j < m_BoardSize; j++)
                {
                    if (m_BoardCells[i, j] == sender)
                    {
                        m_GameOperator.UpdateGame(new sMatrixCoordinate(i, j));
                        runGame();
                        break;
                    }
                }
            }
        }

    }
}