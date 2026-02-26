using MagmaHeart.Abilities;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Execution;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Plans
{
    public class Plan
    {
        public IEnumerable<PlanTask> Tasks { get; init; }
        internal List<AbilityPlan> ExecutedAbilities { get; init; }

        private readonly CommandRunner m_commandRunner;

        public Plan(IEnumerable<PlanTask> tasks, CommandRunner commandRunner)
        {
            Tasks = tasks;
            ExecutedAbilities = new List<AbilityPlan>();
            m_commandRunner = commandRunner;
        }

        public bool TryExecute(Board simulation, AIUnitModel executor)
        {
            ExecutedAbilities.Clear();

            foreach (PlanTask task in Tasks)
            {
                bool executed = task.TryExecute(simulation, executor, m_commandRunner, out AbilityPlan abilityPlan);

                if (executed)
                {
                    ExecutedAbilities.Add(abilityPlan);
                }
                else
                {
                    Undo(simulation);

                    return false;
                }

                if (task.ExecuteUntilFail)
                    while (task.TryExecute(simulation, executor, m_commandRunner, out abilityPlan))
                        ExecutedAbilities.Add(abilityPlan);
            }

            return true;
        }

        internal void Undo(Board simulation)
        {
            foreach (AbilityPlan _ in ExecutedAbilities)
                m_commandRunner.Undo(simulation);
        }
    }
}
