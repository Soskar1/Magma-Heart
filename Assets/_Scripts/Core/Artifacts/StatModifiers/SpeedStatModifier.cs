using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.Artifacts.StatModifiers
{
    public class SpeedStatModifier : IStatModifier
    {
        public int AdditionalSpeed { get; init; }

        public SpeedStatModifier(int additionalSpeed) => AdditionalSpeed = additionalSpeed;

        public void Apply(EntityModel entity) => entity.Speed.CurrentSpeed += AdditionalSpeed;
        public void Revert(EntityModel entity) => entity.Speed.CurrentSpeed -= AdditionalSpeed;
    }
}
