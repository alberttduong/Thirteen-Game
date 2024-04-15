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
        public Card[] cards = new Card[52];

        public struct StandardDeck
        {
            public IEnumerator<(int, int)> GetEnumerator()
            {
                for (int suit = 0; suit < Card.NumOfSuits; suit++)
                {
                    for (int number = 0; number < Card.NumOfNumbers; number++)
                    {
                        yield return (suit, number);
                    }
                }
            }
        }

        public static int suitAndNumToOrderedIndex(int suit, int num)
        {
            return suit * Card.NumOfNumbers + num;
        }

        public void initializeCards()
        {
            var standardDeck = new StandardDeck();
            foreach ((int suit, int num) in standardDeck)
            {
                int index = suitAndNumToOrderedIndex(suit, num);
                this.cards[index] = new Card(num, suit);
            }
        }

        public Deck()
        {
            initializeCards();
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
