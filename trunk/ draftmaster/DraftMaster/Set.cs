using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTG_Drafter
{
    /// <summary>
    /// A set of cards
    /// </summary>
    class Set
    {
        private List<Card> m_cards;
        private string m_name;

        /// <summary>
        /// Creates a set of cards from relevant files
        /// </summary>
        /// <param name="name">Name of the set to load</param>
        public Set(string name)
        {
            try
            {
                m_cards = new List<Card>();
                m_name = name;

                if (System.IO.File.Exists(name + ".dmf-set"))
                {
                    var reader = new System.IO.StreamReader(name + ".dmf-set");
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!line.Contains(@"//"))
                            m_cards.Add(new Card(line, name));
                    }
                }
                else
                    throw new ArgumentException("Set " + name + " doesn't exist.");
            }
            catch (Exception e) { throw e; }
        }

        /// <summary>
        /// A list of all cards in the set
        /// </summary>
        public List<Card> Cards
        {
            get { return m_cards; }
        }

        /// <summary>
        /// Name of the set
        /// </summary>
        public string Name
        {
            get { return m_name; }
        }
    }
}