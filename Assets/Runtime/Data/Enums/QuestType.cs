namespace Runtime.Data.Enums
{
    // GDD 13.7 - Quest categories
    public enum QuestType
    {
        Fetch,      // Collect items and return
        Deliver,    // Bring item to specific NPC
        Gather,     // Collect items (no return required)
        Talk,       // Have conversation with NPC
        Build,      // Construct something
        Hunt,       // Defeat mobs
        Visit       // Go to a location
    }
}
