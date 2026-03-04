using MagmaHeart.Abilities.Effects;
using MagmaHeart.Core.Entities.Presenters;

namespace MagmaHeart.Core.Abilities.Effects.Presenters
{
    public class SpendResourceEffectPresenter : IEffectPresenter<SpendResourceEffect>
    {
        private readonly EnergyPresenter m_energyPresenter;

        public SpendResourceEffectPresenter(EnergyPresenter energyPresenter) => m_energyPresenter = energyPresenter;

        public void Present(GameWorld world, SpendResourceEffect effect)
        {
            if (effect.Parameter.Id == m_energyPresenter.Energy.Id)
                m_energyPresenter.DisplayCost(effect.Amount);
        }
    }
}
