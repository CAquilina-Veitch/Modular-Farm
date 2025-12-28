namespace Runtime.Data.Enums
{
    // GDD 15.2 - Predefined NPC activities (AI cannot create new activities)
    public enum NPCActivity
    {
        Sleeping,       // At home, not interactable
        Working,        // At shop/station, can trade
        Walking,        // Moving between locations
        Idle,           // Standing around, can talk
        Gathering,      // Collecting items in a zone
        Socializing     // Talking to other NPCs
    }
}
