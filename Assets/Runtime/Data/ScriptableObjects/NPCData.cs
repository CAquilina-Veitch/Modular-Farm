using System;
using UnityEngine;
using Runtime.Data.Enums;
using Runtime.Data.Structs;

namespace Runtime.Data.ScriptableObjects
{
    /// <summary>
    /// GDD 13.3 - NPC definitions.
    /// NPCs have schedules, shops, gift preferences, and dialogue.
    /// Every NPC has a shop (both buying and selling), at least one quest, and friendship tracking.
    /// This is the most complex content type.
    /// </summary>
    [CreateAssetMenu(fileName = "NewNPC", menuName = "Prompt Harvest/NPC")]
    public class NPCData : ScriptableObject
    {
        [Header("Identity")]
        public string Id;
        public string Name;
        [Tooltip("Building ID where this NPC lives")]
        public string HomeBuildingId;

        [Header("Schedule")]
        [Tooltip("Where the NPC is during each day/phase (21 entries: 7 days x 3 phases)")]
        public ScheduleEntry[] Schedule;

        [Header("Shop - Selling")]
        [Tooltip("Item IDs this NPC sells")]
        public string[] ShopInventory;
        [Tooltip("Price multipliers for specific items")]
        public ShopPriceEntry[] ShopSellPrices;

        [Header("Shop - Buying")]
        [Tooltip("What this NPC will buy from player and at what price")]
        public BuyPreferenceEntry[] BuyPreferences;

        [Header("Gifts")]
        [Tooltip("Item IDs this NPC likes receiving")]
        public string[] GiftLikes;
        [Tooltip("Item IDs this NPC dislikes receiving")]
        public string[] GiftDislikes;

        [Header("Activities")]
        [Tooltip("How much this NPC enjoys various activities")]
        public ActivityPreferenceEntry[] ActivityPreferences;

        [Header("Dialogue")]
        [Tooltip("Personality tags for dialogue generation (e.g., friendly, grumpy, mysterious)")]
        public string[] DialogueTags;

        [Header("Weather Behavior")]
        [Tooltip("How this NPC behaves in different weather")]
        public WeatherBehaviorEntry[] WeatherBehavior;

        [Header("Visuals")]
        public string SpriteHint;
        public Sprite Sprite;

        [Header("Tags")]
        public string[] Tags;
    }
}
