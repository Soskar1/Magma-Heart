using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Requirements;
using MagmaHeart.Abilities.Targeting;

namespace MagmaHeart.Core.Abilities.Requirements
{
    [System.Serializable]
    public class TileIsAccessibleRequirement : IAbilityRequirement
    {
        public bool IsMet(IGameWorld gameWorld, int executorId, AbilityTarget target)
        {
            bool isTargetingPosition = target.Kind.HasFlag(TargetKind.Position);
            
            if (!isTargetingPosition)
                return false;

            return gameWorld.PositionIsAccessible(target.Position);
        }
    }
}
