using System;
using UnityEngine;
using Runtime.Data.Structs;

namespace Runtime.Data.ScriptableObjects
{
    /// <summary>
    /// GDD 13.12 - Friendship tier definitions.
    /// Defines what unlocks at each relationship level.
    /// Base tiers: Stranger(0), Acquaintance(20), Friend(40), Good Friend(60), Best Friend(80).
    /// AI can generate custom tiers (e.g., Partner at 110 for marriage).
    /// </summary>
    [CreateAssetMenu(fileName = "NewFriendshipTier", menuName = "Prompt Harvest/Friendship Tier")]
    public class FriendshipTierData : ScriptableObject
    {
        [Header("Identity")]
        public string Id;
        public string Name;

        [Header("Requirements")]
        [Tooltip("Minimum friendship points to reach this tier")]
        public int PointsRequired;

        [Header("Unlocks")]
        [Tooltip("What becomes available at this tier")]
        public TierUnlock[] Unlocks;

        [Header("Tags")]
        public string[] Tags;
    }
}
