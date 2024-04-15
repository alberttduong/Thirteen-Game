using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thirteen_Game
{
    public enum Suits
    {
        Spades,
        Clubs,
        Diamonds,
        Hearts
    }

    public class Card
    {
        public int number { get; }
        public string suit { get; }

        public Card(int number, string suit)
        {
            this.number = number;
            this.suit = suit;
        }
    }

    public class Deck
    {
        private Card[] _cards = new Card[52];

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

    internal class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
