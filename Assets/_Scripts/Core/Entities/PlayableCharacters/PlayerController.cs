using MagmaHeart.Core.Input;
using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class PlayerController
    {
        private readonly Player m_player;
        private readonly Rigidbody2D m_rigidbody;
        private readonly UserInput m_userInput;
        private readonly RigidbodyMovement m_movement;
        private readonly EntityAnimation m_animation;
        private readonly Facing m_facing;

        private IInteractable m_currentInteractableObject;
        private Vector2 m_currentMovement;

        public PlayerController(Player player, UserInput userInput)
        {
            m_userInput = userInput;
            m_player = player;
            m_animation = player.Animation;
            m_movement = player.GetComponent<RigidbodyMovement>();
            m_facing = player.Facing;
            m_rigidbody = player.GetComponent<Rigidbody2D>();
        }

        private void HandleOnMovementKeyPressed(object obj, OnMovementKeyPressedEventArgs args)
        {
            m_currentMovement = args.Movement;
            m_facing.TryUpdateFacing(m_currentMovement.x);

            if (m_currentMovement.magnitude > 0)
                m_animation.PlayRunAnimation();
            else
                m_animation.PlayIdleAnimation();
        }

        public void Enable()
        {
            m_animation.PlayIdleAnimation();

            m_userInput.OnMovementKeyPressed += HandleOnMovementKeyPressed;
            m_userInput.OnInteractionKeyPressed += Interact;

            m_player.OnTriggerEnter += OnTriggerEnter;
            m_player.OnTriggerExit += OnTriggerExit;
        }

        public void Disable()
        {
            m_userInput.OnMovementKeyPressed -= HandleOnMovementKeyPressed;
            m_userInput.OnInteractionKeyPressed -= Interact;

            m_player.OnTriggerEnter -= OnTriggerEnter;
            m_player.OnTriggerExit -= OnTriggerExit;

            m_rigidbody.linearVelocity = Vector3.zero;
        }

        public void Update() => m_movement.Move(m_currentMovement);

        private void Interact(object obj, EventArgs args)
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