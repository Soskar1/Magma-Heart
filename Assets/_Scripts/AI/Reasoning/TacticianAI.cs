using System;
using MagmaHeart.Collections;

namespace MagmaHeart.AI.Reasoning
{
    public class TacticianAI
    {
        private int m_depth;
        private AIUnit m_playerUnit;
        private Func<StateSnapshot, AIUnit> m_targetSelection;
        public Strategy CurrentStrategy { get; internal set; }

        public TacticianAI(int depth, AIUnit playerUnit, Func<StateSnapshot, AIUnit> targetSelection)
        {
            m_depth = depth;
            m_playerUnit = playerUnit;
            m_targetSelection = targetSelection;
        }

        public IAction ChooseBestMove(CircularList<AIUnit> unitsToConsider)
        {  
            StateSnapshot stateSnapshot = StateSnapshotMaker.CreateStateSnapshot(unitsToConsider);

            ChainNode<AIUnit> head = (ChainNode<AIUnit>)unitsToConsider;
            float alpha = float.MinValue;
            float beta = float.MaxValue;
            float bestValue = float.MinValue;

            // TODO: filter moves according to current strategy
            IAction bestMove = head.Value.PossibleActions[0];

            foreach (IAction action in head.Value.PossibleActions)
            {
                StateSnapshot newState = action.Simulate(stateSnapshot, m_playerUnit);

                float evaluation = Minimax(newState, m_depth - 1, alpha, beta, head.Next);

                if (evaluation > bestValue)
                {
                    bestValue = evaluation;
                    bestMove = action;
                }

                alpha = Math.Max(alpha, bestValue);
            }

            return bestMove;
        }

        private float Minimax(StateSnapshot position, int currentDepth, float alpha, float beta, ChainNode<AIUnit> units)
        {
            AIUnit currentUnit = units.Value;
            IsAliveProperty isAlive = (IsAliveProperty)position.GetProperty(currentUnit, typeof(IsAliveProperty));

            if (currentDepth <= 0 || !isAlive)
                return position.StaticEvaluation();

            if (currentUnit.IsPlayer)
            {
                float maxEvaluation = float.MinValue;
                foreach (IAction action in currentUnit.PossibleActions)
                {
                    StateSnapshot newPosition = action.Simulate(position, m_targetSelection(position));

                    float evaluation = Minimax(newPosition, currentDepth - 1, alpha, beta, units.Next);
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
                    StateSnapshot newPosition = action.Simulate(position, m_playerUnit);

                    float evaluation = Minimax(newPosition, currentDepth - 1, alpha, beta, units.Next);
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
