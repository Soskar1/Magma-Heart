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

        private float m_time;

        void Update()
        {
            m_time += Time.deltaTime * m_pulseSpeed;

            float t = (Mathf.Sin(m_time) + 1f) / 2f;

            m_light2D.intensity = Mathf.Lerp(m_minIntensity, m_maxIntensity, t);
            m_light2D.pointLightOuterRadius = Mathf.Lerp(m_minRadius, m_maxRadius, t);
        }
    }
}