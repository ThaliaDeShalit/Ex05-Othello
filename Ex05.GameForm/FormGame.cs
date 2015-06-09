using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ex05.GameForm
{
    class FormGame : Form
    {
        int k_buttonSize = 50;
        int k_buttonMargin = 4;
        int k_edgeMargin = 10;

        int m_BoardSize;

        public FormGame(int i_BoardSize, bool i_AgainstComputer)
        {
            ClientSize = new Size(2 * k_edgeMargin + i_BoardSize * k_buttonSize + (i_BoardSize - 1) * k_buttonMargin,
                2 * k_edgeMargin + i_BoardSize * k_buttonSize + (i_BoardSize - 1) * k_buttonMargin);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Fixed3D;

            m_BoardSize = i_BoardSize;

            addButtons();
        }

        private void addButtons()
        {
            int row;
            int line;
            int rowOffset = 10;
            int lineOffset = 10;
            for (row = 0; row < m_BoardSize; row++)
            {
                lineOffset = 10;
                for (line = 0; line < m_BoardSize; line++)
                {
                    Button button = new Button();
                    button.Size = new Size(k_buttonSize, k_buttonSize);

                    int rowMargin = k_buttonMargin * row;
                    int lineMargin = k_buttonMargin * line;

                    button.Location = new Point(rowOffset + rowMargin, lineOffset + lineMargin);
                    button.Enabled = false;
                    Controls.Add(button);
                    lineOffset += k_buttonSize;
                }

                rowOffset += k_buttonSize;
            }
        }
    }
}