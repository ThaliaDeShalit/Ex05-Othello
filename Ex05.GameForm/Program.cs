using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Ex05.GameForm
{
    class Program
    {
        public static void Main()
        {
            FormGameSettings formGameSettings = new FormGameSettings();
            FormGame formGame;
            bool? isGameAgainstComputer = null;

            DialogResult result = formGameSettings.ShowDialog();
            if (result == DialogResult.Yes)
            {
                isGameAgainstComputer = true;
            }
            else if (result == DialogResult.No)
            {
                isGameAgainstComputer = false;
            }

            if (isGameAgainstComputer != null)
            {
                formGame = new FormGame(formGameSettings.BoardSize, (bool)isGameAgainstComputer);

                formGame.ShowDialog();
            }
        }
    }
}
