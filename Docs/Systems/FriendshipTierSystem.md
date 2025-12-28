# Friendship Tier System Design Document

## 1. Purpose

The Friendship Tier System defines relationship progression milestones. As players earn friendship points with NPCs, they unlock tiers with benefits: better shop prices, home entry permission, NPC following, and special dialogue. AI can generate custom tiers (e.g., "Partner" for marriage at 110 points).

## 2. Data Schema

**ScriptableObject:** `FriendshipTierData` (Assets/Runtime/Data/ScriptableObjects/FriendshipTierData.cs)

### Key Fields

| Field | Type | Purpose |
|-------|------|---------|
| Id | string | Unique identifier |
| Name | string | Display name (Stranger, Friend, etc.) |
| PointsRequired | int | Minimum points to reach this tier |
| Unlocks | TierUnlock[] | What becomes available |

### TierUnlock Structure

```csharp
struct TierUnlock {
    UnlockType Type;
    bool BoolValue;      // For CanFollow, CanEnterHome, FreeShop
    float FloatValue;    // For PriceModifier
    string StringValue;  // For DialogueOption, Activity, NPCMovesTo
}
```

### UnlockType Enum

| Type | Description |
|------|-------------|
| CanFollow | NPC can follow player |
| CanEnterHome | Player can enter NPC's home |
| PriceModifier | Shop price discount/markup |
| DialogueOption | New dialogue becomes available |
| Activity | New activity unlocked |
| NPCMovesTo | NPC relocates to zone |
| FreeShop | NPC's shop items become free |

## 3. Requirements

- Must define clear progression thresholds
- Must support multiple unlocks per tier
- Must integrate with RelationshipManager for tier calculation
- Must be extensible for AI-generated tiers
- Must apply unlocks automatically when tier is reached

## 4. Limitations

- **Global tiers** - Same tier definitions for all NPCs (can't have different tiers per NPC).
- **No tier loss** - Once reached, tier can't be lost (even if points drop).
- **No tier-specific dialogue** - DialogueTags are on NPCData, not tiers.
- **Linear progression** - Tiers are point-based only, no other requirements.
- **No tier events** - No celebration/notification when tier is reached (just unlocks apply).

## 5. Modularity

### This System Owns
- Tier definitions (FriendshipTierData ScriptableObjects)
- Tier registry (FriendshipTierRegistry singleton)
- Tier calculation (points → current tier)
- Unlock application

### This System References
- ActivityData (by ID) - for Activity unlocks
- DialogueOptionData (by ID) - for DialogueOption unlocks
- Zone (enum) - for NPCMovesTo unlocks

### Other Systems Reference This
- **RelationshipManager** - calculates current tier per NPC
- **Shop System** (future) - applies PriceModifier
- **Building entry** - checks CanEnterHome
- **Following System** - checks CanFollow

## 6. AI Integration Points

### What AI Can Generate
- New tiers at any point threshold
- Custom unlocks for relationship milestones
- Special relationship types (Partner, Rival, Mentor)

### Example Generation

Prompt: "Add marriage"

AI generates:
- FriendshipTierData: Partner (110 points)
  - Unlocks: FreeShop, NPCMovesTo("farm")
- DialogueOptionData: Proposal dialogue
- ItemData: Wedding Ring
- Building modifications for spouse room

### Validation Requirements
- PointsRequired >= 0
- No duplicate PointsRequired across tiers
- StringValue references in unlocks must point to valid content

## 7. Example Content

### Base Game Tiers (GDD 5.3)

```csharp
// Stranger - 0 points
FriendshipTierData stranger = new FriendshipTierData {
    Id = "tier_stranger",
    Name = "Stranger",
    PointsRequired = 0,
    Unlocks = new TierUnlock[] { }  // Basic dialogue, can buy from shop
};

// Acquaintance - 20 points
FriendshipTierData acquaintance = new FriendshipTierData {
    Id = "tier_acquaintance",
    Name = "Acquaintance",
    PointsRequired = 20,
    Unlocks = new[] { TierUnlock.PriceModifier(0.95f) }  // 5% discount
};

// Friend - 40 points
FriendshipTierData friend = new FriendshipTierData {
    Id = "tier_friend",
    Name = "Friend",
    PointsRequired = 40,
    Unlocks = new[] {
        TierUnlock.CanEnterHome(),
        TierUnlock.PriceModifier(0.9f)  // 10% discount
    }
};

// Good Friend - 60 points
FriendshipTierData goodFriend = new FriendshipTierData {
    Id = "tier_good_friend",
    Name = "Good Friend",
    PointsRequired = 60,
    Unlocks = new[] {
        TierUnlock.CanFollow(),
        TierUnlock.PriceModifier(0.85f)
    }
};

// Best Friend - 80 points
FriendshipTierData bestFriend = new FriendshipTierData {
    Id = "tier_best_friend",
    Name = "Best Friend",
    PointsRequired = 80,
    Unlocks = new[] {
        TierUnlock.PriceModifier(0.8f)  // 20% discount
    }
};
```

## 8. Edge Cases & Considerations

### Tier Calculation
Given X friendship points with NPC, current tier = highest tier where PointsRequired <= X.

```csharp
FriendshipTierData GetTier(int points) {
    return AllTiers
        .Where(t => t.PointsRequired <= points)
        .OrderByDescending(t => t.PointsRequired)
        .First();
}
```

### Cumulative Unlocks
Unlocks are cumulative. At Good Friend, player has all Friend and Acquaintance unlocks too.

### Price Modifier Stacking
Multiple PriceModifier unlocks: use the best (lowest) one, don't multiply.

### NPC Moves
NPCMovesTo changes NPC's schedule permanently. Home location changes.

### FreeShop
All items from NPC's shop are free. Doesn't affect BuyPreferences (NPC still pays for player's items).

### Beyond 100
AI can generate tiers above 100 points (e.g., Partner at 110). Points accumulate infinitely.

### Tier Names
Tier names are for display. Code uses PointsRequired for logic.
