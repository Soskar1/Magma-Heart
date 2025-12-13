using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public record AttackActionArgs(EntityModel TypedExecutor, EntityModel Target) : ActionArgs<EntityModel>(TypedExecutor);
}
