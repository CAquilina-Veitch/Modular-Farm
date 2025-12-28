using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Runtime.Data.Enums;
using Runtime.Data.ScriptableObjects;
using Runtime.Data.Structs;

namespace Editor.Sprites
{
    /// <summary>
    /// Editor window for visually composing modular sprites.
    /// </summary>
    public class SpriteCompositorWindow : EditorWindow
    {
        // Canvas settings
        private Vector2Int canvasSize = new Vector2Int(32, 32);
        private float pixelsPerUnit = 16f;
        private int previewZoom = 8;

        // Layers
        private List<LayerEntry> layers = new();
        private int selectedLayerIndex = -1;

        // Tints
        private TintColors tintColors = TintColors.White;

        // Preview
        private Texture2D previewTexture;
        private bool autoRefresh = true;

        // Scroll positions
        private Vector2 layerScrollPos;
        private Vector2 partPickerScrollPos;

        // Part picker
        private SpritePartData[] availableParts;
        private string partSearchFilter = "";
        private SpritePartType? partTypeFilter = null;

        // Save settings
        private string assetName = "NewCompositeSprite";
        private string savePath = "Assets/Data/CompositeSprites";

        [MenuItem("Prompt Harvest/Sprite Compositor")]
        public static void ShowWindow()
        {
            var window = GetWindow<SpriteCompositorWindow>("Sprite Compositor");
            window.minSize = new Vector2(800, 600);
        }

        private void OnEnable()
        {
            RefreshAvailableParts();
            RefreshPreview();
        }

        private void OnDisable()
        {
            if (previewTexture != null)
                DestroyImmediate(previewTexture);
        }

        private void RefreshAvailableParts()
        {
            var guids = AssetDatabase.FindAssets("t:SpritePartData");
            var parts = new List<SpritePartData>();

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var part = AssetDatabase.LoadAssetAtPath<SpritePartData>(path);
                if (part != null)
                    parts.Add(part);
            }

            availableParts = parts.ToArray();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();

            DrawLeftPanel();
            DrawCenterPanel();
            DrawRightPanel();

            EditorGUILayout.EndHorizontal();

            DrawBottomPanel();

            if (autoRefresh && GUI.changed)
                RefreshPreview();
        }

        private void DrawLeftPanel()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(250));

            EditorGUILayout.LabelField("Layers", EditorStyles.boldLabel);

            layerScrollPos = EditorGUILayout.BeginScrollView(layerScrollPos, GUILayout.Height(300));

            for (int i = 0; i < layers.Count; i++)
            {
                DrawLayerEntry(i);
            }

            EditorGUILayout.EndScrollView();

            // Layer controls
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+ Add Layer"))
            {
                layers.Add(new LayerEntry { SortOrder = layers.Count });
                selectedLayerIndex = layers.Count - 1;
            }

            GUI.enabled = selectedLayerIndex >= 0 && selectedLayerIndex < layers.Count;

            if (GUILayout.Button("Remove"))
            {
                layers.RemoveAt(selectedLayerIndex);
                selectedLayerIndex = Mathf.Min(selectedLayerIndex, layers.Count - 1);
            }

            if (GUILayout.Button("▲") && selectedLayerIndex > 0)
            {
                SwapLayers(selectedLayerIndex, selectedLayerIndex - 1);
                selectedLayerIndex--;
            }

            if (GUILayout.Button("▼") && selectedLayerIndex < layers.Count - 1)
            {
                SwapLayers(selectedLayerIndex, selectedLayerIndex + 1);
                selectedLayerIndex++;
            }

            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);

            if (selectedLayerIndex >= 0 && selectedLayerIndex < layers.Count)
            {
                DrawSelectedLayerProperties();
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawLayerEntry(int index)
        {
            var layer = layers[index];
            bool isSelected = index == selectedLayerIndex;

            var bgColor = isSelected ? new Color(0.3f, 0.5f, 0.8f, 0.3f) : Color.clear;
            var rect = EditorGUILayout.BeginHorizontal();
            EditorGUI.DrawRect(rect, bgColor);

            if (GUILayout.Button(isSelected ? "●" : "○", GUILayout.Width(20)))
            {
                selectedLayerIndex = index;
            }

            if (layer.Part != null && layer.Part.Sprite != null)
            {
                var thumbRect = GUILayoutUtility.GetRect(24, 24, GUILayout.Width(24));
                GUI.DrawTextureWithTexCoords(thumbRect, layer.Part.Sprite.texture, GetSpriteUVRect(layer.Part.Sprite));
            }
            else
            {
                GUILayout.Label("[None]", GUILayout.Width(50));
            }

            string partName = layer.Part != null ? layer.Part.Name : "Empty";
            GUILayout.Label(partName, GUILayout.ExpandWidth(true));
            GUILayout.Label($"#{layer.SortOrder}", GUILayout.Width(30));

            EditorGUILayout.EndHorizontal();
        }

        private void DrawSelectedLayerProperties()
        {
            EditorGUILayout.LabelField("Layer Properties", EditorStyles.boldLabel);

            var layer = layers[selectedLayerIndex];

            EditorGUI.BeginChangeCheck();

            layer.SortOrder = EditorGUILayout.IntField("Sort Order", layer.SortOrder);
            layer.Offset = EditorGUILayout.Vector2IntField("Offset", layer.Offset);
            layer.TintChannel = (TintChannel)EditorGUILayout.EnumPopup("Tint Channel", layer.TintChannel);
            layer.FlipX = EditorGUILayout.Toggle("Flip X", layer.FlipX);
            layer.FlipY = EditorGUILayout.Toggle("Flip Y", layer.FlipY);

            string[] rotOptions = { "0°", "90°", "180°", "270°" };
            int rotIndex = layer.Rotation / 90;
            rotIndex = EditorGUILayout.Popup("Rotation", rotIndex, rotOptions);
            layer.Rotation = rotIndex * 90;

            if (EditorGUI.EndChangeCheck())
            {
                layers[selectedLayerIndex] = layer;
            }
        }

        private void DrawCenterPanel()
        {
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));

            EditorGUILayout.LabelField("Preview", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            canvasSize = EditorGUILayout.Vector2IntField("Canvas", canvasSize);
            previewZoom = EditorGUILayout.IntSlider("Zoom", previewZoom, 1, 16);
            EditorGUILayout.EndHorizontal();

            pixelsPerUnit = EditorGUILayout.FloatField("Pixels Per Unit", pixelsPerUnit);

            EditorGUILayout.Space(10);

            int previewWidth = canvasSize.x * previewZoom;
            int previewHeight = canvasSize.y * previewZoom;

            var previewRect = GUILayoutUtility.GetRect(previewWidth, previewHeight);
            previewRect.width = previewWidth;
            previewRect.height = previewHeight;

            DrawCheckerboard(previewRect);

            if (previewTexture != null)
            {
                GUI.DrawTexture(previewRect, previewTexture, ScaleMode.StretchToFill, true);
            }

            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            autoRefresh = EditorGUILayout.Toggle("Auto Refresh", autoRefresh);
            if (GUILayout.Button("Refresh Now"))
                RefreshPreview();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private void DrawRightPanel()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(250));

            // Tint colors
            EditorGUILayout.LabelField("Tint Colors", EditorStyles.boldLabel);

            tintColors.Primary = EditorGUILayout.ColorField("Primary", tintColors.Primary);
            tintColors.Secondary = EditorGUILayout.ColorField("Secondary", tintColors.Secondary);
            tintColors.Tertiary = EditorGUILayout.ColorField("Tertiary", tintColors.Tertiary);
            tintColors.Glow = EditorGUILayout.ColorField("Glow", tintColors.Glow);

            // Preset buttons - row 1
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("White")) tintColors = TintColors.White;
            if (GUILayout.Button("Wood")) tintColors = TintColors.Wood;
            if (GUILayout.Button("Iron")) tintColors = TintColors.Iron;
            EditorGUILayout.EndHorizontal();

            // Preset buttons - row 2
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Gold")) tintColors = TintColors.Gold;
            if (GUILayout.Button("Copper")) tintColors = TintColors.Copper;
            if (GUILayout.Button("Bronze")) tintColors = TintColors.Bronze;
            EditorGUILayout.EndHorizontal();

            // Preset buttons - row 3
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Stone")) tintColors = TintColors.Stone;
            if (GUILayout.Button("Silver")) tintColors = TintColors.Silver;
            if (GUILayout.Button("Crystal")) tintColors = TintColors.Crystal;
            EditorGUILayout.EndHorizontal();

            // Preset buttons - row 4 (gems)
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Ruby")) tintColors = TintColors.Ruby;
            if (GUILayout.Button("Emerald")) tintColors = TintColors.Emerald;
            if (GUILayout.Button("Sapphire")) tintColors = TintColors.Sapphire;
            EditorGUILayout.EndHorizontal();

            // Preset buttons - row 5
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Leather")) tintColors = TintColors.Leather;
            if (GUILayout.Button("Cloth")) tintColors = TintColors.Cloth;
            if (GUILayout.Button("Bone")) tintColors = TintColors.Bone;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(20);

            // Part picker
            EditorGUILayout.LabelField("Part Picker", EditorStyles.boldLabel);

            if (GUILayout.Button("Refresh Parts"))
                RefreshAvailableParts();

            partSearchFilter = EditorGUILayout.TextField("Search", partSearchFilter);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Type", GUILayout.Width(40));
            if (partTypeFilter.HasValue)
            {
                partTypeFilter = (SpritePartType)EditorGUILayout.EnumPopup(partTypeFilter.Value);
                if (GUILayout.Button("X", GUILayout.Width(20)))
                    partTypeFilter = null;
            }
            else
            {
                if (GUILayout.Button("Filter by type..."))
                    partTypeFilter = SpritePartType.Handle;
            }
            EditorGUILayout.EndHorizontal();

            partPickerScrollPos = EditorGUILayout.BeginScrollView(partPickerScrollPos, GUILayout.Height(200));

            if (availableParts == null || availableParts.Length == 0)
            {
                EditorGUILayout.HelpBox("No SpritePartData assets found.\nCreate parts via Prompt Harvest > Sprite > Part", MessageType.Info);
            }
            else
            {
                foreach (var part in availableParts)
                {
                    if (!MatchesFilter(part))
                        continue;

                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

                    if (part.Sprite != null)
                    {
                        var thumbRect = GUILayoutUtility.GetRect(24, 24, GUILayout.Width(24));
                        GUI.DrawTextureWithTexCoords(thumbRect, part.Sprite.texture, GetSpriteUVRect(part.Sprite));
                    }

                    GUILayout.Label(part.Name, GUILayout.ExpandWidth(true));

                    if (GUILayout.Button("+", GUILayout.Width(25)))
                    {
                        var newLayer = new LayerEntry
                        {
                            Part = part,
                            SortOrder = layers.Count,
                            TintChannel = part.DefaultTintChannel
                        };
                        layers.Add(newLayer);
                        selectedLayerIndex = layers.Count - 1;
                    }

                    if (GUILayout.Button("Set", GUILayout.Width(35)))
                    {
                        if (selectedLayerIndex >= 0 && selectedLayerIndex < layers.Count)
                        {
                            var layer = layers[selectedLayerIndex];
                            layer.Part = part;
                            layer.TintChannel = part.DefaultTintChannel;
                            layers[selectedLayerIndex] = layer;
                        }
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();
        }

        private void DrawBottomPanel()
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

            assetName = EditorGUILayout.TextField("Name", assetName);
            savePath = EditorGUILayout.TextField("Path", savePath);

            if (GUILayout.Button("Save Asset", GUILayout.Width(100)))
                SaveAsset();

            if (GUILayout.Button("Export PNG", GUILayout.Width(100)))
                ExportPng();

            EditorGUILayout.EndHorizontal();
        }

        private bool MatchesFilter(SpritePartData part)
        {
            if (!string.IsNullOrEmpty(partSearchFilter))
            {
                if (!part.Name.ToLower().Contains(partSearchFilter.ToLower()) &&
                    !part.Id.ToLower().Contains(partSearchFilter.ToLower()))
                    return false;
            }

            if (partTypeFilter.HasValue && part.Type != partTypeFilter.Value)
                return false;

            return true;
        }

        private void SwapLayers(int a, int b)
        {
            (layers[a], layers[b]) = (layers[b], layers[a]);
        }

        private void RefreshPreview()
        {
            if (previewTexture != null)
                DestroyImmediate(previewTexture);

            previewTexture = new Texture2D(canvasSize.x, canvasSize.y, TextureFormat.RGBA32, false);
            previewTexture.filterMode = FilterMode.Point;

            var clearPixels = new Color[canvasSize.x * canvasSize.y];
            for (int i = 0; i < clearPixels.Length; i++)
                clearPixels[i] = Color.clear;
            previewTexture.SetPixels(clearPixels);

            var sortedLayers = new List<LayerEntry>(layers);
            sortedLayers.Sort((a, b) => a.SortOrder.CompareTo(b.SortOrder));

            foreach (var layer in sortedLayers)
            {
                if (layer.Part == null || layer.Part.Sprite == null)
                    continue;

                RenderLayer(previewTexture, layer);
            }

            previewTexture.Apply();
        }

        private void RenderLayer(Texture2D output, LayerEntry layer)
        {
            var sprite = layer.Part.Sprite;
            var sourceTexture = sprite.texture;

            var spriteRect = sprite.rect;
            int srcX = Mathf.FloorToInt(spriteRect.x);
            int srcY = Mathf.FloorToInt(spriteRect.y);
            int srcWidth = Mathf.FloorToInt(spriteRect.width);
            int srcHeight = Mathf.FloorToInt(spriteRect.height);

            int destX = (canvasSize.x - srcWidth) / 2 + layer.Offset.x - layer.Part.PivotOffset.x;
            int destY = (canvasSize.y - srcHeight) / 2 + layer.Offset.y - layer.Part.PivotOffset.y;

            Color tint = GetTintColor(layer.TintChannel);

            for (int y = 0; y < srcHeight; y++)
            {
                for (int x = 0; x < srcWidth; x++)
                {
                    int readX = layer.FlipX ? (srcWidth - 1 - x) : x;
                    int readY = layer.FlipY ? (srcHeight - 1 - y) : y;

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

                    if (writeX < 0 || writeX >= canvasSize.x ||
                        writeY < 0 || writeY >= canvasSize.y)
                        continue;

                    Color srcPixel = sourceTexture.GetPixel(srcX + readX, srcY + readY);

                    if (srcPixel.a < 0.01f)
                        continue;

                    if (layer.Part.Tintable && layer.TintChannel != TintChannel.None)
                    {
                        srcPixel.r *= tint.r;
                        srcPixel.g *= tint.g;
                        srcPixel.b *= tint.b;
                    }

                    Color destPixel = output.GetPixel(writeX, writeY);
                    Color blended = AlphaBlend(destPixel, srcPixel);
                    output.SetPixel(writeX, writeY, blended);
                }
            }
        }

        private Color GetTintColor(TintChannel channel)
        {
            return channel switch
            {
                TintChannel.Primary => tintColors.Primary,
                TintChannel.Secondary => tintColors.Secondary,
                TintChannel.Tertiary => tintColors.Tertiary,
                TintChannel.Glow => tintColors.Glow,
                _ => Color.white
            };
        }

        private Color AlphaBlend(Color bg, Color fg)
        {
            float outA = fg.a + bg.a * (1 - fg.a);
            if (outA < 0.001f)
                return Color.clear;

            float outR = (fg.r * fg.a + bg.r * bg.a * (1 - fg.a)) / outA;
            float outG = (fg.g * fg.a + bg.g * bg.a * (1 - fg.a)) / outA;
            float outB = (fg.b * fg.a + bg.b * bg.a * (1 - fg.a)) / outA;

            return new Color(outR, outG, outB, outA);
        }

        private Rect GetSpriteUVRect(Sprite sprite)
        {
            var tex = sprite.texture;
            var rect = sprite.rect;
            return new Rect(
                rect.x / tex.width,
                rect.y / tex.height,
                rect.width / tex.width,
                rect.height / tex.height
            );
        }

        private void DrawCheckerboard(Rect rect)
        {
            int checkSize = previewZoom;
            for (int y = 0; y < rect.height; y += checkSize)
            {
                for (int x = 0; x < rect.width; x += checkSize)
                {
                    bool isDark = ((x / checkSize) + (y / checkSize)) % 2 == 0;
                    EditorGUI.DrawRect(
                        new Rect(rect.x + x, rect.y + y, checkSize, checkSize),
                        isDark ? new Color(0.3f, 0.3f, 0.3f) : new Color(0.4f, 0.4f, 0.4f)
                    );
                }
            }
        }

        private void SaveAsset()
        {
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            string fullPath = $"{savePath}/{assetName}.asset";

            var asset = CreateInstance<CompositeSpriteDef>();
            asset.Id = assetName.ToLower().Replace(" ", "_");
            asset.Name = assetName;
            asset.CanvasSize = canvasSize;
            asset.PixelsPerUnit = pixelsPerUnit;
            asset.DefaultTints = tintColors;

            var spriteLayers = new SpritePartLayer[layers.Count];
            for (int i = 0; i < layers.Count; i++)
            {
                var layer = layers[i];
                spriteLayers[i] = new SpritePartLayer
                {
                    PartId = layer.Part != null ? layer.Part.Id : "",
                    Offset = layer.Offset,
                    SortOrder = layer.SortOrder,
                    TintChannel = layer.TintChannel,
                    FlipX = layer.FlipX,
                    FlipY = layer.FlipY,
                    Rotation = layer.Rotation
                };
            }
            asset.Layers = spriteLayers;

            AssetDatabase.CreateAsset(asset, fullPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;

            Debug.Log($"Saved CompositeSpriteDef to {fullPath}");
        }

        private void ExportPng()
        {
            if (previewTexture == null)
            {
                Debug.LogWarning("No preview to export");
                return;
            }

            string path = EditorUtility.SaveFilePanel("Export PNG", "", assetName, "png");
            if (string.IsNullOrEmpty(path))
                return;

            byte[] pngData = previewTexture.EncodeToPNG();
            File.WriteAllBytes(path, pngData);

            Debug.Log($"Exported PNG to {path}");
            AssetDatabase.Refresh();
        }

        [System.Serializable]
        private struct LayerEntry
        {
            public SpritePartData Part;
            public Vector2Int Offset;
            public int SortOrder;
            public TintChannel TintChannel;
            public bool FlipX;
            public bool FlipY;
            public int Rotation;
        }
    }
}
