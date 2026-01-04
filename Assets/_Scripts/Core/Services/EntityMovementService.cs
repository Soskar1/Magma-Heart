using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Services
{
    public class EntityMovementService
    {
        public async Task MoveEntityAsync(Entity entity, List<RoomTile> aStarPath, int speed)
        {
            Vector3Int from = aStarPath.First().Position;
            Vector3Int to = aStarPath.Last().Position;
            entity.Facing.TryUpdateFacing(to.x - from.x);

            entity.Animation.PlayRunAnimation();

            await entity.TileBasedMovement.StartMovementAsync(aStarPath, speed);

            entity.Animation.PlayIdleAnimation();
        }
    }
}