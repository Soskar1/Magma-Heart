using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning.Plans
{
    public class PlanTask
    {
        public UnitAction Action { get; private set; }

        public PlanTask(UnitAction action)
        {
            Action = action;
        }

        public bool TryExecute(SimulatedBoardState simulation, ActionArgs args)
        {
            if (!Action.CanExecute(args, simulation))
                return false;

            Action.Execute(args, simulation);
            return true;
        }
    }
}
