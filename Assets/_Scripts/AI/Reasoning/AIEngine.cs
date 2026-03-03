using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Effects;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Reasoning.Plans;
using MagmaHeart.Collections;
using System;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning
{
    public class AIEngine
    {
        private readonly int m_depth;
        private Strategy m_strategy;
        private readonly EffectDispatcher m_effectDispatcher;
        private readonly IStartOfTurnEffectFactory m_startOfTurnEffectFactory;
        private readonly AbilityEngine m_abilityEngine;

        public AIEngine(Strategy strategy, int lookAhead, IStartOfTurnEffectFactory startOfTurnEffects, EffectDispatcher effectDispatcher)
        {
            m_strategy = strategy;
            m_depth = lookAhead;
            m_startOfTurnEffectFactory = startOfTurnEffects;
            m_effectDispatcher = effectDispatcher;
            m_abilityEngine = new AbilityEngine();
        }

        public IEnumerable<AbilityPlan> ChooseBestMove(ChainNode<int> unitTurnIds, Board board)
        {  
            Board boardCopy = board.DeepCopy();
            WorldSimulation worldSimulation = new WorldSimulation(boardCopy);

            float alpha = float.MinValue;
            float beta = float.MaxValue;
            float bestValue = float.MinValue;

            AIUnitModel currentUnit = worldSimulation.GetUnit(unitTurnIds.Value);

            IEnumerable<AbilityPlan> bestPlan = null;
            List<Plan> plans = GetPlans(currentUnit);

            foreach (Plan plan in plans)
            {
                bool isExecuted = plan.TryExecute(worldSimulation, currentUnit);
                if (!isExecuted)
                    continue;

                worldSimulation.SaveCheckpoint();

                float evaluation = Minimax(worldSimulation, unitTurnIds.Next, m_depth - 1, alpha, beta);

                worldSimulation.RestoreCheckpoint();

                if (evaluation > bestValue)
                {
                    bestValue = evaluation;
                    bestPlan = plan.ExecutedAbilities;
                }

                alpha = Math.Max(alpha, bestValue);
            }

            return bestPlan;
        }

        public List<Plan> GetPlans(AIUnitModel executor)
        {
            List<Plan> plans = new List<Plan>();

            foreach (PlanDefinition planDefinition in executor.Plans)
            {
                Plan plan = new Plan(planDefinition.TaskDefinitions, m_effectDispatcher, m_abilityEngine);

                if (plan == null)
                    continue;

                plans.Add(plan);
            }

            return plans;
        }

        private float Minimax(WorldSimulation simulation, ChainNode<int> turns, int currentDepth, float alpha, float beta)
        {
            AIUnitModel currentUnit = simulation.GetUnit(turns.Value);

            if (currentDepth <= 0 || currentUnit.IsDisabled())
                return m_strategy.EvaluateState(simulation);

            IReadOnlyList<AbilityEffect> startOfTurnEffects = m_startOfTurnEffectFactory.CreateStartOfTurnEffects(simulation, currentUnit.Id);
            foreach (AbilityEffect effect in startOfTurnEffects) 
                m_effectDispatcher.Apply(simulation, effect);

            simulation.SaveCheckpoint();

            List<Plan> plans = GetPlans(currentUnit);

            if (!currentUnit.IsPlayer)
            {
                // AI
                float maxEvaluation = float.MinValue;
                foreach (Plan plan in plans)
                {
                    bool isExecuted = plan.TryExecute(simulation, currentUnit);
                    if (!isExecuted)
                        continue;
                    simulation.SaveCheckpoint();

                    float evaluation = Minimax(simulation, turns.Next, currentDepth - 1, alpha, beta);

                    simulation.RestoreCheckpoint();

                    maxEvaluation = Math.Max(maxEvaluation, evaluation);
                    alpha = Math.Max(alpha, evaluation);

                    if (beta <= alpha)
                        break;
                }

                simulation.RestoreCheckpoint();
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

                    simulation.SaveCheckpoint();

                    float evaluation = Minimax(simulation, turns.Next, currentDepth - 1, alpha, beta);

                    simulation.RestoreCheckpoint();

                    minEvaluation = Math.Min(minEvaluation, evaluation);
                    beta = Math.Min(beta, evaluation);

                    if (beta <= alpha)
                        break;
                }

                simulation.RestoreCheckpoint();
                return minEvaluation;
            }
        }
    }
}
