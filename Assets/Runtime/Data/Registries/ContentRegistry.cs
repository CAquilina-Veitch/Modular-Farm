using System.Collections.Generic;
using UnityEngine;
using Runtime.Core.Behaviours;

namespace Runtime.Data.Registries
{
    /// <summary>
    /// Generic base class for all content registries.
    /// Registries are singletons that hold all content of a given type.
    /// Content can be preloaded from editor (SerializeField) or registered at runtime (AI-generated).
    /// </summary>
    /// <typeparam name="TData">The ScriptableObject data type</typeparam>
    /// <typeparam name="TRegistry">The concrete registry type (for singleton pattern)</typeparam>
    public abstract class ContentRegistry<TData, TRegistry> : SingletonBehaviour<TRegistry>
        where TData : ScriptableObject
        where TRegistry : ContentRegistry<TData, TRegistry>
    {
        [SerializeField]
        [Tooltip("Content preloaded from editor. AI-generated content is added at runtime.")]
        protected TData[] preloadedContent;

        private readonly Dictionary<string, TData> contentById = new();

        protected override void Awake()
        {
            base.Awake();
            LoadPreloadedContent();
        }

        private void LoadPreloadedContent()
        {
            if (preloadedContent == null) return;

            foreach (var data in preloadedContent)
            {
                if (data != null)
                {
                    Register(data);
                }
            }
        }

        /// <summary>
        /// Register content (used for both preloaded and AI-generated content).
        /// </summary>
        public void Register(TData data)
        {
            string id = GetId(data);
            contentById[id] = data;
        }

        /// <summary>
        /// Get content by ID. Throws if not found (let it crash philosophy).
        /// </summary>
        public TData Get(string id)
        {
            return contentById[id];
        }

        /// <summary>
        /// Try to get content by ID. Returns false if not found.
        /// </summary>
        public bool TryGet(string id, out TData data)
        {
            return contentById.TryGetValue(id, out data);
        }

        /// <summary>
        /// Check if content with given ID exists.
        /// </summary>
        public bool Exists(string id)
        {
            return contentById.ContainsKey(id);
        }

        /// <summary>
        /// Get all registered content.
        /// </summary>
        public IEnumerable<TData> GetAll()
        {
            return contentById.Values;
        }

        /// <summary>
        /// Get the count of registered content.
        /// </summary>
        public int Count => contentById.Count;

        /// <summary>
        /// Get all registered IDs.
        /// </summary>
        public IEnumerable<string> GetAllIds()
        {
            return contentById.Keys;
        }

        /// <summary>
        /// Abstract method to extract ID from data. Implemented by each specific registry.
        /// </summary>
        protected abstract string GetId(TData data);
    }
}
