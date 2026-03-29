using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Entities.Presenters
{
    public class EntitySfxPresenter : MonoBehaviour
    {
        [SerializeField] private AudioSource m_audio;
        [SerializeField] private List<AudioClip> m_steps;
        [SerializeField] private List<AudioClip> m_charge;

        public void PlayStepSound() => PlaySound(m_steps);
        public void PlayChargeSound() => PlaySound(m_charge);

        public void PlaySound(AudioClip clip)
        {
            m_audio.PlayOneShot(clip);
        }

        public void PlaySound(IList<AudioClip> clips)
        {
            var clipToPlay = clips[Random.Range(0, clips.Count)];
            m_audio.PlayOneShot(clipToPlay);
        }
    }
}
