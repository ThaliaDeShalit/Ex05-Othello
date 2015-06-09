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
           // FormGame formGame;
            bool isGameAgainstComputer;

            ap

            if (formGameSettings.ShowDialog() == DialogResult.Yes)
            {
                isGameAgainstComputer = true;
            }
            else
            {
                isGameAgainstComputer = false;
            }

         //   formGame = new FormGame(formGameSettings.BoardSize, isGameAgainstComputer);

            //formGame.ShowDialog();
        }
    }
}
