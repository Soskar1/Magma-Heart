using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.BoardStateSystem.Actions.Input
{
    public record TargetEntityActionInput(EntityModel TypedExecutor, EntityModel Target) : MagmaHeartActionInput(TypedExecutor);
}
