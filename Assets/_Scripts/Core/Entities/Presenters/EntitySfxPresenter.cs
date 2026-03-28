using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Entities.Presenters
{
    public class EntitySfxPresenter : MonoBehaviour
    {
        [SerializeField] private AudioSource m_audio;
        [SerializeField] private List<AudioClip> m_steps;

        public void PlayStepSound()
        {
            var audioClip = m_steps[Random.Range(0, m_steps.Count)];
            m_audio.PlayOneShot(audioClip);
        }
    }
}
