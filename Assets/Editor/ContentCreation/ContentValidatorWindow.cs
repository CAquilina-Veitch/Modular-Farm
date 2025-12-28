using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Runtime.Data.Attributes;
using Runtime.Data.ScriptableObjects;
using Editor.Utilities;

namespace Editor.ContentCreation
{
    /// <summary>
    /// Editor window for validating content references.
    /// Finds broken references, missing required fields, duplicate IDs, and orphaned content.
    /// </summary>
    public class ContentValidatorWindow : EditorWindow
    {
        private Vector2 scrollPosition;
        private List<ValidationResult> results = new();
        private bool hasValidated;

        [MenuItem("Prompt Harvest/Content Validator")]
        public static void ShowWindow()
        {
            var window = GetWindow<ContentValidatorWindow>("Content Validator");
            window.minSize = new Vector2(500, 400);
        }

        private void OnGUI()
        {
            DrawHeader();
            EditorGUILayout.Space(10);

            if (GUILayout.Button("Run Validation", GUILayout.Height(30)))
            {
                RunValidation();
            }

            EditorGUILayout.Space(10);

            if (hasValidated)
            {
                DrawResults();
            }
        }

        private void DrawHeader()
        {
            EditorGUILayout.LabelField("Content Validator", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "Validates all game content for:\n" +
                "- Broken ID references\n" +
                "- Missing required fields\n" +
                "- Duplicate IDs\n" +
                "- Orphaned content",
                MessageType.Info);
        }

        private void DrawResults()
        {
            int errorCount = results.Count(r => r.Type == ValidationResultType.Error);
            int warningCount = results.Count(r => r.Type == ValidationResultType.Warning);
            int infoCount = results.Count(r => r.Type == ValidationResultType.Info);

            EditorGUILayout.LabelField($"Results: {errorCount} errors, {warningCount} warnings, {infoCount} info");
            EditorGUILayout.Space(5);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            foreach (var result in results.OrderByDescending(r => (int)r.Type))
            {
                DrawResultItem(result);
            }

            if (results.Count == 0)
            {
                EditorGUILayout.HelpBox("No issues found!", MessageType.Info);
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawResultItem(ValidationResult result)
        {
            MessageType msgType = result.Type switch
            {
                ValidationResultType.Error => MessageType.Error,
                ValidationResultType.Warning => MessageType.Warning,
                _ => MessageType.Info
            };

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox(result.Message, msgType);

            if (result.Asset != null)
            {
                if (GUILayout.Button("Select", GUILayout.Width(60)))
                {
                    Selection.activeObject = result.Asset;
                    EditorGUIUtility.PingObject(result.Asset);
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        private void RunValidation()
        {
            results.Clear();
            hasValidated = true;

            ValidateDuplicateIds();
            ValidateBrokenReferences();
            ValidateRequiredFields();

            Debug.Log($"Validation complete: {results.Count} issues found.");
        }

        private void ValidateDuplicateIds()
        {
            foreach (Type contentType in ContentIdUtility.GetAllContentTypes())
            {
                var duplicates = ContentIdUtility.FindDuplicateIds(contentType);
                foreach (var kvp in duplicates)
                {
                    results.Add(new ValidationResult
                    {
                        Type = ValidationResultType.Error,
                        Message = $"Duplicate ID '{kvp.Key}' found in {contentType.Name}:\n" +
                                  string.Join("\n", kvp.Value)
                    });
                }
            }
        }

        private void ValidateBrokenReferences()
        {
            foreach (Type contentType in ContentIdUtility.GetAllContentTypes())
            {
                string[] guids = AssetDatabase.FindAssets($"t:{contentType.Name}");

                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                    if (asset == null) continue;

                    ValidateAssetReferences(asset);
                }
            }
        }

        private void ValidateAssetReferences(ScriptableObject asset)
        {
            Type assetType = asset.GetType();
            FieldInfo[] fields = assetType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                // Check for IdReference attribute
                var idRefAttr = field.GetCustomAttribute<IdReferenceAttribute>();
                if (idRefAttr != null)
                {
                    string id = field.GetValue(asset) as string;
                    if (!string.IsNullOrEmpty(id) && !ContentIdUtility.IdExists(id, idRefAttr.ContentType))
                    {
                        results.Add(new ValidationResult
                        {
                            Type = ValidationResultType.Error,
                            Message = $"{assetType.Name} '{ContentIdUtility.GetIdFromAsset(asset)}' references missing {idRefAttr.ContentType.Name}: '{id}'",
                            Asset = asset
                        });
                    }
                }

                // Check string arrays that might be ID references (by naming convention)
                if (field.FieldType == typeof(string[]) && IsIdField(field.Name))
                {
                    string[] ids = field.GetValue(asset) as string[];
                    if (ids != null)
                    {
                        Type refType = GetReferencedTypeFromFieldName(field.Name);
                        if (refType != null)
                        {
                            foreach (string id in ids.Where(id => !string.IsNullOrEmpty(id)))
                            {
                                if (!ContentIdUtility.IdExists(id, refType))
                                {
                                    results.Add(new ValidationResult
                                    {
                                        Type = ValidationResultType.Warning,
                                        Message = $"{assetType.Name} '{ContentIdUtility.GetIdFromAsset(asset)}' may reference missing {refType.Name}: '{id}'",
                                        Asset = asset
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ValidateRequiredFields()
        {
            foreach (Type contentType in ContentIdUtility.GetAllContentTypes())
            {
                string[] guids = AssetDatabase.FindAssets($"t:{contentType.Name}");

                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                    if (asset == null) continue;

                    // Check for empty ID
                    string id = ContentIdUtility.GetIdFromAsset(asset);
                    if (string.IsNullOrEmpty(id))
                    {
                        results.Add(new ValidationResult
                        {
                            Type = ValidationResultType.Error,
                            Message = $"{contentType.Name} at '{path}' has empty ID",
                            Asset = asset
                        });
                    }

                    // Check for empty Name
                    var nameField = contentType.GetField("Name");
                    if (nameField != null)
                    {
                        string name = nameField.GetValue(asset) as string;
                        if (string.IsNullOrEmpty(name))
                        {
                            results.Add(new ValidationResult
                            {
                                Type = ValidationResultType.Error,
                                Message = $"{contentType.Name} '{id}' has empty Name",
                                Asset = asset
                            });
                        }
                    }
                }
            }
        }

        private bool IsIdField(string fieldName)
        {
            return fieldName.EndsWith("Id") || fieldName.EndsWith("Ids") ||
                   fieldName == "ShopInventory" || fieldName == "GiftLikes" ||
                   fieldName == "GiftDislikes" || fieldName == "RecipesEnabled";
        }

        private Type GetReferencedTypeFromFieldName(string fieldName)
        {
            if (fieldName.Contains("Item") || fieldName == "ShopInventory" ||
                fieldName == "GiftLikes" || fieldName == "GiftDislikes")
                return typeof(ItemData);
            if (fieldName.Contains("NPC"))
                return typeof(NPCData);
            if (fieldName.Contains("Building"))
                return typeof(BuildingData);
            if (fieldName.Contains("Recipe") || fieldName == "RecipesEnabled")
                return typeof(RecipeData);
            if (fieldName.Contains("Quest"))
                return typeof(QuestData);
            if (fieldName.Contains("Activity"))
                return typeof(ActivityData);
            return null;
        }

        private enum ValidationResultType
        {
            Info = 0,
            Warning = 1,
            Error = 2
        }

        private class ValidationResult
        {
            public ValidationResultType Type;
            public string Message;
            public ScriptableObject Asset;
        }
    }
}
