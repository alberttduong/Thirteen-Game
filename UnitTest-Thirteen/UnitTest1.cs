using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Thirteen_Game;

namespace UnitTest_Thirteen
{
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
}
