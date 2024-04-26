using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices.WindowsRuntime;
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
            update_to_empty();
        }

        public Sequence(sequenceType type, int size, Card lastCard)
        {
            update(type, size, lastCard);
        }

        public void update(sequenceType type, int size, Card lastCard)
        {
            this.type = type;
            this.size = size;
            this.lastCard = lastCard;
        }

        public void update_to_empty()
        {
            update(sequenceType.Null, 0, new Card(0, 0));
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            if (GetType() != obj.GetType())
                return false;
            
            Sequence other = (Sequence)obj;

            return other.size == this.size
                && other.lastCard == this.lastCard
                && other.type == this.type;
        }

        public override string ToString()
        {
            if (isEmpty())
            {
                return "Empty sequence";
            }
            return $"Type: {this.type}, Size: {this.size}, Last Card: {this.lastCard.ToString()}";
        }

        public static bool operator ==(Sequence a, Sequence b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Sequence a, Sequence b)
        {
            return !a.Equals(b);
        }

        public bool isEmpty()
        {
            return this.size == 0;
        }

        // Sequences are not valid when they are empty.
        // Series must be 3 cards or more or else the
        // sequence is not valid.
        public bool isValidSequence()
        {
            if (isEmpty()) return false;
            if (this.type == sequenceType.Series && this.size < 3) return false;
            return true;
        }

        // Adds a card to this Sequence
        // or makes this Sequence empty
        // if the card can't be included
        // in the Sequence.
        //
        // PRECONDITION: This sequenceType
        // is Single.
        public void addCardToSingle(Card card)
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
            if (isEmpty())
            {
                update(sequenceType.Single, 1, card);
            } else
            {
                if (this.type == sequenceType.Single)
                {
                    addCardToSingle(card);
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
    }
    public enum sequenceType
    {
        Single,
        Flat,
        Series,
        Null
    }
}
