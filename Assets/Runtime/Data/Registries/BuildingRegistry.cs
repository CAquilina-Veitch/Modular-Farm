using Runtime.Data.ScriptableObjects;

namespace Runtime.Data.Registries
{
    /// <summary>
    /// Registry for all buildings.
    /// </summary>
    public class BuildingRegistry : ContentRegistry<BuildingData, BuildingRegistry>
    {
        protected override string GetId(BuildingData data) => data.Id;
    }
}
