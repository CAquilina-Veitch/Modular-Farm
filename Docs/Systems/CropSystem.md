# Crop System Design Document

## 1. Purpose

The Crop System manages plantable, growable vegetation on the farm. Crops are planted from seeds, require watering, grow over days through visual stages, and produce harvest items. Some crops regrow after harvest. Crops are seasonal - planting out of season results in dead plants.

## 2. Data Schema

**ScriptableObject:** `CropData` (Assets/Runtime/Data/ScriptableObjects/CropData.cs)

### Key Fields

| Field | Type | Purpose |
|-------|------|---------|
| Id | string | Unique identifier |
| Name | string | Display name |
| SeedItemId | string | Item ID of the seed that plants this |
| GrowDays | int | Days from planting to harvest |
| WaterNeeds | WaterNeeds | Watering requirements |
| Seasons | Season[] | Valid growing seasons |
| GrowthStages | int | Number of visual stages |
| HarvestItemId | string | Item ID produced when harvested |
| HarvestQuantity | HarvestQuantity | Min/max harvest amount |
| Regrows | bool | Does crop regrow after harvest? |
| RegrowDays | int | Days to regrow (if Regrows) |
| SpriteHints | string[] | Sprite tag per growth stage |

### WaterNeeds Enum

| Value | Description |
|-------|-------------|
| None | Never needs watering |
| Daily | Must water every day |
| EveryOtherDay | Must water at least every other day |

## 3. Requirements

- Must track growth progress per planted crop instance
- Must check watering status and apply growth penalties
- Must check season and kill crops planted in wrong season
- Must support multi-harvest crops (tomatoes, peppers)
- Must support variable harvest quantities
- Must integrate with TimeManager for day progression
- Must integrate with WeatherManager (rain = auto-watered)

## 4. Limitations

- **Fixed watering options** - Only None, Daily, EveryOtherDay. No custom watering logic.
- **No fertilizer** - No quality modifiers from soil/fertilizer.
- **No disease/pests** - Crops don't get sick or infested.
- **Grid-only placement** - Crops only on tilled farm tiles, no freeform.
- **No multi-tile crops** - Each crop is 1 tile. Giant crops would need special handling.

## 5. Modularity

### This System Owns
- Crop definitions (CropData ScriptableObjects)
- Crop registry (CropRegistry singleton)
- Growth stage tracking
- Watering/harvest mechanics

### This System References
- ItemData (by ID) - SeedItemId, HarvestItemId
- Season (enum) - valid growing seasons
- WaterNeeds (enum) - watering requirements

### Other Systems Reference This
- **Farm Manager** (future) - tracks planted crops per tile
- **TimeManager** - triggers daily growth checks
- **WeatherManager** - rain triggers auto-watering
- **InventoryManager** - seeds consumed, harvest added

## 6. AI Integration Points

### What AI Can Generate
- New crop types with any valid configuration
- New seed items (generates ItemData)
- New harvest products (generates ItemData)
- Multi-harvest regrowable crops
- Multi-season crops

### Cascade Generation Example

Prompt: "Add grapes for wine"

AI generates:
- ItemData: Grape Seeds (seed)
- ItemData: Grapes (product)
- CropData: Grape Vine (summer, regrows, 7-day growth)
- RecipeData: Ferment Grapes → Wine
- CraftingStationData: Wine Barrel
- ItemData: Wine (consumable)

### Validation Requirements
- SeedItemId must reference valid ItemData with Type = Seed
- HarvestItemId must reference valid ItemData
- GrowDays must be > 0
- GrowthStages should match SpriteHints length
- If Regrows is true, RegrowDays should be > 0
- HarvestQuantity.Min <= HarvestQuantity.Max

## 7. Example Content

```csharp
CropData turnip = new CropData {
    Id = "crop_turnip",
    Name = "Turnip",
    SeedItemId = "item_turnip_seeds",
    GrowDays = 4,
    WaterNeeds = WaterNeeds.Daily,
    Seasons = new[] { Season.Spring },
    GrowthStages = 4,
    HarvestItemId = "item_turnip",
    HarvestQuantity = new HarvestQuantity(1, 3),
    Regrows = false,
    RegrowDays = 0,
    SpriteHints = new[] {
        "crop_turnip_stage1",
        "crop_turnip_stage2",
        "crop_turnip_stage3",
        "crop_turnip_mature"
    }
};
```

## 8. Edge Cases & Considerations

### Season Change Mid-Growth
If season changes while crop is growing and new season isn't in Seasons[], crop should wilt/die.

### Missed Watering
If Daily watering is missed:
- First miss: growth paused
- Second consecutive miss: crop wilts (stunted harvest?)
- Third miss: crop dies

### Rain as Watering
Rain weather counts as watering for that day. Crops with WaterNeeds.Daily are satisfied.

### Regrowable Harvest
After harvesting a regrowable crop:
1. Reset growth progress (but not to stage 0 - keep "base" plant)
2. After RegrowDays, ready to harvest again
3. Regrowable crops may have different sprites for "harvested but alive"

### Variable Harvest
Random between HarvestQuantity.Min and Max. Could be affected by future quality modifiers.

### Crop Memory
Each planted tile needs instance data:
- Which CropData
- Current growth day
- Last watered day
- Times harvested (for regrowables)
