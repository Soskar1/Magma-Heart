using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Input;
using UnityEngine;

namespace MagmaHeart.Core.CameraControls
{
    public class CombatCameraBehaviour : ICameraBehaviour
    {
        private readonly CameraTargetTracker m_tracker;
        private readonly UserInput m_userInput;
        private readonly int m_movementSpeed;
        private readonly Battle m_battle;
        private readonly Transform m_transform;

        private bool m_stickCameraWithTarget = true;

        private Vector2 m_currentMovement;

        public CombatCameraBehaviour(Transform transform, CameraTargetTracker tracker, UserInput userInput, int movementSpeed, Battle battle)
        {
            m_transform = transform;
            m_userInput = userInput;
            m_movementSpeed = movementSpeed;
            m_tracker = tracker;
            m_battle = battle;
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

        public void Enable()
        {
            m_userInput.OnMovementKeyPressed += HandleOnMovementKeyPressed;
            m_battle.OnTurnSwitched += HandleOnTurnSwitched;
        }

        public void Disable()
        {
            m_userInput.OnMovementKeyPressed -= HandleOnMovementKeyPressed;
            m_battle.OnTurnSwitched -= HandleOnTurnSwitched;
        }

        private void HandleOnMovementKeyPressed(object obj, OnMovementKeyPressedEventArgs args) => m_currentMovement = args.Movement;

        private void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            m_tracker.Track(args.CurrentEntity.transform);
            m_stickCameraWithTarget = true;
        }
    }
}