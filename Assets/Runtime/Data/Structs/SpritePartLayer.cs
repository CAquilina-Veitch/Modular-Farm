using System;
using UnityEngine;
using Runtime.Data.Enums;

namespace Runtime.Data.Structs
{
    /// <summary>
    /// A single layer in a composite sprite.
    /// Defines which part to use, its position, and coloring.
    /// </summary>
    [Serializable]
    public struct SpritePartLayer
    {
        [Tooltip("The sprite part to use for this layer")]
        public string PartId;

        [Tooltip("Offset from sprite center in pixels")]
        public Vector2Int Offset;

        [Tooltip("Layer order (higher = on top)")]
        public int SortOrder;

        [Tooltip("Which tint channel affects this layer")]
        public TintChannel TintChannel;

        [Tooltip("Flip horizontally")]
        public bool FlipX;

        [Tooltip("Flip vertically")]
        public bool FlipY;

        [Tooltip("Rotation in degrees (0, 90, 180, 270)")]
        public int Rotation;

        public SpritePartLayer(string partId, int sortOrder = 0)
        {
            PartId = partId;
            Offset = Vector2Int.zero;
            SortOrder = sortOrder;
            TintChannel = TintChannel.None;
            FlipX = false;
            FlipY = false;
            Rotation = 0;
        }
    }
}
