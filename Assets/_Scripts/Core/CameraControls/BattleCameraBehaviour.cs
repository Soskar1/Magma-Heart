using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Input;
using UnityEngine;

namespace MagmaHeart.Core.CameraControls
{
    public class BattleCameraBehaviour
    {
        private readonly CameraTargetTracker m_tracker;
        private readonly UserInput m_userInput;
        private readonly int m_movementSpeed;
        private readonly Battle m_battle;
        private readonly Transform m_transform;

        private bool m_stickCameraWithTarget = true;
        private bool m_enabled = false;

        private const int m_smoothSpeed = 5;

        private BoundsInt m_currentMovementBounds;
        private Vector2 m_currentMovement;

        public BattleCameraBehaviour(Transform transform, CameraTargetTracker tracker, UserInput userInput, int movementSpeed, Battle battle)
        {
            m_transform = transform;
            m_userInput = userInput;
            m_movementSpeed = movementSpeed;
            m_tracker = tracker;
            m_battle = battle;
        }

        public void Enable(BoundsInt movementBounds)
        {
            m_currentMovementBounds = movementBounds;

            m_userInput.OnMovementKeyPressed += HandleOnMovementKeyPressed;
            m_battle.OnTurnSwitched += HandleOnTurnSwitched;
            
            m_enabled = true;
        }

        public void Disable()
        {
            m_enabled = false;
            m_userInput.OnMovementKeyPressed -= HandleOnMovementKeyPressed;
            m_battle.OnTurnSwitched -= HandleOnTurnSwitched;
        }

        public void Update()
        {
            if (!m_enabled)
                return;

            Vector2 newCameraPosition;
            if (m_stickCameraWithTarget)
                newCameraPosition = m_tracker.GetTrackedObjectPosition();
            else
                newCameraPosition = new Vector3(m_transform.position.x, m_transform.position.y) + (Vector3)m_currentMovement * Time.deltaTime * m_movementSpeed;

            if (newCameraPosition.x < m_currentMovementBounds.xMin)
                newCameraPosition.x = m_currentMovementBounds.xMin;

            if (newCameraPosition.x > m_currentMovementBounds.xMax)
                newCameraPosition.x = m_currentMovementBounds.xMax;

            if (newCameraPosition.y < m_currentMovementBounds.yMin)
                newCameraPosition.y = m_currentMovementBounds.yMin;

            if (newCameraPosition.y > m_currentMovementBounds.yMax)
                newCameraPosition.y = m_currentMovementBounds.yMax;

            if (m_stickCameraWithTarget)
            {
                Vector3 currentPosition = m_transform.position;
                Vector3 targetPosition = new Vector3(newCameraPosition.x, newCameraPosition.y, currentPosition.z);
                m_transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * m_smoothSpeed);
            }
            else
            {
                m_transform.position = new Vector3(newCameraPosition.x, newCameraPosition.y, m_transform.position.z);
            }

            if (m_currentMovement.magnitude > 0)
                m_stickCameraWithTarget = false;
        }

        private void HandleOnMovementKeyPressed(object obj, OnMovementKeyPressedEventArgs args) => m_currentMovement = args.Movement;

        private void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            m_tracker.Track(args.CurrentEntity.transform);
            m_stickCameraWithTarget = true;
        }
    }
}