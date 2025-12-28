using System;

namespace Runtime.Data.Structs
{
    // GDD 13.8 - How much specific NPCs enjoy an activity
    // Inverse of ActivityPreferenceEntry (from activity's perspective)
    [Serializable]
    public struct NPCEnjoymentEntry
    {
        public string NPCId;
        public float Multiplier;    // 1.0 = normal, 2.0 = loves it

        public NPCEnjoymentEntry(string npcId, float multiplier)
        {
            NPCId = npcId;
            Multiplier = multiplier;
        }
    }
}
