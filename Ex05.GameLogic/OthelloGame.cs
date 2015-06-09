using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05.GameLogic
{
    // This class represents the flow of the game - making sure to end the game when it ends, switch turns between players,
    // calling ui methods to get input and to show the current state of the game when needed
    class OthelloGame
    {
        private int m_BoardSize;
        private bool m_AgainstComputer;
        
        public void RunGame()
        {
            bool isGameAgainstComputer;
            int boardSize;
            sMatrixCoordinate move;
            GameState currGameState;
            GameOperations gameOperator;
            UI userInterface;
            bool exitGame = false;

            // Getting the data to initalze all the instances needed for the game
            
            boardSize = UI.GetBoardSize();
            isGameAgainstComputer = UI.PlayAgainstComputer(out secondPlayerName);

            // Initalizing the instances that will run the game
            currGameState = new GameState(firstPlayerName, secondPlayerName, boardSize, isGameAgainstComputer);
            gameOperator = new GameOperations(currGameState);
            userInterface = new UI(boardSize);

            while (true)
            {
                // Calculating the valid moves for both players
                gameOperator.CalcValidMoves(currGameState.FirstPlayer);
                gameOperator.CalcValidMoves(currGameState.SecondPlayer);

                // At the begining of every turn, we show the current board
                userInterface.DrawBoard(currGameState);

                // Checking if the game is over, and if so calculating the score and calling relevant UI method
                if (currGameState.GameOver())
                {
                    gameOperator.CalcScore();
                    userInterface.EndGame(currGameState, out exitGame);
                }
                else if (!currGameState.CurrentPlayer.HasValidMoves())
                {
                    // Checking if the current player has moves to make sure not to let a player with no moves play
                    // and if he has no moves, lets the player know and go to next turn
                    userInterface.NoMoves(currGameState);
                    currGameState.NextTurn();
                    continue;
                }
                else
                {
                    // If we got to this point we can ask player to make a move, and then we update the game according
                    // to the move picked (either by human player or by random for the computer), and finally
                    // goes to next turn
                    move = userInterface.GetNextMove(currGameState, out exitGame);
                    gameOperator.UpdateGame(move);
                    currGameState.NextTurn();
                }

                // If, at any point, the player asked to leave the game, we break out of the loop
                if (exitGame)
                {
                    break;
                }
            }

            // calls a UI function to let the player know he is in the process of quiting the game, and then quits the game
            userInterface.QuitGame();
        }
    }
}
