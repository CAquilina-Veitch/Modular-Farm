using Runtime.Data.ScriptableObjects;

namespace Runtime.Data.Registries
{
    /// <summary>
    /// Registry for all items (tools, materials, products, consumables, seeds, etc.).
    /// </summary>
    public class ItemRegistry : ContentRegistry<ItemData, ItemRegistry>
    {
        protected override string GetId(ItemData data) => data.Id;
    }
}
