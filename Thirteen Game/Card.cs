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

        public static string numberName(int num)
        {
            // 0 - 12
            // Ordered by rank, where 2 is the highest
            // and 3 is the lowest ranking card number
            switch (num)
            {
                case 8:
                    return "Jack";
                case 9:
                    return "Queen";
                case 10:
                    return "King";
                case 11:
                    return "Ace";
                case 12:
                    return "2";
                default:
                    return (num + 3).ToString();
            }
        }

        public string numberName()
        {
            return numberName(this.number);
        }

        public static string suitName(int suit)
        {
            switch (suit)
            {
                case 0:
                    return "Spades";
                case 1:
                    return "Clubs";
                case 2:
                    return "Diamonds";
                default:
                    return "Hearts";
            }
        }

        public string suitName()
        {
            return suitName(this.suit);
        }

        public override string ToString()
        {
            return numberName() + " of " + suitName();
        }
    }
}
