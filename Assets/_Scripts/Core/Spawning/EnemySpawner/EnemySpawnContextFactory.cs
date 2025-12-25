using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Extensions;
using MagmaHeart.Spawning;
using UnityEngine;

namespace MagmaHeart.Core.Spawning
{
    public class EnemySpawnContextFactory : SpawnContextFactory
    {
        private readonly DungeonGrid m_grid;
        private readonly CombatAI m_combatAi;

        public EnemySpawnContextFactory(DungeonGrid grid, CombatAI combatAi)
        {
            m_grid = grid;
            m_combatAi = combatAi;
        }

        public override SpawnContext CreateSpawnContext(Vector2 position) => new EnemySpawnContext(m_grid.ToTileCenter(position.ToVector2Int()), m_grid, m_combatAi);
    }
}