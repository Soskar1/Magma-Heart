using UnityEngine;

namespace MagmaHeart.Spawning
{
    public struct SpawnContext
    {
        public Vector2 Position { get; private set; }
        public Vector2 Direction { get; private set; }
        public GameObject Owner { get; private set; }

        public SpawnContext(Vector2 position, Vector2 direction, GameObject owner = null)
        {
            Position = position;
            Direction = direction;
            Owner = owner;
        }

        public SpawnContext(Vector2 position) : this(position, Vector2.zero) { }
    }
}