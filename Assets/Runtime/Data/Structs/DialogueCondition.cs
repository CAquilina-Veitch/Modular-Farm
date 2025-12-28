using System;
using Runtime.Data.Enums;

namespace Runtime.Data.Structs
{
    // GDD 13.11 - Condition for dialogue option to be available
    [Serializable]
    public struct DialogueCondition
    {
        public ConditionType Type;
        public string TargetId;     // npc_id, item_id, or quest_id
        public int MinValue;        // For friendship requirement

        public static DialogueCondition Friendship(string npcId, int minPoints)
        {
            return new DialogueCondition
            {
                Type = ConditionType.Friendship,
                TargetId = npcId,
                MinValue = minPoints
            };
        }

        public static DialogueCondition HasItem(string itemId)
        {
            return new DialogueCondition
            {
                Type = ConditionType.Item,
                TargetId = itemId
            };
        }

        public static DialogueCondition QuestComplete(string questId)
        {
            return new DialogueCondition
            {
                Type = ConditionType.QuestComplete,
                TargetId = questId
            };
        }
    }
}
