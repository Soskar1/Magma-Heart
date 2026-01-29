using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Execution;
using System.Collections.Generic;

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

        public bool TryExecute(Board simulation, AIUnitModel executor, CommandRunner runner, out ExecutedTask executedTask)
        {
            executedTask = null;

            if (!Action.TryGenerateArgs(executor, ActionDefinition.Data, simulation, out ActionArgs args))
                return false;

            IEnumerable<IBoardCommand> commands = Action.Execute(args, simulation);
            runner.Apply(simulation, commands);

            executedTask = new ExecutedTask(Action, args);
            return true;
        }
    }
}
