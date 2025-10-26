using MagmaHeart.Collections;

namespace MagmaHeart.AI.Reasoning
{
    public class TacticianAI
    {
        private int m_depth;
        public Strategy CurrentStrategy { get; internal set; }

        public TacticianAI(int depth) => m_depth = depth;

        public IAction StartReasoning(CircularList<IAIUnit> unitsToConsider, StateSnapshot snapshot)
        {
            StateSnapshot stateSnapshot = snapshot;

            return ChooseBestAction(unitsToConsider, snapshot);
        }

        private IAction ChooseBestAction(CircularList<IAIUnit> unitsToConsider, StateSnapshot currentSnapshot, int currentDepth = 0)
        {
            IAIUnit unit = unitsToConsider.Head;

            foreach (IAction action in unitsToConsider)
            {
                StateSnapshot newSnapshot = action.Simulate(currentSnapshot);
                
                if (currentDepth > m_depth)
                {
                    
                }
            }
        }
    }
}
