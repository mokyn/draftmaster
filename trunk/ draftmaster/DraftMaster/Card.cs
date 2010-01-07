using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MTG_Drafter
{
    sealed class Card
    {
        private string m_edition, m_name, m_tip, m_cc;
        private bool m_isLegend, m_isTribal;
        private int m_playerRating, m_defRating, m_cmc, m_power, m_toughness, m_drafted;
        private SuperType m_superType;
        private CardColor m_color;
        private Image m_pic;
        private Rarity m_rarity;
        private List<CardTag> m_tags;
        private List<CardColor> m_colors, m_fix;

        public Card(string name, string edition)
        {
            try
            {
                m_tags = new List<CardTag>();
                m_colors = new List<CardColor>();
                m_fix = new List<CardColor>();
                m_name = name;
                m_edition = edition;

                m_pic = Image.FromFile(edition + @"\Img\" + name + ".dmf-img");

                StreamReader reader = new StreamReader(edition + @"\Card\" + name + ".dmf-card");

                var lineX = reader.ReadLine().Split('|');
                m_cc = lineX[0];
                var hybridSections = m_cc.Split('/');
                int hybridSym = hybridSections.Length;
                m_cmc = hybridSections.Length - 1;
                string newCC = m_cc;
                if (m_cc.Contains('/'))
                {
                    for (int i = 0; i < hybridSections.Length; i++)
                    {
                        if (i == 0)
                            newCC = hybridSections[i].Remove(hybridSections[i].Length - 1);
                        else if (i == hybridSections.Length - 1)
                            newCC += hybridSections[i].Remove(0, 1);
                        else
                        {
                            string toAttach = hybridSections[i].Remove(hybridSections[i].Length - 1);
                            newCC += toAttach.Remove(0, 1);
                        }
                    }
                }
                m_cmc += InstancesOf('W', newCC);
                m_cmc += InstancesOf('U', newCC);
                m_cmc += InstancesOf('B', newCC);
                m_cmc += InstancesOf('R', newCC);
                m_cmc += InstancesOf('G', newCC);
                m_cmc += gInclude.Parse.String2Int(newCC.Replace('W', ' ').Replace('U', ' ').Replace('B', ' ').Replace('R', ' ').Replace('G', ' '));


                if (lineX.Length == 3)
                {
                    m_power = gInclude.Parse.String2Int(lineX[1]);
                    m_toughness = gInclude.Parse.String2Int(lineX[2]);
                }
                else if (lineX.Length == 2)
                    m_toughness = gInclude.Parse.String2Int(lineX[1]);

                var line = reader.ReadLine().Split('|');
                m_superType = (SuperType)Enum.Parse(typeof(SuperType), line[0]);
                m_isTribal = String2Bool(line[1]);
                m_isLegend = String2Bool(line[2]);

                line = reader.ReadLine().Split('|');
                m_color = (CardColor)Enum.Parse(typeof(CardColor), line[0]);
                m_rarity = (Rarity)Enum.Parse(typeof(Rarity), reader.ReadLine());
                var tmp = reader.ReadLine();
                if (tmp == null)
                    m_tags.Add(CardTag.Misc);
                else
                {
                    var tempX = tmp.Split('|');
                    foreach (string tag in tempX)
                        m_tags.Add((CardTag)Enum.Parse(typeof(CardTag), tag));
                }

                if (line.Length > 1 && (m_color == CardColor.Multicolor || m_color == CardColor.Hybrid))
                {
                    for (int i = 1; i < line.Length; i++)
                        m_colors.Add((CardColor)Enum.Parse(typeof(CardColor), line[i]));
                }

                var temp = reader.ReadLine();
                if (temp != null)
                {
                    line = temp.Split('|');
                    foreach (string clr in line)
                        m_fix.Add((CardColor)Enum.Parse(typeof(CardColor), clr));
                }

                reader.Close();

                reader = new StreamReader(edition + @"\Tip\" + name + ".dmf-tip");
                m_tip = reader.ReadLine();
                m_defRating = gInclude.Parse.String2Int(reader.ReadLine());
                line = reader.ReadLine().Split('|');
                m_playerRating = gInclude.Parse.String2Int(line[0]);
                if (line.Length > 1)
                    m_drafted = gInclude.Parse.String2Int(line[1]);
                else
                    m_drafted = 0;
                reader.Close();

                if (m_playerRating < 0)
                    m_playerRating = 0;
            }
            catch (Exception e) { throw new ArgumentException("ERROR with card " + name + " - " + e.Data + "; " + e.Message); }
        }
        private int InstancesOf(char searchFor, string inside)
        {
            if (string.IsNullOrEmpty(inside))
                throw new ArgumentNullException();
            return inside.Split(searchFor).Length - 1;
        }

        public void AddRating(int amount)
        {
            if (amount < 1)
                throw new ArgumentOutOfRangeException("Int amount was less than 1");

            m_playerRating += amount;
            m_drafted++;

            var writer = new StreamWriter(m_edition + @"\Tip\" + m_name + ".dmf-tip");
            writer.WriteLine(m_tip);
            writer.WriteLine(m_defRating);
            writer.WriteLine(m_playerRating + "|" + m_drafted);
            writer.Close();
        }

        public int ColoredCC(CardColor clr)
        {
            if (clr == CardColor.Black)
                return InstancesOf('B', m_cc);
            else if (clr == CardColor.Blue)
                return InstancesOf('U', m_cc);
            else if (clr == CardColor.Green)
                return InstancesOf('G', m_cc);
            else if (clr == CardColor.Red)
                return InstancesOf('R', m_cc);
            else if (clr == CardColor.White)
                return InstancesOf('W', m_cc);
            throw new Exception("Only colored (WUBRG) colors are accepted.");
        }


        public Image Picture
        {
            get { return m_pic; }
        }

        public CardColor MTGColor
        {
            get { return m_color; }
        }

        public Color Color
        {
            get
            {
                if (m_color == CardColor.Black)
                    return Color.Black;
                if (m_color == CardColor.Blue)
                    return Color.Blue;
                if (m_color == CardColor.Green)
                    return Color.Green;
                if (m_color == CardColor.Red)
                    return Color.Red;
                if (m_color == CardColor.White)
                    return Color.Yellow;
                if (m_color == CardColor.Colorless)
                    return Color.Gray;
                if (m_color == CardColor.Hybrid)
                    return Color.Purple;
                if (m_color == CardColor.Multicolor)
                    return Color.Goldenrod;
                return Color.Brown;
            }
        }

        public Color RarityColor
        {
            get
            {
                if (m_rarity == Rarity.Common)
                    return Color.Black;
                if (m_rarity == Rarity.Uncommon)
                    return Color.Silver;
                if (m_rarity == Rarity.Rare)
                    return Color.Gold;
                if (m_rarity == Rarity.MythicRare)
                    return Color.OrangeRed;
                return Color.Brown;
            }
        }

        public SuperType SuperType
        {
            get { return m_superType; }
        }

        public Rarity Rarity
        {
            get { return m_rarity; }
        }

        public List<CardColor> Colors
        {
            get { return m_colors; }
        }

        public List<CardColor> ManaFix
        {
            get { return m_fix; }
        }

        public List<CardTag> Tags
        {
            get { return m_tags; }
        }

        public string Tip
        {
            get { return m_tip; }
        }

        public string Name
        {
            get { return m_name; }
        }

        public int DefaultRating
        {
            get { return m_defRating; }
        }

        public double PlayerRating
        {
            get { return (double)m_playerRating / m_drafted; }
        }

        public double Rating
        {
            get
            {
                if (m_drafted < 5)
                    return DefaultRating;
                else if (DefaultRating < 1)
                    return PlayerRating;
                else
                {
                    if (PlayerRating > (DefaultRating + 1.5))
                        return DefaultRating + 1.5;
                    else if (PlayerRating < (DefaultRating - 1.5))
                        return DefaultRating - 1.5;

                    return PlayerRating;
                }
            }
        }

        public int CMC
        {
            get { return m_cmc; }
        }

        public string CC
        {
            get { return m_cc; }
        }

        public int Power
        {
            get { return m_power; }
        }

        public int Toughness
        {
            get { return m_toughness; }
        }

        public bool Tribal
        {
            get { return m_isTribal; }
        }

        public bool Legendary
        {
            get { return m_isLegend; }
        }


        private bool String2Bool(string toParse)
        {
            if (toParse.ToLower() == "true")
                return true;
            else if (toParse.ToLower() == "false")
                return false;
            throw new ArgumentOutOfRangeException("String toParse was not a valid boolean");
        }
    }
}
