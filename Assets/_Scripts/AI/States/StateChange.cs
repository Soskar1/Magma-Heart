using System;
using System.Threading.Tasks;

namespace MagmaHeart.AI.States
{
    public abstract record StateChange
    {
        internal async Task ApplyToAsync(BoardState state)
        {
            if (state is SimulatedBoardState simulatedBoardState)
                ApplyChangeToSimulation(simulatedBoardState);
            else if (state is ActualBoardState actualBoardState)
                await ApplyChangeToActualStateAsync(actualBoardState);
            else
                throw new ArgumentException($"{state.GetType()} state is not supported");
        }

        public abstract void ApplyChangeToSimulation(SimulatedBoardState simulation);
        public virtual Task ApplyChangeToActualStateAsync(ActualBoardState actualBoard)
        {
            return Task.CompletedTask;
        }
    }
}