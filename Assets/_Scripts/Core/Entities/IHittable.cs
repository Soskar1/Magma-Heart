namespace MagmaHeart.Core.Entities
{
    public interface IHittable
    {
        void Hit(in float damage);
    }
}