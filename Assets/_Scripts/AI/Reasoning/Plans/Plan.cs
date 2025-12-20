using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Plans
{
    public class Plan
    {
        public IEnumerable<PlanTask> Tasks { get; init; }
        private readonly List<PlanTask> m_executedTasks;

        public Plan(IEnumerable<PlanTask> tasks)
        {
            Tasks = tasks;
            m_executedTasks = new List<PlanTask>();
        }

        public bool TryExecute(SimulatedBoardState simulation, AIUnitModel executor, AIUnitModel target)
        {
            m_executedTasks.Clear();

            foreach (PlanTask task in Tasks)
            {
                bool executed = task.TryExecute(simulation, executor, target);

                if (executed)
                {
                    m_executedTasks.Add(task);
                }
                else
                {
                    foreach (PlanTask _ in m_executedTasks)
                        simulation.Undo();

                    return false;
                }
            }

            return true;
        }
    }
}
