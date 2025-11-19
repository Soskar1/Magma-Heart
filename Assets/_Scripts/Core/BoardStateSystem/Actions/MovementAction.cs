using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Pathfinding;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
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

        public override ActionArgs CreateSimulationArgument(SimulatedBoardState state, AIUnit unit)
        {
            Vector3Int source = state.GetProperty<PositionPropertySnapshot>(ActionPossessor).Position;
            Vector3Int targetPosition = state.GetProperty<PositionPropertySnapshot>(unit).Position;

            // TODO: Check if no one is standing in that tile

            return new MovementActionArgs(source.ToVector2(), targetPosition.ToVector2());
        }
    }
}