using System;
using UnityEngine;
using Runtime.Data.Enums;

namespace Runtime.Data.ScriptableObjects
{
    /// <summary>
    /// GDD 13.1 - Core item data. Referenced by most other content types.
    /// Items include tools, materials, products, consumables, seeds, decorations, furniture, and gifts.
    /// </summary>
    [CreateAssetMenu(fileName = "NewItem", menuName = "Prompt Harvest/Item")]
    public class ItemData : ScriptableObject
    {
        [Header("Identity")]
        public string Id;
        public string Name;
        public ItemType Type;

        [Header("Description")]
        [TextArea(2, 4)]
        public string Description;

        [Header("Economy")]
        [Tooltip("Base sell price in gold")]
        public int Value;
        [Tooltip("Maximum stack size (default 99)")]
        public int StackMax = 99;

        [Header("Tool Properties")]
        [Tooltip("Uses before breaking (0 = unbreakable)")]
        public int Durability;
        [Tooltip("Tool tier: basic, copper, iron, gold, etc.")]
        public string Tier;

        [Header("Usage")]
        [Tooltip("Entity IDs this item can be used on")]
        public string[] UsedOn;
        [Tooltip("Item ID produced when used")]
        public string UseResult;
        [Tooltip("Time in seconds to use")]
        public float UseTime;

        [Header("Placement")]
        public bool Placeable;
        [Tooltip("Zones where this item can be placed")]
        public Zone[] PlaceableZones;

        [Header("Decay")]
        [Tooltip("Days until item decays (0 = no decay)")]
        public int DecayDays;

        [Header("Visuals")]
        [Tooltip("Hint for AI sprite generation")]
        public string SpriteHint;
        [Tooltip("Editor-assigned sprite")]
        public Sprite Sprite;

        [Header("Tags")]
        [Tooltip("Tags for categorization and AI reference")]
        public string[] Tags;
    }
}
