using MagmaHeart.Core.BoardStateSystem.Actions.Args;
using MagmaHeart.Core.BoardStateSystem.Actions.Input;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public record AttackActionArgs(TargetEntityActionInput TargetEntityInput, int EnergyCost, int AttackDistance, int AttackDamage, AttackType AttackType) : MagmaHeartActionArgs(TargetEntityInput);
}
