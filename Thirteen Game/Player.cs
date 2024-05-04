using System;
using System.Collections.Generic;
using System.Linq;
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
}
