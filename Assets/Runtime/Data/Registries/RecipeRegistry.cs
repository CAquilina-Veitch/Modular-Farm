using Runtime.Data.ScriptableObjects;

namespace Runtime.Data.Registries
{
    /// <summary>
    /// Registry for all recipes.
    /// </summary>
    public class RecipeRegistry : ContentRegistry<RecipeData, RecipeRegistry>
    {
        protected override string GetId(RecipeData data) => data.Id;
    }
}
