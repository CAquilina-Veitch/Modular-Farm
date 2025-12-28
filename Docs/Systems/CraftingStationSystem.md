# Crafting Station System Design Document

## 1. Purpose

The Crafting Station System defines placeable objects where crafting occurs. Stations enable specific recipe types - Furnace for smelting, Workbench for general crafting, Anvil for tool upgrades. AI can generate new station types (Brewing Barrel, Loom, Mechanic's Bench) when new crafting categories are added.

## 2. Data Schema

**ScriptableObject:** `CraftingStationData` (Assets/Runtime/Data/ScriptableObjects/CraftingStationData.cs)

### Key Fields

| Field | Type | Purpose |
|-------|------|---------|
| Id | string | Unique identifier |
| Name | string | Display name |
| StationType | StationType | Category of station |
| RecipesEnabled | string[] | Recipe IDs this station can perform |
| PlaceableZones | Zone[] | Where station can be placed |
| SizeTiles | TileSize | Footprint in tiles |
| CraftCost | RecipeIngredient[] | Materials to build this station |

### StationType Enum

| Type | Description |
|------|-------------|
| Workbench | Basic crafting, furniture |
| Furnace | Smelting ores into bars |
| Anvil | Tool crafting and upgrades |
| Loom | Clothes and fabric |
| Brewing | Alcohol and potions |
| Custom | AI-generated station types |

## 3. Requirements

- Must be placeable on farm/house tiles
- Must enable specific recipes when player is nearby
- Must be craftable from materials
- Must support arbitrary size footprints
- Must integrate with Recipe System

## 4. Limitations

- **Fixed station types** - Only 6 types. New types require StationType.Custom.
- **No upgrade paths** - Can't upgrade Furnace to Super Furnace.
- **No fuel consumption** - Stations don't consume fuel (unlike Entities).
- **No durability** - Stations don't break.
- **Player must be adjacent** - No remote crafting.

## 5. Modularity

### This System Owns
- Station definitions (CraftingStationData ScriptableObjects)
- Station registry (CraftingStationRegistry singleton)
- Placement validation
- Recipe enabling

### This System References
- Zone (enum) - PlaceableZones
- RecipeData (by ID) - RecipesEnabled
- ItemData (by ID) - CraftCost materials

### Other Systems Reference This
- **RecipeData** - StationRequiredId references stations
- **Crafting UI** (future) - lists recipes at current station
- **Placement System** (future) - handles station placement

## 6. AI Integration Points

### What AI Can Generate
- New station types for new crafting categories
- Complete crafting chains (station + recipes + materials)

### Example Generation

Prompt: "Add clothing"

AI generates:
- CraftingStationData: Loom (StationType.Loom)
- ItemData: Cloth, Thread, Wool, Cotton
- CropData: Cotton Plant
- MobData: Sheep (drops Wool)
- RecipeData: Spin Wool → Thread, Weave Thread → Cloth
- RecipeData: Craft Shirt, Craft Pants
- ItemData: Shirt, Pants (equipment)

### Validation Requirements
- RecipesEnabled must reference valid RecipeData IDs
- CraftCost ItemIds must reference valid ItemData
- PlaceableZones must be valid Zone values
- SizeTiles must be positive

## 7. Example Content

```csharp
CraftingStationData furnace = new CraftingStationData {
    Id = "station_furnace",
    Name = "Furnace",
    StationType = StationType.Furnace,
    RecipesEnabled = new[] { "recipe_smelt_iron_bar" },
    PlaceableZones = new[] { Zone.Farm },
    SizeTiles = new TileSize(1, 1),
    CraftCost = new[] {
        new RecipeIngredient("item_stone", 20),
        new RecipeIngredient("item_clay", 10)
    }
};
```

## 8. Edge Cases & Considerations

### Building as Station
Some buildings can function as stations (Kitchen building for cooking). RecipeData.StationRequiredId can reference either CraftingStationData or BuildingData.

### Recipe Discovery
Currently, all recipes in RecipesEnabled are immediately available. Future: require recipe learning.

### Station Placement
- Check PlaceableZones
- Check SizeTiles fits in available space
- Consume CraftCost materials
- Instantiate station

### Station Removal
Stations can be picked up and re-placed. Return to inventory as item?

### No Fuel
Unlike Entity fuel, stations don't consume resources to operate. Each crafting consumes recipe ingredients only.

### Custom Type
StationType.Custom is catch-all for AI-generated stations. All behavior comes from RecipesEnabled, not from type.
