using UnityEngine;

namespace MagmaHeart.Core
{
    [RequireComponent(typeof(RigidbodyMovement))]
    [RequireComponent(typeof(Animator))]
    public class Player : MonoBehaviour
    {
        private UserInput m_userInput;
        private RigidbodyMovement m_movement;
        private Animator m_animator;

        private void Awake() 
        {
            m_userInput = new UserInput();
            m_movement = GetComponent<RigidbodyMovement>();
            m_animator = GetComponent<Animator>();
        }

        private void Start() => m_userInput.Enable();

        public void Update()
        {
            if (Mathf.Abs(m_userInput.Movement.magnitude) > 0)
                m_animator.SetBool("IsRunning", true);
            else
                m_animator.SetBool("IsRunning", false);
        }

        public void FixedUpdate()
        {
            m_movement.Move(m_userInput.Movement);
        }                
    }
}
