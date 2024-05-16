using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thirteen_Game
{
    public class Card : IComparable
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

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            Card card = obj as Card;
            if (card < this) return 1;
            if (card > this) return -1;
            return 0;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            if (GetType() != obj.GetType())
                return false;

            Card other = (Card)obj;

            return other.number == this.number
                && other.suit == this.suit;
        }

        public static bool operator ==(Card a, Card b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Card a, Card b)
        {
            return !a.Equals(b);
        }

        public static bool operator <(Card a, Card b)
        {
            if (a.number < b.number) return true;
            if (a.number == b.number && a.suit < b.suit) return true;
            return false;
        }

        public static bool operator >(Card a, Card b)
        {
            if (a.number > b.number) return true;
            if (a.number == b.number && a.suit > b.suit) return true;
            return false;
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

        public string debugString()
        {
            return $"{this.number} {this.suit}";
        }


        public override string ToString()
        {
            return $"{shortNumberName()}{shortSuit()}";
            //return numberName() + " of " + suitName();
        }

        public string shortNumberName()
        {
            if (number > 7) 
                return char.ToString(numberName().ElementAt(0));
            return numberName();
        }

        public string shortSuit()
        {
            return char.ToString(suitName().ElementAt(0));
        }
    }
}
