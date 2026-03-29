using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class Meteor : MonoBehaviour
    {
        private Vector2 _startPosition;
        private Vector2 _targetPosition;

        [SerializeField] private float _duration = 1.5f;
        [SerializeField] private float _maxHeight = 5f;
        [SerializeField] private float _maxScale = 50f;
        [SerializeField] private float _minScale = 30f;

        [SerializeField] private AudioSource m_audio;
        [SerializeField] private List<AudioClip> m_fireBeam;
        
        private float _timer;

        private TaskCompletionSource<bool> m_meteorHit;

        public Task<bool> Initialize(Vector2 targetPosition)
        {
            _targetPosition = targetPosition;
            _startPosition = transform.position;
            _timer = 0f;
            
            var fireBeam = m_fireBeam[Random.Range(0, m_fireBeam.Count)];
            m_audio.clip = fireBeam;
            m_audio.Play();

            m_meteorHit = new TaskCompletionSource<bool>();
            return m_meteorHit.Task;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            float t = _timer / _duration;

            if (t >= 1f)
            {
                Impact();
                return;
            }

            // Smooth interpolation (ease-in for gravity feel)
            float easedT = t * t;

            // Move along straight line
            Vector2 position = Vector2.Lerp(_startPosition, _targetPosition, easedT);

            // Parabolic height (fake Z axis)
            float height = 4 * _maxHeight * t * (1 - t);
            // classic parabola: peak at t=0.5

            // Apply height visually (Y offset)
            transform.position = position + Vector2.up * height;

            // Optional: scale for extra depth illusion
            float scale = Mathf.Lerp(_maxScale, _minScale, t);
            transform.localScale = Vector3.one * scale;
        }

        private void Impact()
        {
            m_audio.Stop();
            transform.position = _targetPosition;

            m_meteorHit.SetResult(true);
            Destroy(gameObject);
        }
    }
}
