namespace Runtime.Data.Enums
{
    /// <summary>
    /// Predefined tint channels for sprite coloring.
    /// Parts can be assigned to channels, then tinted by material type.
    /// </summary>
    public enum TintChannel
    {
        None,           // No tinting, use original color
        Primary,        // Main material color (wood, iron, gold)
        Secondary,      // Accent color (gem, wrap)
        Tertiary,       // Third color (rare, for complex items)
        Glow            // Emissive/glow color
    }
}
