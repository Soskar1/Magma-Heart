using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
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
        private CommandRunner m_runner;

        public AIEngine(Strategy strategy, ActionDatabase database, int lookAhead, TurnContext turnContext)
        {
            m_strategy = strategy;
            m_depth = lookAhead;
            m_turnContext = turnContext;
            m_runner = new CommandRunner();

            m_planner = new Planner(strategy, database, m_runner);
        }

        public BestPlan ChooseBestMove(ChainNode<int> unitTurnIds, Board board)
        {  
            Board simulation = board.DeepCopy();

            float alpha = float.MinValue;
            float beta = float.MaxValue;
            float bestValue = float.MinValue;

            simulation.TryGetUnit(unitTurnIds.Value, out AIUnitModel currentUnit);
            
            BestPlan bestPlan = null;
            List<Plan> plans = m_planner.GetPlans(currentUnit);

            foreach (Plan plan in plans)
            {
                bool isExecuted = plan.TryExecute(simulation, currentUnit);
                if (!isExecuted)
                    continue;

                float evaluation = Minimax(simulation, unitTurnIds.Next, m_depth - 1, alpha, beta);

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

        private float Minimax(Board simulation, ChainNode<int> turns, int currentDepth, float alpha, float beta)
        {
            simulation.TryGetUnit(turns.Value, out AIUnitModel currentUnit);

            if (currentDepth <= 0 || currentUnit.IsDisabled)
                return m_strategy.EvaluateState(simulation);

            IEnumerable<IBoardCommand> commands = m_turnContext.StartTurn(currentUnit);
            m_runner.Apply(simulation, commands);

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

                m_runner.Undo(simulation);
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

                m_runner.Undo(simulation);
                return minEvaluation;
            }
        }
    }
}
