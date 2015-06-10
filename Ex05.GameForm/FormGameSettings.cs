using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Ex05.GameForm
{
    internal class FormGameSettings : Form
    {
        private const int k_MaxBoardSize = 12;
        private const string k_BoardSize = "Board Size: {0} x {0} (click to increase)";
        private const string k_PlayAgainstFriend = "Play against your friend";
        private const string k_PlayAgainstComputer = "Play against the computer";
        private const string k_SettingsFormTitle = "Othello - Game Settings";

        private const int k_WindowWidth = 300;
        private const int k_WindowHeight = 150;
        private const int k_WindowSideMargins = 10;
        private const int k_WindowTopBottomMargins = 20;
        private const int k_ButtonHeight = (k_WindowHeight - 3 * k_WindowTopBottomMargins) / 2;
        
        private Button m_ButtonBoardSize = new Button();
        private Button m_ButtonPlayAgainstComputer = new Button();
        private Button m_ButtonPlayAgainstFriend = new Button();

        private int m_BoardSize = 6;

        public FormGameSettings()
        {
            ClientSize = new Size(k_WindowWidth, k_WindowHeight);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            StartPosition = FormStartPosition.CenterScreen;
            Text = k_SettingsFormTitle;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitControls();
        }

        private void InitControls()
        {
            m_ButtonBoardSize.Text = string.Format(k_BoardSize, m_BoardSize);
            m_ButtonBoardSize.Location = new Point(k_WindowSideMargins, k_WindowTopBottomMargins);
            m_ButtonBoardSize.Width = k_WindowWidth - 2 * k_WindowSideMargins;
            m_ButtonBoardSize.Height = k_ButtonHeight;

            m_ButtonPlayAgainstComputer.Text = k_PlayAgainstComputer;
            m_ButtonPlayAgainstComputer.Location = new Point(k_WindowSideMargins, m_ButtonBoardSize.Location.Y + k_ButtonHeight + k_WindowTopBottomMargins);
            m_ButtonPlayAgainstComputer.Width = (k_WindowWidth - 3 * k_WindowSideMargins) / 2;
            m_ButtonPlayAgainstComputer.Height = k_ButtonHeight;

            m_ButtonPlayAgainstFriend.Text = k_PlayAgainstFriend;
            m_ButtonPlayAgainstFriend.Location = new Point(m_ButtonPlayAgainstComputer.Location.X + m_ButtonPlayAgainstComputer.Width + k_WindowSideMargins,
                                                            m_ButtonBoardSize.Location.Y + k_ButtonHeight + k_WindowTopBottomMargins);
            m_ButtonPlayAgainstFriend.Width = (k_WindowWidth - 3 * k_WindowSideMargins) / 2;
            m_ButtonPlayAgainstFriend.Height = k_ButtonHeight;

            Controls.AddRange(new Control[] { m_ButtonBoardSize, m_ButtonPlayAgainstComputer, m_ButtonPlayAgainstFriend });

            m_ButtonBoardSize.Click += new EventHandler(m_ButtonBoardSize_Click);
            m_ButtonPlayAgainstComputer.Click += new EventHandler(m_ButtonPlayAgainstComputer_Click);
            m_ButtonPlayAgainstFriend.Click += new EventHandler(m_ButtonPlayAgainstFriend_Click);
        }

        private void m_ButtonBoardSize_Click(object sender, EventArgs e)
        {
            if (m_BoardSize < k_MaxBoardSize)
            {
                m_BoardSize += 2;
            }

            m_ButtonBoardSize.Text = string.Format(k_BoardSize, m_BoardSize);
        }

        private void m_ButtonPlayAgainstComputer_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void m_ButtonPlayAgainstFriend_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        public int BoardSize
        {
            get
            {
                return m_BoardSize;
            }
        }
    }
}
