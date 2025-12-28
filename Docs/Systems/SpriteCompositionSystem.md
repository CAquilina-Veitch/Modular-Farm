# Sprite Composition System Design Document

## 1. Purpose

The Sprite Composition System enables modular, data-driven sprite creation. Instead of creating individual sprites for every item/NPC/entity combination, artists create reusable sprite parts that are combined at runtime or in the editor.

Key benefits:
- **Combinatorial explosion**: 10 handles × 10 heads = 100 tools from 20 sprites
- **Tinting**: Same grayscale part with different tint colors = multiple material variants
- **AI-friendly**: AI can describe composite sprites using part IDs and colors
- **Consistency**: Shared parts ensure visual cohesion

## 2. Data Schema

### SpritePartType (Enum)
Categories of sprite parts:

| Category | Values | Use Cases |
|----------|--------|-----------|
| **Containers** | Box, Cylinder, Sphere, RoundedRect, Jar, Bottle | Buckets, barrels, backpacks, potions |
| **Openings** | OpenTop, Lid, Cork, Spout, Nozzle | What goes on top of containers |
| **Attachments** | HandleTop, HandleSide, Strap, Rope, Spigot | How containers are held/accessed |
| **Tools** | Handle, ToolHead, Blade, Guard, Pommel | Pickaxes, swords, hoes |
| **Stations** | StationBase, StationAccent, DisplaySlot | Workbench, furnace, anvil |
| **Plants** | PlantBase, PlantTop | Crops at various growth stages |
| **Machines** | Gear, Pipe, Valve, Meter, Exhaust, Belt | Automation, steampunk |
| **Furniture** | Leg, Surface, Back, Frame, Fabric | Chairs, tables, beds |
| **Magic** | Aura, Glow, Rune | Overlay effects |

### TintColors (Struct)
Color palette with presets:

| Preset | Primary Color | Notes |
|--------|---------------|-------|
| White | #FFFFFF | No tinting |
| Wood | Brown | Organic materials |
| Iron | Silver-gray | Basic metal |
| Gold | Gold | Precious metal + glow |
| Copper | Copper | Mid-tier metal |
| Stone | Gray | Rock, concrete |
| Bone | Off-white | Skeletal, ivory |
| Bronze | Bronze | Alloy |
| Silver | Silver + glow | Precious metal |
| Crystal | Light blue + glow | Magical crystals |
| Emerald | Green + glow | Gem |
| Ruby | Red + glow | Gem |
| Sapphire | Blue + glow | Gem |
| Leather | Tan | Hide materials |
| Cloth | Beige | Fabric materials |

### SpritePartData (ScriptableObject)
Individual reusable sprite component.

| Field | Type | Description |
|-------|------|-------------|
| Id | string | Unique identifier |
| Name | string | Display name |
| Type | SpritePartType | Category |
| Sprite | Sprite | The actual sprite asset |
| PivotOffset | Vector2Int | Offset from center for alignment |
| Tintable | bool | Whether tint colors apply |
| DefaultTintChannel | TintChannel | Which channel to use by default |
| Tags | string[] | Filtering tags |

### CompositeSpriteDef (ScriptableObject)
Recipe for combining parts into a final sprite.

| Field | Type | Description |
|-------|------|-------------|
| Id | string | Unique identifier |
| Name | string | Display name |
| CanvasSize | Vector2Int | Output texture dimensions |
| PixelsPerUnit | float | Sprite PPU |
| Layers | SpritePartLayer[] | Ordered layers |
| DefaultTints | TintColors | Default color palette |
| Variants | TintVariant[] | Named color presets |

### SpritePartLayer (Struct)
Single layer within a composite.

| Field | Type | Description |
|-------|------|-------------|
| PartId | string | Reference to SpritePartData |
| Offset | Vector2Int | Position offset from center |
| SortOrder | int | Render order (higher = on top) |
| TintChannel | TintChannel | Which color channel affects this |
| FlipX/FlipY | bool | Horizontal/vertical flip |
| Rotation | int | 0, 90, 180, or 270 degrees |

### SlotPosition (Struct)
For station display slots.

| Field | Type | Description |
|-------|------|-------------|
| Offset | Vector2Int | Position offset from center |
| Scale | float | Scale factor (0.5 = half size) |

## 3. Requirements

- Compose sprites at runtime without frame drops
- Support arbitrary layer counts
- Handle missing parts gracefully (warning, not crash)
- Support 90-degree rotation increments
- Support horizontal and vertical flipping
- Apply tint colors via multiplication
- Use alpha blending for transparency

## 4. Limitations

- **Rotation limited to 90° increments**: No arbitrary angles
- **No scaling per-layer**: All parts must be authored at target size
- **No shaders**: Pure CPU pixel manipulation
- **No animation**: Static sprites only
- **Grayscale parts for tinting**: Colored parts tint unpredictably

## 5. Modularity

### This System Owns
- SpritePartType enum
- TintChannel enum
- TintColors, SpritePartLayer, SlotPosition structs
- SpritePartData, CompositeSpriteDef ScriptableObjects
- SpriteCompositor static class
- SpritePartRegistrar MonoBehaviour
- SpritePartRegistry, CompositeSpriteRegistry
- SpriteCompositorWindow editor tool

### Other Systems Reference This
- **ItemData**: May reference CompositeSpriteDef for generated item sprites
- **NPCData**: Character sprite composition
- **EntityData**: Entity sprite composition
- **MobData**: Creature sprite composition
- **CraftingStationData**: Station sprite with display slots

## 6. AI Integration Points

AI can generate content using this system by:

1. **Referencing existing parts**:
```json
{
  "sprite": {
    "layers": [
      { "partId": "wooden_handle", "sortOrder": 0, "tintChannel": "Secondary" },
      { "partId": "pickaxe_head", "sortOrder": 1, "tintChannel": "Primary" }
    ],
    "tints": { "primary": "#B0B0C0", "secondary": "#996633" }
  }
}
```

2. **Using container system**:
```json
{
  "sprite": {
    "layers": [
      { "partId": "cylinder_base", "sortOrder": 0 },
      { "partId": "open_top", "sortOrder": 1 },
      { "partId": "handle_top", "sortOrder": 2 }
    ],
    "tints": { "primary": "#707580" }
  }
}
```

## 7. Composition Examples

### Container: Bucket
```
Layer 0: Cylinder (base shape)
Layer 1: OpenTop (opening)
Layer 2: HandleTop (attachment)
Tint: Primary = Iron gray
```

### Container: Watering Can
```
Layer 0: Cylinder (base shape)
Layer 1: Spout (opening)
Layer 2: HandleTop (attachment)
Tint: Primary = Copper
```

### Tool: Iron Pickaxe
```
Layer 0: Handle (Secondary = Wood brown)
Layer 1: ToolHead (Primary = Iron gray)
```

### Station: Forge
```
Layer 0: StationBase - anvil
Layer 1: StationAccent - fire glow
Layer 2: DisplaySlot @ (-8, 8) - hammer
Layer 3: DisplaySlot @ (0, 10) - ore
Layer 4: DisplaySlot @ (8, 8) - sword
```

## 8. Editor Tool: Sprite Compositor Window

Access via: **Prompt Harvest > Sprite Compositor**

### Features
- **Left Panel**: Layer list with add/remove/reorder
- **Center Panel**: Preview with zoom and canvas size controls
- **Right Panel**: Part picker with search/filter, tint color presets
- **Bottom Panel**: Save as asset, export to PNG

### Workflow
1. Open window
2. Add layers, assign parts from picker
3. Adjust positions, rotations, tint channels
4. Select tint preset or custom colors
5. Preview result in real-time
6. Save as CompositeSpriteDef or export PNG

## 9. Edge Cases & Considerations

### Performance
- Cache composed textures when possible
- Don't recompose every frame
- Consider texture atlasing for frequently-used composites

### Missing Parts
- Log warning with part ID
- Skip layer, don't crash

### Texture Read Access
- Source sprites must have Read/Write enabled in import settings

### Part Count Estimates
- ~6 container bases
- ~6 openings
- ~6 attachments
- ~8 tool/weapon parts
- ~6 station parts
- ~6 plant parts
- ~10 machine parts
- ~8 furniture parts
- ~4 magic overlays
- **Total: ~60-80 base parts for broad coverage**
