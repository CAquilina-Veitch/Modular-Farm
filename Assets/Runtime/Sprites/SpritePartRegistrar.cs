using UnityEngine;
using Runtime.Data.ScriptableObjects;

namespace Runtime.Sprites
{
    /// <summary>
    /// Registers SpritePartData assets with the SpriteCompositor at runtime.
    /// Attach to a GameObject in the scene.
    /// </summary>
    public class SpritePartRegistrar : MonoBehaviour
    {
        [Tooltip("All sprite parts to register with the compositor")]
        [SerializeField] private SpritePartData[] parts;

        [Tooltip("Register parts on Awake")]
        [SerializeField] private bool registerOnAwake = true;

        private void Awake()
        {
            if (registerOnAwake)
                RegisterAll();
        }

        /// <summary>
        /// Register all configured parts with the SpriteCompositor.
        /// </summary>
        public void RegisterAll()
        {
            if (parts == null) return;

            foreach (var part in parts)
            {
                if (part != null)
                    SpriteCompositor.RegisterPart(part);
            }
        }

        /// <summary>
        /// Clear compositor cache and re-register all parts.
        /// </summary>
        public void RefreshAll()
        {
            SpriteCompositor.ClearCache();
            RegisterAll();
        }
    }
}
