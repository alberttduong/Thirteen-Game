using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Thirteen_Game
{
    internal class Thirteen
    {
        static void Main(string[] args)
        {
            var game = new GameState();
            while (!game.won)
            {
                game.printTurn();
                game.printPlayerHands();
                if (game.activePlayer is Bot)
                {
                    game.botPlay();
                } else
                {
                    game.playerPlay();
                }
                game.endTurn();
                Console.ReadLine();
            }
        }
    }
}
