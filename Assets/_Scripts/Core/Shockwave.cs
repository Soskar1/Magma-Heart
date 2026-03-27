using System.Collections;
using UnityEngine;

namespace MagmaHeart.Core
{
    public class Shockwave : MonoBehaviour
    {
        [SerializeField] private Material m_material;

        private void Start()
        {
            StartCoroutine(Ripple());
        }

        public IEnumerator Ripple()
        {
            float t = 0;

            while (t < 1)
            {
                t += Time.deltaTime;
                m_material.SetFloat("_spread", t);
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}