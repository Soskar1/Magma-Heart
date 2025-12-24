using UnityEngine;

namespace MagmaHeart.Spawning
{
    public class SpawnContext
    {
        public Vector2 Position { get; private set; }

        public SpawnContext(Vector2 position) => Position = position;
    }
}