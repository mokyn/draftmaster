namespace MTG_Drafter
{
    /// <summary>
    /// Accepted supertypes for cards
    /// </summary>
    enum SuperType
    {
        Artifact, ArtifactCreature, Creature,
        Enchantment, Instant, Sorcery,
        Land, Planeswalker, BasicLand
    }

    /// <summary>
    /// Accepted colors or combination of colors for cards
    /// </summary>
    enum CardColor
    {
        Colorless, Hybrid, Multicolor,
        White, Blue, Black,
        Red, Green, Land
    }

    /// <summary>
    /// Accepted rarities of cards
    /// </summary>
    enum Rarity
    {
        Common, Uncommon, Rare,
        MythicRare, BasicLand
    }

    /// <summary>
    /// Card tags used to describe what the card does
    /// Currently, all except for Manafixer are unused by AI logic
    /// </summary>
    enum CardTag
    {
        //All spells
        Manafixer, Burn, Pump,
        Aura, Removal, Draw, 
        Prevent, LifeGain, LifeLose,
        Targeted, Pacify, Discard,
        Tutor, Graveyard, Misc,
        Equipment, Aggro, Evasion,
        Sweeper, ReturnToHand, TokenGen,
        Random, Coinflip, Mill,

        //Targeted cards only
        TCreature, TEnchantment, TLand,
        TPlaneswalker, TArtifact,

        //Permanents only
        CIPEffect, LPEffect, MiscEffect,
        Activated, Triggered,
        
        //Creatures only
        Vanilla, Deathtouch, FirstStrike,
        DoubleStrike, Flying, Haste,
        Shroud, Trample, Vigilance, Lifelink,
        Unblockable, Indestructible, ExpertLevel,
        ProtGreen, ProtRed, ProtWhite,
        ProtBlack, ProtBlue, ProtColorless,
        ProtMonocolor, ProtMulticolor, Basiclandwalk,
        Nonbasiclandwalk, Plainswalk, Islandwalk,
        Swampwalk, Mountainwalk, Forestwalk
    }
}