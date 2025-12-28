using System;
using UnityEngine;
using Runtime.Data.Enums;

namespace Runtime.Data.ScriptableObjects
{
    /// <summary>
    /// GDD 13.9 - Entity definitions (vehicles, mounts, pets, automators).
    /// Entities are rideable/usable objects with fuel/feed requirements.
    /// Examples: Horse (mount), Car (vehicle), Roomba (automator).
    /// </summary>
    [CreateAssetMenu(fileName = "NewEntity", menuName = "Prompt Harvest/Entity")]
    public class EntityData : ScriptableObject
    {
        [Header("Identity")]
        public string Id;
        public string Name;
        public EntityType EntityType;

        [Header("Movement")]
        [Tooltip("Speed multiplier (1.0 = normal, 2.0 = double speed)")]
        public float SpeedMultiplier = 1f;

        [Header("Fuel/Feed")]
        [Tooltip("Item ID that this entity consumes")]
        public string FuelItemId;
        [Tooltip("Fuel consumed per in-game day")]
        public float FuelConsumptionRate;
        [Tooltip("Description of bonus effect when fueled")]
        public string FuelBonusEffect;

        [Header("Abilities")]
        [Tooltip("Special ability description (optional)")]
        public string SpecialAbility;

        [Header("Acquisition")]
        [Tooltip("Recipe ID to craft this entity (optional)")]
        public string CraftRecipeId;
        [Tooltip("NPC ID that sells this entity (optional)")]
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
