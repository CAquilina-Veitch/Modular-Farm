using System;

namespace Runtime.Data.Structs
{
    // GDD 13.2 - Item drop from mobs
    [Serializable]
    public struct DropEntry
    {
        public string ItemId;
        public float Chance;        // 0.0 to 1.0
        public int QuantityMin;
        public int QuantityMax;

        public DropEntry(string itemId, float chance, int quantityMin = 1, int quantityMax = 1)
        {
            ItemId = itemId;
            Chance = chance;
            QuantityMin = quantityMin;
            QuantityMax = quantityMax;
        }
    }
}
