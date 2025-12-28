# Entity System Design Document

## 1. Purpose

The Entity System manages rideable/usable objects - mounts (horses), vehicles (cars), automators (roombas), and pets. Entities provide speed bonuses, have fuel/feed requirements, and may have special abilities. They occupy a single inventory slot and require specific items to function.

## 2. Data Schema

**ScriptableObject:** `EntityData` (Assets/Runtime/Data/ScriptableObjects/EntityData.cs)

### Key Fields

| Field | Type | Purpose |
|-------|------|---------|
| Id | string | Unique identifier |
| Name | string | Display name |
| EntityType | EntityType | Mount, Vehicle, Automator, Pet |
| SpeedMultiplier | float | Movement speed modifier (1.0 = normal) |
| FuelItemId | string | Item consumed for operation |
| FuelConsumptionRate | float | Fuel consumed per in-game day |
| FuelBonusEffect | string | Bonus when fueled (description) |
| SpecialAbility | string | Unique ability description |
| CraftRecipeId | string | Recipe to craft this entity |
| PurchasableFromNPCId | string | NPC who sells this entity |
| PurchasePrice | int | Cost to buy |

### EntityType Enum

| Type | Description |
|------|-------------|
| Mount | Rideable animals (horse, etc.) |
| Vehicle | Mechanical transport (car, tractor) |
| Automator | Automated helpers (crop harvester) |
| Pet | Companion animals (dog, cat) |

## 3. Requirements

- Must support riding/using entities for movement bonus
- Must track fuel/feed level per entity instance
- Must consume fuel over time when in use
- Must support entity-specific fuel types
- Must support purchasing from NPCs
- Must support crafting entities
- Must integrate with movement/speed system

## 4. Limitations

- **Fixed entity types** - Only Mount, Vehicle, Automator, Pet. No custom types.
- **Single inventory slot** - Entity takes one slot when not deployed.
- **No breeding/reproduction** - Entities don't create new entities.
- **No damage/health** - Entities don't take damage or die.
- **Fixed abilities** - SpecialAbility is description only, code must implement.
- **One active entity** - Can't ride two horses simultaneously.

## 5. Modularity

### This System Owns
- Entity definitions (EntityData ScriptableObjects)
- Entity registry (EntityRegistry singleton)
- Entity instance tracking (fuel levels, deployment state)
- Speed modifier application

### This System References
- ItemData (by ID) - FuelItemId
- RecipeData (by ID) - CraftRecipeId
- NPCData (by ID) - PurchasableFromNPCId

### Other Systems Reference This
- **ItemData** - UsedOn may reference entity IDs
- **Movement Controller** (future) - applies SpeedMultiplier
- **InventoryManager** - stores entity items

## 6. AI Integration Points

### What AI Can Generate
- New entity types with any valid configuration
- Complete entity ecosystem (entity + fuel item + NPC seller)
- Entities requiring crafting chains

### Complex Example

Prompt: "Add cars"

AI generates:
- EntityData: Car (Vehicle, 4x speed)
- ItemData: Gasoline (fuel)
- ItemData: Engine, Wheels, Car Frame (materials)
- CraftingStationData: Mechanic's Bench
- RecipeData: Craft Car parts
- RecipeData: Assemble Car
- NPCData: Mechanic Mike
- BuildingData: Mike's Garage, Gas Station
- Possible: Mining addition for oil → gasoline chain

### Validation Requirements
- EntityType must be valid enum value
- SpeedMultiplier should be > 0
- FuelItemId must reference valid ItemData
- CraftRecipeId must reference valid RecipeData (or empty)
- PurchasableFromNPCId must reference valid NPCData (or empty)

## 7. Example Content

```csharp
EntityData horse = new EntityData {
    Id = "entity_basic_horse",
    Name = "Horse",
    EntityType = EntityType.Mount,
    SpeedMultiplier = 2f,
    FuelItemId = "item_hay",
    FuelConsumptionRate = 1f,  // 1 hay per day
    FuelBonusEffect = "2.5x speed for 1 day when fed",
    SpecialAbility = "",
    CraftRecipeId = "",
    PurchasableFromNPCId = "",
    PurchasePrice = 500
};
```

## 8. Edge Cases & Considerations

### Entity Instance Data
Each owned entity needs instance tracking:
- Current fuel level
- Deployed/stored state
- Location (if deployed in world)

### Fuel Mechanics
- FuelConsumptionRate = fuel consumed per in-game day while active
- At 0 fuel: entity still works but no FuelBonusEffect
- Or: at 0 fuel, entity refuses to move?

### Mounting/Dismounting
- Player interacts with deployed entity to mount
- While mounted, player speed = base × SpeedMultiplier
- Some areas may disallow mounts (indoors)

### Automator Behavior
Automators have SpeedMultiplier = 0 (don't move player) but have SpecialAbility that does work:
- "Roomba: Auto-harvests crops on farm"
- Implementation requires code, not just data

### Pet Behavior
Pets follow player, provide no speed bonus, may have:
- Combat assistance
- Item finding
- Morale boost (future happiness system)

### Vehicle Fuel
Vehicles might use continuous fuel (gas per distance) vs mounts (feed per day). Current model is per-day; might need distinction.

### Multiple Entities
Player can own multiple entities but only use one at a time. Others stay in inventory or stable/garage.
