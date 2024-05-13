using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
            return betterSeries(lastSequence, new List<int> { }, hand.Count() - 1);
        }

        public List<int> betterFlat(Sequence lastSequence)
        {
            return betterFlat(lastSequence, new List<int> { }, hand.Count() - 1);
        }

        // Params: reverse hand as stack
        // Returns list of indices, none if has to pass
        private List<int> betterFlat(Sequence lastSequence, List<int> cardsFound, int index, int? lastNum = null)
        {
            if (cardsFound.Count() == lastSequence.size)
            {
                var evenBetterFlat = betterFlat(lastSequence, new List<int> { }, lastSequence.size + index - 1);
                if (evenBetterFlat.Count() == 0)
                {
                    return cardsFound;
                } else
                {
                    return evenBetterFlat;
                }
            }
            if (index == -1)
            {
                return new List<int> { };
            }

            Card thisCard = hand[index];

            if (lastNum == null) {
                if (thisCard < lastSequence.lastCard) {
                    return new List<int> { };
                } else
                {
                    lastNum = thisCard.number;
                }
            } else if (thisCard.number != lastNum) {
                lastNum = thisCard.number;
                cardsFound.Clear();
            }
            cardsFound.Add(index);

            return betterFlat(lastSequence, cardsFound, index - 1, lastNum);
        }

        // Params: reverse hand as stack
        // Returns list of indices, none if has to pass

        private List<int> betterSeries(Sequence lastSequence, List<int> cardsFound, int index, int? lastNum = null)
        {
            if (cardsFound.Count() == lastSequence.size)
            {
                for (index--; index != -1;  index--)
                {
                    if (hand[index].number != lastNum)
                    {
                        break;
                    }
                    cardsFound.RemoveAt(cardsFound.Count() - 1);
                    cardsFound.Add(index);
                }
                var evenBetterSeries = betterSeries(lastSequence, new List<int> { }, cardsFound[1]);
                if (evenBetterSeries.Count() != 0)
                {
                    return evenBetterSeries;
                }
                return cardsFound;
            }

            if (index == -1)
            {
                return new List<int> { };
            }

            Card thisCard = hand[index];

            if (lastNum == null)
            {
                if (thisCard < lastSequence.lastCard)
                {
                    return new List<int> { };
                }
                else
                {
                    lastNum = thisCard.number;
                    cardsFound.Add(index);
                }
            }
            else if (thisCard.number == lastNum)
            {
                cardsFound.RemoveAt(cardsFound.Count() - 1);
                cardsFound.Add(index);
            } else if (thisCard.number == lastNum - 1)
            {
                lastNum--;
                cardsFound.Add(index);
            } else
            {
                lastNum = thisCard.number;
                cardsFound.Clear();
                cardsFound.Add(index);
            }

            return betterSeries(lastSequence, cardsFound, index - 1, lastNum);
        }

        public List<int> biggestFlat()
        {
            return biggestFlat(null, new List<int> { }, 0);
        }

        // Sorted hand
        private List<int> biggestFlat(int? lastNum, List<int> cardsFound, int index = 0)
        {
            if (index == hand.Count())
            {
                return cardsFound;
            }
            var thisCard = hand[index];
            if (lastNum == null || thisCard.number == lastNum)
            {
                cardsFound.Add(index);
                return biggestFlat(thisCard.number, cardsFound, index + 1);
            } else
            {
                var evenBiggerFlat = biggestFlat(null, new List<int> { }, index);
                if (evenBiggerFlat.Count() > cardsFound.Count())
                { 
                    return evenBiggerFlat;
                }
                return cardsFound;
            }
        }
    }
}
