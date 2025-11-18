using MagmaHeart.AI.States;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests.StateChanges
{
    internal record MovementStateChange(AIUnit Creator, Vector2 From, Vector2 To) : StateChange
    {
        public override void ApplyChangeToActualState(ActualBoardState actualBoard)
        {
            throw new System.NotImplementedException();
        }

        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            simulation.Board.RemoveUnit(From, Creator);
            simulation.Board.AddUnit(To, Creator);
            simulation.WriteProperty(Creator, new Position(To));
        }
    }
}
