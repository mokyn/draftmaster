using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace MTG_Drafter
{
    public partial class Main : Form
    {
        int passed, boosters, maxBoosters;
        Card selected;
        Set cSet;
        List<Set> sets;
        Dictionary<int, int> setRefs;
        Dictionary<string, string> setLibrary;
        List<Picks> draftedCards;
        List<CardColor[]> AIColors;
        Dictionary<ListViewItem, Card> picks;
        List<Dictionary<ListViewItem, Card>> packs;
        List<List<ListViewItem>> packRefs;

        public Main()
        {
            InitializeComponent();

            //Reset various properties and values
            maxBoosters = 0;
            boosters = 0;
            listViewPack.ShowGroups = true;
            listViewPicked.ShowGroups = true;
            setLibrary = new Dictionary<string, string>();

            var reader = new StreamReader("master.dmf-dat");//Read about all the sets that can be drafted
            string firstLine = reader.ReadLine();
            if (firstLine == null)
                throw new ArgumentNullException("You have no sets available to draft");
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                //first part of array is the longhand name
                //second part is shorthand name
                //if there is a third part, it determines whether the set can be drafted or not
                if (line.Split('|').Length > 2)
                {
                    if (line.Split('|')[2].ToLower() == "true")
                    {
                        //Add to the dictionary with Shorthand corresponding to longhand
                        setLibrary.Add(line.Split('|')[1], line.Split('|')[0]);
                        comboSets.Items.Add(line.Split('|')[1]);
                    }
                }
                else if (line.Split('|').Length == 2)
                {
                    //Add to the dictionary with Shorthand corresponding to longhand
                    setLibrary.Add(line.Split('|')[1], line.Split('|')[0]);
                    comboSets.Items.Add(line.Split('|')[1]);
                }
                else
                    throw new Exception("Format error. The format must be 'LONGNAME|SHORTNAME'.");
            }

            //First line must be properly formatted and is always shown
            setLibrary.Add(firstLine.Split('|')[1], firstLine.Split('|')[0]);
            comboSets.Items.Add(firstLine.Split('|')[1]);
            comboSets.SelectedItem = firstLine.Split('|')[1];

            reader.Close();
        }

        #region GUI
        /// <summary>
        /// Draw the information about the draft in the left-most panel
        /// Boosters, finished, in progress, to do, etc
        /// </summary>
        private void panelPackInfo_Paint(object sender, PaintEventArgs e)
        {
            if (maxBoosters != 0 && setRefs.Count == maxBoosters)
            {
                if (boosters != maxBoosters)
                    e.Graphics.DrawIcon(new Icon("done.dmf-img"), 10, 17);
                else
                    e.Graphics.DrawImage(Image.FromFile("current.dmf-img"), new Rectangle(0, 34, 35, 18));
                for (int i = 0; i < maxBoosters; i++)
                {
                    int height = 95 + 35 * i;
                    if (i < boosters || boosters == maxBoosters)
                        e.Graphics.DrawIcon(new Icon("done.dmf-img"), 10, height - 18);
                    e.Graphics.DrawString("Booster " + (i + 1) + ": " + sets[setRefs[i]].Name, new Font(FontFamily.GenericSansSerif, 8),
                        Brushes.Black, new PointF(37, height));
                }
            }
            else if (maxBoosters != 0)
            {
                e.Graphics.DrawString("CREATING DRAFT", new Font(FontFamily.GenericSansSerif, 8),
                        Brushes.Black, new PointF(2, 2));
                e.Graphics.DrawIcon(new Icon("done.dmf-img"), 10, 17);
                for (int i = 0; i < setRefs.Count; i++)
                {
                    int height = 95 + 35 * i;
                    e.Graphics.DrawIcon(new Icon("done.dmf-img"), 10, height - 18);
                    e.Graphics.DrawString("Booster " + (i + 1) + ": " + sets[setRefs[i]].Name, new Font(FontFamily.GenericSansSerif, 8),
                        Brushes.Black, new PointF(37, height));
                }
                for (int i = setRefs.Count; i < maxBoosters; i++)
                {
                    int height = 95 + 35 * i;
                    e.Graphics.DrawImage(Image.FromFile("current.dmf-img"), new Rectangle(0, height, 35, 18));
                }
            }

            if (boosters == maxBoosters)
            {
                e.Graphics.DrawImage(Image.FromFile("current.dmf-img"), new Rectangle(0, 32, 35, 18));
                e.Graphics.DrawString("CREATE NEW DRAFT", new Font(FontFamily.GenericSansSerif, 8),
                        Brushes.Black, new PointF(2, 2));
            }
            else if (setRefs.Count == maxBoosters)
            {
                int height = 92 + 35 * boosters;
                e.Graphics.DrawImage(Image.FromFile("current.dmf-img"), new Rectangle(0, height, 35, 18));
                e.Graphics.DrawString("DRAFT IN PROGRESS", new Font(FontFamily.GenericSansSerif, 8),
                        Brushes.Black, new PointF(2, 2));
            }
        }

        /// <summary>
        /// Changes the card image whenever you choose a different item
        /// </summary>
        private void listViewPack_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                selected = packs[passed % 8][listViewPack.SelectedItems[0]];
                cardImage.Image = selected.Picture;
                labelTips.Text = "TIPS: " + selected.Tip;
            }
            catch { }
        }

        /// <summary>
        /// Changes the card image whenever you choose a different item
        /// </summary>
        private void listViewPicked_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                selected = picks[listViewPicked.SelectedItems[0]];
                cardImage.Image = selected.Picture;
                labelTips.Text = "TIPS: " + selected.Tip;
            }
            catch { }
        }

        /// <summary>
        /// Draws 'faces' for each player
        /// May be useful at a later data
        /// </summary>
        private void panelPlayerInfo_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Image.FromFile(@"Assets\face1.dmf-img"), 10, 10, 50, 60);
            e.Graphics.DrawString("YOU", new Font(FontFamily.GenericSansSerif, 15), Brushes.Black, 10, 45);

            for (int i = 1; i < 8; i++)
            {
                e.Graphics.DrawImage(Image.FromFile(@"Assets\face1.dmf-img"), 10 + 70 * i, 10, 50, 60);
                e.Graphics.DrawString("AI " + i, new Font(FontFamily.GenericSansSerif, 15), Brushes.White, 10 + 70 * i, 45);
            }
        }

        /// <summary>
        /// Adds the picked item to your pile of drafted cards
        /// </summary>
        private void buttonPick_Click(object sender, EventArgs e)
        {
            if (selected != null && listViewPack.SelectedItems.Count > 0)
            {
                var picked = listViewPack.SelectedItems[0];
                picks.Add(picked, selected);
                packs[passed % 8].Remove(picked);
                packRefs[passed % 8].Remove(picked);
                listViewPack.Items.Remove(picked);
                listViewPicked.Items.Add(picked);
                picked.Group = listViewPicked.Groups[0];
                draftedCards[0].Add(selected);

                Updater();
            }
        }

        /// <summary>
        /// Creates a new game and resets all data
        /// </summary>
        private void buttonNewGame_Click(object sender, EventArgs e)
        {
            sets = new List<Set>();
            setRefs = new Dictionary<int, int>();
            maxBoosters = (int)upDownBoosters.Value;
            buttonAddSet.Enabled = true;
            comboSets.Enabled = true;
            buttonPick.Visible = false;
            buttonSave.Visible = false;
            cardImage.Image = null;
            labelTips.Text = "TIPS";
            panelPackInfo.Invalidate();
            if (packRefs != null)
                foreach (ListViewItem item in packRefs[passed])
                    listViewPack.Items.Remove(item);
        }

        /// <summary>
        /// Saves your deck via a SaveFileDialog
        /// </summary>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists("Save"))
                Directory.CreateDirectory("Save");
            var dialog = new SaveFileDialog();
            dialog.Filter = "SmartMTG Drafted Deck Files (*.smart-draft)|*.smart-draft";
            dialog.DefaultExt = ".smart-draft";
            dialog.Title = "Save SmartMTG Draft";
            dialog.InitialDirectory = "Save";
            var res = dialog.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                string extension = new gInclude.FilePath(dialog.FileName).Extension;
                SaveDeck(draftedCards[0].Cards, "Human.smart-deck", "Drafted Deck " + DateTime.Today.ToString());
                gInclude.FilePath[] compress = new gInclude.FilePath[8];
                compress[0] = new gInclude.FilePath("Human.smart-deck");
                for (int i = 1; i < draftedCards.Count; i++)
                {
                    SaveDeck(draftedCards[i].Cards, "AI " + i + " Drafted Deck.smart-deck", "AI " + i + " Drafted Deck.smart-deck");
                    compress[i] = new gInclude.FilePath("AI " + i + " Drafted Deck.smart-deck");
                }
                gInclude.Zip.AutoCompress(new gInclude.FilePath(dialog.FileName), gInclude.CompressionType.GZip, compress);
                foreach (gInclude.FilePath path in compress)
                    path.Delete();
            }
        }

        /// <summary>
        /// Misnamed; Adds a booster pack to your draft
        /// </summary>
        private void buttonAddSet_Click(object sender, EventArgs e)
        {
            int old = setRefs.Count;

            for (int i = 0; i < sets.Count; i++)
            {
                if (sets[i].Name == setLibrary[comboSets.SelectedItem.ToString()])
                {
                    setRefs.Add(setRefs.Count, i);
                    break;
                }
            }

            if (old == setRefs.Count)
            {
                sets.Add(new Set(setLibrary[comboSets.SelectedItem.ToString()]));
                setRefs.Add(setRefs.Count, sets.Count - 1);
            }

            if (setRefs.Count == maxBoosters)
                NewGame();
            else
                panelPackInfo.Invalidate();
        }
        #endregion

        #region Internal
        /// <summary>
        /// Saves the list of cards as a deck by finding your
        /// two most common colors and creating a deck with
        /// all drafted cards in those colors and provides
        /// basic lands.
        /// </summary>
        /// <param name="dCards"></param>
        /// <param name="path"></param>
        /// <param name="name"></param>
        private void SaveDeck(List<Card> dCards, string path, string name)
        {
            //Write the data to the filepath
            StreamWriter writ = new StreamWriter(path, false);//if the file exists, delete all preexisting content
            //Format the sets of the draft in SET - SET - SET formatting for use in notes
            string draftedSets = sets[setRefs[0]].Name;
            if (setRefs.Count > 1)
            {
                for (int i = 1; i < setRefs.Values.Count; i++)
                    draftedSets = draftedSets + " - " + sets[setRefs[i]].Name;
            }
            writ.WriteLine("name: " + name);//Record the name
            writ.WriteLine("type: drafted");//Type of deck
            writ.WriteLine("notes:");//Write notes
            writ.WriteLine("{");
            writ.WriteLine("Drafted: " + draftedSets);//Sets used in drafting (see above)
            writ.WriteLine("}");

            //Determine the deck's two most frequent colors
            #region DetermineColors
            Dictionary<CardColor, int> picked = new Dictionary<CardColor, int>();//Dictionary to store no of cards for each color
            foreach (Card card in dCards)
            {
                //If the card is a land, colorless, or multicolor, don't consider it
                if (card.MTGColor == CardColor.Multicolor || card.MTGColor == CardColor.Land || card.MTGColor == CardColor.Colorless)
                    continue;
                //if the card is hybrid
                if (card.MTGColor == CardColor.Hybrid)
                {
                    //Increment the number of cards for each of the hybrid's colors by 1
                    foreach (CardColor color in card.Colors)
                    {
                        if (picked.ContainsKey(color))
                            picked[color]++;
                        else
                            picked.Add(color, 1);
                    }
                }

                //If the card is only one color, just increment the card count for that color
                if (picked.ContainsKey(card.MTGColor))
                    picked[card.MTGColor]++;
                else
                    picked.Add(card.MTGColor, 1);
            }

            //Create a temp file to help determine colors
            var temp = new StreamWriter("Clr .dmf-temp");
            temp.WriteLine("-1|Colorless");
            temp.WriteLine("-1|Colorless");
            temp.Close();

            //for each of the five colors, evaluate how many cards you drafted of that color
            foreach (KeyValuePair<CardColor, int> data in picked)
            {
                StreamReader reader2 = new StreamReader("Clr .dmf-temp");//open the temp file

                //Grab the preexisting info
                //First line contains info for the color with the most cards
                var line = reader2.ReadLine().Split('|');
                var greatest = gInclude.Parse.String2Int(line[0]);//First part contains cards
                var grt = (CardColor)Enum.Parse(typeof(CardColor), line[1]);//Second part contains color

                //Second line contains the info for the color with the second most cards
                line = reader2.ReadLine().Split('|');//repeat
                var secondGreatest = gInclude.Parse.String2Int(line[0]);
                var scndGrt = (CardColor)Enum.Parse(typeof(CardColor), line[1]);

                reader2.Close();//close the file now that we have the data we need

                //if our current color's cards exceed that of the prior greatest color
                if (data.Value > greatest)
                {
                    StreamWriter writer = new StreamWriter("Clr .dmf-temp");
                    writer.WriteLine(data.Value + "|" + data.Key);//record that info on the first line
                    if (greatest > secondGreatest)//if the prior greatest color had more cards than the second greatest number (rather than tied)
                    {
                        secondGreatest = greatest;//Consider it the second greatest color
                        scndGrt = grt;
                    }
                    writer.WriteLine(secondGreatest + "|" + scndGrt);//record info for second greatest color
                    writer.Close();
                }
                else if (data.Value > secondGreatest)//if our value was less than the greatest color but more than the second greatest color
                {
                    StreamWriter writer = new StreamWriter("Clr .dmf-temp");
                    writer.WriteLine(greatest + "|" + grt);
                    writer.WriteLine(data.Value + "|" + data.Key);//save that info
                    writer.Close();
                }
            }

            //Now that we have recorded our two greatest colors, pick up that info
            StreamReader reader = new StreamReader("Clr .dmf-temp");
            var grt2 = (CardColor)Enum.Parse(typeof(CardColor), reader.ReadLine().Split('|')[1]);
            var scndGrt2 = (CardColor)Enum.Parse(typeof(CardColor), reader.ReadLine().Split('|')[1]);
            reader.Close();

            File.Delete("Clr .dmf-temp");//And get rid of the temp file
            #endregion

            //Count the number of copies of each card and sort the cards into maindeck or sideboard based on their color
            #region CountCopies
            //Create dictionaries to store the card and quantity
            Dictionary<Card, int> deck = new Dictionary<Card, int>(), sideboard = new Dictionary<Card, int>();
            foreach (Card card in dCards)//For each card that was drafted, based on its color, add it to the deck
            {
                bool addToDeck = false;//Add it to the deck or to the sideboard?

                if (card.MTGColor == CardColor.Colorless)//if it's colorless, add it to the deck
                    addToDeck = true;
                else if (card.MTGColor == CardColor.Hybrid)//if it's hybrid
                {
                    foreach (CardColor color in card.Colors)//and contains at least one of the two colors, add it to the deck
                    {
                        if (grt2 == color || scndGrt2 == color)
                            addToDeck = true;
                    }
                }
                else if (card.MTGColor == CardColor.Multicolor)//if it's multicolor
                {
                    if (card.Colors.Count == 2)//and only two-color multicolor
                    {
                        if ((card.Colors[0] == grt2 && card.Colors[1] == scndGrt2) || (card.Colors[1] == scndGrt2 && card.Colors[0] == grt2))
                            addToDeck = true;//And if its colors are the decks two colors, add it to the deck
                    }
                }
                else if (card.MTGColor == CardColor.Land)//if it's a land
                {
                    //And it 'fixes' only one color
                    if (card.ManaFix.Count == 1 && (card.ManaFix[0] == grt2 || card.ManaFix[0] == scndGrt2))
                    {
                        if (card.Rarity != Rarity.BasicLand && card.SuperType != SuperType.BasicLand)//if it's not a basic land
                            addToDeck = true;//Add it to the deck
                    }
                    else if (card.ManaFix.Contains(grt2) && card.ManaFix.Contains(scndGrt2))//if it fixes both of the deck's colors
                        addToDeck = true;//Add it to the deck
                }
                else//If it's monocolored
                {
                    if (card.MTGColor == grt2 || card.MTGColor == scndGrt2)//And one of our two colors
                        addToDeck = true;//Add it to the deck
                }

                if (addToDeck)
                {
                    //Add it to the deck
                    if (deck.Keys.Contains(card))
                        deck[card]++;
                    else
                        deck.Add(card, 1);
                }
                else if (card.SuperType != SuperType.BasicLand)//if it's not a basic land
                {
                    //add it to the sideboard
                    if (sideboard.Keys.Contains(card))
                        sideboard[card]++;
                    else
                        sideboard.Add(card, 1);
                }
            }
            #endregion

            //Add in basic lands as appropriate
            #region AddLands
            //Dim some stats that will later be needed
            int totalCost = 0, totalNonlandCards = 0, totalCards = 0, sym1 = 0, sym2 = 0;
            foreach (var item in deck)//for each card actually in the deck
            {
                totalCards += item.Value;//Add to the total number of cards in the deck
                if (item.Key.MTGColor == CardColor.Land)//If it's a land, just ignore it for the other stats
                    continue;
                totalNonlandCards += item.Value;//Otherwise, add it to the total number of nonland cards in the deck
                totalCost += item.Key.CMC * item.Value;//Add its cost to the total mana cost of cards in the deck
                if (grt2 != CardColor.Colorless)//if the primary color of the deck isn't colorless
                    sym1 += item.Key.ColoredCC(grt2) * item.Value;//Add to the total number of times the relevant mana symbol appears in CCs
                if (scndGrt2 != CardColor.Colorless)//There are two ifs rather than if-elseif because of hybrid and multicolor cards
                    sym2 += item.Key.ColoredCC(scndGrt2) * item.Value;//Do the same for the secondary color
            }
            //Do soem math for useful rations
            double r1 = (double)sym1 / totalNonlandCards,//Number of times symbol for the greatest color appears on average on a nonland card
                r2 = (double)sym2 / totalNonlandCards,//Number of times symbol for the second greatest color appears on average on a nonland card
                avgCMC = (double)totalCost / totalNonlandCards;//Average converted mana cost of nonland cards in the deck
            if (grt2 != CardColor.Colorless)//If the greatest color isn't colorless
                deck.Add(new Card(BasicLand(grt2), "M10"), (int)(r1 * avgCMC * 10));//Add appropriate basic lands
            if (scndGrt2 != CardColor.Colorless)//If the second greatest color isn't colorless
                deck.Add(new Card(BasicLand(scndGrt2), "M10"), (int)(r2 * avgCMC * 10));//Add appropriate basic lands
            totalCards += (int)(r1 * avgCMC * 10) + (int)(r2 * avgCMC * 10);
            if (totalCards < 40)//If the total amount of cards is still under 40 (minimum for drafted decks)
            {
                int diff = 40 - totalCards;//Find the number of cards needed to reach 40
                //And put in basic lands in proportion to the color proportions
                if (grt2 != CardColor.Colorless)
                    deck[deck.ElementAt(deck.Count - 2).Key] += (int)(diff * r1 / (r1 + r2));
                if (scndGrt2 != CardColor.Colorless)
                    deck[deck.ElementAt(deck.Count - 1).Key] += (int)(diff * r2 / (r1 + r2));
            }
            //Also, add in 50 of each basic land if you want to edit the deck later
            sideboard.Add(new Card(BasicLand(CardColor.White), "M10"), 50);
            sideboard.Add(new Card(BasicLand(CardColor.Blue), "M10"), 50);
            sideboard.Add(new Card(BasicLand(CardColor.Black), "M10"), 50);
            sideboard.Add(new Card(BasicLand(CardColor.Red), "M10"), 50);
            sideboard.Add(new Card(BasicLand(CardColor.Green), "M10"), 50);
            #endregion

            if (grt2 != CardColor.Colorless)//If the greatest color is not colorless
                writ.WriteLine("color: " + grt2.ToString());//record the info in the deck file
            if (scndGrt2 != CardColor.Colorless)//if the second greatest color is not colorless
                writ.WriteLine("color: " + scndGrt2.ToString());//record the info in the deck file
            writ.WriteLine("maindeck:");//begin recording the cards in the maindeck
            writ.WriteLine("{");
            foreach (var cards in deck)
                writ.WriteLine("\tcard: " + cards.Value + "|" + cards.Key.Name);//write each card and how many copies are in the deck
            writ.WriteLine("}");
            writ.WriteLine("sideboard:");
            writ.WriteLine("{");
            foreach (var cards in sideboard)
                writ.WriteLine("\tcard: " + cards.Value + "|" + cards.Key.Name);//write all of the other cards and how many copies there are
            writ.WriteLine("}");
            writ.Close();//the end
        }
        private string BasicLand(CardColor clr)
        {
            //Just return the appropriate basic land name for each of the five colors
            if (clr == CardColor.Black)
                return "Swamp";
            else if (clr == CardColor.Blue)
                return "Island";
            else if (clr == CardColor.Green)
                return "Forest";
            else if (clr == CardColor.Red)
                return "Mountain";
            else if (clr == CardColor.White)
                return "Plains";
            throw new Exception("Only colored (WUBRG) colors are accepted.");
        }

        /// <summary>
        /// Initiates the new draft and opens the first booster pack
        /// </summary>
        private void NewGame()
        {
            comboSets.Enabled = false;
            buttonAddSet.Enabled = false;
            buttonSave.Visible = false;
            buttonPick.Visible = true;

            AIColors = new List<CardColor[]>();
            picks = new Dictionary<ListViewItem, Card>();
            packs = new List<Dictionary<ListViewItem, Card>>();
            packRefs = new List<List<ListViewItem>>();
            draftedCards = new List<Picks>();

            selected = null;
            boosters = -1;

            for (int i = 0; i < 8; i++)
                draftedCards.Add(new Picks());
            foreach (ListViewItem item in listViewPicked.Items)
                listViewPicked.Items.Remove(item);

            NextBooster();
        }

        /// <summary>
        /// Creates and instantiates the next booster pack
        /// </summary>
        private void NextBooster()
        {
            boosters++;
            cSet = sets[setRefs[boosters]];
            packs = new List<Dictionary<ListViewItem, Card>>();
            packRefs = new List<List<ListViewItem>>();
            passed = 0;
            for (int i = 0; i < 8; i++)
                CreateBooster(cSet);
            LoadBooster(0);
            panelPackInfo.Invalidate();
            cardImage.Image = null;
            labelTips.Text = "TIP";
        }

        /// <summary>
        /// Creates a booster pack based on the set
        /// </summary>
        /// <param name="toUse">Set to use for pack creation</param>
        private void CreateBooster(Set toUse)
        {
            packs.Add(new Dictionary<ListViewItem, Card>());
            packRefs.Add(new List<ListViewItem>());

            var booster = packs[packs.Count - 1];
            var refs = packRefs[packRefs.Count - 1];

            int commons = 0, uncommons = 0, rares = 0, mythics = 0, basics = 0;
            foreach (Card card in toUse.Cards)
            {
                if (card.Rarity == Rarity.Common)
                    commons++;
                if (card.Rarity == Rarity.Uncommon)
                    uncommons++;
                if (card.Rarity == Rarity.Rare)
                    rares++;
                if (card.Rarity == Rarity.MythicRare)
                    mythics++;
                if (card.Rarity == Rarity.BasicLand)
                    basics++;
            }

            if ((basics % 5) != 0)
                throw new ArgumentException("The current set has inequally divided its basic lands");
            if (commons == 0 || uncommons == 0 || rares == 0)
                throw new ArgumentException("The current set lacks cards of either common, uncommon, or rare rarity.");

            if (mythics == 0 || (mythics > 0 && TrueRandom(8) != 0))
            {
                int index = 0, current = 0, num = TrueRandom(rares);
                while (true)
                {
                    if (index == num && toUse.Cards[current].Rarity == Rarity.Rare)
                    {
                        Card card = toUse.Cards[current];
                        var item = new ListViewItem(card.Name);
                        item.ForeColor = card.Color;
                        item.SubItems.AddRange(new string[] { "Rare" });
                        booster.Add(item, card);
                        refs.Add(item);
                        break;
                    }
                    if (toUse.Cards[current].Rarity == Rarity.Rare)
                        index++;
                    current++;
                    if (current >= toUse.Cards.Count)
                        throw new ArgumentException("Booster pack generation error");
                }
            }
            else
            {
                int index = 0, current = 0, num = TrueRandom(mythics);
                while (true)
                {
                    if (index == num && toUse.Cards[current].Rarity == Rarity.MythicRare)
                    {
                        Card card = toUse.Cards[current];
                        var item = new ListViewItem(card.Name);
                        item.ForeColor = card.Color;
                        item.SubItems.AddRange(new string[] { "Mythic Rare" });
                        booster.Add(item, card);
                        refs.Add(item);
                        break;
                    }
                    if (toUse.Cards[current].Rarity == Rarity.MythicRare)
                        index++;
                    current++;
                    if (current >= toUse.Cards.Count)
                        throw new ArgumentException("Booster pack generation error");
                }
            }

            for (int i = 0; i < 3; i++)
            {
                int index = 0, current = 0, num = TrueRandom(uncommons);
                while (true)
                {
                    if (index == num && toUse.Cards[current].Rarity == Rarity.Uncommon)
                    {
                        Card card = toUse.Cards[current];
                        var item = new ListViewItem(card.Name);
                        item.ForeColor = card.Color;
                        item.SubItems.AddRange(new string[] { "Uncommon" });
                        booster.Add(item, card);
                        refs.Add(item);
                        break;
                    }
                    if (toUse.Cards[current].Rarity == Rarity.Uncommon)
                        index++;
                    current++;
                    if (current >= toUse.Cards.Count)
                        throw new ArgumentException("Booster pack generation error");
                }
            }

            for (int i = 0; i < 10; i++)
            {
                int index = 0, current = 0, num = TrueRandom(commons);
                while (true)
                {
                    if (index == num && toUse.Cards[current].Rarity == Rarity.Common)
                    {
                        Card card = toUse.Cards[current];
                        var item = new ListViewItem(card.Name);
                        item.ForeColor = card.Color;
                        item.SubItems.AddRange(new string[] { "Common" });
                        booster.Add(item, card);
                        refs.Add(item);
                        break;
                    }
                    if (toUse.Cards[current].Rarity == Rarity.Common)
                        index++;
                    current++;
                    if (current >= toUse.Cards.Count)
                        throw new ArgumentException("Booster pack generation error");
                }
            }

            if (basics != 0)
            {
                int index = 0, current = 0, num = TrueRandom(basics);
                while (true)
                {
                    if (index == num && toUse.Cards[current].Rarity == Rarity.BasicLand)
                    {
                        Card card = toUse.Cards[current];
                        var item = new ListViewItem(card.Name);
                        item.ForeColor = card.Color;
                        item.SubItems.AddRange(new string[] { "Basic Land" });
                        booster.Add(item, card);
                        refs.Add(item);
                        break;
                    }
                    if (toUse.Cards[current].Rarity == Rarity.BasicLand)
                        index++;
                    current++;
                    if (current >= toUse.Cards.Count)
                        throw new ArgumentException("Booster pack generation error");
                }
            }
            else
            {
                int index = 0, current = 0, num = TrueRandom(commons);
                while (true)
                {
                    if (index == num && toUse.Cards[current].Rarity == Rarity.Common)
                    {
                        Card card = toUse.Cards[current];
                        var item = new ListViewItem(card.Name);
                        item.ForeColor = card.Color;
                        item.SubItems.AddRange(new string[] { "Common" });
                        booster.Add(item, card);
                        refs.Add(item);
                        break;
                    }
                    if (toUse.Cards[current].Rarity == Rarity.Common)
                        index++;
                    current++;
                    if (current >= toUse.Cards.Count)
                        throw new ArgumentException("Booster pack generation error");
                }
            }
        }

        /// <summary>
        /// Loads an already created booster pack.
        /// Used for 'passing' packs
        /// </summary>
        /// <param name="index">Booster index to load</param>
        private void LoadBooster(int index)
        {
            try
            {
                foreach (ListViewItem item in listViewPack.Items)
                    listViewPack.Items.Remove(item);

                var refs = packRefs[index];
                foreach (ListViewItem item in refs)
                    listViewPack.Items.Add(item);
            }
            catch (Exception e) { throw e; }
        }

        /// <summary>
        /// Generates a truly random number using RGenCrypto
        /// </summary>
        /// <param name="max">Maximum number to generate</param>
        /// <returns>Number between 0 and the maximum, inclusive of 0 and noninclusive of maximum</returns>
        private int TrueRandom(int max)
        {
            if (max < 1)
                throw new ArgumentOutOfRangeException("Int max must be greater than 0");

            byte[] rand = new byte[1];
            new RNGCryptoServiceProvider().GetBytes(rand);
            int toRet = Convert.ToInt32(rand[0]) % max;
            return toRet;
        }

        /// <summary>
        /// Updates all items, and calls the AI logic.
        /// Also adds a rating to the card you just chose
        /// equal to the 'pick' it is.
        /// </summary>
        private void Updater()
        {
            AI();

            panelPlayerInfo.Invalidate();

            passed++;
            LoadBooster(passed % 8);

            if (packs[passed % 8].Count == 0)
            {
                if (boosters >= (maxBoosters - 1))
                {
                    buttonPick.Visible = false;
                    buttonSave.Visible = true;
                    panelPackInfo.Invalidate();

                    for (int i = 0; i < draftedCards[0].Cards.Count; i++)
                        draftedCards[0].Cards[i].AddRating(i % 15 + 1);
                }
                else
                    NextBooster();
            }
        }
        #endregion

        #region AI
        /// <summary>
        /// The only 'externally' called function of AI.
        /// Does the logic for each of the 7 AIs.
        /// </summary>
        private void AI()
        {
            try
            {
                //Determine the current AIs' top two colors
                DetermineColors();
                for (int i = 1; i < 8; i++)//For each of the 7 AIs
                {
                    int packNum = (passed + i) % 8;
                    var writer = new StreamWriter("AI " + i.ToString() + ".dmf-temp");//creates a temp file
                    writer.Write("101");
                    writer.Close();

                    if (boosters == 0 && passed < 5)//if it is the 1st pack and within the first 5 picks
                    {
                        //choose cards based on their rating rather than comparing to already picked cards
                        for (int j = 0; j < packs[packNum].Count; j++)
                            PointSys(i, j, packNum);
                    }
                    else//Otherwise, pick the best card that fits your deck
                        Logic(i, packNum);//If no card is there from the two colors, choose the best card

                    StreamReader reader2 = new StreamReader("AI " + i.ToString() + ".dmf-temp");//Find out what card the AI picked
                    reader2.ReadLine();
                    int high2 = gInclude.Parse.String2Int(reader2.ReadLine());
                    reader2.Close();

                    //Pick the card
                    var picked = packRefs[packNum][high2];
                    draftedCards[i].Add(packs[packNum][picked]);
                    packs[packNum % 8].Remove(picked);
                    packRefs[packNum % 8].Remove(picked);
                    listViewPack.Items.Remove(picked);

                    File.Delete("AI " + i.ToString() + ".dmf-temp");//delete the temp file
                }
            }
            catch { }
        }

        /// <summary>
        /// Determines the top two colors of the AI.
        /// </summary>
        private void DetermineColors()
        {
            try
            {
                AIColors.Add(new CardColor[] { });//Dim the card colors for the AIs
                for (int i = 1; i < 8; i++)
                {
                    //Logic is the exact same used in SaveDeck.
                    Dictionary<CardColor, int> picked = new Dictionary<CardColor, int>();
                    foreach (Card card in draftedCards[i].Cards)
                    {
                        if (card.MTGColor == CardColor.Multicolor ||
                            card.MTGColor == CardColor.Land ||
                            card.MTGColor == CardColor.Colorless)
                            continue;
                        if (card.MTGColor == CardColor.Hybrid)
                        {
                            foreach (CardColor color in card.Colors)
                            {
                                if (picked.ContainsKey(color))
                                    picked[color]++;
                                else
                                    picked.Add(color, 1);
                            }
                        }


                        if (picked.ContainsKey(card.MTGColor))
                            picked[card.MTGColor]++;
                        else
                            picked.Add(card.MTGColor, 1);
                    }

                    var temp = new StreamWriter("Clr " + i + ".dmf-temp");
                    temp.WriteLine("-1|Colorless");
                    temp.WriteLine("-1|Colorless");
                    temp.Close();

                    foreach (KeyValuePair<CardColor, int> data in picked)
                    {
                        StreamReader reader2 = new StreamReader("Clr " + i + ".dmf-temp");

                        var line = reader2.ReadLine().Split('|');
                        var greatest = gInclude.Parse.String2Int(line[0]);
                        var grt = (CardColor)Enum.Parse(typeof(CardColor), line[1]);

                        line = reader2.ReadLine().Split('|');
                        var secondGreatest = gInclude.Parse.String2Int(line[0]);
                        var scndGrt = (CardColor)Enum.Parse(typeof(CardColor), line[1]);

                        reader2.Close();

                        if (data.Value > greatest)
                        {
                            StreamWriter writer = new StreamWriter("Clr " + i + ".dmf-temp");
                            writer.WriteLine(data.Value + "|" + data.Key);
                            if (greatest > secondGreatest)//if the prior greatest color had more cards than the second greatest number (rather than tied)
                            {
                                secondGreatest = greatest;//Consider it the second greatest color
                                scndGrt = grt;
                            }
                            writer.WriteLine(secondGreatest + "|" + scndGrt);
                            writer.Close();
                        }
                        else if (data.Value > secondGreatest)
                        {
                            StreamWriter writer = new StreamWriter("Clr " + i + ".dmf-temp");
                            writer.WriteLine(greatest + "|" + grt);
                            writer.WriteLine(data.Value + "|" + data.Key);
                            writer.Close();
                        }
                    }

                    StreamReader reader = new StreamReader("Clr " + i + ".dmf-temp");
                    var grt2 = (CardColor)Enum.Parse(typeof(CardColor), reader.ReadLine().Split('|')[1]);
                    var scndGrt2 = (CardColor)Enum.Parse(typeof(CardColor), reader.ReadLine().Split('|')[1]);
                    reader.Close();

                    AIColors.Add(new CardColor[] { grt2, scndGrt2 });
                    File.Delete("Clr " + i + ".dmf-temp");
                }
            }
            catch { }
        }

        /// <summary>
        /// Choose the best card for the AI
        /// based on its deck's top two colors.
        /// </summary>
        /// <param name="AINum">AI number to choose for</param>
        /// <param name="packNum">Pack number to choose from</param>
        private void Logic(int AINum, int packNum)
        {
            try
            {
                var temp = File.Create("AI " + AINum.ToString() + "-think.dmf-temp");//Create temp file
                temp.Close();

                for (int j = 0; j < packs[packNum].Count; j++)
                    SortRelevantColors(AINum, j, packNum);//Sort the relevantly colored cards in the pack

                List<Card> usableX = GetRelevantColors(AINum, packNum);//Get the relevantly colored cards in the pack
                Picks usable = new Picks();//Generate a Picks for use in logic
                foreach (Card card in usableX)
                    usable.Add(card);
                if (usable.Cards.Count < 1)//If there is no usable card in the pack, just choose the best one
                {
                    for (int j = 0; j < packs[packNum].Count; j++)
                        PointSys(AINum, j, packNum);
                }
                else
                {
                    Picks toUse = usable;
                    if (draftedCards[AINum].Proportion < .667F)//If the number of creatures in the usable picks is less than 2/3s
                    {
                        toUse = Picks.List2Picks(toUse.Creatures);//Pick a creature from the pack
                        if (draftedCards[AINum].AvgCMC < 3.5F || toUse.CMCLessThan(3).Count < 1)//If the curve is too high,
                            toUse = Picks.List2Picks(toUse.CMCLessThan(4));//Choose a low cmc creature
                        else if (draftedCards[AINum].AvgCMC < 2.7)//If the curve is moderate,
                            toUse = Picks.List2Picks(Picks.List2Picks(toUse.CMCLessThan(6)).CMCGreaterThan(3));//choose a middle-costed card
                        else//if the curve is low
                            toUse = Picks.List2Picks(toUse.CMCGreaterThan(5));//choose a high cmc creature
                    }
                    else//if we have atleast 2/3 of our usable picks as creatures, pick a noncreature card
                    {
                        toUse = Picks.List2Picks(toUse.Noncreature);
                        if (draftedCards[AINum].AvgCMC < 3.5F)//If the curve is too high,
                            toUse = Picks.List2Picks(toUse.CMCLessThan(4));//Choose a low cmc card
                        else if (draftedCards[AINum].AvgCMC < 2.7)//If the curve is moderate,
                            toUse = Picks.List2Picks(Picks.List2Picks(toUse.CMCLessThan(6)).CMCGreaterThan(3));//Choose a middle-costed card
                        else//if the curve is low
                            toUse = Picks.List2Picks(toUse.CMCGreaterThan(5));//choose a high cmc card
                    }

                    if (toUse.Cards.Count < 1)//if we can't use any of the cards in the pack
                        toUse = usable;//just pick one of the relevantly colored cards in the pack
                    for (int j = 0; j < toUse.Cards.Count; j++)
                        PointSys(AINum, j, toUse.Cards);//Choose the highest rated card in the usable picks

                    //read our highest-scored card
                    StreamReader reader = new StreamReader("AI " + AINum.ToString() + ".dmf-temp");
                    reader.ReadLine();
                    int high = gInclude.Parse.String2Int(reader.ReadLine());
                    reader.Close();

                    reader = new StreamReader("AI " + AINum.ToString() + "-think.dmf-temp");
                    if (high > 0)
                    {
                        for (int i = 1; i < high; i++)
                            reader.ReadLine();
                    }

                    //and tell the AI to pick it
                    StreamWriter writer = new StreamWriter("AI " + AINum.ToString() + ".dmf-temp");
                    writer.WriteLine();
                    writer.WriteLine(reader.ReadLine());
                    reader.Close();
                    writer.Close();
                }
                File.Delete("AI " + AINum.ToString() + "-think.dmf-temp");
            }
            catch { }
        }

        /// <summary>
        /// Sorts the relevantly colored cards from the pack
        /// </summary>
        /// <param name="AINum">AI to sort cards for</param>
        /// <param name="j">Loop value</param>
        /// <param name="packNum">Pack number to sort</param>
        private void SortRelevantColors(int AINum, int j, int packNum)
        {
            try
            {
                var picked = packs[packNum][packRefs[packNum][j]];

                if (picked.MTGColor == CardColor.Colorless)
                    WriteData(AINum, j);
                if (picked.MTGColor == CardColor.Multicolor)
                {
                    if (picked.Colors.Count == 2)
                    {
                        if (picked.Colors.Contains(AIColors[AINum][0]) && picked.Colors.Contains(AIColors[AINum][1]))
                            WriteData(AINum, j);
                    }
                }
                else if (picked.MTGColor == CardColor.Hybrid)
                {
                    if (picked.Colors.Contains(AIColors[AINum][0]) || picked.Colors.Contains(AIColors[AINum][1]))
                        WriteData(AINum, j);
                }
                else if (picked.MTGColor == CardColor.Land)
                {
                    if (picked.ManaFix.Contains(AIColors[AINum][0]) && picked.ManaFix.Contains(AIColors[AINum][1]))
                        WriteData(AINum, j);
                }
                else
                {
                    if (picked.MTGColor == AIColors[AINum][0] || picked.MTGColor == AIColors[AINum][1])
                        WriteData(AINum, j);
                }
            }
            catch { }
        }

        /// <summary>
        /// Write data for an AI file
        /// </summary>
        /// <param name="AINum">AI number</param>
        /// <param name="cardID">ID of the card</param>
        private void WriteData(int AINum, int cardID)
        {
            try
            {
                StreamWriter writer = new StreamWriter("AI " + AINum.ToString() + "-think.dmf-temp", true);
                writer.WriteLine(cardID);
                writer.Close();
            }
            catch { }
        }

        /// <summary>
        /// Gets the relevantly colored cards from the pack
        /// </summary>
        /// <param name="AINum">AI number</param>
        /// <param name="packNum">Pack to get cards from</param>
        /// <returns></returns>
        private List<Card> GetRelevantColors(int AINum, int packNum)
        {
            try
            {
                List<Card> toRet = new List<Card>();
                string line;

                StreamReader reader = new StreamReader("AI " + AINum.ToString() + "-think.dmf-temp");
                while ((line = reader.ReadLine()) != null)
                {
                    var picked = packRefs[packNum][gInclude.Parse.String2Int(line)];
                    toRet.Add(packs[packNum][picked]);
                }
                reader.Close();

                return toRet;
            }
            catch { return new List<Card>(); }
        }

        /// <summary>
        /// Use the points system to get the highest-rated card in the pack
        /// </summary>
        /// <param name="i">Loop number</param>
        /// <param name="j">Nested loop number</param>
        /// <param name="packCards">Cards to choose from</param>
        private void PointSys(int i, int j, List<Card> packCards)
        {
            try
            {
                var temp = packCards[j];

                StreamReader reader = new StreamReader("AI " + i.ToString() + ".dmf-temp");
                double high = gInclude.Parse.String2Double(reader.ReadLine());
                if (high < 1)
                    high = 101;
                reader.Close();

                if (temp.Rating < high)
                {
                    StreamWriter writer2 = new StreamWriter("AI " + i.ToString() + ".dmf-temp");
                    writer2.WriteLine(temp.Rating.ToString());
                    writer2.WriteLine(j.ToString());
                    writer2.Close();
                }
            }
            catch
            {
                StreamWriter writer2 = new StreamWriter("AI " + i.ToString() + ".dmf-temp");
                writer2.WriteLine(packCards[j].DefaultRating.ToString());
                writer2.WriteLine(j.ToString());
                writer2.Close();
            }
        }

        /// <summary>
        /// Use the points system to get the highest-rated card in the pack
        /// </summary>
        /// <param name="i">Loop number</param>
        /// <param name="j">Nested loop number</param>
        /// <param name="packNum">Pack to choose from</param>
        private void PointSys(int i, int j, int packNum)
        {
            try
            {
                var temp = packs[packNum][packRefs[packNum][j]];

                StreamReader reader = new StreamReader("AI " + i.ToString() + ".dmf-temp");
                double high = gInclude.Parse.String2Double(reader.ReadLine());
                if (high < 1)
                    high = 101;
                reader.Close();

                if (temp.Rating < high)
                {
                    StreamWriter writer2 = new StreamWriter("AI " + i.ToString() + ".dmf-temp");
                    writer2.WriteLine(temp.Rating.ToString());
                    writer2.WriteLine(j.ToString());
                    writer2.Close();
                }
            }
            catch
            {
                StreamWriter writer2 = new StreamWriter("AI " + i.ToString() + ".dmf-temp");
                writer2.WriteLine(packs[packNum][packRefs[packNum][j]].Rating.ToString());
                writer2.WriteLine(j.ToString());
                writer2.Close();
            }
        }
        #endregion
    }
}