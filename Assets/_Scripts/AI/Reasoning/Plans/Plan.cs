using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Execution;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Plans
{
    public class Plan
    {
        public IEnumerable<PlanTask> Tasks { get; init; }
        internal List<ExecutedTask> ExecutedTasks { get; init; }

        private readonly CommandRunner m_commandRunner;

        public Plan(IEnumerable<PlanTask> tasks, CommandRunner commandRunner)
        {
            Tasks = tasks;
            ExecutedTasks = new List<ExecutedTask>();
            m_commandRunner = commandRunner;
        }

        public bool TryExecute(Board simulation, AIUnitModel executor)
        {
            ExecutedTasks.Clear();

            foreach (PlanTask task in Tasks)
            {
                bool executed = task.TryExecute(simulation, executor, m_commandRunner, out ExecutedTask executedTask);

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
                    while (task.TryExecute(simulation, executor, m_commandRunner, out executedTask))
                        ExecutedTasks.Add(executedTask);
            }

            return true;
        }

        internal void Undo(Board simulation)
        {
            foreach (ExecutedTask _ in ExecutedTasks)
                m_commandRunner.Undo(simulation);
        }
    }
}
