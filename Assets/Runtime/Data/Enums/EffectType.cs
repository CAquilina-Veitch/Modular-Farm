namespace Runtime.Data.Enums
{
    // GDD 13.11 - Dialogue effect types
    public enum EffectType
    {
        StartQuest,         // Begin a new quest
        GiveItem,           // Give item to player
        ChangeFriendship,   // Modify friendship points
        TriggerEvent,       // Trigger a game event
        ChangeNPCLocation   // Move NPC to new location
    }
}
