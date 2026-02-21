using MagmaHeart.Abilities.Effects;
using MagmaHeart.Abilities.Resources;

namespace MagmaHeart.Core.Abilities.Effects.Handlers
{
    public interface IResourceService
    {
        public void Spend(int entityId, ResourceId resource, int amount);
    }

    public sealed class SpendResourceHandler : IEffectHandler<SpendResourceEffect>
    {
        private readonly IResourceService m_resourceService;

        public SpendResourceHandler(IResourceService resources) => m_resourceService = resources;

        public void Handle(SpendResourceEffect e) => m_resourceService.Spend(e.ExecutorId, e.Resource, e.Amount);
    }
}
