using System;
using MagmaHeart.Collections;

namespace MagmaHeart.AI.Reasoning
{
    public class TacticianAI
    {
        private int m_depth;
        public Strategy CurrentStrategy { get; internal set; }

        public TacticianAI(int depth) => m_depth = depth;

        //public IAction StartReasoning(ChainNode<IAIUnit> unitsToConsider, StateSnapshot snapshot)
        //{
        //    StateSnapshot stateSnapshot = snapshot;



        //    //return ChooseBestAction(unitsToConsider, snapshot);
        //}

        private float Minimax(StateSnapshot position, int currentDepth, float alpha, float beta, ChainNode<IAIUnit> units)
        {
            if (currentDepth >= m_depth || position.IsGameOver)
                return position.StaticEvaluation();

            IAIUnit currentUnit = units.Value;
            if (currentUnit.IsPlayer)
            {
                float maxEvaluation = float.MinValue;
                foreach (IAction action in currentUnit.PossibleActions)
                {
                    StateSnapshot newPosition = action.Simulate(position);

                    float evaluation = Minimax(newPosition, currentDepth + 1, alpha, beta, units.Next);
                    maxEvaluation = Math.Max(maxEvaluation, evaluation);
                    alpha = Math.Max(alpha, evaluation);
                    
                    if (beta <= alpha)
                        break;
                }

                return maxEvaluation;
            }
            else
            {
                float minEvaluation = float.MaxValue;
                foreach (IAction action in currentUnit.PossibleActions)
                {
                    StateSnapshot newPosition = action.Simulate(position);

                    float evaluation = Minimax(newPosition, currentDepth + 1, alpha, beta, units.Next);
                    minEvaluation = Math.Min(minEvaluation, evaluation);
                    beta = Math.Min(beta, evaluation);

                    if (beta <= alpha)
                        break;
                }

                return minEvaluation;
            }
        }
    }
}
