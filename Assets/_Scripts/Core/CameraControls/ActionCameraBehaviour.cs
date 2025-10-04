using UnityEngine;

namespace MagmaHeart.Core.CameraControls
{
    public class ActionCameraBehaviour : ICameraBehaviour
    {
        private readonly CameraTargetTracker m_tracker;

        public ActionCameraBehaviour(Transform transform, Transform playerToTrack)
        {
            m_tracker = new CameraTargetTracker(transform);
            m_tracker.Track(playerToTrack);
        }

        public void Update() => m_tracker.StickWithTarget();
    }
}