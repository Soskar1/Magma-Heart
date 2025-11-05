using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Input;
using UnityEngine;

namespace MagmaHeart.Core.CameraControls
{
    public class CombatCameraBehaviour : ICameraBehaviour, ICombatTurnSwitchListener
    {
        private readonly CameraTargetTracker m_tracker;
        private readonly CameraZoom m_zoom;
        private readonly CombatUserInput m_userInput;
        private readonly int m_movementSpeed;
        
        private Transform m_transform;
        private bool m_stickCameraWithTarget = true;

        public CombatCameraBehaviour(Transform transform, CombatUserInput userInput, int movementSpeed, CameraZoom zoom)
        {
            m_transform = transform;
            m_userInput = userInput;
            m_movementSpeed = movementSpeed;
            m_tracker = new CameraTargetTracker(transform);
            m_zoom = zoom;
        }

        public void Update()
        {
            m_zoom.Zoom(m_userInput.MouseScroll);

            if (m_stickCameraWithTarget)
                m_tracker.StickWithTarget();
            else
                m_transform.position += (Vector3)m_userInput.CameraMovement * Time.deltaTime * m_movementSpeed;

            if (m_userInput.CameraMovement.magnitude > 0)
                m_stickCameraWithTarget = false;
        }

        public void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            m_tracker.Track(args.CurrentEntity.Transform);
            m_stickCameraWithTarget = true;
        }
    }
}