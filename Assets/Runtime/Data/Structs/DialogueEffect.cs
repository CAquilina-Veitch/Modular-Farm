using System;
using Runtime.Data.Enums;

namespace Runtime.Data.Structs
{
    // GDD 13.11 - Effect triggered by dialogue option
    [Serializable]
    public struct DialogueEffect
    {
        public EffectType Type;
        public string TargetId;     // quest_id, item_id, npc_id, event_id, or zone_id
        public int IntValue;        // For friendship change amount

        public static DialogueEffect StartQuest(string questId)
        {
            return new DialogueEffect
            {
                Type = EffectType.StartQuest,
                TargetId = questId
            };
        }

        public static DialogueEffect GiveItem(string itemId)
        {
            return new DialogueEffect
            {
                Type = EffectType.GiveItem,
                TargetId = itemId
            };
        }

        public static DialogueEffect ChangeFriendship(string npcId, int amount)
        {
            return new DialogueEffect
            {
                Type = EffectType.ChangeFriendship,
                TargetId = npcId,
                IntValue = amount
            };
        }

        public static DialogueEffect TriggerEvent(string eventId)
        {
            return new DialogueEffect
            {
                Type = EffectType.TriggerEvent,
                TargetId = eventId
            };
        }

        public static DialogueEffect ChangeNPCLocation(string npcId, string zoneId)
        {
            return new DialogueEffect
            {
                Type = EffectType.ChangeNPCLocation,
                TargetId = npcId
                // Note: zoneId would need separate field or encoding
            };
        }
    }
}
