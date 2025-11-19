using System;

namespace MagmaHeart.AI.States
{
    public abstract record StateChange
    {
        internal void ApplyTo(BoardState state)
        {
            if (state is SimulatedBoardState simulatedBoardState)
                ApplyChangeToSimulation(simulatedBoardState);
            else if (state is ActualBoardState actualBoardState)
                ApplyChangeToActualState(actualBoardState);
            else
                throw new ArgumentException($"{state.GetType()} state is not supported");
        }

        public abstract void ApplyChangeToSimulation(SimulatedBoardState simulation);
        public abstract void ApplyChangeToActualState(ActualBoardState actualBoard);
    }
}