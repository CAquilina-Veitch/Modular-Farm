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

        public static TintColors Stone => new TintColors
        {
            Primary = new Color(0.5f, 0.5f, 0.5f),  // Gray
            Secondary = Color.white,
            Tertiary = Color.white,
            Glow = Color.clear
        };

        public static TintColors Bone => new TintColors
        {
            Primary = new Color(0.95f, 0.9f, 0.8f),  // Off-white
            Secondary = Color.white,
            Tertiary = Color.white,
            Glow = Color.clear
        };

        public static TintColors Bronze => new TintColors
        {
            Primary = new Color(0.8f, 0.5f, 0.2f),  // Bronze
            Secondary = Color.white,
            Tertiary = Color.white,
            Glow = Color.clear
        };

        public static TintColors Silver => new TintColors
        {
            Primary = new Color(0.75f, 0.75f, 0.8f),  // Silver
            Secondary = Color.white,
            Tertiary = Color.white,
            Glow = new Color(0.9f, 0.9f, 1f, 0.3f)
        };

        public static TintColors Crystal => new TintColors
        {
            Primary = new Color(0.7f, 0.85f, 1f),  // Light blue
            Secondary = Color.white,
            Tertiary = Color.white,
            Glow = new Color(0.8f, 0.9f, 1f, 0.5f)
        };

        public static TintColors Emerald => new TintColors
        {
            Primary = new Color(0.2f, 0.8f, 0.4f),  // Green
            Secondary = Color.white,
            Tertiary = Color.white,
            Glow = new Color(0.3f, 1f, 0.5f, 0.4f)
        };

        public static TintColors Ruby => new TintColors
        {
            Primary = new Color(0.9f, 0.1f, 0.2f),  // Red
            Secondary = Color.white,
            Tertiary = Color.white,
            Glow = new Color(1f, 0.2f, 0.3f, 0.4f)
        };

        public static TintColors Sapphire => new TintColors
        {
            Primary = new Color(0.2f, 0.3f, 0.9f),  // Blue
            Secondary = Color.white,
            Tertiary = Color.white,
            Glow = new Color(0.3f, 0.4f, 1f, 0.4f)
        };

        public static TintColors Leather => new TintColors
        {
            Primary = new Color(0.55f, 0.35f, 0.2f),  // Tan
            Secondary = Color.white,
            Tertiary = Color.white,
            Glow = Color.clear
        };

        public static TintColors Cloth => new TintColors
        {
            Primary = new Color(0.9f, 0.85f, 0.75f),  // Beige
            Secondary = Color.white,
            Tertiary = Color.white,
            Glow = Color.clear
        };
    }
}
