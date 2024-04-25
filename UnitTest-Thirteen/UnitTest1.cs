using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
                Assert.AreEqual(deck.cards[i].suit, suit);
                Assert.AreEqual(deck.cards[i].number, num);
            }
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
    }
}
