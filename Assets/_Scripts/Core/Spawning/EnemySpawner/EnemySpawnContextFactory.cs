using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Extensions;
using MagmaHeart.Spawning;
using UnityEngine;

namespace MagmaHeart.Core.Spawning
{
    public class EnemySpawnContextFactory : SpawnContextFactory
    {
        private readonly RoomGrid m_grid;
        private readonly AIEngine m_aiEngine;

        public EnemySpawnContextFactory(RoomGrid grid, AIEngine aiEngine)
        {
            m_grid = grid;
            m_aiEngine = aiEngine;
        }

        public override SpawnContext CreateSpawnContext(Vector2 position) => new EnemySpawnContext(m_grid.ToTileCenter(position.ToVector2Int()), m_grid, m_aiEngine);
    }
}