using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thirteen_Game
{
    public class GameState
    {
        const int NUM_PLAYERS = 4;

        Player[] players = new Player[NUM_PLAYERS];
        Player activePlayer;

        Player lastSequencePlayer;
        Sequence lastSequence = new Sequence();

        public GameState() {
            for (int i = 0; i < NUM_PLAYERS; i++)
            {
                players[i] = new Player(i);
            }

            activePlayer = players[0]; // Should be player with 3 of spades

            Deck deck = new Deck();
            deck.shuffle();

            foreach (Player p in players)
                deck.dealThirteenCards(p);
        }
        public void playerTurn(List<int> indices)
        {
            Sequence seq = activePlayer.sequenceFromHand(indices);

            if (lastSequencePlayer == activePlayer)
                lastSequence = new Sequence();

            if (seq.isValidSequence() && seq > lastSequence) {
                activePlayer.removeFromHand(indices);
                lastSequencePlayer = activePlayer;
                lastSequence = seq;
            }
        }
    }
}
