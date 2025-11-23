using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Pathfinding;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;
using MagmaHeart.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class MovementAction : CombatAction<MovementActionArgs>
    {
        private readonly AStar m_aStar;
        private const int m_movementDistanceInTilesForOneEnergy = 2;

        public MovementAction(EntityModel actionPossessor) : base(actionPossessor)
        {
            m_aStar = new AStar(AStar.ManhattanDistance);
        }

        public override int GetEnergyCost(MovementActionArgs args, BoardState gameState)
        {
            PositionPropertySnapshot position = gameState.GetProperty<PositionPropertySnapshot>(ActionPossessor);
            List<Vector2> path = m_aStar.FindPath(gameState.Board.Graph, args.SourceTile, args.TileToMove);

            if (path == null || !path.Any())
                return int.MaxValue;

            int distance = path.Count - 1;
            return Mathf.CeilToInt(distance / (float)m_movementDistanceInTilesForOneEnergy);
        }

        public override IEnumerable<StateChange> ProduceChanges(MovementActionArgs args, BoardState gameState)
        {
            IEnumerable<StateChange> changes = base.ProduceChanges(args, gameState);

            PositionPropertySnapshot position = gameState.GetProperty<PositionPropertySnapshot>(ActionPossessor);
            List<Vector2> path = m_aStar.FindPath(gameState.Board.Graph, args.SourceTile, args.TileToMove);

            return changes.Concat(new List<StateChange>()
            {
                new MoveEntityStateChange(ActionPossessor, path)
            });
        }

        public override IEnumerable<ActionArgs> CreateSimulationArgument(SimulatedBoardState state, AIUnit unit)
        {
            Vector2 source = state.GetProperty<PositionPropertySnapshot>(ActionPossessor).Position.ToVector2();
            Vector2 targetPosition = state.GetProperty<PositionPropertySnapshot>(unit).Position.ToVector2();

            EnergyPropertySnapshot energy = state.GetProperty<EnergyPropertySnapshot>(ActionPossessor);

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

                Vector2 targetTile = path.Skip(1).Take(energy.CurrentEnergy * m_movementDistanceInTilesForOneEnergy).Last();
                yield return new MovementActionArgs(source, targetTile);
            }
        }
    }
}