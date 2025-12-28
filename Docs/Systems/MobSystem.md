# Mob System Design Document

## 1. Purpose

The Mob System manages creatures in the game world - passive animals, hostile monsters, and everything in between. Mobs have predefined behaviors, spawn in specific zones/conditions, drop items when killed, and can be harvested or tamed. This system enables the farm animal economy (chickens, cows) and world danger (wolves, slimes).

## 2. Data Schema

**ScriptableObject:** `MobData` (Assets/Runtime/Data/ScriptableObjects/MobData.cs)

### Key Fields

| Field | Type | Purpose |
|-------|------|---------|
| Id | string | Unique identifier |
| Name | string | Display name |
| Behavior | MobBehavior | Predefined behavior type (AI cannot create new behaviors) |
| SpawnZones | Zone[] | Where mob can spawn |
| SpawnWeight | float | Rarity (higher = more common) |
| SpawnWeather | Weather[] | Weather conditions for spawning |
| SpawnSeasons | Season[] | Seasons when mob spawns |
| Drops | DropEntry[] | Items dropped when killed |
| Harvestable | bool | Can be harvested without killing |
| HarvestToolId | string | Tool required for harvest |
| HarvestResultId | string | Item produced by harvest |
| HarvestCooldownHours | float | Hours between harvests |
| TameItemId | string | Item that tames this mob |
| PurchasableFromNPCId | string | NPC that sells this mob |
| PurchasePrice | int | Cost to buy |

### MobBehavior Enum (Predefined - AI Cannot Extend)

| Behavior | Description |
|----------|-------------|
| PassiveWander | Moves randomly, ignores player |
| PassiveFlee | Runs away when player approaches |
| Grazing | Wanders slowly, stops to eat, stays in zone |
| Hostile | Chases and attacks player on sight |
| Territorial | Attacks only if player enters radius |
| Nocturnal | Only active at Night phase |
| Pack | Groups with same type, attacks together |
| PetFollow | Follows player when tamed |

## 3. Requirements

- Must support all behaviors defined in GDD 15.1
- Must spawn mobs based on zone, weather, season, and weight
- Must support drop tables with weighted chances
- Must support harvestable mobs (cow → milk, chicken → egg)
- Must support taming mechanic (item converts mob to pet)
- Must support purchase from NPCs
- Must integrate with combat system (when implemented)

## 4. Limitations

- **Fixed behavior set** - AI can only select from predefined MobBehavior enum values. New behavior logic requires code changes.
- **No custom AI** - Mob pathfinding, attack patterns, and reactions are hardcoded per behavior type.
- **No breeding** - Mobs don't reproduce. New mobs come from spawning or purchase.
- **Single drop table** - Can't have different drops for different kill methods.
- **No mob stats** - No HP, attack, defense. Combat (when implemented) will be simple.

## 5. Modularity

### This System Owns
- Mob definitions (MobData ScriptableObjects)
- Mob registry (MobRegistry singleton)
- Mob spawning rules
- Behavior assignments

### This System References
- Zone, Weather, Season (enums) - for spawn conditions
- ItemData (by ID) - for Drops, HarvestResult, TameItem
- NPCData (by ID) - for PurchasableFrom

### Other Systems Reference This
- **Spawning Manager** (future) - uses MobData to spawn instances
- **Combat System** (future) - uses mob behavior for AI
- **Quest System** - hunt quests reference mob IDs

## 6. AI Integration Points

### What AI Can Generate
- New mob types with existing behaviors
- Custom drop tables
- New taming items (generates ItemData too)
- New harvestable products

### What AI Cannot Generate
- New behavior types (requires code)
- Custom AI logic
- New combat mechanics

### Validation Requirements
- Behavior must be valid MobBehavior enum
- SpawnZones must contain valid Zone values
- Drops must reference valid/pending ItemData IDs
- HarvestResultId must reference valid ItemData ID
- PurchasableFromNPCId must reference valid NPCData ID

### Example AI Generation

Prompt: "Add cows"

Generated:
- MobData: Cow (Grazing, Farm zone, purchasable)
- ItemData: Milk (product), Milk Bucket (tool), Steak (product)
- Cow.HarvestToolId = "item_milk_bucket"
- Cow.HarvestResultId = "item_milk"
- Cow.Drops = [Steak, Leather]

## 7. Example Content

```csharp
MobData chicken = new MobData {
    Id = "mob_chicken",
    Name = "Chicken",
    Behavior = MobBehavior.Grazing,
    SpawnZones = new[] { Zone.Farm },
    SpawnWeight = 1f,
    SpawnWeather = new[] { Weather.Any },
    SpawnSeasons = new[] { Season.Spring, Season.Summer, Season.Fall, Season.Winter },
    Drops = new[] { new DropEntry("item_feather", 0.5f, 1, 2) },
    Harvestable = true,
    HarvestResultId = "item_egg",
    HarvestCooldownHours = 24f,
    PurchasableFromNPCId = "npc_farmer_jane",
    PurchasePrice = 100
};
```

## 8. Edge Cases & Considerations

### Harvest Timing
HarvestCooldownHours is real game time. Need to track last harvest timestamp per mob instance.

### Mob Persistence
Farm animals should persist across days. Wild mobs may despawn/respawn. Need spawn instance management.

### Taming
When tamed (TameItemId given), mob switches to PetFollow behavior and belongs to player. Need ownership tracking.

### Nocturnal Mobs
Nocturnal mobs only spawn/activate during Night phase. During day, they either despawn or hide/sleep.

### Pack Behavior
Pack mobs need group tracking. When one is attacked, nearby same-type mobs become aggressive.

### Performance
Limit active mob instances per zone. Use pooling for spawning. Disable AI for off-screen mobs.
