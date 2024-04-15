using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thirteen_Game
{
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

        public void shuffleDeck()
        {
            // Swaps 2 random cards in the deck
            // numShuffles times in order to shuffle the deck.

            const int numShuffles = 300;
            var random = new Random();

            for (int i = 0; i < numShuffles; ++i)
            {
                int first = random.Next(52);
                int second = random.Next(52);

                Card temp = cards[first];
                cards[first] = cards[second];
                cards[second] = temp;
            }
        }

        public void printDeck()
        {
            foreach (Card card in cards)
            {
                Console.WriteLine(card);   
            }
        }

        public Deck()
        {
            initializeCards();
        }
    }
}
