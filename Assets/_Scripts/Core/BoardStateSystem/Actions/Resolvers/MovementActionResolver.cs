using System.Collections.Generic;
using System.Linq;
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
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class MovementActionResolver : IActionResolver
    {
        private readonly AStar m_aStar;

        public MovementActionResolver()
        {
            // TODO: is it ok to have it here? MovementAction also has its own AStar instance.
            m_aStar = new AStar(AStar.ManhattanDistance);
        }

        public bool TryResolve(ActionDefinition definition, AIUnitModel executor, BoardState state, out ActionArgs resolvedArgs)
        {
            MovementActionData data = (MovementActionData)definition.Data;
            Vector2 source = state.GetProperty<PositionPropertySnapshot>(executor).Position.ToVector2();
            EnergyPropertySnapshot energy = state.GetProperty<EnergyPropertySnapshot>(executor);

            foreach (AIUnitModel potentialTarget in state.Board.GetUnits())
            {
                if (potentialTarget == executor)
                    continue;

                Vector2 targetPosition = state.GetProperty<PositionPropertySnapshot>(potentialTarget).Position.ToVector2();

                Vector2[] adjacentTiles =
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
                    resolvedArgs = new MovementActionArgs((EntityModel)executor, source, targetTile, data);
                    return true;
                }
            }

            resolvedArgs = null;
            return false;
        }
    }
}