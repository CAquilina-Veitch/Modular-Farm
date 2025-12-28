# Activity System Design Document

## 1. Purpose

The Activity System enables player-NPC bonding through shared activities. When an NPC follows the player (at Good Friend tier), they can be led to Activity Points and do activities together - fishing at the beach, foraging in the forest, stargazing at night. Activities earn friendship and sometimes item rewards.

## 2. Data Schema

**ScriptableObject:** `ActivityData` (Assets/Runtime/Data/ScriptableObjects/ActivityData.cs)

### Key Fields

| Field | Type | Purpose |
|-------|------|---------|
| Id | string | Unique identifier |
| Name | string | Display name |
| ValidZones | Zone[] | Where activity can be performed |
| ValidPhases | TimePhase[] | When activity is available |
| FriendshipRequired | int | Minimum friendship to do together |
| NPCEnjoyment | NPCEnjoymentEntry[] | How much each NPC enjoys it |
| FriendshipGainBase | int | Base friendship earned |
| AdvancesTimePhase | bool | Does activity advance time? |
| Rewards | ActivityRewardEntry[] | Possible item rewards |

### Supporting Structs

```csharp
struct NPCEnjoymentEntry {
    string NPCId;
    float Multiplier;  // 1.0 = normal, 2.0 = loves it
}

struct ActivityRewardEntry {
    string ItemId;
    float Chance;  // 0.0 to 1.0
}
```

## 3. Requirements

- Must be zone-specific (fishing only at Beach)
- Must be time-phase-specific (stargazing only at Night)
- Must require NPC following player
- Must check friendship requirement
- Must apply NPC enjoyment multiplier to friendship gain
- Must roll for item rewards
- Must optionally advance time phase

## 4. Limitations

- **No mini-games** - Activities are instant or timed, no skill-based gameplay.
- **One NPC at a time** - Can't do activities with multiple followers.
- **Fixed reward pool** - No dynamic rewards based on skill/luck.
- **No activity levels** - Can't improve at activities.
- **Zone-tied** - Activity points are fixed per zone, not placeable.

## 5. Modularity

### This System Owns
- Activity definitions (ActivityData ScriptableObjects)
- Activity registry (ActivityRegistry singleton)
- Activity Point locations (per zone)
- Reward rolling logic

### This System References
- Zone, TimePhase (enums) - validity constraints
- NPCData (by ID) - for enjoyment multipliers
- ItemData (by ID) - for rewards

### Other Systems Reference This
- **NPCData** - ActivityPreferences references activity IDs
- **Zone Manager** - places Activity Points in zones
- **Following System** (future) - checks activity availability

## 6. AI Integration Points

### What AI Can Generate
- New activities with any valid zone/time combinations
- NPC-specific enjoyment modifiers
- Activity-specific rewards

### Example Generation

Prompt: "Add swimming"

AI generates:
- ActivityData: Swimming (Beach, Day only)
- NPCEnjoymentEntry for each relevant NPC
- Possible: ItemData for "Sea Shell" (chance reward)

### Validation Requirements
- ValidZones must contain valid Zone values
- ValidPhases must contain valid TimePhase values
- NPCEnjoyment NPCIds must reference valid NPCData
- Rewards ItemIds must reference valid ItemData
- Chance values between 0.0 and 1.0

## 7. Example Content

```csharp
ActivityData fishing = new ActivityData {
    Id = "activity_fishing",
    Name = "Fishing",
    ValidZones = new[] { Zone.Beach },
    ValidPhases = new[] { TimePhase.Morning, TimePhase.Day },
    FriendshipRequired = 20,
    NPCEnjoyment = new[] {
        new NPCEnjoymentEntry("npc_farmer_jane", 1.5f)
    },
    FriendshipGainBase = 10,
    AdvancesTimePhase = true,
    Rewards = new[] {
        new ActivityRewardEntry("item_fish", 0.8f)
    }
};
```

## 8. Edge Cases & Considerations

### Activity Points
Each zone has predefined Activity Point locations. Player leads follower NPC there.
- Beach: Fishing Spot, Swimming Area, Sunbathing Blanket
- Forest: Foraging Area, Picnic Spot, Stargazing Clearing
- Farm: Farming Together area, Animal Pen

### NPC Enjoyment Calculation
```
FriendshipGain = FriendshipGainBase × NPCEnjoymentMultiplier
```
If NPC not in NPCEnjoyment list, multiplier = 1.0 (default)

### Time Advancement
If AdvancesTimePhase is true:
- Morning → Day
- Day → Night
- Night → Morning (next day)

This is significant - triggers NPC schedule changes, etc.

### Multiple Rewards
Each reward rolls independently. Could get 0, 1, or multiple items.

### Cooldowns
Consider: can same activity be done multiple times per day? Currently no cooldown defined.

### Seasonal Activities
Activities might be season-restricted. Could add Seasons[] field like CropData.
