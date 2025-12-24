using UnityEngine;

namespace MagmaHeart.Spawning
{
    public interface IInstantiator
    {
        public GameObject Instantiate(GameObject prefab, Vector2 position);
    }
}