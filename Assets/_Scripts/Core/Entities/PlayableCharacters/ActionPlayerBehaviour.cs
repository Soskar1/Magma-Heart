using UnityEngine;
using UnityEngine.InputSystem;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class ActionPlayerBehaviour : IPlayerBehaviour
    {
        private readonly Player m_player;
        private readonly Rigidbody2D m_rigidbody;
        private readonly UserInput m_userInput;
        private readonly RigidbodyMovement m_movement;
        private readonly PlayerAnimation m_animation;
        private readonly Facing m_facing;

        private IInteractable m_currentInteractableObject;

        public ActionPlayerBehaviour(Player player, UserInput userInput)
        {
            m_userInput = userInput;
            m_player = player;
            m_animation = player.GetComponent<PlayerAnimation>();
            m_movement = player.GetComponent<RigidbodyMovement>();
            m_facing = player.GetComponent<Facing>();
            m_rigidbody = player.GetComponent<Rigidbody2D>();
        }

        public void Enable()
        {
            m_animation.PlayIdleAnimation();

            m_userInput.Controls.ActionPlayer.Interaction.performed += Interact;
            m_userInput.Controls.ActionPlayer.Enable();

            m_player.OnTriggerEnter += OnTriggerEnter;
            m_player.OnTriggerExit += OnTriggerExit;
        }

        public void Disable()
        {
            m_userInput.Controls.ActionPlayer.Interaction.performed -= Interact;
            m_userInput.Controls.ActionPlayer.Disable();

            m_player.OnTriggerEnter -= OnTriggerEnter;
            m_player.OnTriggerExit -= OnTriggerExit;

            m_rigidbody.linearVelocity = Vector3.zero;
        }

        public void Update()
        {
            Vector2 movement = m_userInput.Controls.ActionPlayer.Move.ReadValue<Vector2>();
            m_facing.TryUpdateFacing(movement.x);
            m_movement.Move(movement);

            if (movement.magnitude > 0)
                m_animation.PlayRunAnimation();
            else
                m_animation.PlayIdleAnimation();
        }

        private void Interact(InputAction.CallbackContext context)
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