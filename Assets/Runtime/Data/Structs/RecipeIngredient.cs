using System;

namespace Runtime.Data.Structs
{
    // GDD 13.6 - Single ingredient in a recipe
    [Serializable]
    public struct RecipeIngredient
    {
        public string ItemId;
        public int Quantity;

        public RecipeIngredient(string itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
        }
    }
}
