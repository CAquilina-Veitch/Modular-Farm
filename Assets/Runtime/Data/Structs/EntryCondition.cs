using System;
using Runtime.Data.Enums;

namespace Runtime.Data.Structs
{
    // GDD 13.4 - Condition to enter a building
    [Serializable]
    public struct EntryCondition
    {
        public ConditionType Type;
        public string TargetId;         // npc_id, item_id, or quest_id depending on type
        public int MinValue;            // For friendship requirement
        public TimePhase[] ValidPhases; // For time condition

        public static EntryCondition Friendship(string npcId, int minPoints)
        {
            return new EntryCondition
            {
                Type = ConditionType.Friendship,
                TargetId = npcId,
                MinValue = minPoints
            };
        }

        public static EntryCondition Time(params TimePhase[] phases)
        {
            return new EntryCondition
            {
                Type = ConditionType.Time,
                ValidPhases = phases
            };
        }

        public static EntryCondition Item(string itemId)
        {
            return new EntryCondition
            {
                Type = ConditionType.Item,
                TargetId = itemId
            };
        }

        public static EntryCondition QuestComplete(string questId)
        {
            return new EntryCondition
            {
                Type = ConditionType.QuestComplete,
                TargetId = questId
            };
        }
    }
}
