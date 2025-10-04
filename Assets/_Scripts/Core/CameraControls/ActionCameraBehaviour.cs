using UnityEngine;

namespace MagmaHeart.Core.CameraControls
{
    public class ActionCameraBehaviour : ICameraBehaviour
    {
        private readonly CameraTargetTracker m_tracker;
        private readonly Transform m_player;

        public ActionCameraBehaviour(CameraTargetTracker cameraTargetTracker, Transform playerToTrack)
        {
            m_tracker = cameraTargetTracker;
            m_player = playerToTrack;
        }

        public void Enable() => m_tracker.Track(m_player);
        public void Update() => m_tracker.StickWithTarget();
    }
}