using MagmaHeart.Core.Entities;

namespace Assets._Scripts.Core.Artifacts.StatModifiers
{
    public interface IStatModifier
    {
        public void Apply(EntityModel entity);
        public void Revert(EntityModel entity);
    }
}
