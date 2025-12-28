namespace Runtime.Data.Enums
{
    /// <summary>
    /// Categories of sprite parts for modular composition.
    /// Parts are combined in layers to create complete item/entity sprites.
    /// </summary>
    public enum SpritePartType
    {
        // === Shape Primitives (Container bases) ===
        Box,            // Crates, chests, boxes
        Cylinder,       // Buckets, barrels, cans
        Sphere,         // Balls, orbs, eggs
        RoundedRect,    // Backpacks, bags, pouches
        Jar,            // Potion bottles, jars
        Bottle,         // Tall bottles, vials

        // === Openings (What goes on top of containers) ===
        OpenTop,        // Open bucket, crate
        Lid,            // Chest lid, pot cover
        Cork,           // Bottle stopper
        Spout,          // Watering can spout
        Nozzle,         // Spray nozzle

        // === Attachments (How containers are held/accessed) ===
        HandleTop,      // Bucket handle, pot grip
        HandleSide,     // Mug handle, drawer pull
        Strap,          // Backpack straps, sling
        Rope,           // Rope handle, binding
        Spigot,         // Barrel tap, keg spout

        // === Tool Parts ===
        Handle,         // Tool/weapon handles
        ToolHead,       // Hoe, pickaxe, rake heads
        Blade,          // Sword, knife, axe blades
        Guard,          // Sword crossguard, hand guard
        Pommel,         // Handle end caps

        // === Station Parts ===
        StationBase,    // Workbench, anvil, furnace body
        StationAccent,  // Fire glow, decorative trim
        DisplaySlot,    // Item display position on station

        // === Plant Parts ===
        PlantBase,      // Stem, stalk, vine, bush base
        PlantTop,       // Leaves, flower, fruit, grain head

        // === Machine Parts ===
        Gear,           // Cogs, gears
        Pipe,           // Straight pipe, elbow, valve
        Valve,          // Control valves
        Meter,          // Gauges, dials
        Exhaust,        // Smokestacks, vents
        Belt,           // Drive belts, chains

        // === Furniture Parts ===
        Leg,            // Chair/table legs
        Surface,        // Tabletop, seat, shelf
        Back,           // Chair back, headboard
        Frame,          // Bed frame, cabinet body
        Fabric,         // Cushion, blanket, curtain

        // === Magic/Effects ===
        Aura,           // Magical glow overlay
        Glow,           // Light emission
        Rune,           // Magical symbols

        // === Generic ===
        Other           // Anything else
    }
}
