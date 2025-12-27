# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Required Reading

**You must read these before writing any code:**
- `GDD.md` - Complete game design document (950+ lines of detailed systems, schemas, AI integration)
- `INSTRUCTIONS.md` - Collaboration workflow, architecture decisions, code style

## Project Summary

**Prompt Harvest** - AI-augmented life simulation in Unity 6 (6000.0.62f1). Players shape their world through natural language prompts during weekly dream sequences. AI generates structured JSON content (NPCs, items, quests, etc.) that integrates into gameplay.

## Build & Run

Unity 2D project with URP. Open in Unity Hub, load `Assets/Scenes/SampleScene.unity`. No custom build scripts or CI/CD yet.

## Architecture (MVC + R3 Reactive)

```
Managers (Model) → Controllers → UI Controls (View)
```

- **Managers** (`{Thing}Manager`): Singletons owning state via `ReadOnlyReactiveProperty<T>`, mutations via simple voids
- **Controllers** (`{Thing}Controller`): Subscribe to Managers, respond to UI events, orchestrate
- **UI Controls** (`{Thing}UIControl`): View layer - human implements, AI only suggests signatures

## Key Patterns

- **ScriptableObjects** for all content (hand-authored and AI-generated use identical SO types)
- **One prefab per entity type** - visual differences driven by data
- **Single scene for MVP** - zones as child GameObjects
- **Let it crash** - no defensive null-checks, loud errors with clear stack traces

## Naming

| Type | Convention | Example |
|------|------------|---------|
| Managers | `{Thing}Manager` | `InventoryManager` |
| Controllers | `{Thing}Controller` | `TimeController` |
| UI Controls | `{Thing}UIControl` | `ShopUIControl` |
| Data classes | `{Thing}Data` | `ItemData`, `NPCData` |
| Public | PascalCase | `Gold`, `AddItem()` |
| Private | camelCase | `gold`, `currentSlot` |

## Collaboration Rules

Per INSTRUCTIONS.md - this project requires explicit approval before implementation:
1. Understand requirements (ask questions)
2. Design discussion with options/pros/cons
3. User flow mapping
4. Technical design approval
5. Implementation in small chunks

**Never skip planning. Never make autonomous decisions. Ask, don't assume.**
