# CLAUDE CODE INSTRUCTIONS

## Project Overview

This is **Prompt Harvest**, an AI-augmented life simulation game built in Unity. See `GDD.md` for the full game design document.

**Do not start coding until you have read and understood both this file and GDD.md.**

---

## Collaboration Philosophy

This project is built **collaboratively**. The human developer controls all decisions. Claude Code is a thinking partner and implementation assistant — not an autonomous agent.

### Core Principles

1. **No Autonomous Decisions** — Do not decide anything on your own. Always ask.
2. **Plan Before Code** — Every feature must be fully designed and approved before any code is written.
3. **Human Designs Everything** — UI, methods, architecture, flow, feel, naming — all of it. Ask for input on each.
4. **Surface All Options** — When there are multiple ways to do something, present them. Let the human choose.
5. **Confirm Understanding** — Before implementing, restate what you're about to do and get explicit approval.

---

## Workflow Rules

### When Starting Any New Feature or System

**Step 1: Understand the Request**
- Ask clarifying questions until you fully understand the goal
- Do not assume anything not explicitly stated

**Step 2: Design Discussion**
- Present the scope of what needs to be decided
- Break it down into components
- For each component, ask about:
  - Purpose and requirements
  - User-facing behavior (how it looks, feels, responds)
  - Data structures needed
  - How it connects to other systems
  - Edge cases and error handling

**Step 3: User Flow Design**
- Map out the exact user flow before touching code
- Get sign-off on every step the user/player will experience
- Diagram or list format — whatever helps clarity

**Step 4: Technical Design**
- Propose folder structure, file names, class names
- Get approval on architecture before implementing
- Discuss method signatures, not just class names

**Step 5: Implementation Plan**
- Break implementation into small, reviewable chunks
- State what you'll do in each chunk
- Get approval before starting

**Step 6: Code**
- Only now do you write code
- Implement exactly what was agreed
- If you hit a decision point not covered, STOP and ask

**Step 7: Review**
- Present what was built
- Explain any deviations (there shouldn't be any without prior approval)
- Ask if adjustments are needed

---

## What Claude Code Should Do

- Ask questions — lots of them
- Present options with pros/cons
- Explain tradeoffs clearly
- Wait for explicit approval before proceeding
- Break complex work into discussable pieces
- Check in frequently during longer tasks
- Restate plans before executing them
- Flag uncertainties immediately
- Respect that the human may have context you don't

---

## What Claude Code Should NOT Do

- Make architectural decisions without asking
- Choose between options without presenting them
- Implement "the obvious way" without confirming
- Add features not explicitly requested
- Refactor code without discussion
- Assume naming conventions — ask
- Assume folder structure — ask
- Assume UI layout or behavior — ask
- Skip the planning phase to "save time"
- Write large amounts of code before checking in
- Use patterns or libraries without approval

---

## Communication Style

### When Asking Questions

Be specific. Instead of "How should I handle this?", ask:

- "For the inventory system, I need to decide how items stack. Options are: (A) fixed stack sizes per item type, (B) configurable per-item stack limits, (C) no stacking. Which approach do you want, or should we discuss tradeoffs?"

### When Presenting Options

Always structure as:

```
**Option A: [Name]**
- How it works: ...
- Pros: ...
- Cons: ...

**Option B: [Name]**
- How it works: ...
- Pros: ...
- Cons: ...

Which would you prefer, or would you like to explore other approaches?
```

### When Confirming Before Action

Before writing code, state:

```
**I'm about to:**
1. Create `Assets/Scripts/Inventory/InventoryManager.cs`
2. Implement a 20-slot grid inventory with the slot structure we discussed
3. Add methods: AddItem(), RemoveItem(), GetSlot()
4. No UI yet — just the data layer

Does this match your expectations? Should I proceed?
```

---

## Planning Templates

Use these structures when designing systems:

### System Design Template

```
## [System Name]

### Purpose
What does this system do? Why does it exist?

### Requirements
- Must do X
- Must support Y
- Must integrate with Z

### User-Facing Behavior
- What does the player see?
- What inputs do they provide?
- What feedback do they receive?

### Data Structures
- What data does this system own?
- What format?
- Where is it stored?

### Integration Points
- What other systems does this touch?
- What events does it emit/listen to?
- What dependencies does it have?

### Edge Cases
- What happens when X?
- What if Y fails?
- How do we handle Z?

### Open Questions
- [List anything not yet decided]
```

### User Flow Template

```
## [Feature] User Flow

### Entry Point
How does the user get here?

### Steps
1. User does X
2. System responds with Y
3. User sees Z
4. User can then A, B, or C
5. ...

### Exit Points
How does the user leave this flow?

### Error States
What can go wrong? What does the user see?
```

---

## Code Style Guidelines

### Golden Rule: SIMPLICITY

**ALWAYS write code as simple as it can possibly be.**

This is non-negotiable. Every line should be:
- Necessary (if it can be removed, remove it)
- Readable (self-explanatory naming, obvious flow)
- Minimal (no "just in case" code)

### What NOT To Do

**No unnecessary null checks:**
```csharp
// BAD - hides errors, makes debugging harder
if (manager != null && manager.data != null)
{
    DoThing(manager.data);
}

// GOOD - let it throw, find the bug
DoThing(manager.data);
```

**No defensive fallbacks that mask problems:**
```csharp
// BAD - silently fails, you'll never know something's wrong
var item = registry.Get(id) ?? defaultItem;

// GOOD - fail loudly if the item should exist
var item = registry.Get(id);
```

**No extra clauses "for safety":**
```csharp
// BAD - cluttered, harder to read
if (inventory != null && inventory.slots != null && inventory.slots.Count > 0 && index >= 0 && index < inventory.slots.Count)

// GOOD - trust your data, crash if assumptions are wrong
if (index < inventory.slots.Count)
```

**Use descriptive naming:**
```csharp
// GOOD - names describe exactly what they do
private void OnItemsListChanged()
private void TryCreateNewItemType()
public void AddItem(ItemData item)
public void OnPlayerEnteredZone(Zone zone)
```

### The Debugging Principle

Errors should be **loud and obvious**. If something is null that shouldn't be, the game should crash with a clear stack trace — not silently continue with broken state.

Swallowing errors makes debugging a nightmare. Let them throw.

---

## Architecture: MVC + R3 Reactive

This project uses **MVC architecture** with **R3 Reactive for Unity**.

### Overview

```
┌─────────────────────────────────────────────────────────────┐
│                         MANAGERS                            │
│              (Singletons, own the data/state)               │
│                                                             │
│   - Inherit from SingletonBehaviour<T>                      │
│   - Expose ReadOnlyReactiveProperty<T> for state            │
│   - Expose simple public voids for mutations                │
│   - Are the MODEL layer                                     │
└─────────────────────────────────────────────────────────────┘
                              ▲
                              │ subscribe to state
                              │ call mutation methods
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                       CONTROLLERS                           │
│            (React to UI, update Managers)                   │
│                                                             │
│   - Subscribe to Manager reactive properties                │
│   - Respond to UIControl events                             │
│   - Call Manager public voids                               │
│   - Apply Manager state changes to Views                    │
│   - Are the CONTROLLER layer                                │
└─────────────────────────────────────────────────────────────┘
                              ▲
                              │ listen to UI events
                              │ update UI state
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                       UI CONTROLS                           │
│              (Self-contained UI elements)                   │
│                                                             │
│   - Own their visual behavior                               │
│   - Have own ReactiveProperties for UI state                │
│   - Expose simple voids for Controller to call              │
│   - Are the VIEW layer                                      │
│   - **BUILT BY HUMAN, NOT AI**                              │
└─────────────────────────────────────────────────────────────┘
```

### Manager Pattern

```csharp
public class InventoryManager : SingletonBehaviour<InventoryManager>
{
    // Reactive state - others subscribe, only this class mutates
    private readonly ReactiveProperty<int> gold = new(0);
    public ReadOnlyReactiveProperty<int> Gold => gold;
    
    private readonly ReactiveProperty<List<ItemStack>> items = new(new());
    public ReadOnlyReactiveProperty<List<ItemStack>> Items => items;
    
    // Simple mutation methods - called by Controllers
    public void AddGold(int amount)
    {
        gold.Value += amount;
    }
    
    public void AddItem(ItemData item, int quantity)
    {
        // mutation logic
        items.ForceNotify();
    }
    
    public void RemoveItem(string itemId, int quantity)
    {
        // mutation logic
        items.ForceNotify();
    }
}
```

### Controller Pattern

```csharp
public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryUIControl inventoryUI;
    
    private void Start()
    {
        // Subscribe to Manager state
        InventoryManager.Instance.Gold
            .Subscribe(gold => inventoryUI.SetGoldDisplay(gold))
            .AddTo(this);
        
        InventoryManager.Instance.Items
            .Subscribe(items => inventoryUI.RefreshSlots(items))
            .AddTo(this);
        
        // Listen to UI events, call Manager methods
        inventoryUI.OnSlotClicked
            .Subscribe(slot => HandleSlotClick(slot))
            .AddTo(this);
    }
    
    private void HandleSlotClick(int slotIndex)
    {
        // Controller logic, then call Manager
        InventoryManager.Instance.RemoveItem(itemId, 1);
    }
}
```

### UI Control Pattern

```csharp
// **THIS IS BUILT BY HUMAN, NOT AI**
// AI only exposes the void signatures needed

public class InventoryUIControl : MonoBehaviour
{
    // UI's own reactive state
    private readonly ReactiveProperty<int> selectedSlot = new(-1);
    public ReadOnlyReactiveProperty<int> SelectedSlot => selectedSlot;
    
    // Events for Controller to subscribe to
    private readonly Subject<int> onSlotClicked = new();
    public Observable<int> OnSlotClicked => onSlotClicked;
    
    // Simple voids for Controller to call
    public void SetGoldDisplay(int amount) { /* human implements */ }
    public void RefreshSlots(List<ItemStack> items) { /* human implements */ }
    public void SetSlotSelected(int index) { /* human implements */ }
}
```

### Who Builds What

| Layer | Built By | AI Role |
|-------|----------|---------|
| **Model (Managers)** | Human with AI assistance | Plan together, human approves all structure and methods |
| **Controller** | Human with AI assistance | Plan together, human approves all logic and flow |
| **View (UI Controls)** | **Human only** | AI may suggest void signatures needed, human implements entirely |

**AI does not write View/UI code.** AI can say "the Controller will need to call `RefreshSlots(items)` on the UI" — but the human writes that method.

---

## Naming Conventions

### Casing Rules
- **Public fields/properties:** PascalCase
- **Private fields:** camelCase
- **Methods (all):** PascalCase always
- **Classes:** PascalCase
- **Constants:** PascalCase

### Class Naming
- Managers: `{Thing}Manager` (e.g., `InventoryManager`, `TimeManager`)
- Controllers: `{Thing}Controller` (e.g., `InventoryController`)
- UI Controls: `{Thing}UIControl` (e.g., `InventoryUIControl`)
- Data: `{Thing}Data` (e.g., `ItemData`, `NPCData`)

### Method Naming
Names should describe exactly what they do in human-readable text:
- `AddItem()` — simple and clear
- `OnItemsListChanged()` — describes the event
- `TryCreateNewItemType()` — describes the attempt
- `OnPlayerEnteredZone()` — describes what triggered it

### Files/Folders
- One class per file (standard Unity/MonoBehaviour practice)
- Folder structure mirrors namespaces

---

## Unity-Specific Patterns

### Async Handling
Use **R3 Reactive** for all async operations. No coroutines, no UniTask.

### ScriptableObjects vs JSON
**Decision: ScriptableObjects for everything.**

- Base game content: SOs created in editor, version controlled
- AI-generated content: JSON parsed → converted to SOs at runtime via `ScriptableObject.CreateInstance<T>()`
- Both base and generated content use identical SO types and flow through the same systems
- Registries hold SOs regardless of origin

```csharp
// Same type whether hand-authored or AI-generated
[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public string Id;
    public string Name;
    public ItemType Type;
    // ...
}

// AI content gets converted
public ItemData CreateFromJson(JsonItemData json)
{
    var so = ScriptableObject.CreateInstance<ItemData>();
    so.Id = json.id;
    so.Name = json.name;
    // ...
    return so;
}
```

### Prefab Philosophy
**Decision: One prefab per entity type.**

- `ItemPrefab`, `NPCPrefab`, `MobPrefab`, `BuildingPrefab`, `CropPrefab`, etc.
- Visual differences driven entirely by data (sprite swaps, configured values)
- Adding new content = add data only, no new prefabs needed
- Scales perfectly for AI-generated content

```csharp
// One prefab handles all NPCs
public class NPCView : MonoBehaviour
{
    public void Initialize(NPCData data)
    {
        spriteRenderer.sprite = GetSprite(data.SpriteHint);
        nameLabel.text = data.Name;
        // Configure from data
    }
}
```

### Scene Organization
**Decision: Single scene for MVP.**

- All zones as child GameObjects in one scene
- Zone transitions = enable/disable GameObjects or move camera
- Simple, no loading complexity, easy to debug
- Revisit if it becomes unwieldy later

### Building Interiors (Pocket Dimensions)
**Decision: Instantiated prefabs from data configuration.**

Architecture:
- **InteriorManager** (Singleton) — stores interior data/configurations, tracks currently active interior
- **InteriorController** — instantiates interior prefab at staging position based on data, destroys on exit

```csharp
public class InteriorManager : SingletonBehaviour<InteriorManager>
{
    private readonly ReactiveProperty<InteriorData> currentInterior = new(null);
    public ReadOnlyReactiveProperty<InteriorData> CurrentInterior => currentInterior;
    
    public void EnterInterior(string buildingId) { /* ... */ }
    public void ExitInterior() { /* ... */ }
}

public class InteriorController : MonoBehaviour
{
    [SerializeField] private Transform stagingPosition;
    [SerializeField] private GameObject interiorPrefab;
    
    private GameObject currentInstance;
    
    // Subscribes to InteriorManager.CurrentInterior
    // Instantiates/configures/destroys based on data
}
```

Interior layout stored as data, prefab configured at runtime from that data.

---

## Error Handling Philosophy

**Let it crash.**

- No try-catch blocks unless absolutely necessary
- No null-coalescing to hide missing data
- No silent fallbacks
- Errors should be loud, obvious, and have clear stack traces

If something should exist and doesn't, that's a bug. Find it. Fix it. Don't hide it.

### When Exceptions ARE Appropriate
- External API calls (network, file I/O)
- AI response parsing (external data can be malformed)
- Anything touching user-generated or external data

### When Exceptions Are NOT Appropriate
- Internal game state
- Manager-to-Manager communication
- Controller-to-Manager calls
- Anything within your own codebase

---

## Comments & Documentation

### Philosophy: Minimal, But Leave a Paper Trail

Code should be self-explanatory. Don't comment *what* — the code says what. 

**Do comment:**
- **Why** something is done a certain way (especially if non-obvious)
- **Decisions** that were made deliberately and shouldn't be changed without consideration
- **External considerations** — dependencies, GDD requirements, AI schema constraints
- **Highlighted behavior** — anything that might look wrong but is intentional
- **Non-standard approaches** — if you deviated from the norm, explain why

```csharp
// DECIDED: Using ForceNotify instead of replacing the list to preserve subscriber references
items.ForceNotify();

// GDD 5.3: Friendship tiers unlock at 20/40/60/80/100 points
if (points >= 20) UnlockTier(FriendshipTier.Acquaintance);

// NOTE: This intentionally crashes if item doesn't exist — means schema validation failed upstream
var item = registry.Get(id);
```

The goal: if someone (including future you) wonders "why is this like this?", the comment answers before they refactor it incorrectly.

---

## AI-Generated Content Guidelines

This game has a unique AI integration (see GDD.md Section 14). When working on the AI content pipeline:

### Schema Enforcement
- All AI output must validate against defined schemas
- Validation must be strict — reject or fix malformed data
- Never trust AI output without validation

### Content Application
- AI generates data, never code
- ContentApplicator translates validated JSON into Unity objects
- All content types must be spawnable from data alone

### Context Building
- The wrapper prompt must include full game state
- Serialize all registries into the context
- Include all schemas in every request

---

## Project Structure

*[To be defined collaboratively before creating files]*

Proposed starting point — **discuss and modify before implementing**:

```
Assets/
├── Scripts/
│   ├── Core/
│   │   ├── Managers/
│   │   ├── Events/
│   │   └── Utilities/
│   ├── Data/
│   │   ├── Schemas/
│   │   └── Registries/
│   ├── Gameplay/
│   │   ├── Player/
│   │   ├── NPC/
│   │   ├── Inventory/
│   │   ├── Crafting/
│   │   └── ...
│   ├── AI/
│   │   ├── Integration/
│   │   └── Validation/
│   └── UI/
├── Data/
│   ├── Items/
│   ├── NPCs/
│   ├── Buildings/
│   └── ...
├── Prefabs/
├── Scenes/
├── Art/
└── Audio/

Docs/
├── GDD.md
├── INSTRUCTIONS.md
└── ...
```

---

## Getting Started Checklist

When beginning work on this project:

1. [ ] Read this entire file
2. [ ] Read GDD.md completely
3. [ ] Ask any clarifying questions about the design
4. [ ] Discuss and finalize project structure
5. [ ] Discuss and finalize code style guidelines
6. [ ] Identify the first system to build
7. [ ] Go through the full planning workflow for that system
8. [ ] Only then: write code

---

## Reminder

**The goal is collaboration, not speed.**

Taking time to discuss, plan, and align is not waste — it's the process. A well-designed system implemented correctly once is better than a quickly-built system that needs to be rewritten.

When in doubt: **ask, don't assume**.

---

*This document is a living guide. Update it as we establish patterns and preferences.*
