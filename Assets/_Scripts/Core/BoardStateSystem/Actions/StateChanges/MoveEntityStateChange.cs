using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;
using MagmaHeart.Extensions;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record MoveEntityStateChange(EntityModel Entity, Vector2 From, Vector2 To) : StateChange
    {
        public override void ApplyChangeToActualState(ActualBoardState actualBoard)
        {
            // TODO: start movement service
            throw new System.NotImplementedException();
        }

        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            simulation.RemoveUnit(From, Entity);
            simulation.AddUnit(To, Entity);
            simulation.UpdateBoardNodeType(From, BoardNodeType.Walkable);
            simulation.UpdateBoardNodeType(To, BoardNodeType.Obstacle);
        }
    }
}
