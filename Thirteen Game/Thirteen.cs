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

                game.play();

                game.endTurn();
                Console.ReadLine();
            }
        }
    }
}
