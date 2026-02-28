using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Targeting;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Plans
{
    public class Plan
    {
        public IEnumerable<PlanTask> Tasks { get; init; }
        internal List<AbilityPlan> ExecutedAbilities { get; init; }
        private readonly EffectDispatcher m_effectDispatcher;
        private readonly AbilityEngine m_abilityEngine;

        public Plan(IEnumerable<PlanTask> tasks, EffectDispatcher effectDispatcher, AbilityEngine abilityEngine)
        {
            Tasks = tasks;
            ExecutedAbilities = new List<AbilityPlan>();
            m_effectDispatcher = effectDispatcher;
            m_abilityEngine = abilityEngine;
        }

        public bool TryExecute(WorldSimulation simulation, AIUnitModel executor)
        {
            ExecutedAbilities.Clear();

            foreach (PlanTask task in Tasks)
            {
                bool executed = TryExecuteTask(task, simulation, executor, out AbilityPlan abilityPlan);

                if (executed)
                {
                    ExecutedAbilities.Add(abilityPlan);
                }
                else
                {
                    simulation.RestoreCheckpoint();

                    return false;
                }

                if (task.ExecuteUntilFail)
                    while (TryExecuteTask(task, simulation, executor, out abilityPlan))
                        ExecutedAbilities.Add(abilityPlan);
            }

            return true;
        }

        private bool TryExecuteTask(PlanTask task, WorldSimulation simulation, AIUnitModel executor, out AbilityPlan abilityPlan)
        {
            abilityPlan = null;

            AbilityTarget target = task.TargetSelector.SelectTarget(simulation, executor.Id);
            abilityPlan = m_abilityEngine.Plan(simulation, executor.Id, task.Ability, target);

            if (abilityPlan.IsLegal)
            {
                foreach (var effect in abilityPlan.Effects)
                    m_effectDispatcher.Apply(simulation, effect);

                return true;
            }

            return false;
        }
    }
}
