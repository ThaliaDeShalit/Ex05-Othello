using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ex05.GameLogic;

namespace Ex05.GameForm
{
    internal class FormGame : Form
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
            int clientSize = (2 * k_EdgeMargin) + (i_BoardSize * k_ButtonSize) + ((i_BoardSize - 1) * k_ButtonMargin);
            ClientSize = new Size(clientSize, clientSize);

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
            startNextGameLoop();
        }

        // this method calculates the possible moves of both player and then starts the
        // actual game loop - runGame. we use it to avoid multiple calculation in cases a player has no moves
        private void startNextGameLoop()
        {
            // calculate each player's current moves at the begining of every turn
            m_GameOperator.CalcValidMoves(m_CurrentGameState.FirstPlayer);
            m_GameOperator.CalcValidMoves(m_CurrentGameState.SecondPlayer);

            // start a new game loop
            runGame();
        }

        // this is the method the runs the "game loop" - according to the current game status decides if the
        // game is over, who's turn is it, and if that player has any moves, and continues the game accordingly
        private void runGame()
        {
            // Update the board presentation at everyturn. happens before any checks are made so that
            // the board is always updated
            updateBoard();

            // first, check if any of the players have a valid move left, cause if not - the game is over
            if (m_CurrentGameState.FirstPlayer.HasValidMoves() || m_CurrentGameState.SecondPlayer.HasValidMoves())
            {
                // still have moves, so we switch turns
                m_CurrentGameState.NextTurn();

                // we check if the current player has a move - if so, a move is chosen (or randomly selected in case it's
                // the computer's turn). otherwise - we create a message box stating that the current player has no moves
                if (m_CurrentGameState.CurrentPlayer.HasValidMoves())
                {
                    // if the game is against the computer and its the computer's turn, we get a random move
                    // from the Player class, and then create a message box showing which move was made
                    // so that the human player won't miss it
                    // otherwise - we set the possible moves on the board and let the human player (or any
                    // player if the game is not against the computer) choose a move
                    if ((m_CurrentGameState.CurrentPlayer == m_CurrentGameState.SecondPlayer) && m_CurrentGameState.IsAgainstComputer)
                    {
                        sMatrixCoordinate move = m_CurrentGameState.CurrentPlayer.MakeMove();
                        m_GameOperator.UpdateGame(move);

                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        MessageBox.Show(string.Format(k_ComputerMove, move.X + 1, move.Y + 1), k_Othello, buttons);

                        // a move was made, so there is a need to calculate the possible moves of both players
                        startNextGameLoop();
                    }
                    else
                    {
                        // shows possible moves on the board
                        setPossibleMoves();
                    }
                }
                else
                {
                    MessageBoxButtons buttons = MessageBoxButtons.OK;

                    // as we know no move was made, we can go ahead and start a new game loop without calculating the
                    // possible moves
                    MessageBox.Show(string.Format(k_NoPossibleMoves, m_CurrentGameState.CurrentPlayer.Name), k_Othello, buttons);
                    runGame();
                }
            }
            else
            {
                // if we got here, it means no player has any moves left
                gameOver();
            }
        }

        // this method is called when no player has any possible moves left. it prompts the message box asking
        // about another games, and also calcualtes the final scores of both players
        private void gameOver()
        {
            // claculating scores of both players
            m_GameOperator.CalcScore();

            Player winner = null;
            Player loser = null;

            bool tie = false;
            string messageBoxMessage;
            MessageBoxButtons buttons; 
            DialogResult result;

            // setting the winner and loser pointers to the correct players, or setting tie to 
            // true in case there's a tie
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

            // augmanting number of games played by 1
            m_NumOfGamesPlayed++;

            // creates relevant message to show in message box
            if (!tie)
            {
                winner.GamesWon++;

                messageBoxMessage = string.Format(k_WinMessage, winner.Name, winner.Score, loser.Score, winner.GamesWon, m_NumOfGamesPlayed);
            }
            else
            {
                messageBoxMessage = string.Format(k_TieMessage, m_CurrentGameState.FirstPlayer.Score);
            }

            buttons = MessageBoxButtons.YesNo;
            result = MessageBox.Show(messageBoxMessage, k_Othello, buttons);

            // according to the users choice, restarts the game or closes the application
            if (result == DialogResult.Yes)
            {
                m_CurrentGameState.Restart();
                startNextGameLoop();
            }
            else
            {
                Close();
            }
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
            GameButton currButton;

            // goes on the entire game board and set the cells to the relevant color
            for (int i = 0; i < m_BoardSize; i++)
            {
                for (int j = 0; j < m_BoardSize; j++)
                {
                    currButton = m_BoardCells[i, j];

                    // removes the button from the event listener's list as the possible moves were changed
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
            List<sMatrixCoordinate> possibleMoves = m_CurrentGameState.CurrentPlayer.ValidMoves;

            // sets the title of the form to the name of the current player
            Text = string.Format(k_Title, currentPlayerName);

            // for each possible move, sets the relevent button on the board to green and adds
            // it to the event listener's list
            foreach (sMatrixCoordinate coord in possibleMoves)
            {
                GameButton buttonToChoose = m_BoardCells[coord.X, coord.Y];
                buttonToChoose.BackColor = Color.LightGreen;
                buttonToChoose.Enabled = true;
                buttonToChoose.Click += buttonToChoose_Click;
            }
        }

        // the event method when a button is clicked
        private void buttonToChoose_Click(object i_Sender, EventArgs i_Args)
        {
            GameButton button = i_Sender as GameButton;

            // updated the game state according to the coordinates of the button that was clicked
            m_GameOperator.UpdateGame(new sMatrixCoordinate(button.X, button.Y));

            // start next game loop
            startNextGameLoop();
        }
    }
}