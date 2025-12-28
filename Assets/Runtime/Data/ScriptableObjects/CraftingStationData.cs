using System;
using UnityEngine;
using Runtime.Data.Enums;
using Runtime.Data.Structs;

namespace Runtime.Data.ScriptableObjects
{
    /// <summary>
    /// GDD 13.10 - Crafting station definitions.
    /// Stations enable specific recipe types. Base stations: Workbench, Furnace, Anvil.
    /// AI can generate new stations (e.g., Brewing Barrel, Loom, Mechanic's Bench).
    /// </summary>
    [CreateAssetMenu(fileName = "NewCraftingStation", menuName = "Prompt Harvest/Crafting Station")]
    public class CraftingStationData : ScriptableObject
    {
        [Header("Identity")]
        public string Id;
        public string Name;
        public StationType StationType;

        [Header("Recipes")]
        [Tooltip("Recipe IDs this station can perform")]
        public string[] RecipesEnabled;

        [Header("Placement")]
        [Tooltip("Zones where this station can be placed")]
        public Zone[] PlaceableZones;
        [Tooltip("Size in tiles")]
        public TileSize SizeTiles;

        [Header("Cost to Build")]
        [Tooltip("Materials required to craft this station")]
        public RecipeIngredient[] CraftCost;

        [Header("Visuals")]
        public string SpriteHint;
        public Sprite Sprite;

        [Header("Tags")]
        public string[] Tags;
    }
}
