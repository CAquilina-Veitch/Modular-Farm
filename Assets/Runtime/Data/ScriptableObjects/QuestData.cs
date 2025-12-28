using System;
using UnityEngine;
using Runtime.Data.Enums;
using Runtime.Data.Structs;

namespace Runtime.Data.ScriptableObjects
{
    /// <summary>
    /// GDD 13.7 - Quest definitions.
    /// Quests have objectives, rewards, and optional prerequisites/deadlines.
    /// Types: Fetch, Deliver, Gather, Talk, Build, Hunt, Visit.
    /// </summary>
    [CreateAssetMenu(fileName = "NewQuest", menuName = "Prompt Harvest/Quest")]
    public class QuestData : ScriptableObject
    {
        [Header("Identity")]
        public string Id;
        public string Name;
        [Tooltip("NPC ID that gives this quest")]
        public string GiverNPCId;

        [Header("Description")]
        [TextArea(2, 4)]
        public string Description;
        public QuestType QuestType;

        [Header("Objectives")]
        [Tooltip("Steps to complete the quest")]
        public QuestObjective[] Objectives;

        [Header("Rewards")]
        public QuestRewards Rewards;

        [Header("Timing")]
        [Tooltip("Days to complete (0 = no deadline)")]
        public int DeadlineDays;
        [Tooltip("Whether this quest can be repeated")]
        public bool Repeatable;

        [Header("Prerequisites")]
        [Tooltip("Quest ID that must be completed first (optional)")]
        public string PrerequisiteQuestId;

        [Header("Tags")]
        public string[] Tags;
    }
}
