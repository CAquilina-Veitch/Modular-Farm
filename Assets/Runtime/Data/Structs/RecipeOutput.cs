using System;

namespace Runtime.Data.Structs
{
    // GDD 13.6 - Recipe output item and quantity
    [Serializable]
    public struct RecipeOutput
    {
        public string ItemId;
        public int Quantity;

        public RecipeOutput(string itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
        }
    }
}
