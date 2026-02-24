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
        private readonly CommandRunner m_commandRunner;

        public MoveCommandPresenter(CommandRunner commandRunner)
        {
            m_commandRunner = commandRunner;
        }

        public async Task Present(Room room, MoveCommand command, CancellationToken token)
        {
            m_commandRunner.Apply(room, command);

            List<Vector3> path = command.Path.Select(p => (Vector3)p).ToList();
            room.TryGetEntity(command.ExecutorId, out Entity entity);

            Vector3 from = path.First();
            Vector3 to = path.Last();
            entity.Facing.TryUpdateFacing(to.x - from.x);

            entity.Animation.PlayRunAnimation();

            path = path
                .Select(point => (Vector3)room.Grid.ToTileCenter(point.ToVector2Int()))
                .ToList();

            await entity.TileBasedMovement.StartMovementAsync(path);

            entity.Animation.PlayIdleAnimation();
        }
    }
}
