using System;
using UnityEngine;

namespace Runtime.Data.Attributes
{
    /// <summary>
    /// Attribute to mark a string field as an ID reference to a specific content type.
    /// Used by editor tools to show a dropdown of available IDs instead of a text field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class IdReferenceAttribute : PropertyAttribute
    {
        public Type ContentType { get; }

        /// <summary>
        /// Mark this string field as an ID reference to the specified content type.
        /// </summary>
        /// <param name="contentType">The ScriptableObject type this ID references (e.g., typeof(ItemData))</param>
        public IdReferenceAttribute(Type contentType)
        {
            ContentType = contentType;
        }
    }
}
