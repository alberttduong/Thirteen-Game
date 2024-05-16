using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Thirteen_Game
{
    public class GameState
    {
        const int NUM_PLAYERS = 4;
        const int NUM_BOTS = 3;

        Player[] players = new Player[NUM_PLAYERS];
        public Player activePlayer;

        Player lastSequencePlayer;
        Sequence lastSequence = new Sequence();

        public bool won = false;
        int turn = 1;
        int activePlayerNum;

        public GameState() {
            players[0] = new Player(0);
            for (int i = 1; i < NUM_BOTS + 1; i++)
                players[i] = new Bot(i);
            
            Deck deck = new Deck();
            deck.shuffle();

            for (int i = 0; i < NUM_PLAYERS; i++)
            {
                var p = players[i];
                deck.dealThirteenCards(p);
                p.sortHand();
                if (p.hand[0] == new Card(0, 0))
                {
                    activePlayer = p;
                    activePlayerNum = i;
                }
            }
            activePlayer.printHeader();
            Console.WriteLine(" has 3S and will go first.");
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

        public void printTurn()
        {
            Console.WriteLine("Turn " + turn);
        }

        public void botPlay()
        {
            Bot player = (Bot)activePlayer;
            List<int> indices;
            if (turn == 1)
                indices = player.biggestSequenceWithThreeSpades();
            else
                indices = player.betterSequence(lastSequence);
           
            if (indices.Count() == 0 && player == lastSequencePlayer)
                indices = player.biggestSequence(lastSequence);

            player.printPlayedCards(indices);
            playIndices(indices);
        }
        
        public void playerPlay()
        {
            activePlayer.printHeader();
            Console.WriteLine("passed");
        }

        public void printLastSequence()
        {
            Console.WriteLine(lastSequence);
        }

        public void endTurn()
        {
            activePlayer = players[++activePlayerNum % NUM_PLAYERS];
            turn++;
        }

        public void playIndices(List<int> indices)
        {
            if (indices.Count > 0)
            {
                lastSequence = activePlayer.sequenceFromHand(indices);
                lastSequencePlayer = activePlayer;
                activePlayer.removeFromHand(indices);
                activePlayer.sortHand();
            }
        }

        public void printPlayerHands()
        {
            foreach (var p in players)
            {
                p.printHeader();
                p.printHand();
            }
        }
    }
}
