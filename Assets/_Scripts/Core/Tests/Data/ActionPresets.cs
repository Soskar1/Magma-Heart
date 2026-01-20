using MagmaHeart.AI.Actions;
using MagmaHeart.Core.BoardStateSystem.Actions.Data;
using MagmaHeart.Core.BoardStateSystem.Actions;
using System.Collections.Generic;

namespace MagmaHeart.Core.Tests
{
    public static class ActionPresets
    {
        public static readonly List<ActionData> MeleeAttacker = new List<ActionData>
        {
            new AttackActionData(2, 1, 1, AttackType.Melee),
            new MovementActionData(2)
        };

        public static readonly List<ActionData> RangedAttacker = new List<ActionData>
        {
            new AttackActionData(2, 6, 1, AttackType.Ranged),
            new MovementActionData(2)
        };
    }
}