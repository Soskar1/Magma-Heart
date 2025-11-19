using MagmaHeart.AI.Actions;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public record AttackActionArgs(EntityModel Target) : ActionArgs;
}
