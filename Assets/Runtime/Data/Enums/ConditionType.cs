namespace Runtime.Data.Enums
{
    // Used in building entry conditions, dialogue conditions
    public enum ConditionType
    {
        Friendship,     // Requires minimum friendship with NPC
        Time,           // Requires specific time phase
        Item,           // Requires item in inventory
        QuestComplete   // Requires completed quest
    }
}
