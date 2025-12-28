using Runtime.Data.ScriptableObjects;

namespace Runtime.Data.Registries
{
    /// <summary>
    /// Registry for all crops.
    /// </summary>
    public class CropRegistry : ContentRegistry<CropData, CropRegistry>
    {
        protected override string GetId(CropData data) => data.Id;
    }
}
