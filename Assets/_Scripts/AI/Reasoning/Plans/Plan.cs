using MagmaHeart.Abilities;
using MagmaHeart.AI.Boards;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Plans
{
    public class Plan
    {
        public IEnumerable<PlanTask> Tasks { get; init; }
        internal List<AbilityPlan> ExecutedAbilities { get; init; }


        public Plan(IEnumerable<PlanTask> tasks)
        {
            Tasks = tasks;
            ExecutedAbilities = new List<AbilityPlan>();
        }

        public bool TryExecute(Board simulation, AIUnitModel executor)
        {
            ExecutedAbilities.Clear();

            foreach (PlanTask task in Tasks)
            {
                bool executed = task.TryExecute(simulation, executor, out AbilityPlan abilityPlan);

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
                    while (task.TryExecute(simulation, executor, out abilityPlan))
                        ExecutedAbilities.Add(abilityPlan);
            }

            return true;
        }

        internal void Undo(Board simulation)
        {
            foreach (AbilityPlan _ in ExecutedAbilities)
            {

            }    
        }
    }
}
