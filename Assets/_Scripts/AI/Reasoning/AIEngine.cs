using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Reasoning.Plans;
using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using System;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning
{
    public class AIEngine
    {
        private readonly int m_depth;
        private readonly Planner m_planner;
        private readonly TurnContext m_turnContext;
        private Strategy m_strategy;

        public AIEngine(Strategy strategy, ActionDatabase database, int lookAhead, TurnContext turnContext)
        {
            m_strategy = strategy;
            m_depth = lookAhead;
            m_turnContext = turnContext;

            m_planner = new Planner(strategy, database);
        }

        public BestPlan ChooseBestMove(ChainNode<AIUnitModel> unitTurns, ActualBoardState gameState)
        {  
            SimulatedBoardState simulation = new SimulatedBoardState(gameState.Board);

            float alpha = float.MinValue;
            float beta = float.MaxValue;
            float bestValue = float.MinValue;

            BestPlan bestPlan = null;
            List<Plan> plans = m_planner.GetPlans(unitTurns.Value);

            foreach (Plan plan in plans)
            {
                bool isExecuted = plan.TryExecute(simulation, unitTurns.Value);
                if (!isExecuted)
                    continue;

                float evaluation = Minimax(simulation, unitTurns.Next, m_depth - 1, alpha, beta);

                plan.Undo(simulation);

                if (evaluation > bestValue)
                {
                    bestValue = evaluation;
                    bestPlan = new BestPlan(plan.ExecutedTasks);
                }

                alpha = Math.Max(alpha, bestValue);
            }

            return bestPlan;
        }

        private float Minimax(SimulatedBoardState simulation, ChainNode<AIUnitModel> turns, int currentDepth, float alpha, float beta)
        {
            AIUnitModel currentUnit = turns.Value;
            IsAlivePropertySnapshot isAlive = simulation.GetProperty<IsAlivePropertySnapshot>(currentUnit);

            if (currentDepth <= 0 || !isAlive)
                return m_strategy.EvaluateState(simulation);

            m_turnContext.StartTurn(simulation, currentUnit);
            List<Plan> plans = m_planner.GetPlans(currentUnit);

            if (!currentUnit.IsPlayer)
            {
                // AI
                float maxEvaluation = float.MinValue;
                foreach (Plan plan in plans)
                {
                    bool isExecuted = plan.TryExecute(simulation, currentUnit);
                    if (!isExecuted)
                        continue;

                    float evaluation = Minimax(simulation, turns.Next, currentDepth - 1, alpha, beta);

                    plan.Undo(simulation);

                    maxEvaluation = Math.Max(maxEvaluation, evaluation);
                    alpha = Math.Max(alpha, evaluation);

                    if (beta <= alpha)
                        break;
                }

                m_turnContext.UndoTurn(simulation);
                return maxEvaluation;
            }
            else
            {
                // PLAYER
                float minEvaluation = float.MaxValue;
                foreach (Plan plan in plans)
                {
                    bool isExecuted = plan.TryExecute(simulation, currentUnit);
                    if (!isExecuted)
                        continue;

                    float evaluation = Minimax(simulation, turns.Next, currentDepth - 1, alpha, beta);

                    plan.Undo(simulation);

                    minEvaluation = Math.Min(minEvaluation, evaluation);
                    beta = Math.Min(beta, evaluation);

                    if (beta <= alpha)
                        break;
                }

                m_turnContext.UndoTurn(simulation);
                return minEvaluation;
            }
        }
    }
}
