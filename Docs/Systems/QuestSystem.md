# Quest System Design Document

## 1. Purpose

The Quest System provides structured objectives and rewards. Quests are given by NPCs and involve collecting items, delivering goods, visiting locations, or talking to people. Completing quests earns gold, items, and friendship. Quests drive player engagement and story progression.

## 2. Data Schema

**ScriptableObject:** `QuestData` (Assets/Runtime/Data/ScriptableObjects/QuestData.cs)

### Key Fields

| Field | Type | Purpose |
|-------|------|---------|
| Id | string | Unique identifier |
| Name | string | Display name |
| GiverNPCId | string | NPC who gives this quest |
| Description | string | Quest description for journal |
| QuestType | QuestType | Category of quest |
| Objectives | QuestObjective[] | Steps to complete |
| Rewards | QuestRewards | Completion rewards |
| DeadlineDays | int | Days to complete (0 = no deadline) |
| Repeatable | bool | Can be done again after completion |
| PrerequisiteQuestId | string | Quest that must be done first |

### QuestType Enum

| Type | Description |
|------|-------------|
| Fetch | Collect items and return |
| Deliver | Bring item to specific NPC |
| Gather | Collect items (no return) |
| Talk | Converse with NPC |
| Build | Construct something |
| Hunt | Defeat mobs |
| Visit | Go to a location |

### QuestObjective Structure

```csharp
struct QuestObjective {
    ObjectiveType Type;  // Collect, Deliver, Visit, TalkTo
    string TargetId;     // item_id, npc_id, or zone_id
    int Quantity;        // For Collect objectives
}
```

### QuestRewards Structure

```csharp
struct QuestRewards {
    int Gold;
    ItemReward[] Items;
    FriendshipReward[] Friendship;
}
```

## 3. Requirements

- Must track quest state: Available, Active, Completed, Failed
- Must track objective progress (e.g., 3/5 turnips collected)
- Must support multi-objective quests
- Must support deadline tracking
- Must support repeatable quests (reset after completion)
- Must support prerequisite chains
- Must integrate with Journal UI

## 4. Limitations

- **Fixed objective types** - Only Collect, Deliver, Visit, TalkTo. No custom objectives.
- **No branching quests** - Linear objectives, no choices affecting outcome.
- **No quest stages** - All objectives must be done, order doesn't matter.
- **No partial rewards** - All or nothing on completion.
- **No quest items** - Items for quests are regular inventory items.
- **Single giver** - Each quest has one NPC giver.

## 5. Modularity

### This System Owns
- Quest definitions (QuestData ScriptableObjects)
- Quest registry (QuestRegistry singleton)
- Objective type logic
- Reward distribution

### This System References
- NPCData (by ID) - GiverNPCId, objectives may reference NPCs
- ItemData (by ID) - objectives, rewards reference items
- Zone (enum) - Visit objectives reference zones

### Other Systems Reference This
- **Journal Manager** (future) - displays active/completed quests
- **DialogueSystem** - talking to NPC may offer/complete quests
- **Achievement System** - achievements are like quests but auto-generated

## 6. AI Integration Points

### What AI Can Generate
- New quests with any valid objectives
- Quest chains via PrerequisiteQuestId
- Multi-part fetch quests
- Story quests that integrate new content

### Story Integration Example

Prompt: "Add a mystery in the forest"

AI generates:
- QuestData: "Strange Sounds" (Visit forest at night)
- QuestData: "Following Clues" (Collect mysterious footprints) - prerequisite: above
- QuestData: "The Hidden Grove" (Find secret location) - prerequisite: above
- NPCData: Mysterious Traveler (quest giver)
- BuildingData: Hidden Grove (secret location)

### Validation Requirements
- GiverNPCId must reference valid NPCData
- Objectives targets must reference valid IDs for their type
- PrerequisiteQuestId must reference valid QuestData (no circular refs)
- DeadlineDays >= 0
- Quantity values > 0 for Collect objectives

## 7. Example Content

```csharp
QuestData bringTurnips = new QuestData {
    Id = "quest_bring_turnips",
    Name = "Fresh Produce",
    GiverNPCId = "npc_farmer_jane",
    Description = "Farmer Jane needs some fresh turnips for her stall.",
    QuestType = QuestType.Fetch,
    Objectives = new[] {
        QuestObjective.Collect("item_turnip", 5)
    },
    Rewards = new QuestRewards {
        Gold = 100,
        Items = new[] { new ItemReward("item_turnip_seeds", 10) },
        Friendship = new[] { new FriendshipReward("npc_farmer_jane", 15) }
    },
    DeadlineDays = 0,
    Repeatable = true,
    PrerequisiteQuestId = ""
};
```

## 8. Edge Cases & Considerations

### Quest Availability
Quest appears in dialogue when:
- Player has sufficient friendship with giver (defined per-quest or tier-based?)
- Prerequisite quest (if any) is completed
- Quest is not already active
- Quest is not completed (unless Repeatable)

### Deadline Tracking
If DeadlineDays > 0:
- Track start day when quest accepted
- If current day > start day + DeadlineDays, quest fails
- Failed quests can't be retried (or can with penalty?)

### Item Consumption
Collect objectives: items remain in inventory until turned in
Deliver objectives: items removed when delivered

### Repeatable Reset
When repeatable quest is re-accepted:
- Objective progress resets to 0
- Deadline restarts (if applicable)
- Same rewards given again

### Circular Prerequisites
A → B → A would be invalid. Validator should detect and reject.

### Missing Giver
If GiverNPCId references deleted NPC, quest is orphaned. Should fail validation or become unavailable.
