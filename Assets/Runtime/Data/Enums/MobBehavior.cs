namespace Runtime.Data.Enums
{
    // GDD 15.1 - Predefined mob behaviors (AI cannot create new behaviors)
    public enum MobBehavior
    {
        PassiveWander,  // Moves randomly, ignores player
        PassiveFlee,    // Runs away if player approaches
        Grazing,        // Wanders slowly, stops to eat, stays in zone
        Hostile,        // Chases and attacks player on sight
        Territorial,    // Attacks only if player enters radius
        Nocturnal,      // Only active at Night phase
        Pack,           // Groups with same type, attacks together
        PetFollow       // Follows player when tamed
    }
}
