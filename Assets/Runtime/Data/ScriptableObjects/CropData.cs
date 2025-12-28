using System;
using UnityEngine;
using Runtime.Data.Enums;
using Runtime.Data.Structs;

namespace Runtime.Data.ScriptableObjects
{
    /// <summary>
    /// GDD 13.5 - Crop definitions.
    /// Crops grow over time, require watering, and are seasonal.
    /// Some crops regrow after harvest.
    /// </summary>
    [CreateAssetMenu(fileName = "NewCrop", menuName = "Prompt Harvest/Crop")]
    public class CropData : ScriptableObject
    {
        [Header("Identity")]
        public string Id;
        public string Name;

        [Header("Seed")]
        [Tooltip("Item ID of the seed that plants this crop")]
        public string SeedItemId;

        [Header("Growth")]
        [Tooltip("Days to fully grow")]
        public int GrowDays;
        [Tooltip("Watering requirements")]
        public WaterNeeds WaterNeeds;
        [Tooltip("Seasons when this crop can grow")]
        public Season[] Seasons;
        [Tooltip("Number of visual growth stages")]
        public int GrowthStages;

        [Header("Harvest")]
        [Tooltip("Item ID produced when harvested")]
        public string HarvestItemId;
        [Tooltip("Min/max quantity when harvested")]
        public HarvestQuantity HarvestQuantity;
        [Tooltip("Whether the crop regrows after harvest")]
        public bool Regrows;
        [Tooltip("Days to regrow after harvest (if Regrows is true)")]
        public int RegrowDays;

        [Header("Visuals")]
        [Tooltip("Sprite hints for each growth stage")]
        public string[] SpriteHints;
        [Tooltip("Editor-assigned sprites for each growth stage")]
        public Sprite[] StageSprites;

        [Header("Tags")]
        public string[] Tags;
    }
}
