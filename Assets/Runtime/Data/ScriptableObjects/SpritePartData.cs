using UnityEngine;
using Runtime.Data.Enums;

namespace Runtime.Data.ScriptableObjects
{
    /// <summary>
    /// Definition of a single sprite part (e.g., "wooden_handle", "iron_pickaxe_head").
    /// Parts are combined to create full item sprites.
    /// </summary>
    [CreateAssetMenu(fileName = "NewSpritePart", menuName = "Prompt Harvest/Sprite/Part")]
    public class SpritePartData : ScriptableObject
    {
        [Header("Identity")]
        public string Id;
        public string Name;
        public SpritePartType Type;

        [Header("Sprite")]
        [Tooltip("The sprite for this part. Should have transparent background.")]
        public Sprite Sprite;

        [Tooltip("Pivot point offset from center (in pixels)")]
        public Vector2Int PivotOffset;

        [Header("Tinting")]
        [Tooltip("Whether this part can be tinted")]
        public bool Tintable = true;

        [Tooltip("Default tint channel when used")]
        public TintChannel DefaultTintChannel = TintChannel.Primary;

        [Header("Tags")]
        [Tooltip("Tags for filtering (e.g., 'metal', 'organic', 'magical')")]
        public string[] Tags;
    }
}
