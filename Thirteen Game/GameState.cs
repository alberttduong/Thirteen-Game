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

        TPlayer[] players = new TPlayer[NUM_PLAYERS];
        public TPlayer activePlayer;

        TPlayer lastSequencePlayer;
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
            Console.WriteLine($"{activePlayer.header()}has 3S and will go first.");
            Console.WriteLine();
        }

        public void play()
        {
            Sequence seq = activePlayer.play(turn, lastSequence, lastSequencePlayer);
            if (seq.type != sequenceType.Null)
            {
                lastSequence = seq;
                lastSequencePlayer = activePlayer;
            }
        }

        public void endTurn()
        {
            if (activePlayer.hand.Count() == 0)
                won = true;

            activePlayer = players[++activePlayerNum % NUM_PLAYERS];
            turn++;
        }

        public void printTurn()
        {
            Console.WriteLine("Turn " + turn);
        }

        public void printLastSequence()
        {
            Console.WriteLine(lastSequence);
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
