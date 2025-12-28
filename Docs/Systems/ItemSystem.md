# Item System Design Document

## 1. Purpose

The Item System is the foundation for all physical objects in the game. Items represent tools, materials, products, consumables, seeds, decorations, furniture, and gifts. Nearly every other system references items - they are bought/sold in shops, used in recipes, given as gifts, dropped by mobs, harvested from crops, and used to fuel entities.

## 2. Data Schema

**ScriptableObject:** `ItemData` (Assets/Runtime/Data/ScriptableObjects/ItemData.cs)

### Key Fields

| Field | Type | Purpose |
|-------|------|---------|
| Id | string | Unique identifier (e.g., "item_iron_ore") |
| Name | string | Display name |
| Type | ItemType | Category: Tool, Material, Product, Consumable, Seed, Decoration, Furniture, Gift |
| Value | int | Base sell price in gold |
| StackMax | int | Maximum stack size (default 99) |
| Durability | int | Tool uses before breaking (0 = unbreakable) |
| Tier | string | Quality tier for tools: "basic", "copper", "iron", "gold" |
| UsedOn | string[] | Entity IDs this item can be used on |
| UseResult | string | Item ID produced when used |
| Placeable | bool | Whether item can be placed in the world |
| PlaceableZones | Zone[] | Where item can be placed |
| DecayDays | int | Days until decay (0 = no decay) |
| SpriteHint | string | Tag for AI sprite generation |
| Tags | string[] | Categorization tags |

## 3. Requirements

- Must support all item types defined in GDD 13.1
- Must integrate with InventoryManager for storage/stacking
- Must integrate with EconomyManager for pricing (Value is base, final price modified by friendship, saturation, season)
- Must support both editor-created and AI-generated items
- Must support tool durability and tier progression
- Must support item decay (food spoilage)
- Must support item usage (Milk Bucket on Cow produces Milk)

## 4. Limitations

- **No custom item behaviors** - Item effects are predefined (tools dig, seeds plant, consumables heal). AI cannot create new effect types.
- **No procedural sprites** - Phase 1 uses curated sprite pool; SpriteHint is a tag for matching, not generation.
- **Single inventory slot** - Items don't have sub-inventories. A backpack is equipment that increases InventoryManager slot count.
- **Fixed tier progression** - Tiers are "basic" → "copper" → "iron" → "gold" etc. AI can add new tier names but behavior is standard.

## 5. Modularity

### This System Owns
- Item definitions (ItemData ScriptableObjects)
- Item registry (ItemRegistry singleton)
- Item type categorization
- Tool durability/tier logic

### This System References
- Zone (enum) - for PlaceableZones
- EntityData - via UsedOn field (by ID reference)

### Other Systems Reference This
- **InventoryManager** - stores items by ID
- **EconomyManager** - uses Value for pricing
- **NPCData** - ShopInventory, GiftLikes, GiftDislikes reference items by ID
- **RecipeData** - Inputs and Outputs reference items by ID
- **CropData** - SeedItemId and HarvestItemId reference items by ID
- **MobData** - Drops reference items by ID
- **EntityData** - FuelItemId references item by ID
- **QuestData** - Objectives may reference items by ID

## 6. AI Integration Points

### Entry
AI generates ItemData JSON during dream sequences. JSON is validated and converted to ItemData ScriptableObject at runtime.

### Validation Requirements
- Id must be unique (not already in ItemRegistry)
- Name is required
- Type must be valid ItemType enum value
- Value must be >= 0
- StackMax must be >= 1
- Durability must be >= 0
- If Placeable is true, PlaceableZones should not be empty
- UsedOn should reference valid EntityData IDs
- UseResult should reference valid ItemData ID (or will be created in same batch)

### Registration
After validation, ItemData is registered with ItemRegistry. Can be accessed like any hand-authored item.

## 7. Example Content

```csharp
// Iron Ore - basic mining material
ItemData ironOre = new ItemData {
    Id = "item_iron_ore",
    Name = "Iron Ore",
    Type = ItemType.Material,
    Description = "Raw iron ore from the mines.",
    Value = 15,
    StackMax = 99,
    Durability = 0,
    SpriteHint = "ore_iron",
    Tags = new[] { "material", "ore", "metal" }
};
```

## 8. Edge Cases & Considerations

### Stack Overflow
When adding items, if stack reaches StackMax, excess goes to next available slot. If no slots available, item is dropped or action fails.

### Tool Breaking
When Durability reaches 0, tool is removed from inventory. Player is notified. Consider: should broken tools drop materials?

### Decay
Items with DecayDays > 0 track days since creation. When days >= DecayDays, item converts to "Rotten X" or is destroyed. Refrigeration (future feature) could pause decay.

### Circular References
UseResult could theoretically reference an item that uses this item. Not currently prevented - would just be weird gameplay.

### Performance
ItemRegistry uses Dictionary<string, ItemData> for O(1) lookup. At scale (1000+ items from many prompts), memory usage is minimal (ScriptableObjects are lightweight).

### Future Extensibility
- **Custom effects**: Could add ItemEffectType enum and handler system
- **Equipment slots**: Some items are equippable (head, body, accessory) - Type could be extended
- **Crafting complexity**: Items could have quality ratings affecting recipe outputs
