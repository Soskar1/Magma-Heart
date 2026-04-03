using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MagmaHeart.Core
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_visual;
        [SerializeField] private Light2D m_light;
        [SerializeField] private AudioSource m_audio;
        [SerializeField] private List<AudioClip> m_explosion;

        private void Start()
        {
            var explosion = m_explosion[Random.Range(0, m_explosion.Count)];
            m_audio.PlayOneShot(explosion);
        }

        public void DestroyExplosion()
        {
            m_visual.enabled = false;
            m_light.enabled = false;
            Destroy(gameObject, 5);
        }
    }
}