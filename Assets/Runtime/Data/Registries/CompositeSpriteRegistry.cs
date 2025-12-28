using Runtime.Core.Behaviours;
using Runtime.Data.ScriptableObjects;

namespace Runtime.Data.Registries
{
    /// <summary>
    /// Registry for composite sprite definitions.
    /// </summary>
    public class CompositeSpriteRegistry : ContentRegistry<CompositeSpriteDef, CompositeSpriteRegistry>
    {
        protected override string GetId(CompositeSpriteDef data) => data.Id;
    }
}
