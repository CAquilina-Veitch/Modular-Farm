namespace Runtime.Data.Enums
{
    /// <summary>
    /// Categories of sprite parts for modular composition.
    /// Each item type uses different part combinations.
    /// </summary>
    public enum SpritePartType
    {
        // Tool parts
        Handle,         // Wooden, iron, gold handles
        ToolHead,       // Hoe blade, pickaxe head, axe blade

        // Weapon parts
        Blade,          // Sword blades, knife edges
        Guard,          // Sword crossguards
        Pommel,         // Handle end caps

        // Container parts
        Container,      // Bottle, jar, chest base
        Lid,            // Caps, lids, locks
        Contents,       // Liquid, gems, items inside

        // Decoration
        Gem,            // Decorative gems
        Wrap,           // Handle wraps, ribbons
        Glow,           // Magical glow effects
        Rune,           // Rune markings

        // Crop parts
        Stem,           // Plant stems
        Leaves,         // Foliage
        Fruit,          // The harvestable part
        Flower,         // Blossoms

        // Character parts (future)
        Body,
        Head,
        Hair,
        Outfit,
        Accessory
    }
}
