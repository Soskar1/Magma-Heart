using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Pathfinding;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Entities.Properties;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class MovementAction : CombatAction
    {
        private readonly AStar m_aStar;

        public MovementAction()
        {
            m_aStar = new AStar(AStar.ManhattanDistance);
        }

        public override int GetEnergyCost(ActionArgs args, BoardState gameState)
        {
            MovementActionArgs movementArgs = args as MovementActionArgs;

            PositionPropertySnapshot position = gameState.GetProperty<PositionPropertySnapshot>(args.Executor);
            List<Vector2> path = m_aStar.FindPath(gameState.Board.Graph, movementArgs.SourceTile, movementArgs.TileToMove);

            if (path == null || !path.Any())
                return int.MaxValue;

            int distance = path.Count - 1;

            return Mathf.CeilToInt(distance / (float)movementArgs.MovementActionData.MovementDistanceInTilesForOneEnergy);
        }

        public override IEnumerable<StateChange> ProduceChanges(ActionArgs args, BoardState gameState)
        {
            MovementActionArgs movementArgs = args as MovementActionArgs;

            IEnumerable<StateChange> changes = base.ProduceChanges(args, gameState);

            PositionPropertySnapshot position = gameState.GetProperty<PositionPropertySnapshot>(args.Executor);
            List<Vector2> path = m_aStar.FindPath(gameState.Board.Graph, movementArgs.SourceTile, movementArgs.TileToMove);

            return changes.Concat(new List<StateChange>()
            {
                new MoveEntityStateChange(movementArgs.TypedExecutor, path)
            });
        }
    }
}