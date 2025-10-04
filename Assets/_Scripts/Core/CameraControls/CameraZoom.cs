using UnityEngine;

namespace MagmaHeart.Core.CameraControls
{
    public class CameraZoom
    {
        private readonly Camera m_camera;
        private readonly float m_speed;
        private readonly float m_maxZoom;
        private readonly float m_minZoom;
        private readonly float m_smoothTime;
        private float m_currentZoom;
        private float m_velocity;

        public CameraZoom(Camera camera, float speed, float minZoom, float maxZoom, float smoothTime = 0.0f)
        {
            m_speed = speed;
            m_camera = camera;
            m_smoothTime = smoothTime;
            m_maxZoom = maxZoom;
            m_minZoom = minZoom;

            m_currentZoom = camera.orthographicSize;
        }

        public void Zoom(float direction)
        {
            m_currentZoom -= direction * m_speed;
            m_currentZoom = Mathf.Clamp(m_currentZoom, m_minZoom, m_maxZoom);

            m_camera.orthographicSize = Mathf.SmoothDamp(m_camera.orthographicSize, m_currentZoom, ref m_velocity, m_smoothTime);
        }
    }
}