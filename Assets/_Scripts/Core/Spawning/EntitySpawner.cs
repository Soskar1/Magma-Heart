using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Spawning
{
    public class EntitySpawner
    {
        private readonly Entity m_prefab;
        private readonly WorldGrid m_worldGrid;

        private int m_nextId = 0;

        public EntitySpawner(Entity prefab, WorldGrid worldGrid)
        {
            m_prefab = prefab;
            m_worldGrid = worldGrid;
        }

        public Entity Spawn(EntityData data, bool isPlayer)
        {
            Entity entity = GameObject.Instantiate(m_prefab);
            entity.Initialize(data, m_worldGrid, isPlayer, m_nextId);
            ++m_nextId;

            return entity;
        }
    }
}