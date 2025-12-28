using System;
using Runtime.Data.Enums;

namespace Runtime.Data.Structs
{
    // GDD 13.7 - Single objective within a quest
    [Serializable]
    public struct QuestObjective
    {
        public ObjectiveType Type;
        public string TargetId;     // item_id, npc_id, or zone_id depending on type
        public int Quantity;        // For collect objectives

        public static QuestObjective Collect(string itemId, int quantity)
        {
            return new QuestObjective
            {
                Type = ObjectiveType.Collect,
                TargetId = itemId,
                Quantity = quantity
            };
        }

        public static QuestObjective Deliver(string npcId, string itemId)
        {
            return new QuestObjective
            {
                Type = ObjectiveType.Deliver,
                TargetId = npcId
                // Note: itemId tracked separately in quest for delivery objectives
            };
        }

        public static QuestObjective Visit(string zoneId)
        {
            return new QuestObjective
            {
                Type = ObjectiveType.Visit,
                TargetId = zoneId
            };
        }

        public static QuestObjective TalkTo(string npcId)
        {
            return new QuestObjective
            {
                Type = ObjectiveType.TalkTo,
                TargetId = npcId
            };
        }
    }
}
