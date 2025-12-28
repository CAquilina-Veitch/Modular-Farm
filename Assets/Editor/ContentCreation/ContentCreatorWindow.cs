using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Runtime.Data.ScriptableObjects;
using Editor.Utilities;

namespace Editor.ContentCreation
{
    /// <summary>
    /// Unified editor window for creating any content type.
    /// Provides guided fields, auto-ID generation, and validation.
    /// </summary>
    public class ContentCreatorWindow : EditorWindow
    {
        private int selectedTypeIndex;
        private string contentName = "";
        private string contentId = "";
        private bool autoGenerateId = true;

        private ScriptableObject createdAsset;
        private UnityEditor.Editor assetEditor;

        private Vector2 scrollPosition;

        private static readonly Type[] ContentTypes = ContentIdUtility.GetAllContentTypes();
        private static readonly string[] ContentTypeNames = Array.ConvertAll(ContentTypes, t => t.Name.Replace("Data", ""));

        [MenuItem("Prompt Harvest/Content Creator")]
        public static void ShowWindow()
        {
            var window = GetWindow<ContentCreatorWindow>("Content Creator");
            window.minSize = new Vector2(400, 500);
        }

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            DrawHeader();
            EditorGUILayout.Space(10);

            DrawTypeSelector();
            EditorGUILayout.Space(10);

            DrawNameAndId();
            EditorGUILayout.Space(10);

            DrawAssetEditor();
            EditorGUILayout.Space(10);

            DrawCreateButton();

            EditorGUILayout.EndScrollView();
        }

        private void DrawHeader()
        {
            EditorGUILayout.LabelField("Content Creator", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "Create new game content. Fill in the fields below and click 'Create Asset' to save.",
                MessageType.Info);
        }

        private void DrawTypeSelector()
        {
            EditorGUILayout.LabelField("Content Type", EditorStyles.boldLabel);
            int newIndex = EditorGUILayout.Popup("Type", selectedTypeIndex, ContentTypeNames);

            if (newIndex != selectedTypeIndex)
            {
                selectedTypeIndex = newIndex;
                ResetCreatedAsset();
            }
        }

        private void DrawNameAndId()
        {
            EditorGUILayout.LabelField("Identity", EditorStyles.boldLabel);

            string newName = EditorGUILayout.TextField("Name", contentName);
            if (newName != contentName)
            {
                contentName = newName;
                if (autoGenerateId)
                {
                    contentId = ContentIdUtility.GenerateId(contentName, ContentTypes[selectedTypeIndex]);
                }
            }

            EditorGUILayout.BeginHorizontal();

            using (new EditorGUI.DisabledScope(autoGenerateId))
            {
                contentId = EditorGUILayout.TextField("ID", contentId);
            }

            autoGenerateId = EditorGUILayout.ToggleLeft("Auto", autoGenerateId, GUILayout.Width(50));

            EditorGUILayout.EndHorizontal();

            // Validation
            if (!string.IsNullOrEmpty(contentId))
            {
                if (ContentIdUtility.IdExists(contentId, ContentTypes[selectedTypeIndex]))
                {
                    EditorGUILayout.HelpBox($"ID '{contentId}' already exists!", MessageType.Warning);
                }
            }
        }

        private void DrawAssetEditor()
        {
            EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);

            EnsureAssetExists();

            if (createdAsset != null)
            {
                if (assetEditor == null || assetEditor.target != createdAsset)
                {
                    if (assetEditor != null)
                        DestroyImmediate(assetEditor);
                    assetEditor = UnityEditor.Editor.CreateEditor(createdAsset);
                }

                // Set name and ID on the temp asset
                SetAssetNameAndId();

                // Draw all properties except name/ID (we handle those above)
                assetEditor.OnInspectorGUI();
            }
        }

        private void DrawCreateButton()
        {
            EditorGUILayout.Space(10);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(contentName) || string.IsNullOrEmpty(contentId)))
            {
                if (GUILayout.Button("Create Asset", GUILayout.Height(30)))
                {
                    CreateAsset();
                }
            }
        }

        private void EnsureAssetExists()
        {
            if (createdAsset == null || createdAsset.GetType() != ContentTypes[selectedTypeIndex])
            {
                ResetCreatedAsset();
                createdAsset = ScriptableObject.CreateInstance(ContentTypes[selectedTypeIndex]);
            }
        }

        private void ResetCreatedAsset()
        {
            if (createdAsset != null)
            {
                DestroyImmediate(createdAsset);
                createdAsset = null;
            }
            if (assetEditor != null)
            {
                DestroyImmediate(assetEditor);
                assetEditor = null;
            }
        }

        private void SetAssetNameAndId()
        {
            var nameField = createdAsset.GetType().GetField("Name");
            var idField = createdAsset.GetType().GetField("Id");

            nameField?.SetValue(createdAsset, contentName);
            idField?.SetValue(createdAsset, contentId);
        }

        private void CreateAsset()
        {
            Type contentType = ContentTypes[selectedTypeIndex];
            string typeName = ContentTypeNames[selectedTypeIndex];
            string folderPath = $"Assets/Data/{typeName}s";

            // Ensure folder exists
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                string parentFolder = "Assets/Data";
                if (!AssetDatabase.IsValidFolder(parentFolder))
                {
                    AssetDatabase.CreateFolder("Assets", "Data");
                }
                AssetDatabase.CreateFolder(parentFolder, $"{typeName}s");
            }

            // Create the asset
            string assetPath = $"{folderPath}/{contentName}.asset";
            assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

            AssetDatabase.CreateAsset(createdAsset, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // Select the new asset
            Selection.activeObject = createdAsset;
            EditorGUIUtility.PingObject(createdAsset);

            Debug.Log($"Created {typeName}: {assetPath}");

            // Reset for next creation
            contentName = "";
            contentId = "";
            ResetCreatedAsset();
        }

        private void OnDestroy()
        {
            ResetCreatedAsset();
        }
    }
}
