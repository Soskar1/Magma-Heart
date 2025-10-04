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

        public TurnBasedCameraBehaviour(Transform transform, TurnBasedUserInput userInput, int speed)
        {
            m_transform = transform;
            m_userInput = userInput;
            m_speed = speed;
            m_tracker = new CameraTargetTracker(transform);
        }

        public void Update()
        {
            // m_transform.position += (Vector3)m_userInput.CameraMovement * Time.deltaTime * m_speed;
            m_tracker.StickWithTarget();
        }

        public void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            m_tracker.Track(args.Entity.Transform);
        }
    }
}