using Runtime.Core.Behaviours;
using Runtime.Data.ScriptableObjects;

namespace Runtime.Data.Registries
{
    /// <summary>
    /// Registry for sprite part data.
    /// </summary>
    public class SpritePartRegistry : ContentRegistry<SpritePartData, SpritePartRegistry>
    {
        protected override string GetId(SpritePartData data) => data.Id;
    }
}
