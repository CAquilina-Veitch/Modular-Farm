using System;
using UnityEngine;
using Runtime.Data.Enums;
using Runtime.Data.Structs;

namespace Runtime.Data.ScriptableObjects
{
    /// <summary>
    /// GDD 13.8 - Activities that can be done with NPCs.
    /// Activities are zone-specific and require minimum friendship levels.
    /// Examples: Fishing, Swimming, Foraging, Picnic, Stargazing.
    /// </summary>
    [CreateAssetMenu(fileName = "NewActivity", menuName = "Prompt Harvest/Activity")]
    public class ActivityData : ScriptableObject
    {
        [Header("Identity")]
        public string Id;
        public string Name;

        [Header("Availability")]
        [Tooltip("Zones where this activity can be performed")]
        public Zone[] ValidZones;
        [Tooltip("Time phases when this activity is available")]
        public TimePhase[] ValidPhases;
        [Tooltip("Minimum friendship points required")]
        public int FriendshipRequired;

        [Header("NPC Enjoyment")]
        [Tooltip("How much specific NPCs enjoy this activity (affects friendship gain)")]
        public NPCEnjoymentEntry[] NPCEnjoyment;

        [Header("Rewards")]
        [Tooltip("Base friendship points gained from this activity")]
        public int FriendshipGainBase;
        [Tooltip("Whether performing this activity advances the time phase")]
        public bool AdvancesTimePhase;
        [Tooltip("Possible item rewards from this activity")]
        public ActivityRewardEntry[] Rewards;

        [Header("Visuals")]
        public string SpriteHint;
        public Sprite Sprite;

        [Header("Tags")]
        public string[] Tags;
    }
}
