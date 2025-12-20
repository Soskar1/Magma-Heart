using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning.Plans
{
    public class PlanTask
    {
        public ActionDefinition ActionDefinition { get; init; }
        public UnitAction Action { get; init; }

        public PlanTask(UnitAction action, ActionDefinition definition)
        {
            Action = action;
            ActionDefinition = definition;
        }

        public bool TryExecute(SimulatedBoardState simulation, AIUnitModel executor, AIUnitModel target)
        {
            ActionArgs args = ActionDefinition.CreateArguments(executor, target, simulation);

            if (args == null)
                return false;

            if (!Action.CanExecute(args, simulation))
                return false;

            Action.Execute(args, simulation);
            return true;
        }
    }
}
