using MagmaHeart.Abilities.Targeting;
using MagmaHeart.AI;
using MagmaHeart.AI.Reasoning;

namespace MagmaHeart.Core.AI
{
    [System.Serializable]
    public class SelfTargetSelector : ITargetSelector
    {
        public AbilityTarget SelectTarget(IBoardGameWorld world, int executorId)
        {
            return AbilityTarget.EntityTarget(executorId, null);
        }
    }
}
