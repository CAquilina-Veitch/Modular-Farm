using Runtime.Data.ScriptableObjects;

namespace Runtime.Data.Registries
{
    /// <summary>
    /// Registry for all entities (mounts, vehicles, automators, pets).
    /// </summary>
    public class EntityRegistry : ContentRegistry<EntityData, EntityRegistry>
    {
        protected override string GetId(EntityData data) => data.Id;
    }
}
