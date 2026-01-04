using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Spawning
{
    public class EntitySpawner
    {
        private readonly Entity m_prefab;
        private readonly RoomGrid m_roomGrid;

        public EntitySpawner(Entity prefab, RoomGrid roomGrid)
        {
            m_prefab = prefab;
            m_roomGrid = roomGrid;
        }

        public Entity Spawn(EntityData data, bool isPlayer, ITurnController turnController)
        {
            Entity entity = GameObject.Instantiate(m_prefab);
            entity.Initialize(data, m_roomGrid, isPlayer, turnController);

            return entity;
        }
    }
}