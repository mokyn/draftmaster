using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTG_Drafter
{
    /// <summary>
    /// Created for NPC logic
    /// Contains useful functions for the same
    /// </summary>
    class Picks
    {
        private List<Card> m_picks;
        private int m_creatures;

        /// <summary>
        /// Created for NPC logic
        /// Contains useful functions for the same
        /// </summary>
        public Picks()
        {
            m_picks = new List<Card>();
            m_creatures = 0;
        }

        /// <summary>
        /// Converts a list of cards to a Picks
        /// </summary>
        /// <param name="toConvert">List to convert</param>
        /// <returns>Picks created using the list</returns>
        public static Picks List2Picks(List<Card> toConvert)
        {
            if (toConvert == null)
                throw new ArgumentNullException("List<Card> toConvert was null");

            Picks toRet = new Picks();
            foreach (Card card in toConvert)
                toRet.Add(card);

            return toRet;
        }

        /// <summary>
        /// Adds a card to Picks
        /// </summary>
        /// <param name="card">Card to add</param>
        public void Add(Card card)
        {
            if (card == null)
                throw new ArgumentNullException("Card card was null");
            m_picks.Add(card);
            if (card.SuperType == SuperType.Creature || card.SuperType == SuperType.ArtifactCreature)
                m_creatures++;
        }

        /// <summary>
        /// Gets all cards with converted mana cost
        /// less than the given value
        /// </summary>
        /// <param name="cmc">Maximum cmc; must be greater than 1</param>
        /// <returns>Returns all cards in the Picks that fit criteria</returns>
        public List<Card> CMCLessThan(int cmc)
        {
            if (cmc < 1)
                throw new ArgumentOutOfRangeException("Int cmc was less than 1");

            var toRet = new List<Card>();
            foreach (Card card in m_picks)
            {
                if (card.CMC < cmc)
                    toRet.Add(card);
            }
            return toRet;
        }

        /// <summary>
        /// Gets all cards with converted mana cost
        /// greater than the given value
        /// </summary>
        /// <param name="cmc">Minimum cmc; must be at least 0</param>
        /// <returns>Returns all cards in the Picks that fit criteria</returns>
        public List<Card> CMCGreaterThan(int cmc)
        {
            if (cmc < 0)
                throw new ArgumentOutOfRangeException("Int cmc was less than 0");

            var toRet = new List<Card>();
            foreach (Card card in m_picks)
            {
                if (card.CMC > cmc)
                    toRet.Add(card);
            }
            return toRet;
        }

        /// <summary>
        /// Gets all cards with the given converted mana cost
        /// </summary>
        /// <param name="cmc">CMC; must be greater than 1</param>
        /// <returns>Returns all cards in the Picks that fit criteria</returns>
        public List<Card> CMCEqualTo(int cmc)
        {
            if (cmc < 0)
                throw new ArgumentOutOfRangeException("Int cmc was less than 0");

            var toRet = new List<Card>();
            foreach (Card card in m_picks)
            {
                if (card.CMC == cmc)
                    toRet.Add(card);
            }
            return toRet;
        }

        /// <summary>
        /// Cards in the Picks
        /// </summary>
        public List<Card> Cards
        {
            get { return m_picks; }
        }

        /// <summary>
        /// Number of creature cards picked
        /// </summary>
        public int NoOfCreature
        {
            get { return m_creatures; }
        }

        /// <summary>
        /// Proportion of creatures to all cards in
        /// the ratio Creatures/Total
        /// </summary>
        public float Proportion
        {
            get { return (m_creatures / m_picks.Count); }
        }

        /// <summary>
        /// Gets the average converted mana cost of cards in the list,
        /// including lands
        /// </summary>
        public float AvgCMC
        {
            get
            {
                float sum = 0;
                foreach (Card card in m_picks)
                    sum += card.CMC;
                return (sum / m_picks.Count);
            }
        }

        /// <summary>
        /// Gets all creatures in the Picks
        /// </summary>
        public List<Card> Creatures
        {
            get
            {
                List<Card> toRet = new List<Card>();
                foreach (Card card in m_picks)
                {
                    if (card.SuperType == SuperType.ArtifactCreature || card.SuperType == SuperType.Creature)
                        toRet.Add(card);
                }
                return toRet;
            }
        }

        /// <summary>
        /// Gets all instants and sorceries in the Picks
        /// </summary>
        public List<Card> Spells
        {
            get
            {
                List<Card> toRet = new List<Card>();
                foreach (Card card in m_picks)
                {
                    if (card.SuperType == SuperType.Instant || card.SuperType == SuperType.Sorcery)
                        toRet.Add(card);
                }
                return toRet;
            }
        }

        /// <summary>
        /// Gets all enchantments, noncreature artifacts, and planeswalkers
        /// in the Picks
        /// </summary>
        public List<Card> NoncreaturePermanent
        {
            get
            {
                List<Card> toRet = new List<Card>();
                foreach (Card card in m_picks)
                {
                    if (card.SuperType == SuperType.Artifact || card.SuperType == SuperType.Enchantment || card.SuperType == SuperType.Planeswalker)
                        toRet.Add(card);
                }
                return toRet;
            }
        }

        /// <summary>
        /// Gets all noncreature cards in the Picks
        /// </summary>
        public List<Card> Noncreature
        {
            get
            {
                List<Card> toRet = new List<Card>();
                foreach (Card card in m_picks)
                {
                    if (card.SuperType != SuperType.ArtifactCreature && card.SuperType != SuperType.Creature)
                        toRet.Add(card);
                }
                return toRet;
            }
        }
    }
}
