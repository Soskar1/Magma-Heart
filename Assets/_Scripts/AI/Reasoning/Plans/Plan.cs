using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Plans
{
    public class Plan
    {
        public IEnumerable<PlanTask> Tasks { get; init; }
        internal List<PlanTask> ExecutedTasks { get; init; }

        public Plan(IEnumerable<PlanTask> tasks)
        {
            Tasks = tasks;
            ExecutedTasks = new List<PlanTask>();
        }

        public bool TryExecute(SimulatedBoardState simulation, AIUnitModel executor, AIUnitModel target)
        {
            ExecutedTasks.Clear();

            foreach (PlanTask task in Tasks)
            {
                bool executed = task.TryExecute(simulation, executor, target);

                if (executed)
                {
                    ExecutedTasks.Add(task);
                }
                else
                {
                    Undo(simulation);

                    return false;
                }

                if (task.ExecuteUntilFail)
                    while (task.TryExecute(simulation, executor, target))
                        ExecutedTasks.Add(task);
            }

            return true;
        }

        internal void Undo(SimulatedBoardState simulation)
        {
            foreach (PlanTask _ in ExecutedTasks)
                simulation.Undo();
        }
    }
}
