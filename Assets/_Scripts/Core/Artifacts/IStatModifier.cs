using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.Artifacts
{
    public interface IStatModifier
    {
        public void Apply(EntityModel entity);
        public void Revert(EntityModel entity);
    }
}
