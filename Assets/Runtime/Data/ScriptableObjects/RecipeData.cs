using System;
using UnityEngine;
using Runtime.Data.Enums;
using Runtime.Data.Structs;

namespace Runtime.Data.ScriptableObjects
{
    /// <summary>
    /// GDD 13.6 - Recipe definitions.
    /// Recipes transform input items into output items at crafting stations.
    /// Types: Crafting, Cooking, Processing, Smelting, Brewing.
    /// Patternless crafting - correct ingredients produce output automatically.
    /// </summary>
    [CreateAssetMenu(fileName = "NewRecipe", menuName = "Prompt Harvest/Recipe")]
    public class RecipeData : ScriptableObject
    {
        [Header("Identity")]
        public string Id;
        public string Name;
        public RecipeType RecipeType;

        [Header("Ingredients")]
        [Tooltip("Required input items")]
        public RecipeIngredient[] Inputs;

        [Header("Output")]
        [Tooltip("Resulting item and quantity")]
        public RecipeOutput Output;

        [Header("Station")]
        [Tooltip("Crafting station or building ID required")]
        public string StationRequiredId;

        [Header("Time")]
        [Tooltip("In-game hours to complete")]
        public float TimeHours;
        [Tooltip("Whether crafting this advances the time phase")]
        public bool AdvancesTimePhase;

        [Header("Tags")]
        public string[] Tags;
    }
}
