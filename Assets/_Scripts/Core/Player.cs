using UnityEngine;

namespace MagmaHeart.Core
{
    [RequireComponent(typeof(RigidbodyMovement))]
    public class Player : MonoBehaviour
    {
        private UserInput m_userInput;
        private RigidbodyMovement m_movement;

        private void Awake() 
        {
            m_userInput = new UserInput();
            m_movement = GetComponent<RigidbodyMovement>();
        }

        private void Start() => m_userInput.Enable();
        
        public void FixedUpdate()
        {
            m_movement.Move(m_userInput.Movement);
        }                
    }
}
