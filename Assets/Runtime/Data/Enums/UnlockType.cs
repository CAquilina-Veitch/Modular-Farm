namespace Runtime.Data.Enums
{
    // GDD 13.12 - Friendship tier unlock types
    public enum UnlockType
    {
        CanFollow,      // NPC can follow player
        CanEnterHome,   // Player can enter NPC's home
        PriceModifier,  // Shop price adjustment
        DialogueOption, // New dialogue becomes available
        Activity,       // New activity unlocked
        NPCMovesTo,     // NPC relocates
        FreeShop        // NPC's shop items become free
    }
}
