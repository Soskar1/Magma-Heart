using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Spawning
{
    public class SpawnService
    {
        private readonly Dictionary<GameObject, ISpawnConfig> m_configs;
        private readonly IInstantiator m_instantiator;

        public SpawnService(Dictionary<GameObject, ISpawnConfig> configs, IInstantiator instantiator)
        {
            m_configs = configs;
            m_instantiator = instantiator;
        }

        public GameObject Spawn(GameObject prefab, SpawnContext context)
        {
            ISpawnConfig config = m_configs[prefab];
            GameObject instance = m_instantiator.Instantiate(config.Prefab, context.Position);
            config.Initialize(instance, context);
            return instance;
        }
    }
}