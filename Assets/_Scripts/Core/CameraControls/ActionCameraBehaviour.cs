using MagmaHeart.Core.Input;
using UnityEngine;

namespace MagmaHeart.Core.CameraControls
{
    public class ActionCameraBehaviour : ICameraBehaviour
    {
        private readonly CameraTargetTracker m_tracker;
        private readonly CameraZoom m_zoom;
        private readonly ActionUserInput m_userInput;

        public ActionCameraBehaviour(ActionUserInput userInput, Transform transform, Transform playerToTrack, CameraZoom zoom)
        {
            m_tracker = new CameraTargetTracker(transform);
            m_tracker.Track(playerToTrack);
            m_zoom = zoom;
            m_userInput = userInput;
        }

        public void Update()
        {
            m_zoom.Zoom(m_userInput.MouseScroll);
            m_tracker.StickWithTarget();
        }
    }
}