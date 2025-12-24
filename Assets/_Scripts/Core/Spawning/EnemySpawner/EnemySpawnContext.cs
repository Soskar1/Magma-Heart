using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Spawning;
using UnityEngine;

namespace MagmaHeart.Core.Spawning
{
    public class EnemySpawnContext : SpawnContext
    {
        public DungeonGrid Grid { get; private set; }
        public CombatAI CombatAI { get; private set; }

        public EnemySpawnContext(Vector2 position, DungeonGrid grid, CombatAI combatAI) : base(position)
        {
            Grid = grid;
            CombatAI = combatAI;
        }
    }
}