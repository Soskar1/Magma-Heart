namespace MagmaHeart.Core.Abilities.Effects.Presenters
{
    public class KillEveryoneEffectPresenter : IEffectPresenter<KillEveryoneEffect>
    {
        private readonly GameWorld m_world;
        private readonly int m_executorId;

        public KillEveryoneEffectPresenter(GameWorld world, int executorId)
        {
            m_world = world;
            m_executorId = executorId;
        }

        public void Present(KillEveryoneEffect effect)
        {
            m_world.TryGetEntity(m_executorId, out var entity);

            entity.EffectsPresenter.Show();
        }

        public void Hide()
        {
            m_world.TryGetEntity(m_executorId, out var entity);

            entity.EffectsPresenter.Hide();
        }
    }
}
