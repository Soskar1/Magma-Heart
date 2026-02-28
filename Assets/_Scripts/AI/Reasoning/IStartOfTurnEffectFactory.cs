using MagmaHeart.Abilities.Effects;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning
{
    public interface IStartOfTurnEffectFactory
    {
        public IReadOnlyList<AbilityEffect> CreateStartOfTurnEffects(IBoardGameWorld world, int entityId);
    }
}
