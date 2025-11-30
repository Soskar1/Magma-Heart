using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Input;
using UnityEngine;

namespace MagmaHeart.Core.CameraControls
{
    public class CombatCameraBehaviour : ICameraBehaviour, ITurnSwitchListener
    {
        private readonly CameraTargetTracker m_tracker;
        private readonly UserInput m_userInput;
        private readonly int m_movementSpeed;
        
        private Transform m_transform;
        private bool m_stickCameraWithTarget = true;

        private Vector2 m_currentMovement;

        public CombatCameraBehaviour(CameraTargetTracker tracker, UserInput userInput, int movementSpeed)
        {
            m_userInput = userInput;
            m_movementSpeed = movementSpeed;
            m_tracker = tracker;
        }

        public void Update()
        {
            if (m_stickCameraWithTarget)
                m_tracker.StickWithTarget();
            else
                m_transform.position += (Vector3)m_currentMovement * Time.deltaTime * m_movementSpeed;

            if (m_currentMovement.magnitude > 0)
                m_stickCameraWithTarget = false;
        }

        public void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            m_tracker.Track(args.Entity.transform);
            m_stickCameraWithTarget = true;
        }

        public void Enable() => m_userInput.OnMovementKeyPressed += HandleOnMovementKeyPressed;
        public void Disable() => m_userInput.OnMovementKeyPressed -= HandleOnMovementKeyPressed;
        private void HandleOnMovementKeyPressed(object obj, OnMovementKeyPressedEventArgs args) => m_currentMovement = args.Movement;

    }
}