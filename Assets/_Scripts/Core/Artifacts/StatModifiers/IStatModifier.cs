using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.Artifacts.StatModifiers
{
    public interface IStatModifier
    {
        public void Apply(EntityModel entity);
        public void Revert(EntityModel entity);
    }
}
