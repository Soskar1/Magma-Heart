using MagmaHeart.AI;
using MagmaHeart.AI.Actions;

namespace MagmaHeart.Core.CombatSystem
{
    public record AttackActionArgs(AIUnit Target) : ActionArgs;
}
