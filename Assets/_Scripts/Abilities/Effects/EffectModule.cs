using MagmaHeart.Abilities.Targeting;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Abilities.Effects
{
    public abstract class EffectModule : ScriptableObject
    {
        public abstract IEnumerable<AbilityEffect> BuildEffects(IGameWorld gameWorld, int executorId, AbilityTarget target);
    }
}
