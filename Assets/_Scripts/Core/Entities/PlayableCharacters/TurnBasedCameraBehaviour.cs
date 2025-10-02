using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class TurnBasedCameraBehaviour : ICameraBehaviour
    {
        private Transform m_transform;
        private TurnBasedUserInput m_userInput;
        private int m_speed;

        public TurnBasedCameraBehaviour(Transform transform, TurnBasedUserInput userInput, int speed)
        {
            m_transform = transform;
            m_userInput = userInput;
            m_speed = speed;
        }

        public void Update()
        {
            m_transform.position += (Vector3)m_userInput.CameraMovement * Time.deltaTime * m_speed;
        }
    }
}