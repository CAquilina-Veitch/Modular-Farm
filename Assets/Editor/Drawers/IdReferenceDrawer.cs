using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Runtime.Data.Attributes;
using Editor.Utilities;

namespace Editor.Drawers
{
    /// <summary>
    /// Custom property drawer for string fields marked with [IdReference].
    /// Shows a searchable dropdown of available IDs instead of a text field.
    /// </summary>
    [CustomPropertyDrawer(typeof(IdReferenceAttribute))]
    public class IdReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            var attr = (IdReferenceAttribute)attribute;
            List<string> availableIds = ContentIdUtility.GetAllIdsOfType(attr.ContentType);

            // Add empty option at start
            availableIds.Insert(0, "(None)");

            string currentValue = property.stringValue;
            int currentIndex = string.IsNullOrEmpty(currentValue) ? 0 : availableIds.IndexOf(currentValue);

            // If current value not found, add it with warning
            bool isMissing = false;
            if (currentIndex < 0 && !string.IsNullOrEmpty(currentValue))
            {
                availableIds.Add($"{currentValue} (MISSING)");
                currentIndex = availableIds.Count - 1;
                isMissing = true;
            }

            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
            EditorGUI.LabelField(labelRect, label);

            // Draw dropdown
            Rect dropdownRect = new Rect(
                position.x + EditorGUIUtility.labelWidth + 2,
                position.y,
                position.width - EditorGUIUtility.labelWidth - 2,
                position.height);

            // Use red color if missing
            Color oldColor = GUI.backgroundColor;
            if (isMissing)
                GUI.backgroundColor = new Color(1f, 0.5f, 0.5f);

            int newIndex = EditorGUI.Popup(dropdownRect, currentIndex, availableIds.ToArray());

            GUI.backgroundColor = oldColor;

            // Update value
            if (newIndex != currentIndex)
            {
                if (newIndex == 0)
                    property.stringValue = "";
                else if (newIndex < availableIds.Count)
                    property.stringValue = availableIds[newIndex].Replace(" (MISSING)", "");
            }

            EditorGUI.EndProperty();
        }
    }
}
