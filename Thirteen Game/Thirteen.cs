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
            Console.WriteLine(".....13Thirteen13.....");
            Console.WriteLine("Play cards using this format: '0 1 2'");
            Console.WriteLine("Pass by entering 'p'");
            Console.WriteLine();

            var game = new GameState();
            while (!game.won)
            {
                game.printTurn();
                game.printPlayerHands();

                game.play();

                game.endTurn();
                Console.ReadLine();
            }
            Console.WriteLine($"{game.activePlayer.header()}won!");
            Console.ReadLine();
        }
    }
}
