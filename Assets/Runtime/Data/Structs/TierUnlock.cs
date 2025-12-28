using System;
using Runtime.Data.Enums;

namespace Runtime.Data.Structs
{
    // GDD 13.12 - What a friendship tier unlocks
    [Serializable]
    public struct TierUnlock
    {
        public UnlockType Type;
        public bool BoolValue;      // For CanFollow, CanEnterHome, FreeShop
        public float FloatValue;    // For PriceModifier (e.g., 0.9 = 10% discount)
        public string StringValue;  // For DialogueOption, Activity, NPCMovesTo

        public static TierUnlock CanFollow()
        {
            return new TierUnlock { Type = UnlockType.CanFollow, BoolValue = true };
        }

        public static TierUnlock CanEnterHome()
        {
            return new TierUnlock { Type = UnlockType.CanEnterHome, BoolValue = true };
        }

        public static TierUnlock PriceModifier(float modifier)
        {
            return new TierUnlock { Type = UnlockType.PriceModifier, FloatValue = modifier };
        }

        public static TierUnlock DialogueOption(string dialogueId)
        {
            return new TierUnlock { Type = UnlockType.DialogueOption, StringValue = dialogueId };
        }

        public static TierUnlock Activity(string activityId)
        {
            return new TierUnlock { Type = UnlockType.Activity, StringValue = activityId };
        }

        public static TierUnlock NPCMovesTo(string zoneId)
        {
            return new TierUnlock { Type = UnlockType.NPCMovesTo, StringValue = zoneId };
        }

        public static TierUnlock FreeShop()
        {
            return new TierUnlock { Type = UnlockType.FreeShop, BoolValue = true };
        }
    }
}
