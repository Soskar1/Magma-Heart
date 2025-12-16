using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public record EmptyArgs(EntityModel TypedExecutor) : ActionArgs<EntityModel>(TypedExecutor);

    public class DoNothingAction : CombatAction<EmptyArgs>
    {
        public override int GetEnergyCost(EmptyArgs args, BoardState boardState) => 0;
    }
}
