using MagmaHeart.Abilities;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Execution;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Plans
{
    public class PlanTask
    {
        public AbilityDefinition Ability { get; init; }
        public bool ExecuteUntilFail { get; init; }

        public PlanTask(AbilityDefinition definition, bool executeUntilFail = false)
        {
            Ability = definition;
            ExecuteUntilFail = executeUntilFail;
        }

        public bool TryExecute(Board simulation, AIUnitModel executor, CommandRunner runner, out AbilityPlan abilityPlan)
        {
            abilityPlan = null;

            //if (!Action.TryGenerateArgs(executor, ActionDefinition.Data, simulation, out ActionArgs args))
            //    return false;

            //IEnumerable<IBoardCommand> commands = Action.Execute(args, simulation);
            //runner.Apply(simulation, commands);

            //abilityPlan = new ExecutedTask(Action, args);
            return true;
        }
    }
}
