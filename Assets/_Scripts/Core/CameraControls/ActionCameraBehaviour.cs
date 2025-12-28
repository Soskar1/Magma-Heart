using UnityEngine;

namespace MagmaHeart.Core.CameraControls
{
    public class ActionCameraBehaviour : ICameraBehaviour
    {
        private readonly CameraTargetTracker m_tracker;

        public ActionCameraBehaviour(CameraTargetTracker tracker, Transform playerToTrack)
        {
            m_tracker = tracker;
            m_tracker.Track(playerToTrack);
        }

        public void Disable() { }
        public void Enable() { }
        
        public void Update()
        {
            m_tracker.StickWithTarget();
        }
    }
}