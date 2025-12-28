# Dialogue System Design Document

## 1. Purpose

The Dialogue System manages special conversation options beyond basic NPC chat. Dialogue options have conditions (friendship level, item in inventory, quest completion) and effects (start quest, give item, change friendship). This enables meaningful choices in conversations.

## 2. Data Schema

**ScriptableObject:** `DialogueOptionData` (Assets/Runtime/Data/ScriptableObjects/DialogueOptionData.cs)

### Key Fields

| Field | Type | Purpose |
|-------|------|---------|
| Id | string | Unique identifier |
| Text | string | What player says |
| Conditions | DialogueCondition[] | Requirements to show option |
| Effects | DialogueEffect[] | What happens when chosen |
| ResponseDialogue | string | NPC's response |

### DialogueCondition Structure

```csharp
struct DialogueCondition {
    ConditionType Type;  // Friendship, Item, QuestComplete
    string TargetId;     // npc_id, item_id, or quest_id
    int MinValue;        // For friendship check
}
```

### DialogueEffect Structure

```csharp
struct DialogueEffect {
    EffectType Type;     // StartQuest, GiveItem, ChangeFriendship, etc.
    string TargetId;     // quest_id, item_id, npc_id, event_id
    int IntValue;        // For friendship change amount
}
```

### ConditionType Enum

| Type | Description |
|------|-------------|
| Friendship | Requires min friendship with NPC |
| Item | Requires item in inventory |
| QuestComplete | Requires completed quest |

### EffectType Enum

| Type | Description |
|------|-------------|
| StartQuest | Begin a new quest |
| GiveItem | Give item to player |
| ChangeFriendship | Modify friendship points |
| TriggerEvent | Trigger game event |
| ChangeNPCLocation | Move NPC to new zone |

## 3. Requirements

- Must check all conditions before showing option
- Must execute all effects when option chosen
- Must integrate with RelationshipManager for friendship checks
- Must integrate with InventoryManager for item checks
- Must integrate with Quest System for quest effects
- Must support multiple conditions per option
- Must support multiple effects per option

## 4. Limitations

- **No branching dialogue trees** - Single option, single response.
- **No persistent dialogue state** - Options reset each conversation.
- **No randomized dialogue** - Same conditions always show same options.
- **No dialogue scripting** - No variables, no complex logic.
- **NPCs share options** - Options are global, conditions determine visibility per NPC.

## 5. Modularity

### This System Owns
- Dialogue option definitions (DialogueOptionData ScriptableObjects)
- Dialogue option registry (DialogueOptionRegistry singleton)
- Condition checking
- Effect execution

### This System References
- NPCData (by ID) - for friendship conditions
- ItemData (by ID) - for item conditions, give effects
- QuestData (by ID) - for quest conditions, start quest effects
- Zone (enum) - for ChangeNPCLocation effect

### Other Systems Reference This
- **Conversation UI** (future) - displays available dialogue options
- **FriendshipTierData** - Unlocks may grant dialogue options

## 6. AI Integration Points

### What AI Can Generate
- New dialogue options with any conditions/effects
- Story-advancing dialogue chains
- Relationship milestones (proposal, confession)

### Example Generation

Prompt: "Add marriage"

AI generates:
- DialogueOptionData: "Will you marry me?"
  - Conditions: Friendship >= 110, HasItem "wedding_ring"
  - Effects: TriggerEvent "marriage", ChangeFriendship +20
  - Response: "Yes! I've been waiting for you to ask!"
- FriendshipTierData: Partner tier at 110
- ItemData: Wedding Ring
- NPCData: Goldsmith (sells rings)

### Validation Requirements
- Condition TargetIds must reference valid content of appropriate type
- Effect TargetIds must reference valid content
- Text and ResponseDialogue should not be empty

## 7. Example Content

```csharp
DialogueOptionData askWeather = new DialogueOptionData {
    Id = "dialogue_ask_weather",
    Text = "How's the weather treating you?",
    Conditions = new DialogueCondition[] { },  // Always available
    Effects = new[] {
        DialogueEffect.ChangeFriendship("", 1)  // +1 with current NPC
    },
    ResponseDialogue = "Oh, it's been lovely! Perfect for the crops."
};
```

## 8. Edge Cases & Considerations

### Option Visibility
Option visible if ALL conditions pass. Empty Conditions[] means always visible.

### Effect Order
Effects execute in array order. Consider: if GiveItem fails (inventory full), should subsequent effects still run?

### NPC Context
Some conditions/effects need "current NPC" context. Empty TargetId in effect might mean "this NPC."

### Event System
TriggerEvent effect calls EventManager.Trigger(eventId). Events are separate system (marriage, disaster, etc.).

### Item Consumption
Conditions check for item but don't consume. To consume on dialogue, add RemoveItem effect type (not currently in EffectType).

### Repeatable Dialogue
Same option can be chosen multiple times. Effects repeat. For one-time dialogue, add condition that checks for first-time flag.

### Dialogue Option Discovery
All dialogue options checked against all NPCs during conversation. Only matching ones shown.
