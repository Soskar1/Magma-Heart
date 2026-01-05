using MagmaHeart.AI.Actions;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public record AttackActionArgs(EntityModel TypedExecutor, EntityModel Target, int EnergyCost, int AttackDistance, int AttackDamage, AttackType AttackType) : ActionArgs<EntityModel>(TypedExecutor);
}
