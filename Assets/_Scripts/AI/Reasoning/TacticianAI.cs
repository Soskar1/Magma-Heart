using System;
using MagmaHeart.AI.Boards;
using MagmaHeart.Collections;

namespace MagmaHeart.AI.Reasoning
{
    public class TacticianAI
    {
        private int m_depth;
        private AIUnit m_playerUnit;
        private Strategy m_strategy;

        public TacticianAI(Strategy strategy)
        {
            m_strategy = strategy;
            m_depth = m_strategy.LookAhead;
            m_playerUnit = strategy.Player;
        }

        public Action ChooseBestMove(CircularList<AIUnit> unitsToConsider, Board board)
        {  
            StateSnapshot stateSnapshot = StateSnapshotMaker.CreateStateSnapshot(unitsToConsider);
            SimulatedBoard simulatedBoard = board.CreateSimulatedBoard();

            ChainNode<AIUnit> head = (ChainNode<AIUnit>)unitsToConsider;
            float alpha = float.MinValue;
            float beta = float.MaxValue;
            float bestValue = float.MinValue;

            // TODO: filter moves according to current strategy
            Action bestMove = null;

            foreach (Action action in head.Value.PossibleActions)
            {
                if (!action.CanSimulate(stateSnapshot,simulatedBoard, m_playerUnit))
                    continue;

                StateSnapshot actionState = action.Simulate(stateSnapshot, simulatedBoard, m_playerUnit);

                float evaluation = Minimax(actionState, head.Next, simulatedBoard, m_depth - 1, alpha, beta);
                simulatedBoard.UndoBoardModification(action);

                if (evaluation > bestValue)
                {
                    bestValue = evaluation;
                    bestMove = action;
                }

                alpha = Math.Max(alpha, bestValue);
            }

            return bestMove;
        }

        private float Minimax(StateSnapshot position, ChainNode<AIUnit> units, SimulatedBoard board, int currentDepth, float alpha, float beta)
        {
            AIUnit currentUnit = units.Value;
            IsAlivePropertySnapshot isAlive = position.GetProperty<IsAlivePropertySnapshot>(currentUnit);

            if (currentDepth <= 0 || !isAlive)
                return m_strategy.EvaluateState(position);

            if (!currentUnit.IsPlayer)
            {
                // AI
                float maxEvaluation = float.MinValue;
                foreach (Action action in currentUnit.PossibleActions)
                {
                    if (!action.CanSimulate(position, board, m_playerUnit))
                        continue;

                    StateSnapshot newPosition = action.Simulate(position, board, m_playerUnit);

                    float evaluation = Minimax(newPosition, units.Next, board, currentDepth - 1, alpha, beta);
                    board.UndoBoardModification(action);

                    maxEvaluation = Math.Max(maxEvaluation, evaluation);
                    alpha = Math.Max(alpha, evaluation);
                    
                    if (beta <= alpha)
                        break;
                }

                return maxEvaluation;
            }
            else
            {
                // PLAYER
                AIUnit target = m_strategy.PlayerTargetSelection(position);

                float minEvaluation = float.MaxValue;
                foreach (Action action in currentUnit.PossibleActions)
                {
                    if (!action.CanSimulate(position, board, target))
                        continue;

                    StateSnapshot newPosition = action.Simulate(position, board,target);

                    float evaluation = Minimax(newPosition, units.Next, board, currentDepth - 1, alpha, beta);
                    board.UndoBoardModification(action);

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
