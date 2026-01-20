namespace MagmaHeart.Core.ObjectPooling
{
    public interface IPoolable
    {
        public void OnSpawn();
        public void OnReturnToPool();
    }
}