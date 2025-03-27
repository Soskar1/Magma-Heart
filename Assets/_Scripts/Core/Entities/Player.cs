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

        private bool m_isAttacking = false;

        private readonly int m_idleAnimationID = Animator.StringToHash("Idle");
        private readonly int m_runAnimationID = Animator.StringToHash("Run");
        private readonly int m_attackAnimationID = Animator.StringToHash("Attack");
        private int m_currentAnimationState;

        private void Awake() 
        {
            m_userInput = new UserInput();
            m_movement = GetComponent<RigidbodyMovement>();
            m_animator = GetComponent<Animator>();

            m_currentAnimationState = m_idleAnimationID;
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
            if ((m_userInput.Movement.x > 0 && !m_facingRight) || (m_userInput.Movement.x < 0 && m_facingRight))
            {
                transform.Rotate(new Vector3(0, 180, 0));
                m_facingRight = !m_facingRight;
            }

            PlayAnimations();
        }

        private void PlayAnimations()
        {
            int stateToPlay = GetAnimationState();
            
            if (stateToPlay == m_currentAnimationState)
                return;
            
            m_animator.CrossFade(stateToPlay, 0);
            m_currentAnimationState = stateToPlay;
        }

        private int GetAnimationState()
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && m_currentAnimationState == m_attackAnimationID)
                m_isAttacking = false;

            if (m_isAttacking)
                return m_attackAnimationID;
            
            if (Mathf.Abs(m_userInput.Movement.magnitude) > 0)
                return m_runAnimationID;
            
            return m_idleAnimationID;
        }

        public void FixedUpdate()
        {
            m_movement.Move(m_userInput.Movement);
        }

        public void Attack(InputAction.CallbackContext context)
        {
            m_isAttacking = true;
        }
    }
}
