using System;
using UnityEngine;
using Runtime.Data.Enums;
using Runtime.Data.Structs;

namespace Runtime.Data.ScriptableObjects
{
    /// <summary>
    /// GDD 13.4 - Building definitions.
    /// Buildings are placed in zones and can have interiors (pocket dimensions).
    /// Entry conditions control when the player can enter.
    /// </summary>
    [CreateAssetMenu(fileName = "NewBuilding", menuName = "Prompt Harvest/Building")]
    public class BuildingData : ScriptableObject
    {
        [Header("Identity")]
        public string Id;
        public string Name;
        [Tooltip("Zone where this building is located")]
        public Zone Zone;

        [Header("Exterior Size")]
        [Tooltip("Footprint in tiles")]
        public TileSize SizeTiles;
        [Tooltip("Door tile position relative to building origin")]
        public TilePosition DoorTile;

        [Header("Interior")]
        [Tooltip("Interior size (None = no interior)")]
        public InteriorSize InteriorSize;
        [Tooltip("Interior layout as flattened tile ID array (row-major)")]
        public string[] InteriorLayout;

        [Header("Ownership")]
        [Tooltip("NPC ID that owns this building (optional)")]
        public string OwnerNPCId;

        [Header("Entry Conditions")]
        [Tooltip("Conditions to enter this building")]
        public EntryCondition[] EntryConditions;

        [Header("Visuals")]
        public string SpriteHint;
        public Sprite Sprite;

        [Header("Tags")]
        public string[] Tags;
    }
}
