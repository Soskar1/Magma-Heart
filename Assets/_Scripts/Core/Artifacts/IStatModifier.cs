using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.Artifacts
{
    public interface IStatModifier
    {
        public void Apply(Entity entity);
        public void Revert(Entity entity);
        public StatModifierModel ToModel();
    }
}
