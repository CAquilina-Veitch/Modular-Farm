using UnityEngine;
using Runtime.Data.Structs;

namespace Runtime.Data.ScriptableObjects
{
    /// <summary>
    /// Definition of a composite sprite made from multiple parts.
    /// This is the "recipe" for assembling an item sprite.
    ///
    /// Example: Iron Pickaxe = wooden_handle (layer 0) + iron_pickaxe_head (layer 1)
    /// </summary>
    [CreateAssetMenu(fileName = "NewCompositeSprite", menuName = "Prompt Harvest/Sprite/Composite")]
    public class CompositeSpriteDef : ScriptableObject
    {
        [Header("Identity")]
        public string Id;
        public string Name;

        [Header("Canvas")]
        [Tooltip("Size of the output sprite in pixels")]
        public Vector2Int CanvasSize = new Vector2Int(32, 32);

        [Tooltip("Pixels per unit for the generated sprite")]
        public float PixelsPerUnit = 16f;

        [Header("Layers")]
        [Tooltip("Parts that make up this sprite, rendered in order")]
        public SpritePartLayer[] Layers;

        [Header("Default Colors")]
        [Tooltip("Default tint colors for this composition")]
        public TintColors DefaultTints = TintColors.White;

        [Header("Variants")]
        [Tooltip("Named color variants (e.g., 'iron', 'gold', 'copper')")]
        public TintVariant[] Variants;
    }

    [System.Serializable]
    public struct TintVariant
    {
        public string Name;
        public TintColors Colors;
    }
}
