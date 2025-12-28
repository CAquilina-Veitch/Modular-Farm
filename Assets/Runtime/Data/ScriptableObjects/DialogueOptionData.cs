using System;
using UnityEngine;
using Runtime.Data.Structs;

namespace Runtime.Data.ScriptableObjects
{
    /// <summary>
    /// GDD 13.11 - Dialogue option definitions.
    /// Dialogue options have conditions to appear and effects when chosen.
    /// Can trigger quests, give items, change friendship, or trigger events.
    /// </summary>
    [CreateAssetMenu(fileName = "NewDialogueOption", menuName = "Prompt Harvest/Dialogue Option")]
    public class DialogueOptionData : ScriptableObject
    {
        [Header("Identity")]
        public string Id;

        [Header("Player Text")]
        [Tooltip("What the player says when choosing this option")]
        [TextArea(1, 2)]
        public string Text;

        [Header("Conditions")]
        [Tooltip("Conditions that must be met for this option to appear")]
        public DialogueCondition[] Conditions;

        [Header("Effects")]
        [Tooltip("Effects triggered when this option is chosen")]
        public DialogueEffect[] Effects;

        [Header("NPC Response")]
        [Tooltip("What the NPC says in response")]
        [TextArea(2, 4)]
        public string ResponseDialogue;

        [Header("Tags")]
        public string[] Tags;
    }
}
