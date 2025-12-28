using Runtime.Data.ScriptableObjects;

namespace Runtime.Data.Registries
{
    /// <summary>
    /// Registry for all crafting stations.
    /// </summary>
    public class CraftingStationRegistry : ContentRegistry<CraftingStationData, CraftingStationRegistry>
    {
        protected override string GetId(CraftingStationData data) => data.Id;
    }
}
