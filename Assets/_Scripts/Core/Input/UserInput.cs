using MagmaHeart.Core.StateMachines;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MagmaHeart.Core.Input
{
    public class UserInput : IActionStateListener, ICombatStateListener
    {
        public Controls Controls { get; private set; }
        private Controls.ActionPlayerActions ActionPlayer => Controls.ActionPlayer;
        private Controls.TurnBasedPlayerActions TurnBasedPlayer => Controls.TurnBasedPlayer;
        private Controls.MouseActions Mouse => Controls.Mouse;

        public event EventHandler<OnMovementKeyPressedEventArgs> OnMovementKeyPressed;
        public event EventHandler<OnMouseScrollEventArgs> OnMouseScroll;
        public event EventHandler OnInteractionKeyPressed;

        public UserInput()
        {
            Controls = new Controls();

            ActionPlayer.Move.performed += HandleMovement;
            ActionPlayer.Move.canceled += HandleMovement;
            ActionPlayer.Interaction.performed += HandleInteraction;

            Mouse.MouseScroll.performed += HandleMouseScroll;
            Mouse.MouseScroll.canceled += HandleMouseScroll;
            ActionPlayer.Enable();
            Mouse.Enable();
        }

        public void Enable() => Controls.Enable();
        public void Disable() => Controls.Disable();

        private void HandleMovement(InputAction.CallbackContext context)
        {
            Vector2 movement = Vector2.zero;

            if (context.performed)
                movement = ActionPlayer.Move.ReadValue<Vector2>();

            OnMovementKeyPressedEventArgs args = new OnMovementKeyPressedEventArgs(movement);
            OnMovementKeyPressed?.Invoke(this, args);
        }

        private void HandleMouseScroll(InputAction.CallbackContext context)
        {
            float mouseScroll = 0;

            if (context.performed)
                mouseScroll = Mouse.MouseScroll.ReadValue<float>();
            
            OnMouseScrollEventArgs args = new OnMouseScrollEventArgs(mouseScroll);
            OnMouseScroll?.Invoke(this, args);
        }

        private void HandleInteraction(InputAction.CallbackContext context)
        {
            OnInteractionKeyPressed?.Invoke(this, EventArgs.Empty);
        }

        public void EnterActionState() => ActionPlayer.Enable();
        public void ExitActionState() => ActionPlayer.Disable();
        public void EnterCombatState() => TurnBasedPlayer.Enable();
        public void ExitCombatState() => TurnBasedPlayer.Disable();
    }
}