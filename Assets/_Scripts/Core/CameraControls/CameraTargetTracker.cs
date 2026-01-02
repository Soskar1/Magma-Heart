using UnityEngine;

namespace MagmaHeart.Core.CameraControls
{
    public class CameraTargetTracker
    {
        private Transform m_objectToTrack;

        public void Track(Transform objectToTrack) => m_objectToTrack = objectToTrack;

        public Vector3 GetTrackedObjectPosition()
        {
            if (m_objectToTrack == null)
            {
                Debug.LogWarning("Tracking object is null!");
                return Vector3.zero;
            }

            return new Vector3(m_objectToTrack.position.x, m_objectToTrack.position.y, 0);
        }
    }
}