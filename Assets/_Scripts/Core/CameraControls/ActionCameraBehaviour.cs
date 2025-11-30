using MagmaHeart.Core.Input;
using UnityEngine;

namespace MagmaHeart.Core.CameraControls
{
    public class ActionCameraBehaviour : ICameraBehaviour
    {
        private readonly CameraTargetTracker m_tracker;
        private readonly CameraZoom m_zoom;
        private readonly UserInput m_userInput;

        private float m_currentMouseScroll;

        public ActionCameraBehaviour(UserInput userInput, Transform transform, Transform playerToTrack, CameraZoom zoom)
        {
            m_tracker = new CameraTargetTracker(transform);
            m_tracker.Track(playerToTrack);
            m_zoom = zoom;
            m_userInput = userInput;
        }

        public void Enable() => m_userInput.OnMouseScroll += HandleOnMouseScroll;
        public void Disable() => m_userInput.OnMouseScroll -= HandleOnMouseScroll;
        public void Update()
        {
            m_tracker.StickWithTarget();
            m_zoom.Zoom(m_currentMouseScroll);
        }

        public void HandleOnMouseScroll(object obj, OnMouseScrollEventArgs args) => m_currentMouseScroll = args.MouseScroll;
    }
}