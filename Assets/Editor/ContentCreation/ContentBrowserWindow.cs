using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Runtime.Data.ScriptableObjects;
using Editor.Utilities;

namespace Editor.ContentCreation
{
    /// <summary>
    /// Visual browser for all game content.
    /// Shows content grouped by type with reference counts and search.
    /// </summary>
    public class ContentBrowserWindow : EditorWindow
    {
        private Vector2 scrollPosition;
        private string searchFilter = "";
        private int selectedTypeIndex = -1; // -1 = all types
        private Dictionary<Type, bool> foldoutState = new();
        private Dictionary<Type, List<ContentInfo>> contentCache = new();
        private bool needsRefresh = true;

        private static readonly Type[] ContentTypes = ContentIdUtility.GetAllContentTypes();
        private static readonly string[] ContentTypeNames =
            new[] { "All" }.Concat(Array.ConvertAll(ContentTypes, t => t.Name.Replace("Data", ""))).ToArray();

        [MenuItem("Prompt Harvest/Content Browser")]
        public static void ShowWindow()
        {
            var window = GetWindow<ContentBrowserWindow>("Content Browser");
            window.minSize = new Vector2(400, 500);
        }

        private void OnEnable()
        {
            needsRefresh = true;
        }

        private void OnFocus()
        {
            needsRefresh = true;
        }

        private void OnGUI()
        {
            DrawHeader();
            DrawToolbar();
            EditorGUILayout.Space(10);
            DrawContent();
        }

        private void DrawHeader()
        {
            EditorGUILayout.LabelField("Content Browser", EditorStyles.boldLabel);

            // Stats
            if (!needsRefresh)
            {
                int totalCount = contentCache.Values.Sum(list => list.Count);
                EditorGUILayout.LabelField($"Total: {totalCount} content items across {contentCache.Count} types");
            }
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            // Search
            GUILayout.Label("Search:", GUILayout.Width(50));
            searchFilter = EditorGUILayout.TextField(searchFilter, EditorStyles.toolbarSearchField, GUILayout.Width(200));

            // Type filter
            GUILayout.Label("Type:", GUILayout.Width(40));
            selectedTypeIndex = EditorGUILayout.Popup(selectedTypeIndex + 1, ContentTypeNames, EditorStyles.toolbarPopup, GUILayout.Width(120)) - 1;

            GUILayout.FlexibleSpace();

            // Refresh button
            if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
            {
                needsRefresh = true;
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawContent()
        {
            if (needsRefresh)
            {
                RefreshContentCache();
                needsRefresh = false;
            }

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            foreach (Type contentType in ContentTypes)
            {
                // Skip if type filter is set and doesn't match
                if (selectedTypeIndex >= 0 && ContentTypes[selectedTypeIndex] != contentType)
                    continue;

                if (!contentCache.TryGetValue(contentType, out var items))
                    continue;

                // Filter by search
                var filteredItems = items;
                if (!string.IsNullOrEmpty(searchFilter))
                {
                    filteredItems = items.Where(i =>
                        i.Name.IndexOf(searchFilter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        i.Id.IndexOf(searchFilter, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();
                }

                if (filteredItems.Count == 0)
                    continue;

                string typeName = contentType.Name.Replace("Data", "");

                if (!foldoutState.TryGetValue(contentType, out bool isOpen))
                {
                    foldoutState[contentType] = true;
                    isOpen = true;
                }

                foldoutState[contentType] = EditorGUILayout.Foldout(isOpen, $"{typeName} ({filteredItems.Count})", true, EditorStyles.foldoutHeader);

                if (foldoutState[contentType])
                {
                    EditorGUI.indentLevel++;
                    foreach (var item in filteredItems.OrderBy(i => i.Name))
                    {
                        DrawContentItem(item);
                    }
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.Space(5);
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawContentItem(ContentInfo item)
        {
            EditorGUILayout.BeginHorizontal();

            // Icon based on type
            GUILayout.Label(GetTypeIcon(item.Asset.GetType()), GUILayout.Width(20));

            // Name and ID
            if (GUILayout.Button($"{item.Name} ({item.Id})", EditorStyles.linkLabel))
            {
                Selection.activeObject = item.Asset;
                EditorGUIUtility.PingObject(item.Asset);
            }

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();
        }

        private void RefreshContentCache()
        {
            contentCache.Clear();

            foreach (Type contentType in ContentTypes)
            {
                var items = new List<ContentInfo>();
                string[] guids = AssetDatabase.FindAssets($"t:{contentType.Name}");

                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                    if (asset == null) continue;

                    items.Add(new ContentInfo
                    {
                        Asset = asset,
                        Id = ContentIdUtility.GetIdFromAsset(asset),
                        Name = GetAssetName(asset),
                        Path = path
                    });
                }

                contentCache[contentType] = items;
            }
        }

        private string GetAssetName(ScriptableObject asset)
        {
            var nameField = asset.GetType().GetField("Name");
            string name = nameField?.GetValue(asset) as string;
            return string.IsNullOrEmpty(name) ? asset.name : name;
        }

        private GUIContent GetTypeIcon(Type type)
        {
            // Use built-in Unity icons
            string iconName = type.Name switch
            {
                "ItemData" => "d_Prefab Icon",
                "NPCData" => "d_Avatar Icon",
                "MobData" => "d_AvatarMask Icon",
                "BuildingData" => "d_BuildSettings.Metro",
                "CropData" => "d_TreeEditor.Leaf",
                "RecipeData" => "d_FilterByType",
                "QuestData" => "d_Clipboard",
                "ActivityData" => "d_Animation Icon",
                "EntityData" => "d_UnityLogo",
                "CraftingStationData" => "d_Settings Icon",
                "DialogueOptionData" => "d_TextAsset Icon",
                "FriendshipTierData" => "d_Favorite Icon",
                _ => "d_ScriptableObject Icon"
            };
            return EditorGUIUtility.IconContent(iconName);
        }

        private class ContentInfo
        {
            public ScriptableObject Asset;
            public string Id;
            public string Name;
            public string Path;
        }
    }
}
