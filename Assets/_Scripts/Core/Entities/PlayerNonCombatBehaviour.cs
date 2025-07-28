using UnityEngine;
using UnityEngine.InputSystem;

namespace MagmaHeart.Core.Entities
{
    public class PlayerNonCombatBehaviour
    {
        private UserInput m_userInput;
        private IMovable m_movement;

        private IInteractable m_currentInteractableObject;
        private Player m_player;

        public PlayerNonCombatBehaviour(UserInput userInput, IMovable movement, Player player)
        {
            m_userInput = userInput;
            m_movement = movement;
            m_player = player;
        }

        public void Enable()
        {
            m_userInput.Controls.Player.Interaction.performed += Interact;
            m_userInput.Enable();

            m_player.OnTriggerEnter += OnTriggerEnter;
            m_player.OnTriggerExit += OnTriggerExit;
        }

        public void Disable()
        {
            m_userInput.Controls.Player.Interaction.performed -= Interact;
            m_userInput.Disable();

            m_player.OnTriggerEnter -= OnTriggerEnter;
            m_player.OnTriggerExit -= OnTriggerExit;
        }

        public void Update() => m_movement.Move(m_userInput.Movement);

        public void Interact(InputAction.CallbackContext context)
        {
            if (m_currentInteractableObject != null)
                m_currentInteractableObject.Interact();
        }

        private void OnTriggerEnter(Collider2D collider)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
                m_currentInteractableObject = interactable;
        }

        private void OnTriggerExit(Collider2D collider)
        {
            if (collider.TryGetComponent(out IInteractable interactable) && interactable == m_currentInteractableObject)
                m_currentInteractableObject = null;
        }
    }
}