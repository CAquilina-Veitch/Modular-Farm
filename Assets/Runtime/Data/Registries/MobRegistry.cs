using Runtime.Data.ScriptableObjects;

namespace Runtime.Data.Registries
{
    /// <summary>
    /// Registry for all mobs (creatures with predefined behaviors).
    /// </summary>
    public class MobRegistry : ContentRegistry<MobData, MobRegistry>
    {
        protected override string GetId(MobData data) => data.Id;
    }
}
