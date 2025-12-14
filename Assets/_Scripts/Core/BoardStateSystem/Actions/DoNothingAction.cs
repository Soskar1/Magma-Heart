using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;
using System.Collections.Generic;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public record EmptyPayload : ActionPayload;
    public record EmptyArgs(EntityModel TypedExecutor, EmptyPayload TypedPayload) : ActionArgs<EntityModel>(TypedExecutor);

    public class DoNothingAction : CombatAction<EmptyArgs, EmptyPayload>
    {
        public override IEnumerable<ActionArgs> CreateSimulationArguments(SimulatedBoardState state, AIUnitModel executor, EmptyPayload payload, IEnumerable<AIUnitModel> targets) => new List<ActionArgs>() { new ActionArgs(executor) };

        public override int GetEnergyCost(EmptyArgs args, BoardState boardState) => 0;
    }
}
