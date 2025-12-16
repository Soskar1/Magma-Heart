using MagmaHeart.AI.Pathfinding;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Entities.Properties;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class MovementAction : CombatAction<MovementActionArgs>
    {
        private readonly AStar m_aStar;

        public MovementAction()
        {
            m_aStar = new AStar(AStar.ManhattanDistance);
        }

        public override int GetEnergyCost(MovementActionArgs args, BoardState gameState)
        {
            PositionPropertySnapshot position = gameState.GetProperty<PositionPropertySnapshot>(args.Executor);
            List<Vector2> path = m_aStar.FindPath(gameState.Board.Graph, args.SourceTile, args.TileToMove);

            if (path == null || !path.Any())
                return int.MaxValue;

            int distance = path.Count - 1;

            return Mathf.CeilToInt(distance / (float)args.MovementDistanceInTilesForOneEnergy);
        }

        public override IEnumerable<StateChange> ProduceChanges(MovementActionArgs args, BoardState gameState)
        {
            IEnumerable<StateChange> changes = base.ProduceChanges(args, gameState);

            PositionPropertySnapshot position = gameState.GetProperty<PositionPropertySnapshot>(args.Executor);
            List<Vector2> path = m_aStar.FindPath(gameState.Board.Graph, args.SourceTile, args.TileToMove);

            return changes.Concat(new List<StateChange>()
            {
                new MoveEntityStateChange(args.TypedExecutor, path)
            });
        }
    }
}