namespace MagmaHeart.Core
{
    public interface IPoolable
    {
        public void OnSpawn();
        public void OnReturnToPool();
    }
}