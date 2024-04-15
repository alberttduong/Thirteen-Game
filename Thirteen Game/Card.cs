using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thirteen_Game
{
    public class Card
    {
        public const int NumOfSuits = 4;
        public const int NumOfNumbers = 13;

        public int number { get; }
        public int suit { get; }

        public Card(int number, int suit)
        {
            this.number = number;
            this.suit = suit;
        }
    }
}
