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
        public void Test_AddingACardToAnEmptySequence()
        {
            Sequence seq = new Sequence();
            Card lastCard = new Card(1, 2);
            seq.addCard(lastCard);
            Assert.AreEqual(seq, new Sequence(sequenceType.Single, 1, lastCard));
        }
    }
}
