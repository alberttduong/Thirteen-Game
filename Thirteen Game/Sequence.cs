﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Thirteen_Game
{
    public class Sequence
    {
        public sequenceType type { get; set; }
        public int size { get; set;  }
        public Card lastCard { get; set;  }

        public Sequence()
        {
            this.size = 0;
        }

        public Sequence(sequenceType type, int size, Card lastCard)
        {
            update(type, size, lastCard);
        }

        public void update_to_empty()
        {
            this.size = 0;
        }

        public void update(sequenceType type, int size, Card lastCard)
        {
            this.type = type;
            this.size = size;
            this.lastCard = lastCard;
        }

        public bool isNotEmpty()
        {
            return this.size > 0;
        }

        public bool isEmpty()
        {
            return this.size == 0;
        }

        // Adding a card to this Sequence, when valid,
        // increments the size of the sequence, updates
        // the lastCard to card just added, and changes
        // the sequenceType if the sequence was just
        // one card.
        //
        // Returns an empty Sequence the card can't be
        // added
        public void addCard(Card card)
        {
            if (isNotEmpty())
            {
                if (this.type == sequenceType.Single)
                {
                    if (card.number == this.lastCard.number)
                    {
                        update(sequenceType.Flat, 2, card);
                    }
                    else if (card.number == this.lastCard.number + 1)
                    {
                        update(sequenceType.Series, 2, card);

                    }
                    else
                    {
                        update_to_empty();
                    }
                }
                else if (this.type == sequenceType.Flat && card.number == this.lastCard.number)
                {
                    update(sequenceType.Flat, this.size + 1, card);

                }
                else if (this.type == sequenceType.Series && card.number == this.lastCard.number + 1)
                {
                    update(sequenceType.Series, this.size + 1, card);
                }
                else
                {
                    update_to_empty();
                }
            } else
            {
                update(sequenceType.Single, 1, card);
            }
        }

        // From an array of cards, generate a Sequence
        // and return it.  If the sequence is invalid,
        // return an empty Sequence.
        public static Sequence sequenceFromCards(Card[] cards)
        {
            Sequence newSeq = new Sequence();
            foreach (var card in cards)
            {
                newSeq.addCard(card);
                if (newSeq.isEmpty())
                {
                    return newSeq;
                }
            }
            return newSeq;
        }

        public enum sequenceType
        {
            Single,
            Flat,
            Series
        }
    }
}
