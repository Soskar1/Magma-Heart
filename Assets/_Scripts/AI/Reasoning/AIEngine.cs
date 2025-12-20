using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Plans;
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

        public AIEngine(Strategy strategy, ActionDatabase database, int lookAhead)
        {
            m_strategy = strategy;
            m_depth = lookAhead;
            m_actionDatabase = database;

            m_filter = new ActionSimulationFilter(strategy, m_actionDatabase);
        }

        public BestPlan ChooseBestMove(ChainNode<TurnContext> unitTurns, ActualBoardState gameState)
        {  
            SimulatedBoardState simulation = new SimulatedBoardState(gameState.Board);

            float alpha = float.MinValue;
            float beta = float.MaxValue;
            float bestValue = float.MinValue;

            BestPlan bestPlan = null;
            List<PlanSimulation> possiblePlans = m_filter.GetPossiblePlans(simulation, unitTurns.Value.Model);

            foreach (PlanSimulation possiblePlan in possiblePlans)
            {
                Plan plan = possiblePlan.Plan;
                foreach (ActionArgs args in possiblePlan.SimulationArgs)
                {
                    bool isExecuted = plan.TryExecute(simulation, args);
                    if (isExecuted)
                    {
                        float evaluation = Minimax(simulation, unitTurns.Next, m_depth - 1, alpha, beta);

                        simulation.Undo();

                        if (evaluation > bestValue)
                        {
                            bestValue = evaluation;
                            bestPlan = new BestPlan(plan, args);
                        }

                        alpha = Math.Max(alpha, bestValue);
                    }
                }
            }

            return bestPlan;
        }

        private float Minimax(SimulatedBoardState simulation, ChainNode<TurnContext> turns, int currentDepth, float alpha, float beta)
        {
            TurnContext currentTurnContext = turns.Value;
            AIUnitModel currentUnit = currentTurnContext.Model;
            IsAlivePropertySnapshot isAlive = simulation.GetProperty<IsAlivePropertySnapshot>(currentUnit);

            if (currentDepth <= 0 || !isAlive)
                return m_strategy.EvaluateState(simulation);

            currentTurnContext.StartTurn(simulation);
            List<PlanSimulation> possiblePlans = m_filter.GetPossiblePlans(simulation, currentUnit);

            if (!currentUnit.IsPlayer)
            {
                // AI
                float maxEvaluation = float.MinValue;
                foreach (PlanSimulation possiblePlan in possiblePlans)
                {
                    Plan plan = possiblePlan.Plan;
                    foreach (ActionArgs args in possiblePlan.SimulationArgs)
                    {
                        bool isExecuted = plan.TryExecute(simulation, args);
                        if (isExecuted)
                        {
                            float evaluation = Minimax(simulation, turns.Next, currentDepth - 1, alpha, beta);

                            simulation.Undo();

                            maxEvaluation = Math.Max(maxEvaluation, evaluation);
                            alpha = Math.Max(alpha, evaluation);

                            if (beta <= alpha)
                                break;
                        }
                    }
                }

                currentTurnContext.UndoTurn(simulation);
                return maxEvaluation;
            }
            else
            {
                // PLAYER
                float minEvaluation = float.MaxValue;
                foreach (PlanSimulation possiblePlan in possiblePlans)
                {
                    Plan plan = possiblePlan.Plan;
                    foreach (ActionArgs args in possiblePlan.SimulationArgs)
                    {
                        bool isExecuted = plan.TryExecute(simulation, args);
                        if (isExecuted)
                        {
                            float evaluation = Minimax(simulation, turns.Next, currentDepth - 1, alpha, beta);

                            simulation.Undo();

                            minEvaluation = Math.Min(minEvaluation, evaluation);
                            beta = Math.Min(beta, evaluation);

                            if (beta <= alpha)
                                break;
                        }
                    }
                }

                currentTurnContext.UndoTurn(simulation);
                return minEvaluation;
            }
        }
    }
}
