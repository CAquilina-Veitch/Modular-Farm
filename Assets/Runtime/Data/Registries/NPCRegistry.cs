using Runtime.Data.ScriptableObjects;

namespace Runtime.Data.Registries
{
    /// <summary>
    /// Registry for all NPCs.
    /// </summary>
    public class NPCRegistry : ContentRegistry<NPCData, NPCRegistry>
    {
        protected override string GetId(NPCData data) => data.Id;
    }
}
