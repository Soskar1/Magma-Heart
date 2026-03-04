using MagmaHeart.Abilities.Targeting;
using System;
using System.Collections.Generic;

namespace MagmaHeart.Abilities.Effects
{
    [Serializable]
    public abstract class EffectModule
    {
        public abstract IEnumerable<AbilityEffect> BuildEffects(IGameWorld gameWorld, int executorId, AbilityTarget target);
    }
}
