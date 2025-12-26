using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning.Plans
{
    public class PlanTask
    {
        public ActionDefinition ActionDefinition { get; init; }
        public UnitAction Action { get; init; }
        public bool ExecuteUntilFail { get; init; }

        public PlanTask(UnitAction action, ActionDefinition definition, bool executeUntilFail = false)
        {
            Action = action;
            ActionDefinition = definition;
            ExecuteUntilFail = executeUntilFail;
        }

        public bool TryExecute(SimulatedBoardState simulation, AIUnitModel executor, AIUnitModel target)
        {
            if (!ActionDefinition.TryResolve(executor, simulation, out ActionArgs args))
                return false;

            if (!Action.CanExecute(args, simulation))
                return false;

            Action.Execute(args, simulation);
            return true;
        }
    }
}
