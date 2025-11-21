using MagmaHeart.AI.States;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record MovementStateChange(AIUnit Creator, Vector2 From, Vector2 To) : StateChange
    {
        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            simulation.RemoveUnit(From, Creator);
            simulation.AddUnit(To, Creator);
            simulation.UpdateProperty(Creator, new Position(To));
        }
    }
}
