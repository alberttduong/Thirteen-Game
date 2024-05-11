using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Thirteen_Game;

namespace UnitTest_Thirteen
{
    [TestClass]
    public class UnitTest_Card
    {
        Card card = new Card(0, 0);

        [TestMethod]
        public void Test_CardToString()
        {
            Assert.AreEqual(card.ToString(), "3 of Spades");
        }

        [TestMethod]
        public void Test_CardEquality()
        {
            Card equalCard = new Card(0, 0);
            Card unequalCard = new Card(0, 1);
            Assert.AreEqual(card, card);
            Assert.AreEqual(card, equalCard);
            Assert.AreNotEqual(card, unequalCard);
            Assert.IsTrue(card == equalCard);
        }

        [TestMethod]
        public void Test_NumberNameIsCorrectInteger()
        {
            Assert.AreEqual(Card.numberName(7), "10");
            Assert.AreEqual(Card.numberName(8), "Jack");
        }

        [TestMethod]
        public void Test_CardLessThan()
        {
            Card card = new Card(3, 2);
            Assert.IsTrue(card < new Card(4, 2));
            Assert.IsTrue(card < new Card(3, 3));

            Assert.IsFalse(card < new Card(2, 1));
            Assert.IsFalse(card < new Card(3, 1));
        }

        [TestMethod]
        public void Test_CardGreaterThan()
        {
            Card card = new Card(3, 2);
            Assert.IsTrue(card > new Card(1, 2));
            Assert.IsTrue(card > new Card(3, 0));

            Assert.IsFalse(card > new Card(10, 1));
            Assert.IsFalse(card > new Card(3, 3));
        }
    }

    [TestClass]
    public class UnitTest_Deck
    {
        Deck deck = new Deck();
        Deck.StandardDeck stdDeck = new Deck.StandardDeck();

        [TestMethod]
        public void Test_SuitAndNumToOrderedIndex()
        {
            Assert.AreEqual(Deck.suitAndNumToOrderedIndex(0, 0), 0);
            Assert.AreEqual(Deck.suitAndNumToOrderedIndex(0, 12), 12);
            Assert.AreEqual(Deck.suitAndNumToOrderedIndex(1, 0), 13);
            Assert.AreEqual(Deck.suitAndNumToOrderedIndex(3, 12), 51);
        }

        [TestMethod]
        public void Test_DeckInitialization()
        {
            IEnumerator<(int, int)> enumerator = stdDeck.GetEnumerator();
            for (int i = 0; i < 52; i++)
            {
                enumerator.MoveNext();
                (int suit, int num) = enumerator.Current;
                Assert.AreEqual(deck.temp_cards[i].suit, suit);
                Assert.AreEqual(deck.temp_cards[i].number, num);
            }
        }

        [TestMethod]
        public void Test_Deal13Cards()
        {
            Deck deck = new Deck();
            deck.shuffle();
            Player player = new Player(1);

            Assert.AreEqual(deck.count(), 52);
            Assert.AreEqual(player.hand.Count, 0);

            deck.dealThirteenCards(player);

            Assert.AreEqual(deck.count(), 52 - 13);
            Assert.AreEqual(player.hand.Count, 13);
        }
    }

    [TestClass]
    public class UnitTest_Sequence
    {
        [TestMethod]
        public void Test_SequenceIsEmptyUntilAddingACard()
        {
            Sequence seq = new Sequence();
            Assert.IsTrue(seq.isEmpty());
            seq.addCard(new Card(0, 0));
        }

        [TestMethod]
        public void Test_SequenceEquality()
        {
            Sequence seq1 = new Sequence(sequenceType.Single, 1, new Card(0, 0));
            Sequence seq2 = new Sequence(sequenceType.Single, 1, new Card(0, 0));
            Sequence seq3 = new Sequence(sequenceType.Single, 1, new Card(1, 0));
            Assert.AreEqual(seq1, seq1);
            Assert.AreEqual(seq1, seq2);
            Assert.AreNotEqual(seq1, seq3);
        }

        [TestMethod]
        public void Test_AddingACardToAnEmptySequence()
        {
            Sequence seq = new Sequence();
            Card lastCard = new Card(1, 2);
            seq.addCard(lastCard);
            Assert.AreEqual(seq, new Sequence(sequenceType.Single, 1, lastCard));
            Assert.AreNotEqual(seq, new Sequence());
        }

        [TestMethod]
        public void Test_AddingALesserCardToASingleSequenceFails()
        {
            Sequence seq = new Sequence(sequenceType.Single, 1, new Card(1, 1));
            seq.addCard(new Card(0, 0));
            Assert.AreEqual(seq, new Sequence());
            Assert.AreNotEqual(seq, new Sequence(sequenceType.Series, 2, new Card(0, 0)));
        }

        [TestMethod]
        public void Test_AddingAnIncompatibleGreaterCardToASingleFails()
        {
            Sequence seq = new Sequence(sequenceType.Single, 1, new Card(1, 1));
            Card incompatibleCard = new Card(3, 1);
            seq.addCard(incompatibleCard);
            Assert.AreEqual(seq, new Sequence());
        }

        [TestMethod]
        public void Test_AddingASingleGivesAFlatSequence()
        {
            Sequence seq = new Sequence(sequenceType.Single, 1, new Card(1, 1));
            Card cardWithSameNum = new Card(1, 0);
            seq.addCard(cardWithSameNum);
            Assert.AreEqual(seq, new Sequence(sequenceType.Flat, 2, cardWithSameNum));
        }

        [TestMethod]
        public void Test_AddingToASingleGivesASeriesSequence()
        {
            Sequence seq = new Sequence(sequenceType.Single, 1, new Card(1, 1));
            Card cardIncremented = new Card(2, 3);
            seq.addCard(cardIncremented);
            Assert.AreEqual(seq, new Sequence(sequenceType.Series, 2, cardIncremented));
        }

        [TestMethod]
        public void Test_AddingToAFlatSequence()
        {
            const int TEST_CASES = 3;

            Card[] cardsAdded =
            {
                new Card(1, 0),
                new Card(2, 3),
                new Card(3, 1),
            };

            Sequence[] expectedSequences = {
                new Sequence(),
                new Sequence(sequenceType.Flat, 3, cardsAdded[1]),
                new Sequence(),
            };

            Sequence seq = new Sequence();
            for (int i = 0; i < TEST_CASES; i++)
            {
                seq.update(sequenceType.Flat, 2, new Card(2, 1));
                seq.addCard(cardsAdded[i]);
                Assert.AreEqual(seq, expectedSequences[i]);
            }
        }

        [TestMethod]
        public void Test_AddingToASeriesSequence()
        {
            const int TEST_CASES = 3;

            Card[] cardsAdded =
            {
                new Card(3, 0),
                new Card(4, 2),
                new Card(5, 1),
            };

            Sequence[] expectedSequences = {
                new Sequence(),
                new Sequence(sequenceType.Series, 5, cardsAdded[1]),
                new Sequence(),
            };

            Sequence seq = new Sequence();
            for (int i = 0; i < TEST_CASES; i++)
            {
                seq.update(sequenceType.Series, 4, new Card(3, 3));
                seq.addCard(cardsAdded[i]);
                Assert.AreEqual(seq, expectedSequences[i]);
            }
        }

        [TestMethod]
        public void Test_ValidatingSequences()
        {
            Sequence empty = new Sequence();
            Sequence seriesOfTwo = new Sequence(sequenceType.Series, 2, new Card(0, 0));
            Sequence validSingleSeq = new Sequence(sequenceType.Single, 1, new Card(0, 0));
            Sequence validSeq = new Sequence(sequenceType.Flat, 4, new Card(0, 0)); // Valid even though impossible

            Assert.IsFalse(empty.isValidSequence());
            Assert.IsFalse(seriesOfTwo.isValidSequence());
            Assert.IsTrue(validSingleSeq.isValidSequence());
            Assert.IsTrue(validSeq.isValidSequence());
        }

        [TestMethod]
        public void Test_SequenceGreaterThan()
        {
            Sequence empty = new Sequence();
            Sequence single = new Sequence(sequenceType.Single, 1, new Card(2, 2));
            Sequence higherSingle = new Sequence(sequenceType.Single, 1, new Card(2, 3));
            Sequence lowerSingle = new Sequence(sequenceType.Single, 1, new Card(0, 0));

            Assert.IsTrue(single > empty);
            Assert.IsTrue(higherSingle > single);
            Assert.IsFalse(lowerSingle > single);

            Sequence flat = new Sequence(sequenceType.Flat, 2, new Card(1, 1));
            Sequence diffSizeFlat = new Sequence(sequenceType.Flat, 3, new Card(2, 2));
            Sequence higherFlat = new Sequence(sequenceType.Flat, 2, new Card(1, 2));
            Sequence anotherHigherFlat = new Sequence(sequenceType.Flat, 2, new Card(2, 1));

            Assert.IsTrue(flat > empty);
            Assert.IsTrue(higherFlat > flat);
            Assert.IsTrue(anotherHigherFlat > flat);
            Assert.IsFalse(diffSizeFlat > flat);
            Assert.IsFalse(flat > single);

            Sequence seriesOf3 = new Sequence(sequenceType.Series, 3, new Card(5, 1));
            Sequence seriesOf4 = new Sequence(sequenceType.Series, 4, new Card(10, 0));
            Sequence lowerSeriesOf3 = new Sequence(sequenceType.Series, 3, new Card(5, 0));
            Sequence higherSeriesOf3 = new Sequence(sequenceType.Series, 3, new Card(6, 1));
            Sequence anotherHigherSeriesOf3 = new Sequence(sequenceType.Series, 3, new Card(5, 2));

            Assert.IsTrue(seriesOf3 > empty);
            Assert.IsTrue(higherSeriesOf3 > seriesOf3);
            Assert.IsTrue(anotherHigherSeriesOf3 > seriesOf3);
            Assert.IsFalse(lowerSeriesOf3 > seriesOf3);
            Assert.IsFalse(seriesOf4 > seriesOf3);
            Assert.IsFalse(flat > seriesOf3);
        }

        [TestMethod]
        public void Test_SequenceFromCards()
        {
            Card[] cards = { new Card(0, 0), new Card(1, 2), new Card(2, 1) };
            Assert.AreEqual(Sequence.sequenceFromCards(cards),
                new Sequence(sequenceType.Series, 3, new Card(2, 1)));

            cards = new Card[] { new Card(0, 0), new Card(0, 2) };
            Assert.AreEqual(Sequence.sequenceFromCards(cards),
                new Sequence(sequenceType.Flat, 2, new Card(0, 2)));

            cards = new Card[] { new Card(0, 0) };
            Assert.AreEqual(Sequence.sequenceFromCards(cards),
                new Sequence(sequenceType.Single, 1, new Card(0, 0)));

            // Invalid Sequences
            cards = new Card[] { new Card(0, 0), new Card(2, 1) };
            Assert.AreEqual(Sequence.sequenceFromCards(cards),
                new Sequence());

            cards = new Card[] { new Card(1, 1), new Card(1, 2), new Card(2, 3) };
            Assert.AreEqual(Sequence.sequenceFromCards(cards),
                new Sequence());

            cards = new Card[] { new Card(1, 1), new Card(2, 2), new Card(4, 3) };
            Assert.AreEqual(Sequence.sequenceFromCards(cards),
                new Sequence());
        }
    }

    [TestClass]
    public class UnitTest_Player
    {
        Player p = new Player(0);

        [TestMethod]
        public void Test_SortHand()
        {
            p.hand = new List<Card> { new Card(0, 1), new Card(2, 1), new Card(1, 0), new Card(2, 2) };
            p.sortHand();
            Assert.AreEqual(p.hand[0], new Card(0, 1));
            Assert.AreEqual(p.hand[1], new Card(1, 0));
            Assert.AreEqual(p.hand[2], new Card(2, 1));
            Assert.AreEqual(p.hand[3], new Card(2, 2));
        }

        [TestMethod]
        public void Test_RemoveFromHand()
        {
            p.hand = new List<Card> { new Card(0, 1), new Card(2, 1), new Card(1, 0), new Card(2, 2) };
            p.removeFromHand(new List<int>{1, 2});

            Assert.AreEqual(p.hand.Count, 2);
            Assert.AreEqual(p.hand[0], new Card(0, 1));
            Assert.AreEqual(p.hand[1], new Card(2, 2));
        }

        [TestMethod]
        public void Test_BetterSeries()
        {
            var bot = new Bot(1);
            bot.hand = new List<Card> { 
                new Card(0, 1), // 0
                new Card(2, 0), 
                new Card(2, 1), 
                new Card(2, 2),
                new Card(3, 0),
                new Card(4, 2),
                new Card(5, 0),
                new Card(5, 2),
                new Card(6, 0),
                new Card(6, 2), // 9
            };

            var actual1 = bot.betterSeries(new Sequence(sequenceType.Series, 2, new Card(5, 1)));
            var expected1 = new List<int> { 9, 8 };

            var actual2 = bot.betterSeries(new Sequence(sequenceType.Series, 3, new Card(1, 1)));
            var expected2 = new List<int> { 3, 2, 1 };

            var actual3 = bot.betterSeries(new Sequence(sequenceType.Series, 4, new Card(0, 0)));
            var actual4 = bot.betterSeries(new Sequence(sequenceType.Series, 2, new Card(6, 3)));
            var expectedNone = new List<int> { };

            Assert.IsTrue(actual1.SequenceEqual(expected1));
            Assert.IsTrue(actual2.SequenceEqual(expected2));
            Assert.IsTrue(actual3.SequenceEqual(expectedNone));
            Assert.IsTrue(actual4.SequenceEqual(expectedNone));
        }
    }
}
