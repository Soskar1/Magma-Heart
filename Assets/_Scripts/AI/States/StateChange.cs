using System;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.AI.States
{
    public abstract record StateChange
    {
        internal async Task ApplyToAsync(BoardState state, CancellationToken cancellationToken)
        {
            if (state is ActualBoardState actualBoardState)
                await ApplyChangeToActualStateAsync(actualBoardState, cancellationToken);
            else
                throw new ArgumentException($"{state.GetType()} state is not supported in async operation");
        }

        internal void ApplyTo(BoardState state)
        {
            if (state is SimulatedBoardState simulatedBoardState)
                ApplyChangeToSimulation(simulatedBoardState);
            else
                throw new ArgumentException($"{state.GetType()} state is not supported in sync operation");
        }

        public abstract void ApplyChangeToSimulation(SimulatedBoardState simulation);
        public abstract void UndoChangeToSimulation(SimulatedBoardState simulation);
        public virtual Task ApplyChangeToActualStateAsync(ActualBoardState actualBoard, CancellationToken cancellationToken) => Task.CompletedTask;
    }
}