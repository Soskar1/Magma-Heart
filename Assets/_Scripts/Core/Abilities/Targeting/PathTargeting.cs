using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Targeting;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Targeting
{
    [CreateAssetMenu(menuName = "Abilities/Targeting/Path Targeting")]
    public class PathTargeting : TargetingModule
    {
        public override bool ValidateChosenTarget(IGameWorld world, int executorId, AbilityTarget target)
        {
            if (target.Kind == TargetKind.Path && target.Path != null && target.Path.Count > 0)
                return true;

            return false;
        }
    }
}
