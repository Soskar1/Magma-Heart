using UnityEngine;

namespace MagmaHeart.Spawning
{
    public class SpawnContextFactory
    {
        public virtual SpawnContext CreateSpawnContext(Vector2 position) => new SpawnContext(position);
    }
}