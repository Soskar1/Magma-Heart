using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Pathfinding;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions.Data;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;
using MagmaHeart.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions.ArgumentCreators
{
    public class MovementActionArgumentCreator : IActionArgumentCreator<MovementActionData>
    {
        private readonly AStar m_aStar;

        public MovementActionArgumentCreator()
        {
            // TODO: is it ok to have it here? MovementAction also has its own AStar instance.
            m_aStar = new AStar(AStar.ManhattanDistance);
        }

        public IEnumerable<ActionArgs> CreateArguments(MovementActionData data, AIUnitModel executor, AIUnitModel target, BoardState state)
        {
            Vector2 source = state.GetProperty<PositionPropertySnapshot>(executor).Position.ToVector2();
            Vector2 targetPosition = state.GetProperty<PositionPropertySnapshot>(target).Position.ToVector2();

            EnergyPropertySnapshot energy = state.GetProperty<EnergyPropertySnapshot>(executor);

            Vector2[] adjacentTiles = new Vector2[]
            {
                targetPosition + Vector2.up,
                targetPosition + Vector2.down,
                targetPosition + Vector2.left,
                targetPosition + Vector2.right
            };

            List<Vector2> roomTiles =
                adjacentTiles.Where(v => state.Board.Graph.ContainsNode(v) && state.Board.GetNodeType(v) == BoardNodeType.Walkable)
                .OrderBy(t => DungeonGrid.ManhattanDistance(source.ToVector3Int(), t.ToVector3Int()))
                .ToList();

            foreach (Vector2 tile in roomTiles)
            {
                List<Vector2> path = m_aStar.FindPath(state.Board.Graph, source, tile);
                if (path == null || !path.Any())
                    continue;

                Vector2 targetTile = path.Skip(1).Take(energy.CurrentEnergy * data.MovementDistanceInTilesForOneEnergy).Last();
                yield return new MovementActionArgs((EntityModel)executor, source, targetTile, data.MovementDistanceInTilesForOneEnergy); // TODO: remove casts
                break;
            }
        }

        public IEnumerable<ActionArgs> CreateArguments(ActionData data, AIUnitModel executor, AIUnitModel target, BoardState state) => CreateArguments((MovementActionData)data, executor, target, state);
    }
}
