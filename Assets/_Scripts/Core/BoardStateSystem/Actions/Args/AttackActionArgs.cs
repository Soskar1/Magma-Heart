using MagmaHeart.AI.Actions;
using MagmaHeart.Core.BoardStateSystem.Actions.Data;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public record AttackActionArgs(EntityModel TypedExecutor, EntityModel Target, AttackActionData AttackActionData) : ActionArgs<EntityModel>(TypedExecutor, AttackActionData);
}
