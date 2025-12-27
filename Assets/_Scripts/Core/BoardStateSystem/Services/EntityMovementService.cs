using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem
{
    public class EntityMovementService
    {
        public async Task MoveEntityAsync(Entity entity, List<RoomTile> aStarPath)
        {
            Vector3Int from = aStarPath.First().Position;
            Vector3Int to = aStarPath.Last().Position;
            entity.Facing.TryUpdateFacing(to.x - from.x);

            entity.Animation.PlayRunAnimation();

            await entity.TurnBasedMovement.StartMovementAsync(aStarPath);

            entity.Animation.PlayIdleAnimation();
        }
    }
}