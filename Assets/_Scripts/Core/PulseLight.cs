using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MagmaHeart.Core
{
    public class LightPulse2D : MonoBehaviour
    {
        [SerializeField] private Light2D m_light2D;

        [Header("Pulse Settings")]
        [SerializeField] private float m_minIntensity = 0.5f;
        [SerializeField] private float m_maxIntensity = 1.5f;
        [SerializeField] private float m_pulseSpeed = 2f;

        [Header("Optional Radius Pulse")]
        [SerializeField] private float m_minRadius = 2f;
        [SerializeField] private float m_maxRadius = 3f;

        [SerializeField] private bool m_forcePulsing = true;

        private float m_time;
        private bool m_isPulsing;

        private Coroutine m_currentRoutine;

        public void StartPulse()
        {
            if (m_currentRoutine != null)
                StopCoroutine(m_currentRoutine);

            m_currentRoutine = StartCoroutine(StartPulseRoutine());
        }

        private IEnumerator StartPulseRoutine()
        {
            m_isPulsing = false;

            float duration = 1f; // tweak this for how fast it fades in
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                m_light2D.intensity = Mathf.Lerp(0f, m_minIntensity, t);

                yield return null;
            }

            m_light2D.intensity = m_minIntensity;

            // reset time so pulse starts clean
            m_time = 0f;

            m_isPulsing = true;
        }

        public void StopPulse()
        {
            if (m_currentRoutine != null)
                StopCoroutine(m_currentRoutine);

            m_currentRoutine = StartCoroutine(StopPulseRoutine());
        }

        private IEnumerator StopPulseRoutine()
        {
            m_isPulsing = false;

            float startIntensity = m_light2D.intensity;
            float startRadius = m_light2D.pointLightOuterRadius;

            float duration = 1f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                m_light2D.intensity = Mathf.Lerp(startIntensity, 0f, t);
                m_light2D.pointLightOuterRadius = Mathf.Lerp(startRadius, m_minRadius, t);

                yield return null;
            }

            m_light2D.intensity = 0f;
            m_light2D.pointLightOuterRadius = m_minRadius;
        }

        void Update()
        {
            if (!m_isPulsing && !m_forcePulsing)
                return;

            m_time += Time.deltaTime * m_pulseSpeed;

            float t = Mathf.PingPong(m_time, 1f);

            m_light2D.intensity = Mathf.Lerp(m_minIntensity, m_maxIntensity, t);
            m_light2D.pointLightOuterRadius = Mathf.Lerp(m_minRadius, m_maxRadius, t);
        }
    }
}