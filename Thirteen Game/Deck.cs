using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thirteen_Game
{
    public class Deck
    {
        public Card[] temp_cards = new Card[52];
        public Stack<Card> cards = new Stack<Card>(52); // Stack of cards is initialized after shuffling

        public struct StandardDeck
        {
            public IEnumerator<(int, int)> GetEnumerator()
            {
                for (int suit = 0; suit < Card.NumOfSuits; suit++)
                {
                    for (int number = 0; number < Card.NumOfNumbers; number++)
                        yield return (suit, number);
                }
            }
        }

        public static int suitAndNumToOrderedIndex(int suit, int num)
        {
            return suit * Card.NumOfNumbers + num;
        }

        public Deck() {
            var standardDeck = new StandardDeck();
            foreach ((int suit, int num) in standardDeck)
            {
                int index = suitAndNumToOrderedIndex(suit, num);
                this.temp_cards[index] = new Card(num, suit);
            }
        }

        public void shuffle()
        {
            // Swaps 2 random cards in the deck
            // numShuffles times in order to shuffle the deck.

            const int numShuffles = 300;
            var random = new Random();

            for (int i = 0; i < numShuffles; ++i)
            {
                int first = random.Next(52);
                int second = random.Next(52);

                Card temp = temp_cards[first];
                temp_cards[first] = temp_cards[second];
                temp_cards[second] = temp;
            }

            cards = new Stack<Card>(temp_cards);
        }

        public void dealThirteenCards(Player player)
        {
            for (int i = 0; i < 13; ++i)
            {
                player.hand.Add(this.cards.Pop());
            }
        }

        public int count()
        {
            return this.cards.Count;
        }

        public void printDeck()
        {
            foreach (Card card in cards)
                Console.WriteLine(card);
        }
    }
}
