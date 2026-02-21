using MagmaHeart.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Services
{
    public class EntityMovementService
    {
        public async Task MoveEntityAsync(Entity entity, List<Vector2> aStarPath, int speed)
        {
            Vector2 from = aStarPath.First();
            Vector2 to = aStarPath.Last();
            entity.Facing.TryUpdateFacing(to.x - from.x);

            entity.Animation.PlayRunAnimation();

            await entity.TileBasedMovement.StartMovementAsync(aStarPath, speed);

            entity.Animation.PlayIdleAnimation();
        }
    }
}