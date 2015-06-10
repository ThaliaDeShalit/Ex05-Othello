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
        private int k_buttonSize = 50;
        private int k_buttonMargin = 4;
        private int k_edgeMargin = 10;

        private int m_BoardSize;
        private Button[,] m_BoardCells;
        private GameState m_CurrentGameState;

        public FormGame(int i_BoardSize, bool i_AgainstComputer)
        {
            ClientSize = new Size(2 * k_edgeMargin + i_BoardSize * k_buttonSize + (i_BoardSize - 1) * k_buttonMargin,
                2 * k_edgeMargin + i_BoardSize * k_buttonSize + (i_BoardSize - 1) * k_buttonMargin);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Fixed3D;

            m_BoardSize = i_BoardSize;
            m_BoardCells = new Button[m_BoardSize, m_BoardSize];

            m_CurrentGameState = new GameState("White", "Black", i_BoardSize, i_AgainstComputer);

            addButtons();
            initializeGame();
            setPossibleMoves();
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

        private void initializeGame()
        {
            for (int i = 0; i < m_BoardSize - 1; i++)
            {
                for (int j = 0; j < m_BoardSize - 1; j++)
                {
                    switch (m_CurrentGameState.CurrentBoard.GameBoard[i, j])
                    {
                        case eBoardCell.Black:
                            m_BoardCells[i, j].BackColor = Color.Black;
                            m_BoardCells[i, j].Text = "O";
                            m_BoardCells[i, j].ForeColor = Color.White;
                            break;
                        case eBoardCell.White:
                            m_BoardCells[i, j].BackColor = Color.White;
                            m_BoardCells[i, j].Text = "O";
                            m_BoardCells[i, j].ForeColor = Color.Black;
                            break;
                    }

                    m_BoardCells[i, j].Enabled = false;
                }
            }
        }

        private void setPossibleMoves()
        {
            for (int i = 0; i < m_BoardSize; i++)
            {
                for (int j = 0; j < m_BoardSize; j++)
                {
                    if (m_CurrentGameState.CurrentPlayer.ValidMoves.Contains(new sMatrixCoordinate(i, j)))
                    {
                        Button currButton = m_BoardCells[i, j];
                        currButton.BackColor = Color.LightGreen;
                        currButton.Enabled = true;
                        Controls.Add(currButton);
                        currButton.Click += currButton_Click;

                    }
                }
            }
        }

        private void currButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}