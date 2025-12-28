using System;

namespace Runtime.Data.Structs
{
    // GDD 13.8 - Possible item reward from an activity
    [Serializable]
    public struct ActivityRewardEntry
    {
        public string ItemId;
        public float Chance;    // 0.0 to 1.0

        public ActivityRewardEntry(string itemId, float chance)
        {
            ItemId = itemId;
            Chance = chance;
        }
    }
}
