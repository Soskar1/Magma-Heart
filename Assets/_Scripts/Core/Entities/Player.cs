using UnityEngine;
using UnityEngine.InputSystem;

namespace MagmaHeart.Core.Entities
{
    [RequireComponent(typeof(RigidbodyMovement))]
    [RequireComponent(typeof(Animator))]
    public class Player : MonoBehaviour
    {
        private UserInput m_userInput;
        private RigidbodyMovement m_movement;
        private Animator m_animator;
        [SerializeField] private bool m_facingRight;

        private void Awake() 
        {
            m_userInput = new UserInput();
            m_movement = GetComponent<RigidbodyMovement>();
            m_animator = GetComponent<Animator>();
        }

        private void Start()
        {
            m_userInput.Controls.Player.Attack.performed += Attack;
            m_userInput.Enable();
        }

        private void OnDisable()
        {
            m_userInput.Controls.Player.Attack.performed -= Attack;
            m_userInput.Disable();
        }

        public void Update()
        {
            if (Mathf.Abs(m_userInput.Movement.magnitude) > 0)
                m_animator.SetBool("IsRunning", true);
            else
                m_animator.SetBool("IsRunning", false);
            
            if ((m_userInput.Movement.x > 0 && !m_facingRight) || (m_userInput.Movement.x < 0 && m_facingRight))
            {
                transform.Rotate(new Vector3(0, 180, 0));
                m_facingRight = !m_facingRight;
            }
        }

        public void FixedUpdate()
        {
            m_movement.Move(m_userInput.Movement);
        }

        public void Attack(InputAction.CallbackContext context)
        {
            m_animator.SetTrigger("Attack");
        }
    }
}
