namespace MagmaHeart.Core.Entities
{
    public interface IHittable
    {
        public Health Health { get; }
        public void Hit(float damage);
    }
}