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
        
        private Button m_ButtonBoardSize = new Button();
        private Button m_ButtonPlayAgainstComputer = new Button();
        private Button m_ButtonPlayAgainstFriend = new Button();

        private int m_BoardSize = 6;

        public FormGameSettings()
        {
            this.Size = new Size(200, 200);
           // this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Othello - Game Settings";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitControls();
        }

        private void InitControls()
        {
            m_ButtonBoardSize.Text = string.Format("Board Size: {0}x{0} (click to increase)", m_BoardSize);
            m_ButtonBoardSize.Location = new Point(90, 25);
            m_ButtonBoardSize.Width = 180;
            m_ButtonBoardSize.Height = 30;

            m_ButtonPlayAgainstComputer.Text = "Play against the computer";
            m_ButtonPlayAgainstComputer.Location = new Point(45, m_ButtonBoardSize.Location.Y + 45);
            m_ButtonPlayAgainstComputer.Width = 90;
            m_ButtonPlayAgainstComputer.Height = 30;

            m_ButtonPlayAgainstFriend.Text = "Play against your friend";
            m_ButtonPlayAgainstFriend.Location = new Point(135, m_ButtonBoardSize.Location.Y + 45);
            m_ButtonPlayAgainstFriend.Width = 90;
            m_ButtonPlayAgainstFriend.Height = 30;

            this.Controls.AddRange(new Control[] { m_ButtonBoardSize, m_ButtonPlayAgainstComputer, m_ButtonPlayAgainstFriend });

            this.m_ButtonBoardSize.Click += new EventHandler(m_ButtonBoardSize_Click);
            this.m_ButtonPlayAgainstComputer.Click += new EventHandler(m_ButtonPlayAgainstComputer_Click);
            this.m_ButtonPlayAgainstFriend.Click += new EventHandler(m_ButtonPlayAgainstFriend_Click);
        }

        private void m_ButtonBoardSize_Click(object sender, EventArgs e)
        {
            if (m_BoardSize < k_MaxBoardSize)
            {
                m_BoardSize += 2;
            }

            m_ButtonBoardSize.Text = string.Format("Board Size: {0}x{0} (click to increase)", m_BoardSize);
        }

        private void m_ButtonPlayAgainstComputer_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void m_ButtonPlayAgainstFriend_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
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
