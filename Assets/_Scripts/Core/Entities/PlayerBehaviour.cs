using UnityEngine;
using UnityEngine.InputSystem;

namespace MagmaHeart.Core.Entities
{
    public class PlayerBehaviour : MonoBehaviour
    {
        private UserInput m_userInput;
        private IAttacker m_entityAttack;
        private IMovable m_entityMovement;

        public void Awake()
        {
            m_userInput = new UserInput();
            Entity entity = GetComponent<Entity>();
            m_entityAttack = entity as IAttacker;
            m_entityMovement = entity as IMovable;

            if (m_entityAttack == null)
                Debug.LogError($"Entity {transform.name} does not support IAttacker interface");

            if (m_entityMovement == null)
                Debug.LogError($"Entity {transform.name} does not support IMovable interface");
        }

        private void OnEnable()
        {
            m_userInput.Controls.Player.Attack.performed += Attack;
            m_userInput.Enable();
        }

        private void OnDisable()
        {
            m_userInput.Controls.Player.Attack.performed -= Attack;
            m_userInput.Disable();
        }

        public void FixedUpdate() => m_entityMovement.Move(m_userInput.Movement);

        public void Attack(InputAction.CallbackContext context) => m_entityAttack.Attack();
    }
}