using System.Collections.Generic;
using UnityEngine;
using Runtime.Data.Enums;
using Runtime.Data.ScriptableObjects;
using Runtime.Data.Structs;

namespace Runtime.Sprites
{
    /// <summary>
    /// Composes sprites from parts at runtime.
    /// Creates textures by layering and tinting sprite parts.
    /// </summary>
    public static class SpriteCompositor
    {
        private static readonly Dictionary<string, SpritePartData> PartCache = new();

        /// <summary>
        /// Register a sprite part for use in compositions.
        /// </summary>
        public static void RegisterPart(SpritePartData part)
        {
            if (part != null && !string.IsNullOrEmpty(part.Id))
                PartCache[part.Id] = part;
        }

        /// <summary>
        /// Check if a part is registered.
        /// </summary>
        public static bool HasPart(string partId)
        {
            return PartCache.ContainsKey(partId);
        }

        /// <summary>
        /// Get a registered part by ID.
        /// </summary>
        public static SpritePartData GetPart(string partId)
        {
            return PartCache.TryGetValue(partId, out var part) ? part : null;
        }

        /// <summary>
        /// Compose a sprite from a definition using default tints.
        /// </summary>
        public static Sprite Compose(CompositeSpriteDef def)
        {
            return Compose(def, def.DefaultTints);
        }

        /// <summary>
        /// Compose a sprite from a definition with specific tint colors.
        /// </summary>
        public static Sprite Compose(CompositeSpriteDef def, TintColors tints)
        {
            var output = new Texture2D(
                def.CanvasSize.x,
                def.CanvasSize.y,
                TextureFormat.RGBA32,
                false
            );
            output.filterMode = FilterMode.Point;

            // Clear to transparent
            var clearPixels = new Color[def.CanvasSize.x * def.CanvasSize.y];
            for (int i = 0; i < clearPixels.Length; i++)
                clearPixels[i] = Color.clear;
            output.SetPixels(clearPixels);

            // Sort layers by sort order
            var sortedLayers = new List<SpritePartLayer>(def.Layers);
            sortedLayers.Sort((a, b) => a.SortOrder.CompareTo(b.SortOrder));

            // Render each layer
            foreach (var layer in sortedLayers)
            {
                if (string.IsNullOrEmpty(layer.PartId))
                    continue;

                if (!PartCache.TryGetValue(layer.PartId, out var part))
                {
                    Debug.LogWarning($"Sprite part not found: {layer.PartId}");
                    continue;
                }

                if (part.Sprite == null)
                    continue;

                RenderLayer(output, part, layer, tints, def.CanvasSize);
            }

            output.Apply();

            return Sprite.Create(
                output,
                new Rect(0, 0, def.CanvasSize.x, def.CanvasSize.y),
                new Vector2(0.5f, 0.5f),
                def.PixelsPerUnit
            );
        }

        /// <summary>
        /// Compose a sprite using a named variant.
        /// </summary>
        public static Sprite Compose(CompositeSpriteDef def, string variantName)
        {
            foreach (var variant in def.Variants)
            {
                if (variant.Name == variantName)
                    return Compose(def, variant.Colors);
            }

            Debug.LogWarning($"Variant not found: {variantName}, using default");
            return Compose(def);
        }

        private static void RenderLayer(
            Texture2D output,
            SpritePartData part,
            SpritePartLayer layer,
            TintColors tints,
            Vector2Int canvasSize)
        {
            var sprite = part.Sprite;
            var sourceTexture = sprite.texture;

            // Get sprite rect in texture
            var spriteRect = sprite.rect;
            int srcX = Mathf.FloorToInt(spriteRect.x);
            int srcY = Mathf.FloorToInt(spriteRect.y);
            int srcWidth = Mathf.FloorToInt(spriteRect.width);
            int srcHeight = Mathf.FloorToInt(spriteRect.height);

            // Calculate destination position (centered + offset)
            int destX = (canvasSize.x - srcWidth) / 2 + layer.Offset.x - part.PivotOffset.x;
            int destY = (canvasSize.y - srcHeight) / 2 + layer.Offset.y - part.PivotOffset.y;

            // Get tint color
            Color tint = GetTintColor(layer.TintChannel, tints);

            // Copy pixels with tinting
            for (int y = 0; y < srcHeight; y++)
            {
                for (int x = 0; x < srcWidth; x++)
                {
                    // Handle flipping
                    int readX = layer.FlipX ? (srcWidth - 1 - x) : x;
                    int readY = layer.FlipY ? (srcHeight - 1 - y) : y;

                    // Handle rotation (90-degree increments)
                    int finalX = x;
                    int finalY = y;
                    switch (layer.Rotation)
                    {
                        case 90:
                            finalX = y;
                            finalY = srcWidth - 1 - x;
                            break;
                        case 180:
                            finalX = srcWidth - 1 - x;
                            finalY = srcHeight - 1 - y;
                            break;
                        case 270:
                            finalX = srcHeight - 1 - y;
                            finalY = x;
                            break;
                    }

                    int writeX = destX + finalX;
                    int writeY = destY + finalY;

                    // Bounds check
                    if (writeX < 0 || writeX >= canvasSize.x ||
                        writeY < 0 || writeY >= canvasSize.y)
                        continue;

                    // Read source pixel
                    Color srcPixel = sourceTexture.GetPixel(srcX + readX, srcY + readY);

                    if (srcPixel.a < 0.01f)
                        continue;

                    // Apply tint (multiply)
                    if (part.Tintable && layer.TintChannel != TintChannel.None)
                    {
                        srcPixel.r *= tint.r;
                        srcPixel.g *= tint.g;
                        srcPixel.b *= tint.b;
                    }

                    // Alpha blend onto output
                    Color destPixel = output.GetPixel(writeX, writeY);
                    Color blended = AlphaBlend(destPixel, srcPixel);
                    output.SetPixel(writeX, writeY, blended);
                }
            }
        }

        private static Color GetTintColor(TintChannel channel, TintColors tints)
        {
            return channel switch
            {
                TintChannel.Primary => tints.Primary,
                TintChannel.Secondary => tints.Secondary,
                TintChannel.Tertiary => tints.Tertiary,
                TintChannel.Glow => tints.Glow,
                _ => Color.white
            };
        }

        private static Color AlphaBlend(Color background, Color foreground)
        {
            float outA = foreground.a + background.a * (1 - foreground.a);
            if (outA < 0.001f)
                return Color.clear;

            float outR = (foreground.r * foreground.a + background.r * background.a * (1 - foreground.a)) / outA;
            float outG = (foreground.g * foreground.a + background.g * background.a * (1 - foreground.a)) / outA;
            float outB = (foreground.b * foreground.a + background.b * background.a * (1 - foreground.a)) / outA;

            return new Color(outR, outG, outB, outA);
        }

        /// <summary>
        /// Clear the part cache.
        /// </summary>
        public static void ClearCache()
        {
            PartCache.Clear();
        }

        /// <summary>
        /// Get all registered part IDs.
        /// </summary>
        public static IEnumerable<string> GetRegisteredPartIds()
        {
            return PartCache.Keys;
        }
    }
}
