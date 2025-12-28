using System;
using UnityEngine;

namespace Runtime.Data.Structs
{
    /// <summary>
    /// Defines a display slot position for station sprites.
    /// Used to show representative items on crafting stations.
    /// </summary>
    [Serializable]
    public struct SlotPosition
    {
        [Tooltip("Offset from station center in pixels")]
        public Vector2Int Offset;

        [Tooltip("Scale factor for displayed items (0.5 = half size)")]
        [Range(0.1f, 1f)]
        public float Scale;

        public SlotPosition(int x, int y, float scale = 0.5f)
        {
            Offset = new Vector2Int(x, y);
            Scale = scale;
        }

        // Common slot layouts for 32x32 stations
        public static SlotPosition Left => new SlotPosition(-8, 8, 0.5f);
        public static SlotPosition Center => new SlotPosition(0, 10, 0.5f);
        public static SlotPosition Right => new SlotPosition(8, 8, 0.5f);
    }
}
