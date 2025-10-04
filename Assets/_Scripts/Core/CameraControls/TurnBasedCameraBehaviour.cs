using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Input;
using UnityEngine;

namespace MagmaHeart.Core.CameraControls
{
    public class TurnBasedCameraBehaviour : ICameraBehaviour
    {
        private readonly CameraTargetTracker m_tracker;
        private Transform m_transform;
        private TurnBasedUserInput m_userInput;
        private int m_speed;

        private bool m_stickCameraWithTarget = true;

        public TurnBasedCameraBehaviour(Transform transform, TurnBasedUserInput userInput, int speed)
        {
            m_transform = transform;
            m_userInput = userInput;
            m_speed = speed;
            m_tracker = new CameraTargetTracker(transform);
        }

        public void Update()
        {
            if (m_stickCameraWithTarget)
                m_tracker.StickWithTarget();
            else
                m_transform.position += (Vector3)m_userInput.CameraMovement * Time.deltaTime * m_speed;

            if (m_userInput.CameraMovement.magnitude > 0)
                m_stickCameraWithTarget = false;
        }

        public void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            m_tracker.Track(args.Entity.Transform);
            m_stickCameraWithTarget = true;
        }
    }
}