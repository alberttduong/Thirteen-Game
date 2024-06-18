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
    abstract public class TPlayer
    {
        public int id { get; set; }
        public List<Card> hand { get; set; }

        public TPlayer(int id)
        {
            this.id = id;
            this.hand = new List<Card>();
        }

        public void sortHand()
        {
            hand.Sort();
        }

        public void printHeader()
        {
            Console.Write(header());
        }

        public abstract void printHand();

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

        public void printPlayedCards(List<int> indices)
        {
            printHeader();
            if (indices.Count == 0)
            {
                Console.WriteLine("passed");
                return;
            }
            Console.Write("played ");
            foreach (var i in indices)
            {
                Console.Write(hand[i] + " ");
            }
            Console.WriteLine();
        }

        public abstract string header();

        public abstract List<int> indicesPlayed(int turn, Sequence lastSequence, TPlayer lastSequencePlayer);

        public Sequence play(int turn, Sequence lastSequence, TPlayer lastSequencePlayer)
        {
            var indices = indicesPlayed(turn, lastSequence, lastSequencePlayer);
            if (indices == null)
            {
                printHeader();
                Console.WriteLine("passed");
                return new Sequence();
            }
            printPlayedCards(indices);
            var returnSequence = sequenceFromHand(indices);
            removeFromHand(indices);
            sortHand();
            return returnSequence;
        }
    }

    public class Player : TPlayer
    {
        public Player(int id) : base(id) { }

        public override string header()
        {
            return $"Player {id} ";
        }
        public override void printHand()
        {
            foreach (Card card in hand)
            {
                Console.Write(card + " ");
            }
            Console.WriteLine();
        }

        public List<int> playerInput()
        {
            while (true)
            {
                Console.WriteLine("Enter which cards you want to play");
                List<int> indices = new List<int> { };

                String input = Console.ReadLine();
                if (input == "p")
                {
                    // pass
                    return null;
                }

                String[] words = input.Trim().Split();
                try
                {
                    foreach (var word in words)
                        indices.Add(ushort.Parse(word));

                    indices = indices.Distinct().ToList();
                    indices.Sort();

                    return indices;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: Invalid indices");
                }
            }
        }

        public override List<int> indicesPlayed(int turn, Sequence lastSequence, TPlayer lastSequencePlayer)
        {

            if (lastSequencePlayer == this)
                lastSequence = new Sequence();

            while (true)
            {
                var indices = playerInput();

                //pass
                if (indices == null)
                {
                    if (turn == 1)
                    {
                        Console.WriteLine("Error: Can't pass on the first turn");
                        continue;
                    }
                    return null;
                }

                Sequence seq = sequenceFromHand(indices);

                if (turn == 1 && (indices.Count() == 0 || hand[indices[0]] != new Card(0, 0)))
                {
                    Console.WriteLine("Error: Starting play must include 3S");
                    continue;
                }

                if (seq.isValidSequence() && seq > lastSequence)
                {
                    return indices;
                }

                Console.WriteLine("Error: Invalid Sequence");
            }
        }
    }

    public class Bot : TPlayer
    {
        public Bot(int id) : base(id) { }

        public override string header()
        {
            return $"Bot {id} ";
        }

        public override void printHand()
        {
            Console.WriteLine($"# cards: {hand.Count()}");
        }

        public override List<int> indicesPlayed(int turn, Sequence lastSequence, TPlayer lastSequencePlayer)
        {
            List<int> indices;
            if (turn == 1)
                indices = biggestSequenceWithThreeSpades();
            else
                indices = betterSequence(lastSequence);

            if (indices.Count() == 0 && this == lastSequencePlayer)
                indices = biggestSequence(lastSequence);

            return indices;
        }

        public List<int> betterFlat(Sequence lastSequence)
        {
            return betterFlat(lastSequence, new List<int> { }, hand.Count() - 1);
        }

        // Returns list of indices, none if has to pass
        private List<int> betterFlat(Sequence lastSequence, List<int> cardsFound, int index, int? lastNum = null)
        {
            if (cardsFound.Count() == lastSequence.size)
            {
                var evenBetterFlat = betterFlat(lastSequence, new List<int> { }, lastSequence.size + index - 1);
                if (evenBetterFlat.Count() == 0)
                    return cardsFound;
                else
                    return evenBetterFlat;
            }

            if (index == -1)
                return new List<int> { };

            Card thisCard = hand[index];

            if (lastNum == null)
            {
                if (thisCard > lastSequence.lastCard)
                    lastNum = thisCard.number;
                else
                    return new List<int> { };
            } 
            else if (thisCard.number != lastNum) 
            {
                lastNum = thisCard.number;
                cardsFound.Clear();
            }
            cardsFound.Add(index);

            return betterFlat(lastSequence, cardsFound, index - 1, lastNum);
        }

        public List<int> betterSeries(Sequence lastSequence)
        {
            return betterSeries(lastSequence, new List<int> { }, hand.Count() - 1);
        }

        // Params: reverse hand as stack
        // Returns list of indices, none if has to pass

        private List<int> betterSeries(Sequence lastSequence, List<int> cardsFound, int index, int? lastNum = null)
        {
            if (cardsFound.Count() == lastSequence.size)
            {
                for (; index != -1; index--)
                {
                    if (hand[index].number != lastNum)
                        break;

                    cardsFound.RemoveAt(cardsFound.Count() - 1);
                    cardsFound.Add(index);
                }

                var evenBetterSeries = betterSeries(lastSequence, new List<int> { }, cardsFound[1]);
                if (evenBetterSeries.Count() != 0)
                    return evenBetterSeries;

                return cardsFound;
            }

            if (index == -1)
                return new List<int> { };

            Card thisCard = hand[index];

            if (lastNum == null)
            {
                if (thisCard > lastSequence.lastCard)
                {
                    lastNum = thisCard.number;
                    cardsFound.Add(index);
                }
                else
                    return new List<int> { };
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
                return cardsFound;

            var thisCard = hand[index];
            if (lastNum == null || thisCard.number == lastNum)
            {
                cardsFound.Add(index);
                return biggestFlat(thisCard.number, cardsFound, index + 1);
            }

            var evenBiggerFlat = biggestFlat(null, new List<int> { }, index);
            if (evenBiggerFlat.Count() > cardsFound.Count())
                return evenBiggerFlat;
            return cardsFound;
        }

        public List<int> biggestSeries()
        {
            return biggestSeries(null, new List<int> { });
        }

        private List<int> biggestSeries(int? lastNum, List<int> cardsFound, int index = 0)
        {
            if (index == hand.Count())
                return cardsFound;

            var thisCard = hand[index];

            if (lastNum == null || thisCard.number == lastNum + 1)
            {
                cardsFound.Add(index);
                return biggestSeries(thisCard.number, cardsFound, index + 1);
            }

            if (thisCard.number == lastNum)
                return biggestSeries(lastNum, cardsFound, index + 1);
            
            var evenBiggerSeries = biggestSeries(null, new List<int> { }, index);
            if (evenBiggerSeries.Count() > cardsFound.Count())
                return evenBiggerSeries;

            return cardsFound;
        }

        // Precondition: This bot has the 3 of spades
        public List<int> biggestFlatWithThreeSpades()
        {
            List<int> indices = new List<int>();

            for (int i = 0; i < 4 && hand[i].number == 0;  ++i)
                indices.Add(i);

            return indices;
        }

        // Precondition: This bot has the 3 of spades
        public List<int> biggestSeriesWithThreeSpades()
        {
            List<int> indices = new List<int>() { 0 };
            int lastNum = 0;
            for (int i = 1; i < hand.Count(); ++i)
            {
                if (hand[i].number == lastNum)
                    continue;
                else if (hand[i].number == lastNum + 1) {
                    lastNum++;
                    indices.Add(i);
                } else
                    break;
            }
            return indices;
        }

        public List<int> biggestSequenceWithThreeSpades()
        {
            List<int> series = biggestSeriesWithThreeSpades();
            List<int> flat = biggestFlatWithThreeSpades();
            if (series.Count() == 1 && flat.Count() == 1)
                return series;
            if (series.Count() == 2)
                return flat;
            return series.Count() > flat.Count() ? series : flat;
        }

        public List<int> betterSingle(Sequence lastSeq)
        {
            for (int i = 0; i < hand.Count(); ++i)
            {
                if (hand[i] > lastSeq.lastCard)
                    return new List<int> { i };
            }
            return new List<int> { };
        }

        public List<int> betterSequence(Sequence lastSeq)
        {
            switch (lastSeq.type)
            {
                case sequenceType.Single:
                    return betterSingle(lastSeq);
                case sequenceType.Flat:
                    return betterFlat(lastSeq);
                case sequenceType.Series:
                    return betterSeries(lastSeq);
                case sequenceType.Null:
                    return biggestSequence(lastSeq);
                default:
                    // Should throw an error
                    return new List<int> ();
            }
        }

        public List<int> biggestSequence(Sequence lastSeq)
        {
            var flat = biggestFlat();
            var series = biggestSeries();

            if (series.Count() == 2)
                return flat;

            return series.Count() >= flat.Count() ? series : flat;
        }
    }
}