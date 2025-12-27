# PROMPT HARVEST

**AI-Augmented Generative Life Simulation**  
*Game Design Document v2.0*

---

## Table of Contents

1. [Vision & Concept](#1-vision--concept)
2. [Core Game Loop](#2-core-game-loop)
3. [World Structure](#3-world-structure)
4. [Weather & Seasons](#4-weather--seasons)
5. [NPC System](#5-npc-system)
6. [Economy System](#6-economy-system)
7. [Crafting & Tools](#7-crafting--tools)
8. [Entity System (Vehicles & Mounts)](#8-entity-system-vehicles--mounts)
9. [Inventory System](#9-inventory-system)
10. [Journal & Achievements](#10-journal--achievements)
11. [Music & Audio](#11-music--audio)
12. [Save System](#12-save-system)
13. [Content Data Schemas](#13-content-data-schemas)
14. [AI Integration](#14-ai-integration)
15. [Predefined Behaviors](#15-predefined-behaviors)
16. [Scope & Phases](#16-scope--phases)
17. [Unity Implementation Notes](#17-unity-implementation-notes)
18. [Example Prompts & Outputs](#18-example-prompts--outputs)
19. [Next Steps](#19-next-steps)

---

## 1. Vision & Concept

### 1.1 Elevator Pitch

**Prompt Harvest** is a cozy pixel-art life simulation where you farm, craft, and build relationships — but the world evolves based on **your prompts**. Each in-game week, you dream, and in that dream you describe something you want to exist. When you wake, the world has changed to include it.

The AI generates structured data that your modular game systems interpret. Every addition — cows, breweries, new villagers, marriage, cars, whatever you imagine — follows consistent rules while feeling emergent and personal.

### 1.2 Design Pillars

- **Player Authorship** — The player literally shapes what exists through natural language prompts.
- **Emergent Consequences** — Every addition ripples through interconnected systems. Adding alcohol might create a pub, or make an NPC develop a drinking problem, or both.
- **Cozy Core Loop** — Satisfying rhythms of farming, gathering, crafting, and socializing at your own pace.
- **Bounded Creativity** — The AI works within defined schemas, so additions are always playable.
- **No Restrictions** — Players can prompt anything. The game doesn't judge — it creates.

### 1.3 Genre & Inspirations

**Primary Genre:** Generative Life Sim / AI-Augmented Sandbox

**Inspirations:** Stardew Valley (structure, tone), Scribblenauts (freeform creation), Dwarf Fortress (emergent complexity), Animal Crossing (cozy progression)

---

## 2. Core Game Loop

### 2.1 Time System

**Three Phases:** Morning, Day, Night

**Time Does NOT Auto-Advance** — Time only progresses when the player performs a phase-advancing action:

- Sleep in bed (skips to next morning)
- Certain activities with NPCs (swimming at beach, fishing together, etc.)
- Using certain crafting stations that take time

This lets players explore, farm, mine, and interact at their own pace within each phase.

### 2.2 Daily Activities

- Plant, water, harvest crops
- Mine for ores and minerals
- Forage in forest/beach
- Fish (alone or with NPCs)
- Craft items at stations
- Talk to NPCs, give gifts, build relationships
- Buy/sell at shops
- Complete quests
- Place items/furniture on your farm and in your house

### 2.3 Weekly Loop

1. Week = 7 in-game days.
2. On Day 7, player must pay loan installment.
3. **If paid:** Player receives 1 Prompt Token + unlocks new Achievements for the week.
4. **If NOT paid:** Player still gets Prompt Token but NO new Achievements unlock that week.
5. **Dream Sequence:** Player enters a dream state. A text input appears: "I want to add..." Player types their prompt. When they wake, the world has changed.
6. **Auto-Save:** Game saves before every prompt. These saves are protected and never overwritten.

### 2.4 Main Story & End State

**Main Story Arc:** Spans approximately 3 weeks (21 days). Player has a goal (pay off farm, restore the town, etc.).

**Achievements:** 3 major milestone quests per NPC. These are the primary progression markers. Generated programmatically (not by AI) based on NPC preferences and available content.

**After Main Story:** Game continues indefinitely. Player still gets weekly prompts but no new achievements. Pure sandbox mode.

**Player Can Change Goals:** Via prompts, players can add new story elements, win conditions, or narrative arcs.

---

## 3. World Structure

### 3.1 Zones Overview

| Zone | Description | Content Types |
|------|-------------|---------------|
| Village | Central hub, grows organically as content is added | Buildings, NPCs, shops, paths, decorations |
| Farm | Player's home base, fully controllable | Crops, animals, storage, player house, placed items |
| Forest | Dense wooded area | Forageables, wild animals, trees, secrets |
| Beach | Coastal zone | Fishing spots, shells, swimming areas, docks |
| Mine | Underground caverns, multiple levels | Ores, gems, minerals for tools/smelting |

### 3.2 Village Layout System

Village starts as a grassy area with a few pre-built houses and NPCs in one corner.

Path network generated procedurally (noise-based) connecting existing buildings.

**Building Placement Algorithm:**

- When a new building is needed, find empty tile near existing village cluster
- Reserve a footprint (e.g., 4x3 tiles) with one tile designated as door
- Fill surrounding tiles to prevent immediate adjacency (buffer zone)
- Generate path connecting new building to existing path network
- Village expands outward from starting corner as more buildings are added
- Random decorations (flowers, benches, etc.) spawn in village area

### 3.3 Building Interiors (Pocket Dimensions)

Each building with an interior is a separate "pocket dimension" — a completely different scene/area.

**Standard Interior Sizes:**

| Size | Dimensions | Use Cases |
|------|------------|-----------|
| Small | 6x4 tiles | Small shops, cottages |
| Medium | 8x5 tiles | Standard homes, most shops |
| Large | 10x7 tiles | Large establishments, player house (upgraded) |

**Entry Conditions:** Doors can have requirements to enter:

- Friendship level with owner
- Time of day (shop hours)
- Specific item in inventory
- Quest completion
- Payment (cover charge)

### 3.4 Player House

- Starts as Small interior. Can be expanded via resources (wood, stone, etc.) or prompts.
- Player can place furniture freely inside their house.
- Player can also place items on their farm area (outdoor decorations, functional items).
- Interior layout stored as array — can be modified, saved, loaded.

---

## 4. Weather & Seasons

### 4.1 Weather Types

| Weather | Effects |
|---------|---------|
| Sunny | Default. All shops open, NPCs roam freely, all activities available. |
| Rain | Crops auto-watered. Some NPCs stay indoors. Some shops may close. Fishing bonuses. |
| Snow | Winter-only. Movement slower. Some NPCs stay indoors. Seasonal forageables. |

Weather affects NPC schedules, shop availability, and certain gameplay mechanics.

AI can generate new weather types or weather-dependent content.

### 4.2 Seasons

Four seasons: Spring, Summer, Fall, Winter (each = 1 week = 7 days).

Seasons affect: crop availability, forageable spawns, fish types, NPC dialogue, some prices.

Certain items are seasonal (pumpkins in Fall, etc.).

AI can generate season-locked content.

---

## 5. NPC System

### 5.1 Core NPC Features

**Every NPC has ALL of the following:**

- **Shop:** Items they sell and items they buy (with preferences/prices)
- **Quest:** At least one quest (often multi-day)
- **Friendship:** Relationship meter (0-100+ points)
- **Schedule:** Where they are during Morning/Day/Night for each day of the week
- **Preferences:** Gift likes/dislikes, activity preferences, dialogue style

### 5.2 NPC Schedules

Simple 3-phase system: Morning, Day, Night

For each phase, each day, NPC has a location goal and activity:

```json
{ "day": "Monday", "phase": "Morning", "location": "home", "activity": "sleeping" }
{ "day": "Monday", "phase": "Day", "location": "shop", "activity": "working" }
{ "day": "Monday", "phase": "Night", "location": "beach", "activity": "walking" }
```

Most NPCs work at their home/shop during Day phase.

Weather can override schedules (rain → stay indoors).

### 5.3 Friendship & Relationship Levels

**Friendship Points:** Gained by talking, giving gifts, completing quests, doing activities together.

**Relationship Tiers:**

| Level | Points | Unlocks |
|-------|--------|---------|
| Stranger | 0-19 | Basic dialogue, can buy from shop |
| Acquaintance | 20-39 | More dialogue, can receive quests |
| Friend | 40-59 | Can enter their home, gift bonuses |
| Good Friend | 60-79 | Can ask them to follow you, better prices |
| Best Friend | 80-99 | Unique dialogue, special activities |
| [Special] | 100+ | AI-generated tiers (e.g., Marriage at 110) |

### 5.4 NPC Following & Activities

At Good Friend level (60+ points), player can ask NPC to follow them.

While following, player can walk NPC to an Activity Point and do that activity together.

**Activity Points:** Specific locations in zones where activities can happen:

- Beach → Swimming, Fishing, Sunbathing
- Forest → Foraging, Picnic, Stargazing
- Village → Shopping, Festival activities
- Farm → Farming together, Animal care

**Activity Properties (all modular):**

- Which NPCs enjoy it (affects friendship gain)
- Friendship requirement to unlock
- Valid zones/locations
- Time of day restrictions
- Whether it advances time phase
- Rewards (items, friendship, etc.)

### 5.5 Example: Marriage System (via Prompt)

*Player prompts: "Add marriage"*

**AI might generate:**

- New Friendship Tier: 'Partner' at 110 points
- New NPC: Goldsmith (sells rings)
- New Building: Church
- New Item: Wedding Ring (gift item that triggers marriage proposal)
- New Dialogue Option: "Will you marry me?" (at 110+ friendship, ring in inventory)
- Marriage Effect: Spouse moves to player's farm, all their shop items become free, new dialogue
- Rejection Penalty: -50 friendship if conditions not met

---

## 6. Economy System

### 6.1 Currency

**Gold (G):** Primary currency. Earned by selling items, completing quests, finding treasure.

### 6.2 Pricing Dynamics

Base prices are set per item. Actual prices vary based on:

- **Friendship Bonus:** Higher friendship with NPC = better buy/sell prices
- **Saturation Penalty:** Selling the same item repeatedly to the same NPC lowers the price they'll pay
- **Seasonal Modifiers:** Some items worth more/less in certain seasons

Formula: `FinalPrice = BasePrice × FriendshipMod × SaturationMod × SeasonMod`

### 6.3 Loan System

Player starts with debt (e.g., 5000G for the farm).

Weekly payments escalate: Week 1 = 500G, Week 2 = 750G, Week 3 = 1000G, etc.

**Payment Success:** Receive Prompt Token + new Achievements unlock

**Payment Failure:** Still receive Prompt Token, but NO new Achievements that week. Debt carries over.

---

## 7. Crafting & Tools

### 7.1 Tool System

**Tool Properties:**

- Tier (Basic, Copper, Iron, Gold, etc.)
- Durability (uses before breaking)
- Efficiency (speed of action)
- Special Effects (e.g., Gold Pickaxe finds gems more often)

**Tool Upgrade Path:** Mine ores → Smelt into bars → Craft at appropriate station

### 7.2 Crafting Stations

**Core Stations (base game):**

- Workbench — Basic crafting, furniture
- Furnace — Smelting ores into bars
- Anvil — Tool crafting and upgrades

**AI-Generated Stations:** When player prompts add content that needs new processing:

- "Add brewing" → Brewing Barrel
- "Add clothes" → Loom
- "Add drugs" → Apothecary Table
- "Add cars" → Mechanic's Bench

**Crafting Method:** Patternless — player adds correct ingredients to the station, output is produced. No grid puzzles.

### 7.3 Mining & Smelting

Mine has multiple levels, deeper = rarer ores.

**Ore → Bar progression:**

- Copper Ore → Copper Bar (Furnace)
- Iron Ore → Iron Bar (Furnace)
- Gold Ore → Gold Bar (Furnace)
- Gems can be found, used for decoration or special crafting.

AI can generate new ore types, processing chains.

---

## 8. Entity System (Vehicles & Mounts)

### 8.1 Entity vs NPC

- **NPCs:** Have shops, quests, relationships, schedules, dialogue
- **Entities:** Rideable/usable objects. Single inventory slot. Require specific item to function.

### 8.2 Entity Properties

- Name
- Type (mount, vehicle, automator, pet)
- Speed modifier
- Fuel/Feed slot (single item type it accepts)
- Fuel consumption rate
- Special abilities (optional)

### 8.3 Entity Examples

**Horse (from "add horses"):**
- Type: mount, Speed: 2x, Feed: Hay/Apples, Feed Effect: 2.5x speed for 1 day

**Car (from "add cars"):**
- Type: vehicle, Speed: 4x, Fuel: Gasoline, Requires: Mechanic's Bench to craft parts
- May generate: Mechanic NPC, Gas Station building, Oil mining in mine, Car Parts recipes

**Roomba (from "add automation"):**
- Type: automator, Speed: 0, Fuel: Batteries, Effect: Auto-harvests crops on farm

---

## 9. Inventory System

### 9.1 Inventory Structure

- **Main Inventory:** 20 slots (expandable via prompts/backpacks)
- **Hotbar:** 8 slots, quick-access to main inventory items
- **Equipment Slots:** Head, Body, Accessory (armor, hats, tools)

Tile-based grid system. Items stack (up to stack_max).

### 9.2 Expansion via Prompts

*"Add backpacks"* might generate:

- Leather (new material from hunting or buying)
- Backpack item (+10 slots when equipped)
- Leatherworker NPC or add to existing crafting
- Recipe: Leather × 5 → Backpack

---

## 10. Journal & Achievements

### 10.1 Journal Tabs

- **Quests:** Active and completed quests
- **Achievements:** Major milestone progress (3 per NPC)
- **Relationships:** Current friendship levels with all NPCs
- **Map:** Discovered areas and points of interest
- **Codex:** Encyclopedia of discovered items, mobs, NPCs, recipes

### 10.2 Achievement System

**3 Achievements per NPC** — these are the major progression goals.

**Generated Programmatically** (not by AI) as friendship increases:

- At Acquaintance (20): First achievement revealed based on NPC preferences
- At Friend (40): Second achievement revealed
- At Good Friend (60): Third achievement revealed

**Achievement Generation Logic:**

- Look at NPC's likes/preferences
- Look at current available items, crafting stations, activities
- Generate a challenging but achievable goal (e.g., "Bring 50 Iron Bars to Blacksmith Bob")

**Achievements unlock only if loan was paid that week.**

---

## 11. Music & Audio

### 11.1 Adaptive Music System

Base song with multiple instrument/layer tracks.

Different zones/moods crossfade different layers:

| Zone/Mood | Layers Active |
|-----------|---------------|
| Village (Day) | Acoustic guitar, light percussion, flute |
| Forest | Ambient pads, soft strings, bird sounds |
| Mine | Deep drums, bass, echoing percussion |
| Beach | Ukulele, steel drums, wave sounds |
| Night | Slower tempo, piano, crickets |
| Tense/Evil | Minor key strings, dissonant brass |
| Chaotic | Fast tempo, all instruments, distortion |

Zone mood tags can be affected by AI-generated content ("add crime" → village becomes tense).

**Note:** Music layers created manually, limited set of moods.

---

## 12. Save System

### 12.1 Save Structure

**Multiple Save Slots:** Players can have several different worlds/playthroughs.

**Auto-Save Points:**

- Every night when player sleeps
- Before every Dream Sequence (prompt)

**Protected Weekly Saves:**

- Pre-prompt saves are NEVER overwritten
- Player can always roll back to any week's state
- Acts as safety net without explicit undo feature

---

## 13. Content Data Schemas

All game content is defined as data. The AI generates JSON that fits these schemas. The game validates and interprets at runtime.

### 13.1 Item Schema

```json
{
  "id": "string",
  "name": "string (required)",
  "type": "tool | material | product | consumable | seed | decoration | furniture | gift",
  "description": "string",
  "value": "number (base sell price)",
  "stack_max": "number (default 99)",
  "durability": "number (for tools, 0 = unbreakable)",
  "tier": "string (basic, copper, iron, gold, etc.)",
  "used_on": ["entity_ids this can be used on"],
  "use_result": "item_id produced",
  "use_time": "number (seconds)",
  "placeable": "boolean",
  "placeable_zones": ["farm", "house"],
  "decay_days": "number (0 = no decay)",
  "sprite_hint": "string",
  "tags": ["array"]
}
```

### 13.2 Mob Schema

```json
{
  "id": "string",
  "name": "string (required)",
  "behavior": "passive_wander | passive_flee | hostile | grazing | nocturnal | pack",
  "spawn_zones": ["zone_ids"],
  "spawn_weight": "number (rarity)",
  "spawn_weather": ["sunny", "rain", "any"],
  "spawn_season": ["spring", "summer", "fall", "winter", "any"],
  "drops": [{"item_id": "string", "chance": 0.0-1.0, "qty_min": 1, "qty_max": 1}],
  "harvestable": "boolean",
  "harvest_tool": "item_id",
  "harvest_result": "item_id",
  "harvest_cooldown_hours": "number",
  "tame_item": "item_id (optional)",
  "purchasable_from": "npc_id",
  "purchase_price": "number",
  "sprite_hint": "string",
  "tags": ["tags"]
}
```

### 13.3 NPC Schema

```json
{
  "id": "string",
  "name": "string (required)",
  "home_building": "building_id",
  "schedule": [
    {"day": "Monday", "phase": "Morning", "location": "zone_id", "activity": "string"}
  ],
  "shop_inventory": ["item_ids"],
  "shop_sell_prices": {"item_id": "multiplier"},
  "buy_preferences": {"item_id": "buy_price"},
  "gift_likes": ["item_ids"],
  "gift_dislikes": ["item_ids"],
  "activity_preferences": {"activity_id": "enjoyment_multiplier"},
  "dialogue_tags": ["friendly", "grumpy", "mysterious"],
  "weather_behavior": {"rain": "stays_indoors", "snow": "stays_indoors"},
  "sprite_hint": "string",
  "tags": ["tags"]
}
```

### 13.4 Building Schema

```json
{
  "id": "string",
  "name": "string (required)",
  "zone": "village | farm | forest | beach",
  "size_tiles": {"width": 4, "height": 3},
  "door_tile": {"x": 1, "y": 0},
  "interior_size": "small | medium | large | none",
  "interior_layout": [["tile_ids"]],
  "owner_npc": "npc_id",
  "entry_conditions": [
    {"type": "friendship", "npc": "npc_id", "min": 40},
    {"type": "time", "phases": ["Day"]},
    {"type": "item", "item_id": "key_item"},
    {"type": "quest_complete", "quest_id": "quest_id"}
  ],
  "sprite_hint": "string",
  "tags": ["tags"]
}
```

### 13.5 Crop Schema

```json
{
  "id": "string",
  "name": "string (required)",
  "seed_item": "item_id",
  "grow_days": "number",
  "water_needs": "none | daily | every_other_day",
  "seasons": ["spring", "summer", "fall", "winter"],
  "harvest_item": "item_id",
  "harvest_qty": {"min": 1, "max": 3},
  "regrows": "boolean",
  "regrow_days": "number",
  "growth_stages": "number",
  "sprite_hints": ["stage1", "stage2", "stage3", "mature"],
  "tags": ["tags"]
}
```

### 13.6 Recipe Schema

```json
{
  "id": "string",
  "name": "string (required)",
  "recipe_type": "crafting | cooking | processing | smelting | brewing",
  "inputs": [{"item_id": "string", "quantity": "number"}],
  "output": {"item_id": "string", "quantity": "number"},
  "station_required": "item_id or building_id",
  "time_hours": "number (in-game)",
  "advances_time_phase": "boolean",
  "tags": ["tags"]
}
```

### 13.7 Quest Schema

```json
{
  "id": "string",
  "name": "string (required)",
  "giver_npc": "npc_id",
  "description": "string",
  "quest_type": "fetch | deliver | gather | talk | build | hunt | visit",
  "objectives": [
    {"type": "collect", "item_id": "string", "quantity": "number"},
    {"type": "deliver", "npc_id": "string", "item_id": "string"},
    {"type": "visit", "zone_id": "string"},
    {"type": "talk_to", "npc_id": "string"}
  ],
  "rewards": {
    "gold": "number",
    "items": [{"item_id": "string", "quantity": "number"}],
    "friendship": {"npc_id": "points"}
  },
  "deadline_days": "number (0 = no deadline)",
  "repeatable": "boolean",
  "prerequisite_quest": "quest_id",
  "tags": ["tags"]
}
```

### 13.8 Activity Schema

```json
{
  "id": "string",
  "name": "string (required)",
  "valid_zones": ["zone_ids"],
  "valid_phases": ["Morning", "Day", "Night"],
  "friendship_required": "number",
  "npc_enjoyment": {"npc_id": "multiplier"},
  "friendship_gain_base": "number",
  "advances_time_phase": "boolean",
  "rewards": {"item_id": "chance"},
  "sprite_hint": "string",
  "tags": ["tags"]
}
```

### 13.9 Entity Schema (Vehicles/Mounts)

```json
{
  "id": "string",
  "name": "string (required)",
  "entity_type": "mount | vehicle | automator | pet",
  "speed_multiplier": "number",
  "fuel_item": "item_id (what it consumes)",
  "fuel_consumption_rate": "number (per day)",
  "fuel_bonus_effect": "string description",
  "special_ability": "string (optional)",
  "craft_recipe": "recipe_id (optional)",
  "purchasable_from": "npc_id (optional)",
  "purchase_price": "number",
  "sprite_hint": "string",
  "tags": ["tags"]
}
```

### 13.10 Crafting Station Schema

```json
{
  "id": "string",
  "name": "string (required)",
  "station_type": "workbench | furnace | anvil | loom | brewing | custom",
  "recipes_enabled": ["recipe_ids this station can perform"],
  "placeable_zones": ["farm", "house"],
  "size_tiles": {"width": 1, "height": 1},
  "craft_cost": [{"item_id": "string", "quantity": "number"}],
  "sprite_hint": "string",
  "tags": ["tags"]
}
```

### 13.11 Dialogue Option Schema

```json
{
  "id": "string",
  "text": "string (what player says)",
  "conditions": [
    {"type": "friendship", "npc": "npc_id", "min": "number"},
    {"type": "has_item", "item_id": "string"},
    {"type": "quest_complete", "quest_id": "string"}
  ],
  "effects": [
    {"type": "start_quest", "quest_id": "string"},
    {"type": "give_item", "item_id": "string"},
    {"type": "change_friendship", "amount": "number"},
    {"type": "trigger_event", "event_id": "string"},
    {"type": "change_npc_location", "npc_id": "string", "new_location": "zone_id"}
  ],
  "response_dialogue": "string (NPC response)",
  "tags": ["tags"]
}
```

### 13.12 Friendship Tier Schema

```json
{
  "id": "string",
  "name": "string (required)",
  "points_required": "number",
  "unlocks": [
    {"type": "can_follow", "value": true},
    {"type": "can_enter_home", "value": true},
    {"type": "price_modifier", "value": 0.9},
    {"type": "dialogue_option", "dialogue_id": "string"},
    {"type": "activity", "activity_id": "string"},
    {"type": "npc_moves_to", "zone_id": "string"},
    {"type": "free_shop", "value": true}
  ],
  "tags": ["tags"]
}
```

---

## 14. AI Integration

### 14.1 When AI Is Called

1. Once per week, during Dream Sequence, after loan payment resolution.
2. Player types prompt (prefilled: "I want to add...").
3. Game sends prompt + full context to AI.
4. AI returns structured JSON.
5. Game validates, applies changes, player wakes to new world.

### 14.2 Context Sent to AI

The wrapper prompt must include:

- All schemas (so AI knows valid structure)
- All current game content (items, NPCs, buildings, etc.)
- Current game state (week number, season, weather, player stats)
- Player's prompt

### 14.3 Wrapper Prompt Philosophy

- **Neutral:** Not inherently good or bad. Creates what player asks for.
- **Creative:** Should think through implications, connections, cascading content.
- **Detailed:** Generate complete content with all required fields.
- **Balanced:** New content should fit economically and mechanically.
- **Consequence-Aware:** Can add positive OR negative effects. Adding wolves might threaten livestock. Adding alcohol might create addiction mechanics.
- **No Restrictions:** Player can ask for anything. AI interprets creatively.

### 14.4 Response Format

```json
{
  "items": [],
  "mobs": [],
  "npcs": [],
  "buildings": [],
  "crops": [],
  "recipes": [],
  "quests": [],
  "activities": [],
  "entities": [],
  "crafting_stations": [],
  "dialogue_options": [],
  "friendship_tiers": [],
  "narrative_note": "What was added and why, for player to read"
}
```

### 14.5 Validation Layer

- All output validated against schemas.
- Missing required fields → reject or apply defaults.
- References to non-existent content → either cascade-generate or use fallbacks.
- IDs auto-generated if not provided.

---

## 15. Predefined Behaviors

AI cannot create new behaviors — only select from this list.

### 15.1 Mob Behaviors

| Behavior | Description |
|----------|-------------|
| passive_wander | Moves randomly, ignores player |
| passive_flee | Runs away if player approaches |
| grazing | Wanders slowly, stops to eat, stays in zone |
| hostile | Chases and attacks player on sight |
| territorial | Attacks only if player enters radius |
| nocturnal | Only active at Night phase |
| pack | Groups with same type, attacks together |
| pet_follow | Follows player when tamed |

### 15.2 NPC Activities

| Activity | Description |
|----------|-------------|
| sleeping | At home, not interactable |
| working | At shop/station, can trade |
| walking | Moving between locations |
| idle | Standing around, can talk |
| gathering | Collecting items in a zone |
| socializing | Talking to other NPCs |

---

## 16. Scope & Phases

### 16.1 MVP (Vertical Slice)

- Pixel art, tile-based, top-down view
- 3 zones: Farm, Village (5-6 buildings), Forest
- 3 starting NPCs with shops, quests, friendship
- 5 base crops, 2 base animals
- Basic mining (copper, iron)
- 3 crafting stations (workbench, furnace, anvil)
- Time system (3 phases), weather (sunny, rain)
- Inventory, hotbar, equipment
- Dream sequence prompting
- Save system with protected weekly saves
- 3-week main arc with achievements

### 16.2 Phase 2

- Beach zone, Mine interior (multiple levels)
- Fishing system
- NPC following and activities
- Player house interior, furniture placement
- Full 4-season cycle
- Snow weather
- More NPCs (6-8 total)

### 16.3 Phase 3 / Polish

- Adaptive music system
- Entity system (mounts, vehicles)
- AI-generated pixel sprites
- Events and festivals
- Extended post-game

### 16.4 Out of Scope

- Multiplayer
- 3D graphics
- Voice acting
- AI code generation (AI outputs data only)
- Infinite procedural terrain

---

## 17. Unity Implementation Notes

### 17.1 Architecture

- **Data-Driven:** All content is ScriptableObjects or JSON. No hardcoded entities.
- **Registry Pattern:** ItemRegistry, NPCRegistry, BuildingRegistry, etc. AI content added at runtime.
- **Event System:** TimeManager emits PhaseChanged, DayEnded, WeekEnded. Systems subscribe.

### 17.2 Key Managers

- **TimeManager:** Tracks phase, day, week, season. Emits events.
- **WeatherManager:** Current weather, affects NPC schedules, gameplay.
- **EconomyManager:** Gold, loan, transactions, price calculations.
- **RelationshipManager:** Friendship points, tier unlocks, NPC states.
- **AIIntegrationService:** API calls, context building, response parsing.
- **ContentApplicator:** Takes validated AI JSON, instantiates into world.
- **SaveManager:** Serializes world state, manages save slots, protected saves.
- **ZoneManager:** Handles zone transitions, pocket dimensions.

### 17.3 Sprite Strategy

- **Phase 1:** Curated sprite pool. AI picks closest match via tags.
- **Phase 2:** Placeholder sprites with text labels for AI content.
- **Phase 3:** AI-generated pixel sprites via image model (DALL-E, Stable Diffusion).

---

## 18. Example Prompts & Outputs

### 18.1 "Add cows and milk"

**Might Generate:**

- Mob: Cow (grazing, farm spawn, purchasable)
- Items: Milk Bucket (tool), Milk (product), Steak (product), Leather (material)
- NPC: Farmer Jeb (sells cows, buys milk)
- Building: Jeb's Ranch
- Recipe: Milk + Sugar → Butter
- Quest: "Deliver 10 Milk to Baker"

### 18.2 "Add cars"

**Might Generate:**

- Entity: Car (vehicle, 4x speed, fuel: Gasoline)
- Crafting Station: Mechanic's Bench
- Items: Engine, Wheels, Car Frame, Gasoline
- Recipes: Iron Bars → Engine, Rubber + Iron → Wheels, etc.
- NPC: Mechanic Mike (sells car parts)
- Building: Mike's Garage, Gas Station
- Possible: Oil as new mine resource

### 18.3 "Add marriage"

**Might Generate:**

- Friendship Tier: Partner (110 points)
- NPC: Goldsmith (sells rings)
- Building: Chapel
- Item: Wedding Ring (gift, triggers proposal)
- Dialogue Option: "Will you marry me?" (requires ring, 110+ friendship)
- Effects: Spouse moves to farm, free shop items, new dialogue

### 18.4 "Add wolves"

**Might Generate:**

- Mob: Wolf (hostile, pack, nocturnal, forest spawn)
- Item: Wolf Pelt (drop)
- **Consequence:** Wolves might attack livestock if they wander near farm at night
- Quest: "Protect the Farm" from new NPC or existing one

### 18.5 "Add alcohol"

**Might Generate:**

- Crop: Hops, Grapes
- Crafting Station: Brewing Barrel
- Items: Beer, Wine (consumables with effects)
- Building: The Rusty Hoe Pub
- NPC: Barkeep
- **Possible Consequence:** Drunk status effect, or NPC develops drinking habit

---

## 19. Next Steps

1. Review this document, finalize any remaining decisions.
2. Set up Unity project with core folder structure and architecture.
3. Build vertical slice: farm zone, time system, 1 crop, 1 NPC, sell flow.
4. Implement AI integration: API call, context builder, validator, applicator.
5. Test with 10+ sample prompts, iterate on schemas and wrapper prompt.
6. Expand to full MVP scope.

---

**Document Version:** 2.0  
**Last Updated:** December 2024  
**Status:** Ready for Development
