using Runtime.Data.ScriptableObjects;

namespace Runtime.Data.Registries
{
    /// <summary>
    /// Registry for all activities.
    /// </summary>
    public class ActivityRegistry : ContentRegistry<ActivityData, ActivityRegistry>
    {
        protected override string GetId(ActivityData data) => data.Id;
    }
}
