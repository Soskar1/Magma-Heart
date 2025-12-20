using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Plans
{
    public class Plan
    {
        //private List<PlanTask> m_tasks = new List<PlanTask>();
        public PlanTask Task { get; init; }

        public Plan(PlanTask task)
        {
            Task = task;
        }

        public bool TryExecute(SimulatedBoardState simulation, ActionArgs args)
        {
            return Task.TryExecute(simulation, args);

            // TODO: Handle TryExecute false case after adding task chaining
        }
    }
}
