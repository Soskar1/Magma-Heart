using UnityEngine;

namespace MagmaHeart.Core
{
    public class Explosion : MonoBehaviour
    {
        public void DestroyExplosion()
        {
            Destroy(gameObject);
        }
    }
}