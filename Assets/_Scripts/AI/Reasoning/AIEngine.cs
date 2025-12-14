using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using System;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning
{
    public class AIEngine
    {
        private readonly int m_depth;
        private readonly ActionDatabase m_actionDatabase;
        private readonly ActionSimulationFilter m_filter;
        private Strategy m_strategy;
        private IArgumentResolver m_argumentResolver;

        public AIEngine(Strategy strategy, ActionDatabase database, int lookAhead)
        {
            m_strategy = strategy;
            m_depth = lookAhead;
            m_actionDatabase = database;

            m_argumentResolver = new DefaultArgumentResolver();
            m_filter = new ActionSimulationFilter(m_argumentResolver, m_actionDatabase);
        }

        public BestAction ChooseBestMove(ChainNode<TurnContext> unitTurns, ActualBoardState gameState)
        {  
            SimulatedBoardState simulation = new SimulatedBoardState(gameState.Board);

            float alpha = float.MinValue;
            float beta = float.MaxValue;
            float bestValue = float.MinValue;

            BestAction bestAction = null;
            List<ActionSimulation> possibleSimulations = m_filter.GetActionSimulations(simulation, unitTurns.Value.Model);

            foreach (ActionSimulation possibleSimulation in possibleSimulations)
            {
                UnitAction action = possibleSimulation.Action;
                foreach (ActionArgs args in possibleSimulation.SimulationArgs)
                {
                    action.Execute(args, simulation);

                    float evaluation = Minimax(simulation, unitTurns.Next, m_depth - 1, alpha, beta);

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

        private float Minimax(SimulatedBoardState simulation, ChainNode<TurnContext> turns, int currentDepth, float alpha, float beta)
        {
            TurnContext currentTurnContext = turns.Value;
            AIUnitModel currentUnit = currentTurnContext.Model;
            IsAlivePropertySnapshot isAlive = simulation.GetProperty<IsAlivePropertySnapshot>(currentUnit);

            if (currentDepth <= 0 || !isAlive)
                return m_strategy.EvaluateState(simulation);

            currentTurnContext.StartTurn(simulation);
            List<ActionSimulation> possibleSimulations = m_filter.GetActionSimulations(simulation, currentUnit);

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
                        
                        float evaluation = Minimax(simulation, turns.Next, currentDepth - 1, alpha, beta);
                        
                        simulation.Undo();
                        
                        maxEvaluation = Math.Max(maxEvaluation, evaluation);
                        alpha = Math.Max(alpha, evaluation);

                        if (beta <= alpha)
                            break;
                    }
                }

                currentTurnContext.UndoTurn(simulation);
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

                        float evaluation = Minimax(simulation, turns.Next, currentDepth - 1, alpha, beta);
                        
                        simulation.Undo();

                        minEvaluation = Math.Min(minEvaluation, evaluation);
                        beta = Math.Min(beta, evaluation);

                        if (beta <= alpha)
                            break;
                    }
                }

                currentTurnContext.UndoTurn(simulation);
                return minEvaluation;
            }
        }
    }
}
