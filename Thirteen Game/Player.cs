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
    }
}
