using MagmaHeart.Core.Entities.Models;

namespace MagmaHeart.Core.Entities
{
    public interface IHittable
    {
        public HealthModel Health { get; }
        public void Hit(float damage);
    }
}