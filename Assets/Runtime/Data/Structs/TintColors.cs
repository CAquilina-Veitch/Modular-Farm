using System;
using UnityEngine;

namespace Runtime.Data.Structs
{
    /// <summary>
    /// Color set for tinting a composite sprite.
    /// Defines colors for each tint channel.
    /// </summary>
    [Serializable]
    public struct TintColors
    {
        public Color Primary;
        public Color Secondary;
        public Color Tertiary;
        public Color Glow;

        public static TintColors White => new TintColors
        {
            Primary = Color.white,
            Secondary = Color.white,
            Tertiary = Color.white,
            Glow = Color.white
        };

        // Preset material colors
        public static TintColors Wood => new TintColors
        {
            Primary = new Color(0.6f, 0.4f, 0.2f),  // Brown
            Secondary = Color.white,
            Tertiary = Color.white,
            Glow = Color.clear
        };

        public static TintColors Iron => new TintColors
        {
            Primary = new Color(0.7f, 0.7f, 0.75f),  // Silver-gray
            Secondary = Color.white,
            Tertiary = Color.white,
            Glow = Color.clear
        };

        public static TintColors Gold => new TintColors
        {
            Primary = new Color(1f, 0.84f, 0f),  // Gold
            Secondary = Color.white,
            Tertiary = Color.white,
            Glow = new Color(1f, 0.9f, 0.5f, 0.5f)
        };

        public static TintColors Copper => new TintColors
        {
            Primary = new Color(0.72f, 0.45f, 0.2f),  // Copper
            Secondary = Color.white,
            Tertiary = Color.white,
            Glow = Color.clear
        };
    }
}
