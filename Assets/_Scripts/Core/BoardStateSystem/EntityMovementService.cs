using MagmaHeart.Core.Entities;
using System.Collections.Generic;

namespace MagmaHeart.Core.BoardStateSystem
{
    public class EntityMovementService
    {
        public void MoveEntity(Entity entity, List<RoomTile> aStarPath)
        {
            entity.TurnBasedMovement.StartMovement(aStarPath);
        }
    }
}