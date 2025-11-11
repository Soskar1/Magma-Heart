using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Action = MagmaHeart.AI.Actions.Action;

namespace MagmaHeart.AI.Reasoning
{
    public class TacticianAI
    {
        private int m_depth;
        private Strategy m_strategy;

        public TacticianAI(Strategy strategy)
        {
            m_strategy = strategy;
            m_depth = m_strategy.LookAhead;
        }

        public BestAction ChooseBestMove(CircularList<AIUnit> unitsToConsider, Board board)
        {  
            StateSnapshot stateSnapshot = StateSnapshotMaker.CreateStateSnapshot(unitsToConsider);
            SimulatedBoard simulatedBoard = board.CreateSimulatedBoard();

            ChainNode<AIUnit> head = (ChainNode<AIUnit>)unitsToConsider;
            float alpha = float.MinValue;
            float beta = float.MaxValue;
            float bestValue = float.MinValue;

            BestAction bestAction = null;
            List<ActionSimulation> possibleSimulations = ActionSimulationFilter.GetActionSimulations(stateSnapshot, simulatedBoard, head.Value.PossibleActions.ToList());

            foreach (ActionSimulation simulation in possibleSimulations)
            {
                Action action = simulation.Action;
                foreach (ActionArgs args in simulation.SimulationArgs)
                {
                    StateSnapshot actionState = action.Simulate(stateSnapshot, simulatedBoard, args);
                    
                    float evaluation = Minimax(actionState, head.Next, simulatedBoard, m_depth - 1, alpha, beta);
                    simulatedBoard.UndoBoardModification(actionState.CurrentSimulationDepth);
                    
                    if (evaluation > bestValue)
                    {
                        bestValue = evaluation;
                        bestAction = new BestAction(action, args);
                    }
                    
                    alpha = Math.Max(alpha, bestValue);
                }
            }

            return bestAction;
        }

        // TODO: Remove code duplication
        private float Minimax(StateSnapshot state, ChainNode<AIUnit> units, SimulatedBoard board, int currentDepth, float alpha, float beta)
        {
            AIUnit currentUnit = units.Value;
            IsAlivePropertySnapshot isAlive = state.GetProperty<IsAlivePropertySnapshot>(currentUnit);

            if (currentDepth <= 0 || !isAlive)
                return m_strategy.EvaluateState(state);

            List<ActionSimulation> simulations = ActionSimulationFilter.GetActionSimulations(state, board, currentUnit.PossibleActions.ToList());

            if (!currentUnit.IsPlayer)
            {
                // AI
                float maxEvaluation = float.MinValue;
                foreach (ActionSimulation simulation in simulations)
                {
                    Action action = simulation.Action;
                    foreach (ActionArgs args in simulation.SimulationArgs)
                    {
                        StateSnapshot newState = action.Simulate(state, board, args);
                        float evaluation = Minimax(newState, units.Next, board, currentDepth - 1, alpha, beta);
                        
                        board.UndoBoardModification(newState.CurrentSimulationDepth);
                        
                        maxEvaluation = Math.Max(maxEvaluation, evaluation);
                        alpha = Math.Max(alpha, evaluation);

                        if (beta <= alpha)
                            break;
                    }
                }

                return maxEvaluation;
            }
            else
            {
                // PLAYER
                float minEvaluation = float.MaxValue;
                foreach (ActionSimulation simulation in simulations)
                {
                    Action action = simulation.Action;
                    foreach (ActionArgs args in simulation.SimulationArgs)
                    {
                        StateSnapshot newPosition = action.Simulate(state, board, args);

                        float evaluation = Minimax(newPosition, units.Next, board, currentDepth - 1, alpha, beta);
                        board.UndoBoardModification(newPosition.CurrentSimulationDepth);

                        minEvaluation = Math.Min(minEvaluation, evaluation);
                        beta = Math.Min(beta, evaluation);

                        if (beta <= alpha)
                            break;
                    }
                }

                return minEvaluation;
            }
        }
    }
}
