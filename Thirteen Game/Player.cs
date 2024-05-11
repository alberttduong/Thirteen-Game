using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Thirteen_Game
{
    public class Player
    {
        public int id { get; set; }
        public List<Card> hand { get; set; }

        public Player(int id)
        {
            this.id = id;
            this.hand = new List<Card>();
        }

        public void sortHand()
        {
            hand.Sort();
        }

        // Indices do not need to be sorted
        public Sequence sequenceFromHand(List<int> indices)
        {
            List<Card> cards = new List<Card>(12); // Max amount of cards in a sequence

            foreach (int index in indices)
                cards.Add(hand[index]);

            cards.Sort();

            return Sequence.sequenceFromCards(cards);
        }

        // Indices do not need to be sorted
        public void removeFromHand(List<int> indices)
        {
            List<int> descIndices = new List<int>(indices);

            descIndices.Sort();
            descIndices.Reverse();

            foreach (int index in descIndices)
                hand.RemoveAt(index);
        }
    }

    public class Bot : Player
    {

        public Bot(int id) : base(id) { }

        public List<int> betterSeries(Sequence lastSequence)
        {
            return betterSeries(new Stack<Card>(hand), lastSequence, new List<int> { }, hand.Count() - 1);
        }

        // Param: reverse hand as stack
        // Returns list of indices, none if has to pass
        private List<int> betterSeries(Stack<Card> hand, Sequence lastSequence, List<int> cardsFound, int index, int? lastNum = null)
        {
            if (cardsFound.Count() == lastSequence.size)
            {
                return cardsFound;
            }
            if (hand.Count() == 0)
            {
                return new List<int> { };
            }

            Card thisCard = hand.Pop();

            if (thisCard < lastSequence.lastCard)
            {
                return new List<int> { };
            }
            else
            {
                if (lastNum == null || thisCard.number != lastNum)
                {
                    lastNum = thisCard.number;
                    cardsFound.Clear();
                }
                cardsFound.Add(index);
            }
            return betterSeries(hand, lastSequence, cardsFound, index - 1, lastNum);
        }

    }
}
