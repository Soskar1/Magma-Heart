using MagmaHeart.AI.Actions;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.BoardStateSystem.Actions.Input
{
    public record MagmaHeartActionInput(EntityModel TypedExecutor) : ActionInput(TypedExecutor);
}
