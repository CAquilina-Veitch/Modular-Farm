# Recipe System Design Document

## 1. Purpose

The Recipe System enables crafting, cooking, smelting, and processing. Recipes transform input items into output items at crafting stations. The system uses "patternless" crafting - correct ingredients automatically produce output, no grid puzzles or specific arrangements required.

## 2. Data Schema

**ScriptableObject:** `RecipeData` (Assets/Runtime/Data/ScriptableObjects/RecipeData.cs)

### Key Fields

| Field | Type | Purpose |
|-------|------|---------|
| Id | string | Unique identifier |
| Name | string | Display name |
| RecipeType | RecipeType | Category of recipe |
| Inputs | RecipeIngredient[] | Required items and quantities |
| Output | RecipeOutput | Resulting item and quantity |
| StationRequiredId | string | Crafting station or building ID |
| TimeHours | float | In-game hours to complete |
| AdvancesTimePhase | bool | Does crafting this advance time? |

### RecipeType Enum

| Type | Description |
|------|-------------|
| Crafting | General crafting at workbench |
| Cooking | Food preparation |
| Processing | Material processing |
| Smelting | Ore → bar at furnace |
| Brewing | Alcohol/potions |

### Supporting Structs

```csharp
struct RecipeIngredient {
    string ItemId;
    int Quantity;
}

struct RecipeOutput {
    string ItemId;
    int Quantity;
}
```

## 3. Requirements

- Must validate player has all required ingredients
- Must validate player is at correct station
- Must consume ingredients and produce output
- Must integrate with InventoryManager for item checks
- Must integrate with TimeManager if AdvancesTimePhase
- Must support multi-ingredient recipes
- Must support multi-output recipes (future: multiple RecipeOutput)

## 4. Limitations

- **Patternless only** - No grid-based crafting patterns
- **Fixed stations** - Recipe bound to one station type, no alternatives
- **No quality tiers** - Output is fixed, no quality variation based on ingredients
- **No partial completion** - Can't pause/resume crafting
- **Single output** - Currently one Output. Multi-output would need array.
- **No discovery** - Recipes known immediately (no learning/research)

## 5. Modularity

### This System Owns
- Recipe definitions (RecipeData ScriptableObjects)
- Recipe registry (RecipeRegistry singleton)
- Crafting validation logic
- Recipe type categorization

### This System References
- ItemData (by ID) - for Inputs and Output
- CraftingStationData or BuildingData (by ID) - StationRequiredId

### Other Systems Reference This
- **CraftingStationData** - RecipesEnabled references recipe IDs
- **Crafting UI** (future) - displays available recipes at station
- **Achievement System** (future) - may track recipes completed

## 6. AI Integration Points

### What AI Can Generate
- New recipes with any valid inputs/outputs
- New recipe chains (ore → bar → tool)
- New crafting stations for new recipe types

### Processing Chain Example

Prompt: "Add brewing"

AI generates:
1. CraftingStationData: Brewing Barrel
2. ItemData: Hops, Wheat, Barley (materials)
3. ItemData: Beer, Ale, Mead (consumables)
4. RecipeData: Brew Beer (Hops + Wheat → Beer)
5. RecipeData: Brew Ale (Barley + Hops → Ale)
6. CropData: Hops plant, Wheat plant, Barley plant

### Validation Requirements
- All Input ItemIds must reference valid ItemData
- Output ItemId must reference valid ItemData
- StationRequiredId must reference valid CraftingStationData or BuildingData
- TimeHours should be >= 0
- Quantity values should be > 0

## 7. Example Content

```csharp
RecipeData smeltIronBar = new RecipeData {
    Id = "recipe_smelt_iron_bar",
    Name = "Smelt Iron Bar",
    RecipeType = RecipeType.Smelting,
    Inputs = new[] {
        new RecipeIngredient("item_iron_ore", 5)
    },
    Output = new RecipeOutput("item_iron_bar", 1),
    StationRequiredId = "station_furnace",
    TimeHours = 1f,
    AdvancesTimePhase = false
};
```

## 8. Edge Cases & Considerations

### Insufficient Ingredients
UI should show recipe but disabled. Attempting to craft fails silently or with feedback.

### Full Inventory
If inventory full when output ready, either:
- Block crafting start
- Drop output on ground
- Queue output until space available

### Station Discovery
Recipe only visible/available when player has access to required station. If station destroyed, recipes inaccessible.

### Time Advancement
If AdvancesTimePhase is true, completing recipe calls TimeManager.AdvancePhase(). This affects NPC schedules, weather, etc.

### Recipe Chains
Some recipes require outputs of other recipes (Iron Ore → Iron Bar → Iron Tool). No explicit chain tracking, just ingredient dependencies.

### Duplicate Recipes
Multiple recipes can produce same output. Player chooses based on available ingredients.

### Performance
Recipe matching: compare player inventory against all known recipes. With 100s of recipes, may need optimization (hash by first ingredient, etc.).
