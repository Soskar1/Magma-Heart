using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MagmaHeart.Core.Input
{
    public class UserInput
    {
        public Controls Controls { get; private set; }
        private Controls.PlayerActions Player => Controls.Player;
        private Controls.MouseActions Mouse => Controls.Mouse;

        public event EventHandler<OnMovementKeyPressedEventArgs> OnMovementKeyPressed;
        public event EventHandler<OnMouseScrollEventArgs> OnMouseScroll;
        public event EventHandler<OnMousePositionChangedEventArgs> OnMousePositionChanged;
        public event EventHandler OnLeftMouseButtonClick;
        public event EventHandler OnDebugButtonClick;

        public UserInput()
        {
            Controls = new Controls();

            Player.Enable();
            
            Enable();
        }

        public void Enable()
        {
            Player.Move.performed += HandleMovement;
            Player.Move.canceled += HandleMovement;
            Player.DebugWindow.performed += HandleOnDebugButtonClick;

            Mouse.MouseScroll.performed += HandleMouseScroll;
            Mouse.MouseScroll.canceled += HandleMouseScroll;
            Mouse.MousePosition.performed += HandleMousePosition;
            Mouse.MouseClick.performed += HandleLeftMouseButtonClick;

            Controls.Enable();
            Mouse.Enable();
        }

        public void Disable()
        {
            Player.Move.performed -= HandleMovement;
            Player.Move.canceled -= HandleMovement;
            Player.DebugWindow.performed -= HandleOnDebugButtonClick;

            Mouse.MouseScroll.performed -= HandleMouseScroll;
            Mouse.MouseScroll.canceled -= HandleMouseScroll;
            Mouse.MousePosition.performed -= HandleMousePosition;
            Mouse.MouseClick.performed -= HandleLeftMouseButtonClick;

            Controls.Disable();
            Mouse.Disable();
        }

        private void HandleMovement(InputAction.CallbackContext context)
        {
            Vector2 movement = Vector2.zero;

            if (context.performed)
                movement = Player.Move.ReadValue<Vector2>();

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

        private void HandleMousePosition(InputAction.CallbackContext context)
        {
            Vector2 position = Mouse.MousePosition.ReadValue<Vector2>();

            OnMousePositionChangedEventArgs args = new OnMousePositionChangedEventArgs(position);
            OnMousePositionChanged?.Invoke(this, args);
        }

        private void HandleLeftMouseButtonClick(InputAction.CallbackContext context) => OnLeftMouseButtonClick?.Invoke(this, EventArgs.Empty);

        private void HandleOnDebugButtonClick(InputAction.CallbackContext context) => OnDebugButtonClick?.Invoke(this, EventArgs.Empty);
    }
}