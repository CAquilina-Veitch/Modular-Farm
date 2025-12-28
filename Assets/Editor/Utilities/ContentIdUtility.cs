using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Runtime.Data.ScriptableObjects;

namespace Editor.Utilities
{
    /// <summary>
    /// Utility class for working with content IDs in the editor.
    /// Provides ID generation, validation, and content discovery.
    /// </summary>
    public static class ContentIdUtility
    {
        /// <summary>
        /// Generate a unique ID based on name and type.
        /// Format: {type_prefix}_{snake_case_name}
        /// </summary>
        public static string GenerateId(string name, Type contentType)
        {
            string prefix = GetTypePrefix(contentType);
            string snakeName = ToSnakeCase(name);
            return $"{prefix}_{snakeName}";
        }

        /// <summary>
        /// Get all content assets of a specific type from the project.
        /// </summary>
        public static List<T> GetAllContentOfType<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            return guids
                .Select(guid => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(asset => asset != null)
                .ToList();
        }

        /// <summary>
        /// Get all IDs of a specific content type.
        /// </summary>
        public static List<string> GetAllIdsOfType(Type contentType)
        {
            string[] guids = AssetDatabase.FindAssets($"t:{contentType.Name}");
            var ids = new List<string>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                if (asset != null)
                {
                    string id = GetIdFromAsset(asset);
                    if (!string.IsNullOrEmpty(id))
                    {
                        ids.Add(id);
                    }
                }
            }

            return ids;
        }

        /// <summary>
        /// Get the ID field value from a content asset.
        /// </summary>
        public static string GetIdFromAsset(ScriptableObject asset)
        {
            var idField = asset.GetType().GetField("Id", BindingFlags.Public | BindingFlags.Instance);
            return idField?.GetValue(asset) as string ?? "";
        }

        /// <summary>
        /// Check if an ID exists for a given content type.
        /// </summary>
        public static bool IdExists(string id, Type contentType)
        {
            return GetAllIdsOfType(contentType).Contains(id);
        }

        /// <summary>
        /// Find duplicate IDs across all content of a specific type.
        /// </summary>
        public static Dictionary<string, List<string>> FindDuplicateIds(Type contentType)
        {
            string[] guids = AssetDatabase.FindAssets($"t:{contentType.Name}");
            var idToAssets = new Dictionary<string, List<string>>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                if (asset != null)
                {
                    string id = GetIdFromAsset(asset);
                    if (!string.IsNullOrEmpty(id))
                    {
                        if (!idToAssets.ContainsKey(id))
                            idToAssets[id] = new List<string>();
                        idToAssets[id].Add(path);
                    }
                }
            }

            // Return only duplicates
            return idToAssets.Where(kvp => kvp.Value.Count > 1)
                             .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        /// <summary>
        /// Get all content types (ScriptableObjects that are game content).
        /// </summary>
        public static Type[] GetAllContentTypes()
        {
            return new Type[]
            {
                typeof(ItemData),
                typeof(MobData),
                typeof(NPCData),
                typeof(BuildingData),
                typeof(CropData),
                typeof(RecipeData),
                typeof(QuestData),
                typeof(ActivityData),
                typeof(EntityData),
                typeof(CraftingStationData),
                typeof(DialogueOptionData),
                typeof(FriendshipTierData)
            };
        }

        private static string GetTypePrefix(Type contentType)
        {
            return contentType.Name switch
            {
                "ItemData" => "item",
                "MobData" => "mob",
                "NPCData" => "npc",
                "BuildingData" => "building",
                "CropData" => "crop",
                "RecipeData" => "recipe",
                "QuestData" => "quest",
                "ActivityData" => "activity",
                "EntityData" => "entity",
                "CraftingStationData" => "station",
                "DialogueOptionData" => "dialogue",
                "FriendshipTierData" => "tier",
                _ => "content"
            };
        }

        private static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            return string.Concat(input.Select((c, i) =>
                i > 0 && char.IsUpper(c) ? "_" + char.ToLower(c) : char.ToLower(c).ToString()))
                .Replace(" ", "_")
                .Replace("__", "_");
        }
    }
}
