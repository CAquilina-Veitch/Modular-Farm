using System;

namespace Runtime.Data.Structs
{
    // GDD 13.3 - How much an NPC enjoys a particular activity
    [Serializable]
    public struct ActivityPreferenceEntry
    {
        public string ActivityId;
        public float EnjoymentMultiplier;   // 1.0 = normal, 2.0 = loves it

        public ActivityPreferenceEntry(string activityId, float enjoymentMultiplier)
        {
            ActivityId = activityId;
            EnjoymentMultiplier = enjoymentMultiplier;
        }
    }
}
