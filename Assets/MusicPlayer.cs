using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> m_music;
        [SerializeField] private AudioSource m_audio;

        private int m_current = 0;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            StartPlaying();
        }

        public void StartPlaying()
        {
            if (m_music == null || m_music.Count == 0) return;

            PlayCurrent();
        }

        private void PlayCurrent()
        {
            m_audio.clip = m_music[m_current];
            m_audio.Play();
        }

        private void Update()
        {
            if (!m_audio.isPlaying)
            {
                NextTrack();
            }
        }

        private void NextTrack()
        {
            m_current++;

            // Loop back to start (optional)
            if (m_current >= m_music.Count)
            {
                m_current = 0;
            }

            PlayCurrent();
        }
    }
}

