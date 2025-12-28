# Building System Design Document

## 1. Purpose

The Building System manages structures in the world - homes, shops, and special locations. Buildings occupy tile space in zones and can have interiors (pocket dimensions). Entry conditions control access based on time, friendship, items, or quest progress. Buildings are where NPCs live and work.

## 2. Data Schema

**ScriptableObject:** `BuildingData` (Assets/Runtime/Data/ScriptableObjects/BuildingData.cs)

### Key Fields

| Field | Type | Purpose |
|-------|------|---------|
| Id | string | Unique identifier |
| Name | string | Display name |
| Zone | Zone | Which zone contains this building |
| SizeTiles | TileSize | Footprint in tiles (width × height) |
| DoorTile | TilePosition | Door position relative to origin |
| InteriorSize | InteriorSize | Interior dimensions (None, Small, Medium, Large) |
| InteriorLayout | string[] | Flattened tile IDs for interior |
| OwnerNPCId | string | NPC who owns this building |
| EntryConditions | EntryCondition[] | Requirements to enter |

### InteriorSize Enum

| Size | Dimensions | Use Cases |
|------|------------|-----------|
| None | N/A | No interior (wells, statues) |
| Small | 6×4 tiles | Small shops, cottages |
| Medium | 8×5 tiles | Standard homes, most shops |
| Large | 10×7 tiles | Large establishments, upgraded player house |

### EntryCondition Types

| Type | Description |
|------|-------------|
| Friendship | Requires minimum friendship with specified NPC |
| Time | Only during specified time phases |
| Item | Requires item in inventory |
| QuestComplete | Requires completed quest |

## 3. Requirements

- Must support placement in any zone (Village primarily, but also Farm, etc.)
- Must support buildings with and without interiors
- Must support entry conditions combining multiple requirements
- Must support NPC ownership (for schedule/shop association)
- Must integrate with village layout algorithm (procedural placement)
- Must support interior customization (furniture placement)

## 4. Limitations

- **Fixed interior sizes** - Only 3 size options. Custom dimensions require code changes.
- **No multi-floor buildings** - All interiors are single-floor.
- **No building construction** - Buildings appear instantly (no construction animation/time).
- **Rectangular only** - All buildings are rectangular footprints.
- **Static placement** - Once placed, buildings don't move.

## 5. Modularity

### This System Owns
- Building definitions (BuildingData ScriptableObjects)
- Building registry (BuildingRegistry singleton)
- Interior configurations
- Entry condition logic

### This System References
- Zone (enum) - which zone contains building
- NPCData (by ID) - OwnerNPCId
- ItemData (by ID) - for Item entry conditions
- QuestData (by ID) - for QuestComplete entry conditions
- TimePhase (enum) - for Time entry conditions

### Other Systems Reference This
- **NPCData** - HomeBuildingId references buildings
- **ZoneManager** - places buildings in world
- **InteriorManager** - loads interior layouts as pocket dimensions
- **RecipeData** - StationRequiredId may reference building (e.g., Kitchen building)

## 6. AI Integration Points

### What AI Can Generate
- New buildings with any zone/size
- Custom entry conditions
- Buildings as crafting locations (instead of portable stations)
- Connected content (building + owner NPC + shop inventory)

### Cascade Generation Example

Prompt: "Add a church for weddings"

AI generates:
- BuildingData: Chapel (Village, Medium interior)
- NPCData: Priest (lives in Chapel, sells wedding rings)
- FriendshipTierData: Partner tier at 110 points
- ItemData: Wedding Ring
- DialogueOptionData: Proposal dialogue

### Validation Requirements
- Zone must be valid Zone enum
- OwnerNPCId must reference valid NPCData (or pending)
- EntryCondition targets must reference valid IDs
- InteriorLayout length should match InteriorSize dimensions (width × height)

## 7. Example Content

```csharp
BuildingData janesHome = new BuildingData {
    Id = "building_janes_homestead",
    Name = "Jane's Homestead",
    Zone = Zone.Village,
    SizeTiles = new TileSize(4, 3),
    DoorTile = new TilePosition(1, 0),
    InteriorSize = InteriorSize.Medium,
    InteriorLayout = new string[] { }, // Empty = default layout
    OwnerNPCId = "npc_farmer_jane",
    EntryConditions = new[] {
        EntryCondition.Time(TimePhase.Day),
        EntryCondition.Friendship("npc_farmer_jane", 40)
    }
};
```

## 8. Edge Cases & Considerations

### Multiple Entry Conditions
All conditions must be met (AND logic). For OR logic, create separate door/entrance.

### No Owner
Buildings without OwnerNPCId are public (shop hours still apply if relevant).

### Village Expansion
New buildings should trigger village layout algorithm:
1. Find empty space near existing buildings
2. Reserve footprint + buffer
3. Generate path connection
4. Instantiate building

### Interior Loading
Interiors are separate scenes/areas (pocket dimensions). Transition:
1. Check entry conditions
2. If passed, disable exterior, enable interior
3. Player spawns at interior door position
4. Exiting reverses this

### Orphaned Buildings
If OwnerNPCId references deleted/invalid NPC, building has no owner. Should still be enterable if conditions allow.
