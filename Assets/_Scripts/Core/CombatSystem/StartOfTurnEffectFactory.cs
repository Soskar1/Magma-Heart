using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Effects;
using MagmaHeart.AI;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.Abilities.Effects;
using System.Collections.Generic;

namespace MagmaHeart.Core.CombatSystem
{
    public class StartOfTurnEffectFactory : IStartOfTurnEffectFactory
    {
        private readonly ParameterId m_energy;
        private readonly int m_energyRestorationAmount;

        public StartOfTurnEffectFactory(ParameterId energy, int energyRestorationAmount)
        {
            m_energy = energy;
            m_energyRestorationAmount = energyRestorationAmount;
        }

        public IReadOnlyList<AbilityEffect> CreateStartOfTurnEffects(IBoardGameWorld world, int entityId)
        {
            return new List<AbilityEffect>()
            {
                new RestoreParameterEffect(entityId, m_energy, m_energyRestorationAmount)
            };
        }
    }
}
