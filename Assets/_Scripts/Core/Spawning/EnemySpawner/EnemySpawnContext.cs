using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Spawning;
using UnityEngine;

namespace MagmaHeart.Core.Spawning
{
    public class EnemySpawnContext : SpawnContext
    {
        public RoomGrid Grid { get; private set; }
        public AIEngine AiEngine { get; private set; }

        public EnemySpawnContext(Vector2 position, RoomGrid grid, AIEngine aiEngine) : base(position)
        {
            Grid = grid;
            AiEngine = aiEngine;
        }
    }
}