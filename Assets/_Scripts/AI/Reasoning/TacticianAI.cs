using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        public BestAction ChooseBestMove(ChainNode<AIUnit> unitOrder, ActualBoardState gameState)
        {  
            SimulatedBoardState simulation = new SimulatedBoardState(gameState.Board);

            float alpha = float.MinValue;
            float beta = float.MaxValue;
            float bestValue = float.MinValue;

            BestAction bestAction = null;
            List<ActionSimulation> possibleSimulations = ActionSimulationFilter.GetActionSimulations(simulation, unitOrder.Value.PossibleActions.ToList());

            Debug.Log($"[ROOT] Got all possible simulations. Count: {possibleSimulations.Count}");

            foreach (ActionSimulation possibleSimulation in possibleSimulations)
            {
                UnitAction action = possibleSimulation.Action;
                foreach (ActionArgs args in possibleSimulation.SimulationArgs)
                {
                    action.Execute(args, simulation);

                    float evaluation = Minimax(simulation, unitOrder.Next, m_depth - 1, alpha, beta);

                    simulation.Undo();
                    
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
        private float Minimax(SimulatedBoardState simulation, ChainNode<AIUnit> units, int currentDepth, float alpha, float beta)
        {
            AIUnit currentUnit = units.Value;
            IsAlivePropertySnapshot isAlive = simulation.GetProperty<IsAlivePropertySnapshot>(currentUnit);

            if (currentDepth <= 0 || !isAlive)
                return m_strategy.EvaluateState(simulation);

            List<ActionSimulation> possibleSimulations = ActionSimulationFilter.GetActionSimulations(simulation, currentUnit.PossibleActions.ToList());
            Debug.Log($"[{currentDepth}] Got all possible simulations. Count: {possibleSimulations.Count}");

            if (!currentUnit.IsPlayer)
            {
                // AI
                float maxEvaluation = float.MinValue;
                foreach (ActionSimulation possibleSimulation in possibleSimulations)
                {
                    UnitAction action = possibleSimulation.Action;
                    foreach (ActionArgs args in possibleSimulation.SimulationArgs)
                    {
                        action.Execute(args, simulation);
                        
                        float evaluation = Minimax(simulation, units.Next, currentDepth - 1, alpha, beta);
                        
                        simulation.Undo();
                        
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
                foreach (ActionSimulation possibleSimulation in possibleSimulations)
                {
                    UnitAction action = possibleSimulation.Action;
                    foreach (ActionArgs args in possibleSimulation.SimulationArgs)
                    {
                        action.Execute(args, simulation);

                        float evaluation = Minimax(simulation, units.Next, currentDepth - 1, alpha, beta);
                        
                        simulation.Undo();

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
