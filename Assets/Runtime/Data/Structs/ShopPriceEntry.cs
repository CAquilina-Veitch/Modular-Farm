using System;

namespace Runtime.Data.Structs
{
    // GDD 13.3 - Price multiplier for items an NPC sells
    [Serializable]
    public struct ShopPriceEntry
    {
        public string ItemId;
        public float Multiplier;    // 1.0 = base price, 1.2 = 20% markup

        public ShopPriceEntry(string itemId, float multiplier)
        {
            ItemId = itemId;
            Multiplier = multiplier;
        }
    }
}
