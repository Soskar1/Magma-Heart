using UnityEngine;

namespace MagmaHeart.Core.CameraControls
{
    public class CameraTargetTracker
    {
        private Transform m_transform;
        private Transform m_objectToTrack;

        public CameraTargetTracker(Transform transform) => m_transform = transform;

        public void Track(Transform objectToTrack) => m_objectToTrack = objectToTrack;

        public void StickWithTarget()
        {
            if (m_objectToTrack == null)
            {
                Debug.LogWarning("Tracking object is null!");
                return;
            }

            m_transform.position = new Vector3(m_objectToTrack.position.x, m_objectToTrack.position.y, m_transform.position.z);
        }
    }
}