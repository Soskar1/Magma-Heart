using UnityEngine;
using UnityEngine.InputSystem;

namespace MagmaHeart.Core.Entities
{
    public class PlayerBehaviour : MonoBehaviour
    {
        private UserInput m_userInput;
        private Entity m_entityToControl;

        public void Awake()
        {
            m_userInput = new UserInput();
            m_entityToControl = GetComponent<Entity>();
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

        public void Update() => m_entityToControl.ApplyMovement(m_userInput.Movement);

        public void Attack(InputAction.CallbackContext context) => m_entityToControl.Attack();
    }
}