using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Spawning
{
    public class SpawnService
    {
        private readonly Dictionary<GameObject, SpawnConfig> m_configs;
        private readonly IInstantiator m_instantiator;

        public SpawnService(Dictionary<GameObject, SpawnConfig> configs, IInstantiator instantiator)
        {
            m_configs = configs;
            m_instantiator = instantiator;
        }

        public GameObject Spawn(GameObject prefab, SpawnContext context)
        {
            SpawnConfig config = m_configs[prefab];
            GameObject instance = m_instantiator.Instantiate(config.Prefab, context.Position);
            config.Initialize(instance, context);
            return instance;
        }
    }
}