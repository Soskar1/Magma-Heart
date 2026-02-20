using UnityEngine;

namespace MagmaHeart.Abilities.Targeting
{
    public abstract class TargetingModule : ScriptableObject
    {
        public abstract bool ValidateChosenTarget(IGameWorld gameWorld, int executorId, AbilityTarget target);
    }
}
