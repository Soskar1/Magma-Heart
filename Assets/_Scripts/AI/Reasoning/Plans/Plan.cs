using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Plans
{
    public class Plan
    {
        public IEnumerable<PlanTask> Tasks { get; init; }
        internal List<ExecutedTask> ExecutedTasks { get; init; }

        public Plan(IEnumerable<PlanTask> tasks)
        {
            Tasks = tasks;
            ExecutedTasks = new List<ExecutedTask>();
        }

        public bool TryExecute(SimulatedBoardState simulation, AIUnitModel executor)
        {
            ExecutedTasks.Clear();

            foreach (PlanTask task in Tasks)
            {
                bool executed = task.TryExecute(simulation, executor, out ExecutedTask executedTask);

                if (executed)
                {
                    ExecutedTasks.Add(executedTask);
                }
                else
                {
                    Undo(simulation);

                    return false;
                }

                if (task.ExecuteUntilFail)
                    while (task.TryExecute(simulation, executor, out executedTask))
                        ExecutedTasks.Add(executedTask);
            }

            return true;
        }

        internal void Undo(SimulatedBoardState simulation)
        {
            foreach (ExecutedTask _ in ExecutedTasks)
                simulation.Undo();
        }
    }
}
