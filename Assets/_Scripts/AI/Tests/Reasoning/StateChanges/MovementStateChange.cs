using MagmaHeart.AI.States;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record MovementStateChange(int ExecutorId, Vector2 From, Vector2 To) : StateChange
    {
        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            if (simulation.Board.TryGetUnit(ExecutorId, out AIUnitModel Executor))
            {
                Entity entity = (Entity)Executor;

                entity.Position = To;
                simulation.Board.RemoveUnit(From);
                simulation.Board.AddUnit(To, entity);
            }
        }

        public override void UndoChangeToSimulation(SimulatedBoardState simulation)
        {
            if (simulation.Board.TryGetUnit(ExecutorId, out AIUnitModel Executor))
            {
                Entity entity = (Entity)Executor;
                entity.Position = From;
                simulation.Board.RemoveUnit(To);
                simulation.Board.AddUnit(From, entity);
            }
        }
    }
}
