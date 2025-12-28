using System;

namespace Runtime.Data.Structs
{
    // GDD 13.3 - Price an NPC pays when buying items from player
    [Serializable]
    public struct BuyPreferenceEntry
    {
        public string ItemId;
        public int BuyPrice;

        public BuyPreferenceEntry(string itemId, int buyPrice)
        {
            ItemId = itemId;
            BuyPrice = buyPrice;
        }
    }
}
