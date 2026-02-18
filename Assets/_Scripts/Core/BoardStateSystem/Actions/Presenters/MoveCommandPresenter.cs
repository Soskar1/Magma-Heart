using MagmaHeart.AI.Execution;
using MagmaHeart.Core.BoardStateSystem.Actions.Commands;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions.Presenters
{
    public class MoveCommandPresenter : IBoardCommandPresenter<MoveCommand>
    {
        private readonly int m_movementSpeed;
        private readonly CommandRunner m_commandRunner;

        public MoveCommandPresenter(CommandRunner commandRunner, int movementSpeed)
        {
            m_movementSpeed = movementSpeed;
            m_commandRunner = commandRunner;
        }

        public async Task Present(Room room, MoveCommand command, CancellationToken token)
        {
            m_commandRunner.Apply(room, command);

            List<Vector2> path = command.Path;
            room.TryGetEntity(command.ExecutorId, out Entity entity);

            Vector3Int from = path.First().ToVector3Int();
            Vector3Int to = path.Last().ToVector3Int();
            entity.Facing.TryUpdateFacing(to.x - from.x);

            List<RoomTile> tilePath = new List<RoomTile>();
            foreach (Vector2 pathPoint in path)
            {
                RoomTile tile = room.GetRoomTile(pathPoint);
                tilePath.Add(tile);
            }

            entity.Animation.PlayRunAnimation();

            await entity.TileBasedMovement.StartMovementAsync(tilePath, m_movementSpeed);

            entity.Animation.PlayIdleAnimation();
        }
    }
}
