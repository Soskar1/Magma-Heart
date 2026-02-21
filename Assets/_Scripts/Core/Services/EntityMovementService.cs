using MagmaHeart.Core.Entities;
using MagmaHeart.DungeonGeneration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Services
{
    public class EntityMovementService
    {
        public async Task MoveEntityAsync(Entity entity, List<DungeonTile> aStarPath, int speed)
        {
            Vector2Int from = aStarPath.First().Position;
            Vector2Int to = aStarPath.Last().Position;
            entity.Facing.TryUpdateFacing(to.x - from.x);

            entity.Animation.PlayRunAnimation();

            await entity.TileBasedMovement.StartMovementAsync(aStarPath, speed);

            entity.Animation.PlayIdleAnimation();
        }
    }
}