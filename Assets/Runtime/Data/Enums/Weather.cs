namespace Runtime.Data.Enums
{
    // GDD 4.1 - Weather types
    public enum Weather
    {
        Sunny,  // Default, all shops open, NPCs roam freely
        Rain,   // Crops auto-watered, some NPCs stay indoors
        Snow,   // Winter-only, movement slower
        Any     // Used for spawn conditions that don't care about weather
    }
}
