# NPC System Design Document

## 1. Purpose

The NPC System is the heart of social gameplay. NPCs are villagers who have schedules, run shops, offer quests, give gifts, and form relationships with the player. Every NPC has ALL of these features - there are no "background" NPCs. This is the most complex content type in the game.

## 2. Data Schema

**ScriptableObject:** `NPCData` (Assets/Runtime/Data/ScriptableObjects/NPCData.cs)

### Key Fields

| Field | Type | Purpose |
|-------|------|---------|
| Id | string | Unique identifier |
| Name | string | Display name |
| HomeBuildingId | string | Building where NPC lives |
| Schedule | ScheduleEntry[] | Where NPC is each day/phase (21 entries) |
| ShopInventory | string[] | Item IDs this NPC sells |
| ShopSellPrices | ShopPriceEntry[] | Price multipliers for selling |
| BuyPreferences | BuyPreferenceEntry[] | What NPC buys and at what price |
| GiftLikes | string[] | Items NPC likes receiving |
| GiftDislikes | string[] | Items NPC dislikes receiving |
| ActivityPreferences | ActivityPreferenceEntry[] | How much NPC enjoys activities |
| DialogueTags | string[] | Personality for dialogue generation |
| WeatherBehavior | WeatherBehaviorEntry[] | How NPC responds to weather |

### ScheduleEntry Structure

```csharp
struct ScheduleEntry {
    DayOfWeek Day;      // Monday-Sunday
    TimePhase Phase;    // Morning, Day, Night
    Zone Location;      // Where NPC is
    NPCActivity Activity; // What they're doing
}
```

NPCActivity is predefined: Sleeping, Working, Walking, Idle, Gathering, Socializing

## 3. Requirements

- Must support 21-slot schedules (7 days × 3 phases)
- Must support dual-direction shops (NPC sells to player AND buys from player)
- Must track friendship points per-NPC (handled by RelationshipManager)
- Must support gift giving with like/dislike reactions
- Must support weather-based schedule overrides
- Must support NPC following (at Good Friend tier)
- Must support activities when following

## 4. Limitations

- **Fixed activity types** - NPCActivity enum cannot be extended by AI
- **No dynamic schedules** - Schedule is fixed 21 entries, no conditional routing
- **No NPC-to-NPC relationships** - NPCs don't have friendships with each other
- **No NPC inventory persistence** - Shop inventory doesn't deplete
- **No NPC aging/lifecycle** - NPCs don't grow old, have children, etc.

## 5. Modularity

### This System Owns
- NPC definitions (NPCData ScriptableObjects)
- NPC registry (NPCRegistry singleton)
- Schedule definitions
- Shop configurations
- Gift preferences

### This System References
- BuildingData (by ID) - HomeBuildingId
- ItemData (by ID) - ShopInventory, GiftLikes, GiftDislikes
- Zone, TimePhase, DayOfWeek, NPCActivity, Weather (enums)
- ActivityData (by ID) - ActivityPreferences

### Other Systems Reference This
- **RelationshipManager** - tracks friendship points per NPC
- **QuestData** - GiverNPCId, objectives may involve NPCs
- **BuildingData** - OwnerNPCId
- **MobData** - PurchasableFromNPCId
- **EntityData** - PurchasableFromNPCId

## 6. AI Integration Points

### What AI Can Generate
- New NPCs with any combination of existing features
- New shops (any items in inventory)
- New gift preferences
- New schedules using existing zones/activities
- New dialogue personalities (tags)

### What AI Cannot Generate
- New activity types (Sleeping, Working, etc. are fixed)
- New schedule logic (always 21 fixed entries)
- NPC-specific dialogue responses (just tags, not full scripts)

### Cascade Generation Example

Prompt: "Add a blacksmith"

AI generates:
- NPCData: Blacksmith Bob
- BuildingData: Bob's Forge (new building for HomeBuildingId)
- ItemData: Various metal items for ShopInventory
- RecipeData: Recipes using Bob's Forge as station
- QuestData: Quest from Bob about finding rare ore

### Validation Requirements
- HomeBuildingId must reference valid BuildingData (or be pending in same batch)
- Schedule must have exactly 21 entries covering all day/phase combinations
- ShopInventory items must reference valid ItemData IDs
- GiftLikes/GiftDislikes must reference valid ItemData IDs

## 7. Example Content

```csharp
NPCData jane = new NPCData {
    Id = "npc_farmer_jane",
    Name = "Farmer Jane",
    HomeBuildingId = "building_janes_homestead",
    Schedule = GenerateDefaultSchedule(Zone.Village, Zone.Farm),
    ShopInventory = new[] { "item_turnip_seeds", "item_hay" },
    ShopSellPrices = new[] {
        new ShopPriceEntry("item_turnip_seeds", 1.0f)
    },
    BuyPreferences = new[] {
        new BuyPreferenceEntry("item_turnip", 20)
    },
    GiftLikes = new[] { "item_turnip" },
    GiftDislikes = new[] { "item_iron_ore" },
    DialogueTags = new[] { "friendly", "hardworking" },
    WeatherBehavior = new[] {
        WeatherBehaviorEntry.StaysIndoorsDuring(Weather.Rain)
    }
};
```

## 8. Edge Cases & Considerations

### Schedule Weather Override
When Weather matches WeatherBehavior with StaysIndoors, NPC's Location changes to home regardless of schedule.

### Following Mechanics
At Good Friend tier (60 points), player can ask NPC to follow. Following NPC ignores schedule until dismissed.

### Shop Hours
ShopInventory only accessible when NPC's current Activity is Working. Otherwise, just conversation.

### Gift Cooldown
One gift per day per NPC. Multiple gifts don't stack friendship.

### Missing References
If HomeBuildingId references non-existent building, NPC has no "home" location. Should fail validation or use fallback.

### Performance
NPCs need pathfinding between schedule locations. Consider: do all NPCs need active AI, or just those in player's current zone?
