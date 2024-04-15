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

    public class Deck
    {
        private Card[] _cards = new Card[52];

        private void initializeCards()
        {
        }

        public Deck()
        {
            
        }
    }

    public class Player
    {
        public int id { get; set; }
        public Player(int id)
        {
            this.id = id;
        }

    }

    internal class Thirteen
    {
        static void Main(string[] args)
        {
        }
    }
}
