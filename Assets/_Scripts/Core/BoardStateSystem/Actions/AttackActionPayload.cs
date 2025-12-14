using MagmaHeart.AI;
using MagmaHeart.AI.Actions;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public record AttackActionPayload(int EnergyCost, int AttackDistance, int AttackDamage) : ActionPayload;
}
