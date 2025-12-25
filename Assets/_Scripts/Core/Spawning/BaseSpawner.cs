using MagmaHeart.Spawning;
using UnityEngine;

namespace MagmaHeart.Core.Spawning
{
    public abstract class BaseSpawner<TContext> where TContext : SpawnContext
    {
        public SpawnService SpawnService { get; init; }

        public BaseSpawner(SpawnService spawnService) => SpawnService = spawnService;

        public abstract GameObject Spawn(TContext context);
    }
}