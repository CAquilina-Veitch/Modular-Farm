using System;
using UnityEngine;
using Runtime.Data.Enums;
using Runtime.Data.Structs;

namespace Runtime.Data.ScriptableObjects
{
    /// <summary>
    /// GDD 13.2 - Mob definitions.
    /// Mobs are creatures with predefined behaviors (AI cannot create new behaviors).
    /// Can be harvestable, tameable, and purchasable.
    /// Examples: Chicken (grazing), Wolf (hostile, pack), Cow (grazing, harvestable).
    /// </summary>
    [CreateAssetMenu(fileName = "NewMob", menuName = "Prompt Harvest/Mob")]
    public class MobData : ScriptableObject
    {
        [Header("Identity")]
        public string Id;
        public string Name;

        [Header("Behavior")]
        [Tooltip("Predefined behavior type (AI cannot create new behaviors)")]
        public MobBehavior Behavior;

        [Header("Spawning")]
        [Tooltip("Zones where this mob can spawn")]
        public Zone[] SpawnZones;
        [Tooltip("Spawn weight (higher = more common)")]
        public float SpawnWeight = 1f;
        [Tooltip("Weather conditions for spawning")]
        public Weather[] SpawnWeather;
        [Tooltip("Seasons when this mob spawns")]
        public Season[] SpawnSeasons;

        [Header("Drops")]
        [Tooltip("Items dropped when killed/harvested")]
        public DropEntry[] Drops;

        [Header("Harvesting")]
        [Tooltip("Whether this mob can be harvested without killing")]
        public bool Harvestable;
        [Tooltip("Tool item ID required to harvest")]
        public string HarvestToolId;
        [Tooltip("Item ID produced when harvested")]
        public string HarvestResultId;
        [Tooltip("Hours until harvestable again")]
        public float HarvestCooldownHours;

        [Header("Taming")]
        [Tooltip("Item ID that can tame this mob (optional)")]
        public string TameItemId;

        [Header("Purchase")]
        [Tooltip("NPC ID that sells this mob (optional)")]
        public string PurchasableFromNPCId;
        [Tooltip("Purchase price in gold")]
        public int PurchasePrice;

        [Header("Visuals")]
        public string SpriteHint;
        public Sprite Sprite;

        [Header("Tags")]
        public string[] Tags;
    }
}
