using UnityEngine;

namespace MagmaHeart.Spawning
{
    public class UnityInstantiator : IInstantiator
    {
        public GameObject Instantiate(GameObject prefab, Vector2 position) => Object.Instantiate(prefab, position, Quaternion.identity);
    }
}